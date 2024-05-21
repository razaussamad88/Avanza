
using System;

namespace ThreadPoolConsoleApp
{
    public class clsMachineInterfaceParam
    {
        private string m_ExpiryDate_MMyy = null;
        private string m_ExpiryDate = null;
        private static Random rand = new Random();

        public string ExpiryDate_MMyy
        {
            get
            {
                if (m_ExpiryDate_MMyy == null)
                {
                    m_ExpiryDate_MMyy = DateTime.Now.AddDays(rand.Next(100, 1800)).ToString("MMyy");
                }

                return m_ExpiryDate_MMyy;
            }
        }

        public string ExpiryDate
        {
            get
            {
                if (m_ExpiryDate == null)
                {
                    m_ExpiryDate = DateTime.Now.AddDays(rand.Next(100, 1800)).ToString("yyMM");
                }

                return m_ExpiryDate;
            }
        }

        public DebitCard Card;
        public bool IsEMVCard;
        public ModHelper.FinancialService PaymentTechnology;
    }

    public static partial class ModHelper
    {
        public enum FinancialService { Master, Visa, PayPak };

        public static void GenerateHSM_Bulk(clsMachineInterfaceParam queryObj, ref DebitCard card, ref CustomerChannelAuthen custPin)
        {
            bool isHsmOpen = false;
            BulkHSM bulkHSM = new BulkHSM();

            Logger.WriteLine("\t\t\tGenerateHSM_Bulk Method Begins");

            try
            {
                //Guard.CheckNull(DeviceFactory.PinGenerator, "HSM is not properly initialized");
                Logger.WriteLine("\t\t\tOpening HSM port ...");
                //DeviceFactory.PinGenerator.Open();
                isHsmOpen = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("\t\t\tUnable to open HSM port", ex);
                throw new InvalidOperationException("Unable to open HSM port");
            }



            try
            {
                card.CVV = bulkHSM.calculateCVV1(queryObj.Card.Pan, queryObj.ExpiryDate, queryObj.IsEMVCard);
                card.CVV2 = bulkHSM.calculateCVV2(queryObj.Card.Pan, queryObj.ExpiryDate_MMyy);
                card.ICVV = bulkHSM.calculateICVV(queryObj.Card.Pan, queryObj.ExpiryDate);

                custPin.PinCode = bulkHSM.calculatePin(queryObj.Card.Pan);
                custPin.PVV = bulkHSM.calculatePVV(queryObj.Card.Pan, custPin.PinCode);
            }
            catch (Exception ex)
            {
                Logger.WriteLine("\t\t\tError occurred while executing HSM query", ex);
            }
            finally
            {
                Logger.WriteLine("\t\t\tGenerateHSM_Bulk Method Ends");
            }
        }
    }



    public class BulkHSM
    {
        private enum CvvType { CVV1, CVV2, ICVV };

        public string calculateCVV1(string pan, string expiryDate, bool isEMVCard)
        {
            string serviceCode = "";

            return calculateCVV(pan, expiryDate, serviceCode, CvvType.CVV1);
        }

        public string calculateCVV2(string pan, string expiryDate)
        {
            string serviceCode = "000";
            return calculateCVV(pan, expiryDate, serviceCode, CvvType.CVV2);
        }

        public string calculateICVV(string pan, string expiryDate)
        {
            string serviceCode = "999";
            return calculateCVV(pan, expiryDate, serviceCode, CvvType.ICVV);
        }

        private string calculateCVV(string pan, string expiryDate, string serviceCode, CvvType cvvType)
        {
            string cvv;


            try
            {
                Logger.WriteLine("\t\t\tCalculating {0} for Debit card...", cvvType);
                Logger.WriteLine("\t\t\tGenerating {0} debit card: \"{1}\" ", cvvType, pan);

                cvv = this.getCode();

                Logger.WriteLine("\t\t\t{0} generated for card \"{1}\"", cvvType, pan);

                return cvv;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("Error occurred while generating {0}", cvvType), ex);
                throw new Exception(String.Format("Unable to Generate {0} - Check HSM connection and configuration settings.", cvvType));
            }
        }

        public string calculatePVV(string pan, string offset)
        {
            string pvv;

            try
            {
                Logger.WriteLine("\t\t\tCalculating PVV for Debit card...");
                Logger.WriteLine("\t\t\tGenerating PVV debit card: \"{0}\" ", pan);
                //pvv = DeviceFactory.PinGenerator.GeneratePVV(pvk1, pvk2, pan.Substring(pan.Length - 13, 12), offset, ModHelper.PVKI);
                pvv = this.getCode();
                Logger.WriteLine("\t\t\tPVV generated for card \"{0}\"", pan);
                return pvv;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("\t\t\tError occurred while generating PVV", ex);
                throw new Exception("Unable to Generate PVV - Check HSM connection and configuration settings.");
            }
        }

        public string calculatePin(string pan)
        {
            string pin;

            try
            {
                Logger.WriteLine("\t\t\tCalculating Pin for Debit card...");
                Logger.WriteLine("\t\t\tGenerating Pin debit card: \"{0}\" ", pan);
                //pin = DeviceFactory.PinGenerator.GeneratePin(pan.Substring(pan.Length - 13, 12));
                pin = this.getCode();
                Logger.WriteLine("\t\t\tPin generated for card \"{0}\"", pan);
                return pin;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("\t\t\tError occurred while generating PinCode", ex);
                throw new Exception("Unable to Generate PinCode - Check HSM connection and configuration settings.");
            }
        }

        private string getCode()
        {
            return new Random().Next(999).ToString().Trim().PadLeft(3, '0');
        }
    }
}
