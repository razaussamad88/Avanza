//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.Reflection;
using System.Collections.Generic;

namespace Avanza.Core.Module
{
    internal class ModuleInfo
    {
        private string _name;
        private List<AsmInfo> _lstAsmInfo;

        public ModuleInfo(string name, List<AsmInfo> lstAsmInfo)
        {
            this._name = name;
            _lstAsmInfo = lstAsmInfo;
            //Set parent modules for assembly information
            foreach (AsmInfo asmInfo in _lstAsmInfo)
                asmInfo.Module = this;
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public List<AsmInfo> AsmInfo
        {
            get
            {
                return this._lstAsmInfo;
            }
            set
            {
                this._lstAsmInfo = value;
            }
        }
    }
}