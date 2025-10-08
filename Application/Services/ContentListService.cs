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

    // Injeções foram limpas para seguir o padrão da Unit of Work
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

        var dtos = contentList.Contents.Select(c => new ContentSummaryDTO(c.Id, c.Title, c.ImageUrl));
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

        var dtos = contentList.Contents.Select(c => new ContentSummaryDTO(c.Id, c.Title, c.ImageUrl));
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

    public async Task<Response<IEnumerable<ListSummaryDTO>>> GetListsAsync()
    {
        var userId = _contextUser.Id;
        var lists = await _unitOfWork.ContentsList.GetListsByUserAsync(userId);

        var dtos = lists.Select(l => new ListSummaryDTO(l.Id, l.Name, l.Contents.Count));

        return new Response<IEnumerable<ListSummaryDTO>>(dtos, "Listas do usuário recuperadas com sucesso.", 200);
    }

    public async Task<Response<ListDetailsDTO>> GetListDetailsAsync(int listId)
    {
        var userId = _contextUser.Id;
        var contentList = await _unitOfWork.ContentsList.GetByIdAsync(listId);

        if (contentList == null || contentList.UserId != userId)
            return new Response<ListDetailsDTO>(null, "Lista não encontrada ou não pertence a este usuário.", 404);

        var contentDtos = contentList.Contents.Select(c => new ContentSummaryDTO(c.Id, c.Title, c.ImageUrl));
        var responseDto = new ListDetailsDTO(contentList.Id, contentList.Name, contentList.Description, contentDtos);

        return new Response<ListDetailsDTO>(responseDto, "Detalhes da lista recuperados com sucesso.", 200);
    }
}