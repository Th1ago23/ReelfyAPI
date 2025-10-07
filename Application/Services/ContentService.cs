using Application.DTO.Content;
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
    private readonly IUserRepository _userRepository;
    private readonly IContextUser _userInContext;
    private readonly IContentRepository _context;
    private readonly IFavoriteContentRepository _favoriteContentRepository;
    private readonly IMemoryCache _cache;
    private readonly IContentMapper _mapper;

    public ContentService(IUnitOfWork unitOfWork, IUserRepository userRepository, IContextUser userInContext, IContentRepository context, IFavoriteContentRepository favoriteContentRepository, IMemoryCache cache, IContentMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userInContext = userInContext;
        _context = context;
        _favoriteContentRepository = favoriteContentRepository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<Response<FavoriteContentDTO>> Favorite(FavoriteContentDTO favoriteContentDTO)
    {
        if (favoriteContentDTO is null)
        {
            return new Response<FavoriteContentDTO>(null, "O conteúdo favorito não pode ser nulo.", 400);
        }

        var user = await _userRepository.GetById(_userInContext.Id);
        if (user == null)
            return new Response<FavoriteContentDTO>(null, "Usuário sem permissão. Por favor, faça login novamente.", 401);

        var content = await _context.Find(favoriteContentDTO.id);

        if (content == null)
        {
            content = _mapper.ToEntity(favoriteContentDTO);
            await _context.Add(content);
            await _unitOfWork.CommitAsync();
        }

        var favorite = await _favoriteContentRepository.GetByUserAndContentAsync(user.Id, content.Id);
        if (favorite != null) return new Response<FavoriteContentDTO>(null, "Este conteúdo já foi favoritado.", 409);

        favorite = new FavoriteContent
        {
            UserId = user.Id,
            ContentId = content.Id,
            AlreadySeen = false
        };

        await _favoriteContentRepository.Add(favorite);
        await _unitOfWork.CommitAsync();

        _cache.Remove($"UserFavorites_{user.Id}");

        return new Response<FavoriteContentDTO>(favoriteContentDTO, "Conteúdo favoritado com sucesso!", 200);
    }

    public async Task<Response<bool>> Unfavorite(int contentId)
    {
        var user = await _userRepository.GetById(_userInContext.Id);

        if (user == null)
            return new Response<bool>(false, "Usuário não autenticado. Faça Login novamente.", 404);

        var favorite = await _favoriteContentRepository.GetByUserAndContentAsync(user.Id, contentId);

        if (favorite == null)
            return new Response<bool>(false, "Conteúdo não encontrado na sua lista de favoritos.", 404);

        await _favoriteContentRepository.Delete(favorite.Id);
        await _unitOfWork.CommitAsync();

        _cache.Remove($"UserFavorites_{user.Id}");

        return new Response<bool>(true, "Conteúdo desfavoritado com sucesso!", 200);
    }

    public async Task<Response<IEnumerable<FavoriteCountDTO>>> CountContents()
    {
        var favorites = await _favoriteContentRepository.GetAllAsync();

        var counts = favorites
            .GroupBy(fc => fc.ContentId)
            .Select(g => new FavoriteCountDTO(g.Key, g.Count()))
            .OrderByDescending(x => x.usersCount)
            .ToList();

        return new Response<IEnumerable<FavoriteCountDTO>>(counts, "Requisição processada com sucesso", 200);
    }


    public async Task<Response<FavoriteContentDTO>> MarkAlreadySeen(int id, bool result)
    {
        var user = await _userRepository.GetById(_userInContext.Id);

        if (user is null)
        {
            return new Response<FavoriteContentDTO>(null, "Usuário não autenticado. Faça Login novamente.", 404);
        }

        var content = user.FavoriteContents.FirstOrDefault(c => c.Id == id);

        if (content is null)
        {
            await _context.Add(content);
        }

        content.AlreadySeen = result;

        _context.Update(content);
        await _unitOfWork.CommitAsync();

        var updatedContentDTO = new FavoriteContentDTO(content.Id, content.AlreadySeen);

        return new Response<FavoriteContentDTO>(updatedContentDTO, "Conteúdo atualizado com sucesso!", 200);
    }

}
