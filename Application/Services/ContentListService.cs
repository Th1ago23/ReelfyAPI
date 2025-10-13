using Application.DTO.Content;
using Application.Interface.ContentInterface;
using Domain.Interface.HttpContext;
using Domain.Interface.Repository;
using Domain.Models.Contents;
using ReelfyAPI.Models;

namespace Application.Services;

public class ContentListService : IContentListService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContextUser _contextUser;

    public ContentListService(IUnitOfWork unitOfWork, IContextUser contextUser)
    {
        _unitOfWork = unitOfWork;
        _contextUser = contextUser;
    }

    public async Task<Response<ListDetailsDTO>> CreateListAsync(CreateContentListRequestDTO dto)
    {
        var userId = _contextUser.Id;

        var listExists = await _unitOfWork.ContentsList.AnyAsync(l => l.UserId == userId && l.Name == dto.Name);
        if (listExists)
            return new Response<ListDetailsDTO>(null, "Já existe uma lista com este nome.", 409);

        var newList = new ContentsList
        {
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description
        };

        await _unitOfWork.ContentsList.AddAsync(newList);
        await _unitOfWork.CommitAsync();

        var responseDto = new ListDetailsDTO(newList.Id, newList.Name, newList.Description, new List<ContentSummaryDTO>());
        return new Response<ListDetailsDTO>(responseDto, $"Lista '{newList.Name}' criada com sucesso.", 201);
    }

    public async Task<Response<ListDetailsDTO>> AddContentToListAsync(int listId, int contentId)
    {
        var userId = _contextUser.Id;
        var contentList = await _unitOfWork.ContentsList.GetByIdAsync(listId);

        if (contentList == null || contentList.UserId != userId)
            return new Response<ListDetailsDTO>(null, "Lista não encontrada ou não pertence a este usuário.", 404);

        var content = await _unitOfWork.Content.Find(contentId);
        if (content == null)
        {
            content = new Content { Id = contentId };
            await _unitOfWork.Content.Add(content);
        }

        if (contentList.Contents.Any(c => c.Id == contentId))
            return new Response<ListDetailsDTO>(null, "Este conteúdo já existe na lista.", 409);

        contentList.Contents.Add(content);
        await _unitOfWork.CommitAsync();

        var dtos = contentList.Contents.Select(c => new ContentSummaryDTO(c.Id,c.ContentType));
        var responseDto = new ListDetailsDTO(contentList.Id, contentList.Name, contentList.Description, dtos);

        return new Response<ListDetailsDTO>(responseDto, "Conteúdo adicionado com sucesso.", 200);
    }

    public async Task<Response<ListDetailsDTO>> RemoveContentFromListAsync(int listId, int contentId)
    {
        var userId = _contextUser.Id;
        var contentList = await _unitOfWork.ContentsList.GetByIdAsync(listId);

        if (contentList == null || contentList.UserId != userId)
            return new Response<ListDetailsDTO>(null, "Lista não encontrada ou não pertence a este usuário.", 404);

        var contentToRemove = contentList.Contents.FirstOrDefault(c => c.Id == contentId);
        if (contentToRemove == null)
            return new Response<ListDetailsDTO>(null, "Esse conteúdo não existe nesta lista.", 404);

        contentList.Contents.Remove(contentToRemove);
        await _unitOfWork.CommitAsync();

        var dtos = contentList.Contents.Select(c => new ContentSummaryDTO(c.Id, c.ContentType));
        var responseDto = new ListDetailsDTO(contentList.Id, contentList.Name, contentList.Description, dtos);

        return new Response<ListDetailsDTO>(responseDto, "Conteúdo removido com sucesso.", 200);
    }

    public async Task<Response<bool>> DeleteListAsync(int listId)
    {
        var userId = _contextUser.Id;
        var contentList = await _unitOfWork.ContentsList.GetByIdAsync(listId);

        if (contentList == null || contentList.UserId != userId)
            return new Response<bool>(false, "Lista não encontrada ou não pertence a este usuário.", 404);

        _unitOfWork.ContentsList.Delete(contentList);
        await _unitOfWork.CommitAsync();

        return new Response<bool>(true, $"Lista '{contentList.Name}' deletada com sucesso.", 200);
    }

    public async Task<Response<ContentListEnumerableDTO>> GetListsAsync()
    {
        var userId = _contextUser.Id;

        var lists = await _unitOfWork.ContentsList.GetListsByUserAsync(userId);
        if (lists == null || !lists.Any())
        {
            lists = new List<ContentsList>();
        }

        var allContentIds = lists.SelectMany(l => l.Contents.Select(c => c.Id)).Distinct().ToList();

        var seenContentIds = await _unitOfWork.AlreadySeenContent.GetSeenContentIdsByUserAsync(userId, allContentIds);
        var favoritedContentIds = await _unitOfWork.FavoriteContent.GetFavoritedContentIdsByUserAsync(userId, allContentIds);

        var dtos = lists.Select(list =>
        {
            var contentDtos = list.Contents.Select(content =>
            {
                bool isSeen = seenContentIds.Contains(content.Id);
                bool isFavorited = favoritedContentIds.Contains(content.Id);
                return new ContentDTO(content.Id,content.ContentType, isSeen, isFavorited);
            });

            return new ListSummaryDTO(list.Id, list.Name, list.Description, contentDtos);
        });

        var response = new ContentListEnumerableDTO(userId, dtos);
        return new Response<ContentListEnumerableDTO>(response, "Listas do usuário recuperadas com sucesso.", 200);
    }
    public async Task<Response<ListDetailsDTO>> GetListDetailsAsync(int listId)
    {
        var userId = _contextUser.Id;
        var contentList = await _unitOfWork.ContentsList.GetByIdAsync(listId);

        if (contentList.UserId != userId)
            return new Response<ListDetailsDTO>(null, "Lista não encontrada ou não pertence a este usuário.", 404);

        if (contentList is null) contentList = new ContentsList();
        else
        {
            var contentDtos = contentList.Contents.Select(c => new ContentSummaryDTO(c.Id, c.ContentType));
            var responseDto = new ListDetailsDTO(contentList.Id, contentList.Name, contentList.Description, contentDtos);
            return new Response<ListDetailsDTO>(responseDto, "Detalhes da lista recuperados com sucesso.", 200);
        }
            return new Response<ListDetailsDTO>(null,"Erro desconhecido. Por favor, crie uma lista novamente" ,404);
    }

    public async Task<Response<ContentFromListDTO>> GetContentFromList(int listId, int contentId)
    {
        var userId = _contextUser.Id;

        var list = await _unitOfWork.ContentsList.GetByIdAndUserIdAsync(listId, userId);
        var content = await _unitOfWork.Content.Find(contentId);

        if (list == null) return new Response<ContentFromListDTO>(null, "Lista não encontrada ou não pertence a este usuário.", 404);

        if (list.Contents.Any(i=>i.Id != contentId)) return new Response<ContentFromListDTO>(null, $"Esse conteúdo não está na lista {list.Name}.", 401);

        var seenContent = await _unitOfWork.AlreadySeenContent.IsAlreadySeen(userId, contentId);
        var favoritedContent = await _unitOfWork.FavoriteContent.IsFavorited(userId, contentId);        

        return new Response<ContentFromListDTO>(new ContentFromListDTO(userId,listId, new ContentDTO(contentId, content.ContentType, seenContent, favoritedContent)), "Conteúdo retornado com sucesso.", 200);
        
    }
}