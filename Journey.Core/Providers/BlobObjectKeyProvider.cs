using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Providers
{
    public interface IBlobKeyProvider
    {
        string Provide(string key);
    }
    public class BlobObjectKeyProvider : IBlobKeyProvider
    {
        public string Provide(string key) => $"{DateTime.UtcNow:yyyy\\/\\MM\\/dd/hhmmss}-{key}";
    }
}
