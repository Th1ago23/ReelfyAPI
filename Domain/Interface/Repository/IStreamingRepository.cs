using Domain.Models.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository;

public interface IStreamingRepository
{
    public Task Add(Streaming streaming);
    public Task Delete(int id);
}
