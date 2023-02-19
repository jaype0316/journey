using Journey.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Tags
{
    public static class Defaults
    {
        public static IEnumerable<UserTag.Tag> Tags 
        {
            get 
            {
                return new List<UserTag.Tag>(3)
                {
                    new UserTag.Tag() { Id = Guid.NewGuid().ToString(), IsDefault = true, Name = "Perseverance" },
                    new UserTag.Tag() { Id = Guid.NewGuid().ToString(), IsDefault = true, Name = "Joyful" },
                    new UserTag.Tag() { Id = Guid.NewGuid().ToString(), IsDefault = true, Name = "Bitter" },
                };
            } 
        }
    }
}
