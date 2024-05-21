
namespace Avanza.Core.AvanzaKeyStore
{
    /* ----- Operation Code -----
     * DECRYPT_DEK			:	1
     * ENCRYPT_DEK			:	2
     * GENERATE_KEK_DEK     :	3
     * RESET_DEK			:	4
     */
    public enum KeyStoreOpCode { DECRYPT_DEK = 1, ENCRYPT_DEK, GENERATE_KEK_DEK, RESET_DEK };

    public enum PasswordManagerOpCode { CHANGE_CON_STR = 1, CHANGE_SWITCH_CONFIG_CONN_STR, RESET_KEK_DEK };



    /*
     * Rendezvous	:	1
     * Nimbus		:	2
     * RDVSM        :	3
     * Vision		:	4
     */
    public enum ProductIndex { Rendezvous = 1, Nimbus, RDVSM, Vision };



    public class WrapperHelper
    {
        //public static string KEYSTORE_MESSAGE_FORMAT = "{0},{1},{2}";
        //public static string PRODUCT_MESSAGE_FORMAT = "{0},{1}";
        public static string FIELD_SEPERATOR = "|";
        public static string KEYSTORE_FIELD_SEPERATOR = ",";
        public static string KEYSTORE_MESSAGE_FORMAT = "{0}" + KEYSTORE_FIELD_SEPERATOR + "{1}" + KEYSTORE_FIELD_SEPERATOR + "{2}";
        public static string PRODUCT_MESSAGE_FORMAT = "{0}" + FIELD_SEPERATOR + "{1}";

        public static string GetString(KeyStoreOpCode opCode)
        {
            return ((int)opCode).ToString();
        }

        public static string GetString(PasswordManagerOpCode opCode)
        {
            return ((int)opCode).ToString();
        }
    }
}
