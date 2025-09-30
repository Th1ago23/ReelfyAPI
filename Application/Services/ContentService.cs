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
    private readonly IContentsListRepository _contentsListRepository;
    private readonly IContentRepository _context;
    private readonly IMemoryCache _cache;
    private readonly IContentMapper _mapper;

    public ContentService(IContentsListRepository contentsListRepository, IMemoryCache cache, IContentMapper mapper, IUnitOfWork unit, IUserRepository userRepository, IContentRepository context, IContextUser contextUser)
    {
        _contentsListRepository = contentsListRepository;
        _userInContext = contextUser;
        _cache = cache;
        _unitOfWork = unit;
        _mapper = mapper;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<Response<FavoriteContentDTO>> Favorite(FavoriteContentDTO favoriteContentDTO)
    {
        if (favoriteContentDTO is null)
        {
            return new Response<FavoriteContentDTO>(null, "O objeto de conteúdo favorito não pode ser nulo.", 400);
        }

        var user = await _userRepository.GetById(_userInContext.Id);

        var content = _mapper.ToEntity(favoriteContentDTO);

        if (user.FavoriteContents is null) user.FavoriteContents = new List<Content>();

        if (user.FavoriteContents.Any(c => c.Id == content.Id))
        {
            return new Response<FavoriteContentDTO>(null, "Este conteúdo já foi favoritado.", 409);
        }

        user.FavoriteContents.Add(content);

        await _context.Add(content, user);
        await _unitOfWork.CommitAsync();

        _cache.Remove($"UserFavorites_{user.Id}");

        return new Response<FavoriteContentDTO>(favoriteContentDTO, "Conteúdo favoritado com sucesso!", 200);
    }

    public async Task<Response<bool>> Unfavorite(int id)
    {
        var user = await _userRepository.GetById(_userInContext.Id);

        if (user is null)
        {
            return new Response<bool>(false, "Usuário não autenticado. Faça Login novamente.", 404);
        }

        var contentToRemove = user.FavoriteContents.FirstOrDefault(i => i.Id == id);

        if (contentToRemove is null)
        {
            return new Response<bool>(false, "Conteúdo não encontrado na sua lista de favoritos.", 404);
        }

        user.FavoriteContents.Remove(contentToRemove);

        await _unitOfWork.CommitAsync();

        _cache.Remove($"UserFavorites_{user.Id}");

        return new Response<bool>(true, "Conteúdo desfavoritado com sucesso!", 200);
    }

    public async Task<IEnumerable<FavoriteCountDTO>> CountContents()
    {
        var contents = await _context.FindAll();
        var contentsWithCount = new List<FavoriteCountDTO>();

        foreach (var content in contents)
        {
            var usersCount = content.FavoritedByUsers?.Count() ?? 0;
            contentsWithCount.Add(new FavoriteCountDTO(content.Title, content.category, content.Id, usersCount));
        }
        return contentsWithCount;
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
            return new Response<FavoriteContentDTO>(null, "O conteúdo não está na sua lista de favoritos.", 404);
        }

        content.AlreadySeen = result;

        _context.Update(content);
        await _unitOfWork.CommitAsync();

        var updatedContentDTO = new FavoriteContentDTO(content.Id, content.Title, content.category, content.ImageUrl, content.AlreadySeen);

        return new Response<FavoriteContentDTO>(updatedContentDTO, "Conteúdo atualizado com sucesso!", 200);
    }

}
