using System;

namespace Domain.Common
{
    public static class Utils
    {
        /// <exception cref="ArgumentException">
        ///     All characters must be digits.
        ///     Length must be {UniqueMasterCitizenNumberLength}.
        ///     Control digit is invalid.
        /// </exception>
        public static void ValidateUniqueMasterCitizenNumber(string uniqueMasterCitizenNumber)
        {
            if (string.IsNullOrWhiteSpace(uniqueMasterCitizenNumber) || uniqueMasterCitizenNumber.Length != UniqueMasterCitizenNumberLength)
            {
                throw new ArgumentException($"Invalid unique master citizen number: length must be {UniqueMasterCitizenNumberLength}.");
            }
            int nominalControlDigit = DigitCharToInt(uniqueMasterCitizenNumber[12]);
            int actualControlDigit =
                11 - (
                    7 * (DigitCharToInt(uniqueMasterCitizenNumber[0]) + DigitCharToInt(uniqueMasterCitizenNumber[6])) +
                    6 * (DigitCharToInt(uniqueMasterCitizenNumber[1]) + DigitCharToInt(uniqueMasterCitizenNumber[7])) +
                    5 * (DigitCharToInt(uniqueMasterCitizenNumber[2]) + DigitCharToInt(uniqueMasterCitizenNumber[8])) +
                    4 * (DigitCharToInt(uniqueMasterCitizenNumber[3]) + DigitCharToInt(uniqueMasterCitizenNumber[9])) +
                    3 * (DigitCharToInt(uniqueMasterCitizenNumber[4]) + DigitCharToInt(uniqueMasterCitizenNumber[10])) +
                    2 * (DigitCharToInt(uniqueMasterCitizenNumber[5]) + DigitCharToInt(uniqueMasterCitizenNumber[11]))
                ) % 11;
            if (actualControlDigit > 9)
            {
                actualControlDigit = 0;
            }
            if (actualControlDigit != nominalControlDigit)
            {
                throw new ArgumentException($"Invalid unique master citizen number: control digit is invalid.");
            }
        }

        /// <exception cref="ArgumentException">
        ///     All characters must be digits.
        /// </exception>
        public static int DigitCharToInt(char digitChar)
        {
            if (char.IsDigit(digitChar))
            {
                throw new ArgumentException("Invalid unique master citizen number: all characters must be digits.");
            }
            double digitDouble = char.GetNumericValue(digitChar);
            int digit = (int)digitDouble;
            return digit;
        }
        public const int UniqueMasterCitizenNumberLength = 13;
    }
}
