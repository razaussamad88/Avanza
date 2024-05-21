using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadPoolConsoleApp
{

    public class MBLStrategy
    {
        public void Calculate(string imd, DebitCard card, CustomerChannelAuthen custPin, bool isEMVCard)
        {
            Thread.Sleep(100);

            //if (!String.IsNullOrEmpty(HelperFunctions.VisaIMDs) && HelperFunctions.VisaIMDs.Contains(imd))
            {
                // ---- Code By RAZA for Bulk HSM POC.
                ModHelper.GenerateHSM_Bulk(new clsMachineInterfaceParam()
                {
                    Card = card,
                    IsEMVCard = isEMVCard,
                    PaymentTechnology = ModHelper.FinancialService.Visa
                }, ref card, ref custPin);

                //custPin.Reserved_7 = StrategyHelper.GetEncryptedPinInfo(custPin);
            }
            /*
            else if (!String.IsNullOrEmpty(HelperFunctions.MasterCardIMDs) && HelperFunctions.MasterCardIMDs.Contains(imd))
            {
                // ---- Code By RAZA for Bulk HSM POC.
                ModHelper.GenerateHSM_Bulk(new clsMachineInterfaceParam()
                {
                    Card = card,
                    IsEMVCard = isEMVCard,
                    PaymentTechnology = ModHelper.FinancialService.Master
                }, ref card, ref custPin);

                //custPin.Reserved_7 = StrategyHelper.GetEncryptedPinInfo(custPin);
            }
            else if (!String.IsNullOrEmpty(HelperFunctions.PayPakIMDs) && HelperFunctions.PayPakIMDs.Contains(imd))
            {
                // ---- Code By RAZA for Bulk HSM POC.
                ModHelper.GenerateHSM_Bulk(new clsMachineInterfaceParam()
                {
                    Card = card,
                    IsEMVCard = isEMVCard,
                    PaymentTechnology = ModHelper.FinancialService.PayPak
                }, ref card, ref custPin);

                //custPin.Reserved_7 = StrategyHelper.GetEncryptedPinInfo(custPin);
            }
            */
        }
    }
}
