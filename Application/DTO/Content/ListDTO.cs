using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Content
{
    public record ListDTO(int listId, string name, string description, IEnumerable<FavoriteContentDTO> contents)
    {
    }
}
