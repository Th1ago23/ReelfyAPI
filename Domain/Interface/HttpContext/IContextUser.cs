using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.HttpContext
{
    public interface IContextUser
    {
        public int Id { get; }
        public string? Email { get; }
    }
}
