using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ErrorHandling
{
    public static class ValidateExtension
    {
        /// <summary>
        /// เช็ค ภาษาไทย/อังกฤษ ตัวเลข อักขระพิเศษ โดยส่งค่าเข้ามาเช็คเงื่อนไข
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isThai"></param>
        /// <param name="isNumber"></param>
        /// <param name="isSpecial"></param>
        /// <param name="isMaxLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="fixSpecialCharacter"></param>
        /// <returns></returns>
        public static bool CheckLang(this string value, bool isThai, bool isNumber, bool isSpecial, bool isMaxLength, int? maxLength = null, string fixSpecialCharacter = null)
        {
            string pattern = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (isThai)
                sb.Append("^[\u0E00-\u0E7F ");
            else
                sb.Append("^[a-zA-Z ");
            if (isNumber)
                sb.Append("0-9");
            if (isSpecial)
                sb.Append(@"-$@$!%*?&#/^\-_.() +/,");
            if (!string.IsNullOrEmpty(fixSpecialCharacter) && isSpecial == false)
                sb.Append(fixSpecialCharacter);
            if (isMaxLength)
            {
                sb.Append(".]{0,");
                sb.Append(maxLength.ToString());
                sb.Append("}$");
            }
            else
            {
                sb.Append("]+$");
            }
            if (value != null)
            {
                return Regex.IsMatch(value, sb.ToString());
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// เช็ค ภาษาไทยและอังกฤษ หรืออย่างใดอย่างหนึ่ง ตัวเลข อักขระพิเศษ โดยส่งค่าเข้ามาเช็คเงื่อนไข
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isNumber"></param>
        /// <param name="isSpecial"></param>
        /// <param name="isMaxLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="fixSpecialCharacter"></param>
        /// <returns></returns>
        public static bool CheckAllLang(this string value, bool isNumber, bool isSpecial, bool isMaxLength, int? maxLength = null, string fixSpecialCharacter = null)
        {
            string pattern = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("^[\u0E00-\u0E7Fa-zA-Z ");
            if (isNumber)
                sb.Append("0-9");
            if (isSpecial)
                sb.Append(@"-$@$!%*?&#/^\-_.() +/,");
            if (!string.IsNullOrEmpty(fixSpecialCharacter) && isSpecial == false)
                sb.Append(fixSpecialCharacter);
            if (isMaxLength)
            {
                sb.Append(".]{0,");
                sb.Append(maxLength.ToString());
                sb.Append("}$");
            }
            else
            {
                sb.Append("]+$");
            }
            if (value != null)
            {
                return Regex.IsMatch(value, sb.ToString());
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// เช็คตัวเลขโดยกำหนด ความยาว ก่อน . และหลัง .
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="decimalLength"></param>
        /// <returns></returns>
        public static bool IsOnlyNumberWithMaxLengthAndDigit(this string value, int maxLength, int decimalLength)
        {
            if (value != null)
            {
                string pattern = @"^[0-9]{0," + maxLength + @"}(?:\.[0-9]{0," + decimalLength + @"})?$";
                return Regex.IsMatch(value, pattern);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ERR0001 ERR0004 ERR0025 ERR0027
        /// เช็คตัวเลข 0-9
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOnlyNumber(this string value)
        {
            if (value != null)
            {
                string pattern = @"^[0-9]+$";
                return Regex.IsMatch(value, pattern);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ERR0023 ERR0024 ERR0032 ERR0035
        /// เช็คตัวเลข 0-9 โดยกำหนดหลัก
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool IsOnlyNumberWithMaxLength(this string value, int maxLength)
        {
            if (value != null)
            {
                string pattern = @"^[0-9]{0," + maxLength + "}$";
                return Regex.IsMatch(value, pattern);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ERR0016
        /// เช็คตัวเลข 0-9 และอักขระพิเศษแบบกำหนด
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isSpecial"></param>
        /// <param name="fixSpecialCharacter"></param>
        /// <returns></returns>
        public static bool IsOnlyNumberWithSpecialCharacter(this string value, bool isSpecial, string fixSpecialCharacter = null)
        {
            StringBuilder sb = new StringBuilder();

            if (value != null)
            {
                sb.Append(@"^[0-9");

                if (isSpecial)
                {
                    sb.Append(@"-$@$!%*?&#/^\-_.() +/,");
                }

                if (!string.IsNullOrEmpty(fixSpecialCharacter) && isSpecial == false)
                {
                    sb.Append(fixSpecialCharacter);
                }

                sb.Append("]+$");

                return Regex.IsMatch(value, sb.ToString());
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ERR0015
        /// เช็ค Format Email
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string value)
        {
            if (value != null)
            {
                string email = value;

                try
                {
                    return Regex.IsMatch(email,
                        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
