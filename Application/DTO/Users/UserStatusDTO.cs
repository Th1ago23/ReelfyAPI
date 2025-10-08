using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Users;

public record UserStatusDTO(bool IsFavorited, bool HasSeen);
