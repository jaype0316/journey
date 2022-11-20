using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Blobs
{
    public interface IBlobUriResolver
    {
        string Resolve(string bucketName, string region, string blobName);
    }
}
