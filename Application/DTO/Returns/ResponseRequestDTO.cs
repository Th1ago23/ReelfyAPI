using Application.DTO.Users;

namespace Application.DTO.Returns;

public record ResponseRequestDTO(UserResponseDTO userResponseDTO, string token)
{
}
