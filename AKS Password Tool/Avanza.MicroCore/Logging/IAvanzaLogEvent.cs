using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Avanza.Common.Logging
{
    public enum LogType
    {
        System = 2,
        Activity = 3
    }

    public enum ActionType
    {
        View = 1,
        Add = 2,
        Edit = 3,
        Delete = 4,
        Undefined = 5,
        RdvServiceCall=6,
        CoreFunction = 7,
        NimbusServiceCall = 8,
        Processing = 9
    }
}
