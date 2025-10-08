using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Content
{
    public record ListDetailsDTO(int Id, string Name, string? Description, IEnumerable<ContentSummaryDTO> Contents);

}
