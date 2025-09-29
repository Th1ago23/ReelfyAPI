using Application.DTO.Content.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Users
{
    public record UserResponseLoginDTO(UserResponseDTO User, string token, PreferenceResponseDTO PreferenceResponseDTO)
    {
    }
}
