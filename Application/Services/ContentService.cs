using Application.DTO.Content;
using Application.DTO.Users;
using Application.Interface.ContentInterface;
using Domain.Interface.HttpContext;
using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.Extensions.Caching.Memory;
using ReelfyAPI.Models;

namespace Application.Services;

public class ContentService : IContentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContextUser _userInContext;
    private readonly IMemoryCache _cache;

    public ContentService(IUnitOfWork unitOfWork, IContextUser userInContext, IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _userInContext = userInContext;
        _cache = cache;
    }

    public async Task<Response<ContentDTO>> FavoriteAsync(int contentId, string contentType)
    {
        var userId = _userInContext.Id;
        var user = await _unitOfWork.User.GetById(userId);
        if (user == null)
            return new Response<ContentDTO>(null, "Usuário não autenticado.", 401);

        var content = await _unitOfWork.Content.Find(contentId);

        if (content == null)
        {
            content = new Content { Id = contentId, ContentType = contentType };
            await _unitOfWork.Content.Add(content);
        }

        var alreadyFavorited = await _unitOfWork.FavoriteContent.AnyAsync(user.Id, content.Id);
        if (alreadyFavorited)
            return new Response<ContentDTO>(null, "Este conteúdo já foi favoritado.", 409);

        var newFavorite = new FavoriteContent { UserId = user.Id, ContentType = content.ContentType, ContentId = content.Id };
        await _unitOfWork.FavoriteContent.AddAsync(newFavorite);

        await _unitOfWork.CommitAsync();

        var response = new ContentDTO(newFavorite.ContentId, content.ContentType, await _unitOfWork.AlreadySeenContent.IsAlreadySeen(userId,newFavorite.ContentId), true);

        _cache.Remove($"UserFavorites_{user.Id}");
        return new Response<ContentDTO>(response, "Conteúdo favoritado com sucesso!", 201);
    }

    public async Task<Response<bool>> UnfavoriteAsync(int contentId)
    {
        var userId = _userInContext.Id;
        var favorite = await _unitOfWork.FavoriteContent.GetByUserAndContentAsync(userId, contentId);

        if (favorite == null)
            return new Response<bool>(false, "Conteúdo não encontrado na sua lista de favoritos.", 404);

        _unitOfWork.FavoriteContent.Delete(favorite);
        await _unitOfWork.CommitAsync();

        _cache.Remove($"UserFavorites_{userId}");
        return new Response<bool>(true, "Conteúdo desfavoritado com sucesso!", 200);
    }

    public async Task<Response<bool>> SetSeenStatusAsync(int contentId, bool hasSeen)
    {
        var userId = _userInContext.Id;
        if (await _unitOfWork.User.GetById(userId) == null)
            return new Response<bool>(false, "Usuário não autenticado.", 401);

        var content = await _unitOfWork.Content.Find(contentId);
        if (content == null)
        {
            await _unitOfWork.Content.Add(new Content
                {
                    Id = contentId
                });
            await _unitOfWork.CommitAsync();
        }

        var seenRecord = await _unitOfWork.AlreadySeenContent.GetByUserAndContentAsync(userId, contentId);

        if (hasSeen && seenRecord == null)
        {
            var newSeenRecord = new AlreadySeenContent { UserId = userId, ContentId = contentId };
            await _unitOfWork.AlreadySeenContent.AddAsync(newSeenRecord);
        }
        else if (!hasSeen && seenRecord != null)
        {
            _unitOfWork.AlreadySeenContent.Delete(seenRecord);
        }

        await _unitOfWork.CommitAsync();
        _cache.Remove($"UserSeenContents_{userId}");
        return new Response<bool>(true, "Status de 'visto' atualizado com sucesso!", 200);
    }

    public async Task<Response<ContentDetailsDTO>> GetContentDetailsAsync(int contentId)
    {
        var content = await _unitOfWork.Content.Find(contentId);
        if (content == null)
            return new Response<ContentDetailsDTO>(null, "Conteúdo não encontrado.", 404);

        var userId = _userInContext.Id;
        bool isFavorited = await _unitOfWork.FavoriteContent.AnyAsync(userId, contentId);
        bool hasSeen = await _unitOfWork.AlreadySeenContent.AnyAsync(userId, contentId);

        var userStatus = new UserStatusDTO(isFavorited, hasSeen);
        var dto = new ContentDetailsDTO(content.Id, content.ContentType, userStatus);

        return new Response<ContentDetailsDTO>(dto, "Detalhes do conteúdo recuperados com sucesso.", 200);
    }

    public async Task<Response<ContentsHomeDTO>> GetFavoritesInContext()
    {
        var userId = _userInContext.Id;
        var user = await _unitOfWork.User.GetById(userId);
        if (user == null)
        {
            return new Response<ContentsHomeDTO>(null, "Usuário sem permissão. Por favor, faça login novamente.", 401);
        }
        var favoriteContents = await _unitOfWork.FavoriteContent.GetFavoritesByUserAsync(userId);

        if (!favoriteContents.Any())
        {
            var emptyResponse = new ContentsHomeDTO(userId, user.Email, new List<ContentDTO>());
            return new Response<ContentsHomeDTO>(emptyResponse, "Nenhum favorito encontrado.", 200);
        }

        var favoriteContentIds = favoriteContents.Select(c => c.Id).ToList();

        var seenContentIds = await _unitOfWork.AlreadySeenContent.GetSeenContentIdsByUserAsync(userId, favoriteContentIds);

        var contentList = favoriteContents.Select(content =>
        {
            bool isSeen = seenContentIds.Contains(content.Id);
            bool isFavorited = true;

            return new ContentDTO(content.Id, content.ContentType, isSeen, isFavorited);
        });

        var response = new ContentsHomeDTO(userId, user.Email, contentList);

        return new Response<ContentsHomeDTO>(response, "Favoritos recuperados com sucesso.", 200);
    }

    public async Task<Response<IEnumerable<ContentSummaryDTO>>> GetSeenAsync()
    {
        var userId = _userInContext.Id;
        var contents = await _unitOfWork.AlreadySeenContent.GetSeenByUserAsync(userId);

        var dtos = contents.Select(c => new ContentSummaryDTO(c.Id, c.ContentType));

        return new Response<IEnumerable<ContentSummaryDTO>>(dtos, "Conteúdos vistos recuperados com sucesso.", 200);
    }
}