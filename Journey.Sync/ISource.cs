using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Sync
{
    public interface ISource
    {
        string Uri { get; set; }
    }

    public interface ITarget
    {

    }

    public interface ISyncManager
    {
        
    }
}
