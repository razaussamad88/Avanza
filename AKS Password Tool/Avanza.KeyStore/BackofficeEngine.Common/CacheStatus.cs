using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanza.Core.Caching
{
    public enum CacheStatus
    {
        INITIALIZING,
        NOT_INITIALIZED,
        WORKING,
        NOT_WORKING,
        BUSY
    }
}
