using Domain.Models.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface IGenreRepository
    {
        public Task Add(Genre genre);
        public Task Delete(int id);
    }
}
