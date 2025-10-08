using Application.DTO.Content;
using Application.DTO.Content.Preferences;
using Application.DTO.Users;
using Application.Interface.Mappers;
using Application.Interface.UserInterface;
using Domain.Models.Users;

namespace ReelfyAPI.Utils // Ou onde quer que sua implementação fique
{
    public class UserMapper : IUserMapper
    {
        // De DTO de Requisição para uma nova Entidade User
        public User ToUser(UserRegisterDTO dto)
        {
            return new User
            {
                Name = dto.Name,
                Birthday = dto.Birthday,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };
        }

        // De uma Entidade User para um DTO de resposta simples
        public UserResponseDTO ToUserResponseDTO(User user)
        {
            return new UserResponseDTO(user.Id, user.Name, user.Email, user.CreatedAt);
        }

        // De uma Entidade User para o DTO de resumo completo
        public UserSummaryDTO ToSummaryDTO(User user)
        {
            var castsDTO = user.Preference.Casts.Select(c => new CastAddDTO(c.Id, c.Name, c.ProfilePath));
            var crewsDTO = user.Preference.Crews.Select(c => new CrewAddDTO(c.Id, c.Name, c.ProfilePath));
            var genresDTO = user.Preference.Genres.Select(g => new GenreAddDTO(g.Id, g.Name));
            var streamingsDTO = user.Preference.Streamings.Select(s => new StreamingAddDTO(s.Id, s.Name));

            var preference = new PreferenceResponseDTO(
                user.Id,
                user.Preference.Id,
                castsDTO,
                crewsDTO,
                genresDTO,
                streamingsDTO
            );

            var favorites = user.FavoriteContents
                .Select(fc => new ContentSummaryDTO(fc.Content.Id, fc.Content.Title, fc.Content.ImageUrl));

            // LÓGICA ADICIONADA PARA OS CONTEÚDOS VISTOS
            var seenContents = user.AlreadySeenContents
                .Select(asc => new ContentSummaryDTO(asc.Content.Id, asc.Content.Title, asc.Content.ImageUrl));

            return new UserSummaryDTO(
                user.Id,
                user.Name,
                user.GetAge(),
                user.PhoneNumber,
                preference,
                favorites,
                seenContents, // <-- NOVA PROPRIEDADE PASSADA AQUI
                user.IsPreemium
            );
        }

        // Método para ATUALIZAR uma entidade existente com dados de um DTO
        public void UpdateEntity(User user, UpdateUserDTO dto)
        {
            user.Name = dto.Name ?? user.Name;
            user.Email = dto.Email ?? user.Email;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
        }
    }
}