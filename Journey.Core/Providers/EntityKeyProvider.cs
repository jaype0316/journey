using Journey.Core.Models;
using Journey.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Providers
{
    public interface IEntityKeyProvider
    {
        string Provide(UserContext context);
    }

    public class DefaultEntityKeyProvider : IEntityKeyProvider
    {
        public string Provide(UserContext context)
        {
            return $"{context.UserId}-{Guid.NewGuid().ToString()}";
        }
    }
}
