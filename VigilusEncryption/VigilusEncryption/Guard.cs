using System;
using System.Collections.Generic;
using System.Text;

namespace VigilusEncryption
{
    /// <summary>
    /// Utility Class that can be used for basic argument checking
    /// </summary>
    public static class Guard
    {

        /// <summary>
        /// Checks an argument to ensure it isn't null
        /// </summary>
        /// <param name="argValue">The argument value to check.</param>
        /// <param name="argName">The name of the argument.</param>
        public static void CheckNull(object argVal, string argName)
        {
            if (argVal == null)
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// Checks a string argument to ensure it isn't null or empty
        /// </summary>
        /// <param name="argValue">The argument value to check.</param>
        /// <param name="argName">The name of the argument.</param>
        public static string CheckNullOrEmpty(string argVal, string argName)
        {
            Guard.CheckNull(argVal, argName);
            Guard.CheckEmpty(argVal, argName);

            return argVal;
        }

        public static string CheckEmpty(string argVal, string argName)
        {
            if (argVal.Length == 0)
                throw new ArgumentException(string.Format("Argument[{0}] can't be empty", argName));

            return argVal;
        }

        /// <summary>
        /// Checks an Enum argument to ensure that its value is defined by the specified Enum type.
        /// </summary>
        /// <param name="enumType">The Enum type the value should correspond to.</param>
        /// <param name="value">The value to check for.</param>
        /// <param name="argName">The name of the argument holding the value.</param>
        public static void CheckEnumValue(Type enumType, object value, string argName)
        {
            if (Enum.IsDefined(enumType, value) == false)
                throw new ArgumentException(String.Format("Argument[{0}] contain invalid value for Enum[{1}]", argName, enumType.ToString()));
        }

    }
}
