//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Avanza.Core.Configuration;

namespace Avanza.Core.Module
{
    public class ModuleFactory<Mod>
    {
        private const string xmlName = "name";
        private const string xmlType = "type";
        private const string xmlAssembly = "assembly";

        private  System.Collections.Generic.Dictionary<string, Assembly> _assemblyRefs;
        private  System.Collections.Generic.Dictionary<string, ModuleInfo> _moduleRefs;

        public ModuleFactory(int modRefCount, int asmRefCount)
        {
            this._assemblyRefs = new Dictionary<string, Assembly>(asmRefCount);
            this._moduleRefs = new Dictionary<string, ModuleInfo>(modRefCount);
        }

        public ModuleFactory() : this(10,10)
        {}

        public ModuleFactory(int modRefCount) : this(modRefCount, 10)
        {}

        public void Load(IConfigSection factoryConf, string sectionName)
        {
            IConfigSection[] moduleList = factoryConf.GetChildSections(sectionName);

            for (int Cnt = 0; Cnt < moduleList.Length; Cnt++)
            {
                string name = moduleList[Cnt].GetTextValue(ModuleFactory<Mod>.xmlName).ToUpperInvariant();
                
                //string impAssembly = moduleList[Cnt].GetTextValue(ModuleFactory<Mod>.xmlAssembly);
                List<AsmInfo> lstImpAssembly = new List<AsmInfo>();
                IConfigSection[] asmList = moduleList[Cnt].GetChildSections(ModuleFactory<Mod>.xmlAssembly);
                foreach (IConfigSection asmSection in asmList)
                {
                    string impAssembly = asmSection.GetTextValue(ModuleFactory<Mod>.xmlName);
                    string impClass = asmSection.GetTextValue(ModuleFactory<Mod>.xmlType);
                    lstImpAssembly.Add(GetAsmInfo(impClass, impAssembly));
                }

                this.AddModule(new ModuleInfo(name, lstImpAssembly));
            }
        }

        /*public Mod GetModule(string name)
        {
            Mod retVal = default(Mod);
            ModuleInfo tempInfo = null;

            name = name.ToUpperInvariant();
            if( this._moduleRefs.TryGetValue(name, out tempInfo) )
            {
                retVal = tempInfo.GetModule<Mod>();
            }

           return retVal;
        }*/

        public List<AsmInfo> GetAssemblyInfoList(string name)
        {            
            ModuleInfo tempInfo = null;

            name = name.ToUpperInvariant();
            if (this._moduleRefs.TryGetValue(name, out tempInfo))
            {
                return tempInfo.AsmInfo;
            }

            return null;
        }

        private void AddModule ( ModuleInfo modInfo )
        {
            string srcKey = modInfo.Name.ToUpperInvariant();   
            
            ModuleInfo tempInfo = null;
            if (!this._moduleRefs.TryGetValue(srcKey, out tempInfo))
                this._moduleRefs.Add(srcKey, modInfo);
        }

        private AsmInfo GetAsmInfo(string impClass, string impAssembly)
        {
            Assembly tempImp = this.GetAssembly(impAssembly);
            AsmInfo tempInfo = null;

            try
            {               
                tempInfo = new AsmInfo( impClass, tempImp);
                // instantiate the object of the class, to test that whether the implementation exist in the assembly
                tempInfo.GetModule<Mod>(); 
            }
            catch(Exception e)
            {
                throw new ModuleFactoryException(e, string.Format("Failed to load Module {0}. Assembly name: {2}"),
                                                 impClass, impAssembly);
            }
            
            return tempInfo;
        }

        private Assembly GetAssembly(string assemblyName)
        {
            string srcKey = assemblyName.ToUpperInvariant();
            Assembly retVal = null;

            if( !this._assemblyRefs.TryGetValue(srcKey, out retVal) )
            {
                try
                {
                    assemblyName = this.GetCompleteUrl(assemblyName);

                    FileInfo file = new FileInfo(assemblyName);

                    retVal = Assembly.LoadFrom(file.FullName);
                    this._assemblyRefs.Add(srcKey, retVal);
                }
                catch (Exception e)
                {
                    throw new ModuleFactoryException(e, string.Format("Failed to load {0} assembly", assemblyName));
                }
            }
            return retVal;
        }

        private string GetCompleteUrl(string fileName)
        {
            string tmpFilePath = null;
            if (Path.IsPathRooted(fileName) == false)
                if (File.Exists(tmpFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                    return tmpFilePath;
                else
                    return Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, fileName);
            return fileName;
        }
    }
}