using Application.DTO.Content;
using Application.Interface.ContentInterface;
using Domain.Interface.HttpContext;
using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services;

public class ContentService : IContentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IContextUser _userInContext;
    private readonly IContentRepository _context;
    private readonly IMemoryCache _cache;
    private readonly IContentMapper _mapper;

    public ContentService(IMemoryCache cache, IContentMapper mapper, IUnitOfWork unit, IUserRepository userRepository, IContentRepository context, IContextUser contextUser)
    {
        _userInContext = contextUser;
        _cache = cache;
        _unitOfWork = unit;
        _mapper = mapper;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<FavoriteContentDTO> Favorite(FavoriteContentDTO favoritecontentDTO)
    {
        if (favoritecontentDTO is null) throw new ArgumentException("O objeto de filme favorito não pode ser nulo.");

        var user = await _userRepository.GetById(_userInContext.Id);

        var content = _mapper.ToEntity(favoritecontentDTO);

        if (user.Contents is null) user.Contents = new List<Content>();
        if (user.Contents.Contains(content)) throw new ArgumentException("Este livro já foi favoritado");

        user.Contents.Add(content);

        await _context.Add(content, user);
        await _unitOfWork.CommitAsync();

        _cache.Remove($"UserFavorites_{user.Id}");

        return favoritecontentDTO;

    }

    public async Task<bool> Unfavorite(int id)
    {
        var user = await _userRepository.GetById(_userInContext.Id);

        if (user is null) throw new UnauthorizedAccessException("Usuário sem permissão");

        var contentToRemove = user.Contents.FirstOrDefault(i => i.Id == id);

        if (contentToRemove != null) user.Contents.Remove(contentToRemove);

        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task<IEnumerable<FavoriteCountDTO>> CountContents()
    {
        var contents = await _context.FindAll();
        var contentsWithCount = new List<FavoriteCountDTO>();

        foreach (var content in contents)
        {
            var usersCount = content.User.Count();
            contentsWithCount.Add(new FavoriteCountDTO(content.Title, content.category, content.Id, usersCount));
        }
        return contentsWithCount;
    }

    public async Task<FavoriteContentDTO> MarkAlreadySeen(int id, bool result)
    {
        var content = await _context.Find(id);
        var user = await _userRepository.GetById(_userInContext.Id);

        if (!user.Contents.Any(i => i.Id == content.Id)) throw new UnauthorizedAccessException("Conteúdo não disponível");

        content.AlreadySeen = result;

        _context.Update(content);
        await _unitOfWork.CommitAsync();

        return new FavoriteContentDTO(content.Id, content.Title, content.category, content.ImageUrl, content.AlreadySeen);

    }
}
