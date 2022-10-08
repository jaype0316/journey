using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Blobs
{
    public interface IBlobStorageService
    {
        Task<bool> Add(string key, IFormFile file);
        Task<(Stream,string)> Get(string key);
    }
}
