using Journey.Core.Query;
using Journey.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Repository
{
    public interface IRepository
    {
        Task<T> GetAsync<T>(string id, string userId) where T : IDTO, IEntity;
        Task<bool> CreateAsync<T>(T entity) where T : IDTO, IEntity;
        Task<bool> UpdateAsync<T>(T entity) where T : IDTO, IEntity;
        Task<bool> DeleteAsync<T>(string id, string userId) where T : IDTO, IEntity;
    }

    public interface IReadRepository
    {
        Task<T> GetAsync<T>(string id);
    }

    public interface IIndexedRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(QueryOption query) where T : IIndexedEntity;
    }
}
