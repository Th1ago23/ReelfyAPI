using Application.DTO.Content;
using Application.Interface.ContentInterface;
using Domain.Interface.HttpContext;
using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class ContentService : IContentService
    {   private readonly IUnitOfWork _unitOfWork;
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

        public async Task<FavoriteContentDTO> Favorite(FavoriteContentDTO favoriteMovieDTO)
        {
            if (favoriteMovieDTO is null) throw new ArgumentException("O objeto de filme favorito não pode ser nulo.");

            var user = await _userRepository.GetById(_userInContext.Id);

            var movie = _mapper.ToEntity(favoriteMovieDTO);

            if (user.Contents is null) user.Contents = new List<Content>();
            if (user.Contents.Contains(movie)) throw new ArgumentException("Este livro já foi favoritado");

            user.Contents.Add(movie);

            await _context.Add(movie, user);

            _cache.Remove($"UserFavorites_{user.Id}");

            return favoriteMovieDTO;

        }

        public async Task<bool> RemoveFavorite(int id)
        {
            return await _userRepository.RemoveFavorite(id, _userInContext.Id);
        }


    }
}
