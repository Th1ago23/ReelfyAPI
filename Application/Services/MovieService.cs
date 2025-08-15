using Domain.Interface.Services.IUser;
using Domain.Interface.Services.Movie;
using Domain.Models;
using Domain.Models.DTO;

namespace Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _context;
        private readonly IMovieMapper _mapper;
        private readonly IUserRepository _userRepository;

        public MovieService(IMovieRepository context, IMovieMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<FavoriteMovieDTO> Favorite(FavoriteMovieDTO favoriteMovieDTO)
        {
            if (favoriteMovieDTO is null) throw new ArgumentException("O objeto de filme favorito não pode ser nulo.");

            var user = await _userRepository.GetUserInContext();
            
            var movie = _mapper.ToEntity(favoriteMovieDTO);
            
            if (user.Movies is null) user.Movies = new List<FavoriteMovie>();
            if (user.Movies.Contains(movie)) throw new ArgumentException("Este livro já foi favoritado");

            user.Movies.Add(movie);

            await _context.Add(movie, user);
            return favoriteMovieDTO;
            
        }

        public async Task<bool> RemoveFavorite(int id)
        {
            return await _userRepository.RemoveFavorite(id);
        }

        
    }
}
