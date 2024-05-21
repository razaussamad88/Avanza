//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.Reflection;

namespace Avanza.Core.Module
{
    public class AsmInfo
    {
        private string _impClass;
        private System.Reflection.Assembly _impAssembly;
        private ModuleInfo _module;

        public Assembly ImpAssembly
        {
            get
            {
                return this._impAssembly;
            }
            set
            {
                this._impAssembly = value;
            }
        }       

        public string ImpClass
        {
            get
            {
                return this._impClass;
            }
            set
            {
                this._impClass = value;
            }
        }

        internal ModuleInfo Module
        {
            get
            {
                return _module;
            }
            set
            {
                _module = value;
            }
        }

        public AsmInfo(string impClass, Assembly impAssembly)
        {
            _impClass = impClass;
            _impAssembly = impAssembly;
        }

        public Mod GetModule<Mod>()
        {
            Mod retVal;

            try
            {
                retVal = (Mod)this._impAssembly.CreateInstance(this._impClass);
            }
            catch (Exception e)
            {
                throw new ModuleFactoryException(e, string.Format("Failed to instantiate module {0}. Name: {1}; Assembly name: {2}"),
                                                 this._impClass, _module.Name, this._impAssembly);
            }

            if (retVal == null)
                throw new ModuleFactoryException(string.Format("Failed to instantiate module {0}. Name: {1}; Assembly name: {2}"),
                                                 this._impClass, _module.Name, this._impAssembly);

            return retVal;
        }
    }
}
