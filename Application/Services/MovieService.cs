using Domain.Interface.Services.IUser;
using Domain.Interface.Services.Movie;
using Domain.Models;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _context;
        private readonly IMovieMapper _mapper;
        private readonly IHttpContextAccessor _acessor;
        private readonly IUserRepository _userRepository;

        public MovieService(IMovieRepository context, IMovieMapper mapper, IHttpContextAccessor acessor, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _acessor = acessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<FavoriteMovieDTO> Favorite(FavoriteMovieDTO favoriteMovieDTO)
        {
            if (favoriteMovieDTO != null)
            {
                var user = await _userRepository.GetUserInContext();
                var movie = _mapper.ToEntity(favoriteMovieDTO);

                if (user.Movies == null)
                {
                    user.Movies = new List<FavoriteMovie>();
                }
                user.Movies.Add(movie);

                await _context.Add(movie, user);
                return favoriteMovieDTO;
            }
            else
            {
                throw new ArgumentException("O objeto de filme favorito não pode ser nulo.");
            }
        }

        public async Task RemoveFavorite(FavoriteMovieDTO favoriteMovieDTO)
        {
            var movie = _mapper.ToEntity(favoriteMovieDTO);

            _context.Delete(movie);
        }
    }
}
