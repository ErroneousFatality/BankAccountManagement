using System;
using System.Linq;

namespace Domain.Common
{
    public static class Utils
    {
        /// <exception cref="ArgumentException">
        ///     Character does not represent a decimal digit.
        ///     Length must be {UniqueMasterCitizenNumberLength}.
        ///     Control digit is invalid.
        /// </exception>
        public static void ValidateUniqueMasterCitizenNumber(string uniqueMasterCitizenNumber)
        {
            if (string.IsNullOrWhiteSpace(uniqueMasterCitizenNumber) || uniqueMasterCitizenNumber.Length != UniqueMasterCitizenNumberLength)
            {
                throw new ArgumentException($"Length must be {UniqueMasterCitizenNumberLength}.");
            }
            byte[] digits = uniqueMasterCitizenNumber
                .Select((char digitChar) => ParseDigitCharToByte(digitChar))
                .ToArray();
            byte actualControlDigit = (byte)(
                11 - (
                    7 * (digits[0] + digits[6]) +
                    6 * (digits[1] + digits[7]) +
                    5 * (digits[2] + digits[8]) +
                    4 * (digits[3] + digits[9]) +
                    3 * (digits[4] + digits[10]) +
                    2 * (digits[5] + digits[11])
                ) % 11
            );
            if (actualControlDigit > 9)
            {
                actualControlDigit = 0;
            }
            if (actualControlDigit != digits[12])
            {
                // The 13th digit is the control digit.
                throw new ArgumentException("The control digit is invalid.");
            }

            byte politicalRegionOfBirthCode = (byte)(10 * digits[7] + digits[8]);
            if (60 < politicalRegionOfBirthCode || politicalRegionOfBirthCode > 99)
            {
                throw new ArgumentException("Political region of birth is not within Republic of Serbia.");
            }

            byte day = (byte)(10 * digits[0] + digits[1]);
            byte month = (byte)(10 * digits[2] + digits[3]);
            DateTime now = DateTime.Now;
            int currentMillennium = now.Year / 1000;
            int year = 1000 * currentMillennium 
                + 100 * digits[4] 
                + 10 * digits[5] + 
                digits[6];
            DateTime dateOfBirth = new(year, month, day);

            if (dateOfBirth > now)
            {
                dateOfBirth = dateOfBirth.AddYears(-1000);
                if (dateOfBirth > now)
                {
                    throw new ArgumentException("Date of birth can not be in the future.");
                }
            }

            DateTime dateOfAchievingAdulthood = dateOfBirth.AddYears(AgeOfAdulthood);
            if (dateOfAchievingAdulthood > now)
            {
                // this also validates that the user's date of birth is in the past.
                throw new ArgumentException("Citizen must be an adult.");
            }
        }

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Character does not represent a decimal digit.
        /// </exception>
        public static byte ParseDigitCharToByte(char digitChar)
        {
            if (char.IsDigit(digitChar))
            {
                throw new ArgumentOutOfRangeException(nameof(digitChar), $"Character does not represent a decimal digit: \"{digitChar}\"");
            }
            double digitDouble = char.GetNumericValue(digitChar);
            byte digit = (byte)digitDouble;
            return digit;
        }

        // Constants
        public const byte UniqueMasterCitizenNumberLength = 13;
        public const byte AgeOfAdulthood = 18;
    }
}
