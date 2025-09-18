using Domain.Models.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository;

public interface ICrewRepository
{
    public Task Add(Crew crew);
    public Task Delete(int id);
}
