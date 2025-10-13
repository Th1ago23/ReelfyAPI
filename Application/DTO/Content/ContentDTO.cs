using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Content
{
    public record ContentDTO(int ContentId, string ContentType, bool IsAlreadySeen, bool IsFavorited);
}
