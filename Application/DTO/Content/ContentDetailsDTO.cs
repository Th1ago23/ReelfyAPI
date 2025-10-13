using Application.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Content;

public record ContentDetailsDTO(int Id,string ContentType ,UserStatusDTO UserStatus);
