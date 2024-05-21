//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;

namespace Avanza.Core.Module
{
    public class ModuleFactoryException : System.Exception
    {
        public ModuleFactoryException(string message)
            : base(message)
        { }

        public ModuleFactoryException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        public ModuleFactoryException(Exception innerExcep, string message)
            : base(message, innerExcep)
        { }

        public ModuleFactoryException(Exception innerExcep, string format, params object[] args)
            : base(string.Format(format, args), innerExcep)
        { }
    }
}
