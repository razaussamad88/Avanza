using Avanza.Core.Logging;
using RdvSMPassManClient.RdvSMPasswordManagerSvc;
using System;

namespace Avanza.Core.AvanzaKeyStore
{
    public class RdvSmPasswordManagerWrapper
    {
        #region Members
        private static Logger logger = LogManager.GetLogger(typeof(RdvSmPasswordManagerWrapper).FullName);
        private static RdvSmPasswordManagerWrapper _Singleton;
        private static RdvSMPasswordManagerClient _RdvSmPassMgrClient;
        #endregion

        #region Properties
        public static RdvSmPasswordManagerWrapper Instance
        {
            get
            {
                if (_Singleton == null)
                {
                    throw new Exception("Object not created");
                }

                return _Singleton;
            }
        }
        #endregion

        #region Constructor
        public RdvSmPasswordManagerWrapper() { }
        #endregion

        public static void Create()
        {
            if (_Singleton == null)
            {
                _Singleton = new RdvSmPasswordManagerWrapper();
            }
        }

        private static RdvSMPasswordManagerClient RdvSMPasswordManagerObj
        {
            get
            {
                if (_RdvSmPassMgrClient == null)
                {
                    try
                    {
                        _RdvSmPassMgrClient = new RdvSMPasswordManagerClient();
                    }
                    catch (Exception ex)
                    {
                        logger.LogInfo("ProxyInstance: " + ex.Message);
                    }
                }

                logger.LogInfo("_RdvSmPassMgrClient is " + ((_RdvSmPassMgrClient == null) ? "Null" : "Created"));

                return _RdvSmPassMgrClient;
            }
        }

        #region RdvSmPasswordManager Methods
        public string ChangeConnString(object sUserID, object sPassword)
        {
            logger.LogInfo("Calling ChangeConnString");
            var result = RdvSMPasswordManagerObj.Change_RdvSM_DSN(sUserID.ToString(), sPassword.ToString());

            return (string)result;
        }

        public string Change_RdvSM_RdvDSN(object sUserID, object sPassword)
        {
            logger.LogInfo("Calling Change_RdvSM_RdvDSN");
            var result = RdvSMPasswordManagerObj.Change_RdvSM_RdvDSN(sUserID.ToString(), sPassword.ToString());

            return (string)result;
        }

        public string Change_RdvSM_VisionDSN(object sUserID, object sPassword)
        {
            logger.LogInfo("Calling Change_RdvSM_VisionDSN");

            var result = RdvSMPasswordManagerObj.Change_RdvSM_VisionDSN(sUserID.ToString(), sPassword.ToString());

            return (string)result;
        }

        public string ResetDEKnKEK()
        {
            logger.LogInfo("Calling ResetDEKnKEK");

            var result = RdvSMPasswordManagerObj.Change_RDVSM_KEK();

            return (string)result;
        }
        #endregion
    }
}
