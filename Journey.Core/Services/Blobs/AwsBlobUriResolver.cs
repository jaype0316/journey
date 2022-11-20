using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Blobs
{
    public class AwsBlobUriResolver : IBlobUriResolver
    {
        public string Resolve(string bucketName, string region, string blobName)
        {
            return $"https://{bucketName}.s3.{region}.amazonaws.com/{blobName}";
        }
    }
}
