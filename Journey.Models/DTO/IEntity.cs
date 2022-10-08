using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Models.DTO
{
    public interface IEntity
    {
        string ItemName { get; }
    }

    public interface IIndexedEntity : IEntity
    {
        string IndexName { get; }
    }
}
