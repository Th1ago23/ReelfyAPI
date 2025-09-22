using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Content.Preferences
{
    public record PreferenceResponseDTO(int userId, int preferenceId, IEnumerable<CastAddDTO> casts, IEnumerable<CrewAddDTO> crews, IEnumerable<GenreAddDTO> genres, IEnumerable<StreamingAddDTO> Streamings)
    {
    }
}
