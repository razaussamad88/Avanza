//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;

namespace Avanza.KeyStore
{
    public interface IConfigSection
    {
        string Name
        {
            get;
        }

        string Value
        {
            get;
        }

        bool HasChildSections
        {
            get;
        }

        bool HasChild(string name);
        bool HasAttribute(string name);

        IConfigSection GetChild(string name);

        bool GetBoolValue(string name);

        string GetTextValue(string name);

        short GetShortValue(string name);

        int GetIntValue(string name);

        long GetLongValue(string name);

        double GetDoubleValue(string name);

        decimal GetDecimalValue(string name);

        T GetEnumValue<T>(string name, T defVal);

        T GetEnumValue<T>(string name);

        char GetCharValue(string name);

        /// <summary>
        /// Returns Datetime value
        /// </summary>
        /// <param name="name">name of Attribute</param>
        /// <returns></returns>
        /// <remarks>Value should be in universal format</remarks>
        DateTime GetDateTimeValue(string name);

        string GetValue(string name, string defVal);

        short GetValue(string name, short defVal);

        int GetValue(string name, int defVal);

        long GetValue(string name, long defVal);

        bool GetValue(string name, bool defVal);

        double GetValue(string name, double defVal);

        decimal GetValue(string name, decimal defVal);

        char GetValue(string name, char defVal);
        /// <summary>
        /// Returns Datetime value
        /// </summary>
        /// <param name="name">name of Attribute</param>
        /// <returns></returns>
        /// <remarks>Value should be in universal format</remarks>
        DateTime GetValue(string name, DateTime defVal);

        IConfigSection[] ChildSections
        {
            get;
        }

        IConfigSection[] GetChildSections(string Xpath);
    }
}