using Domain.Interface.Services;
using Domain.Interface.Services.Movie;
using Domain.Models.Contents;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IContentRepository _context;
        private readonly IMemoryCache _cache;
        private readonly IMovieMapper _mapper;
        private readonly IUserRepository _userRepository;

        public MovieService(IMemoryCache cache, IContentRepository context, IMovieMapper mapper, IUserRepository userRepository)
        {
            _cache = cache;
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<FavoriteMovieDTO> Favorite(FavoriteMovieDTO favoriteMovieDTO)
        {
            if (favoriteMovieDTO is null) throw new ArgumentException("O objeto de filme favorito não pode ser nulo.");

            var user = await _userRepository.GetUserInContext();

            var movie = _mapper.ToEntity(favoriteMovieDTO);

            if (user.Movies is null) user.Movies = new List<Content>();
            if (user.Movies.Contains(movie)) throw new ArgumentException("Este livro já foi favoritado");

            user.Movies.Add(movie);

            await _context.Add(movie, user);

            _cache.Remove($"UserFavorites_{user.Id}");

            return favoriteMovieDTO;

        }

        public async Task<bool> RemoveFavorite(int id)
        {
            return await _userRepository.RemoveFavorite(id);
        }


    }
}
