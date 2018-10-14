﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using dexih.functions.Query;
using Dexih.Utils.DataType;

namespace dexih.functions.BuiltIn
{
    public class ConditionFunctions
    {
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "General Condition", Name = "Less Than",
            Description = "Less than")]
        [TransformFunctionCompare(Compare = Filter.ECompare.LessThan)]
        public bool LessThan(object value, object compare)
        {
            return DataType.Compare(null, value, compare) == DataType.ECompareResult.Less;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "General Condition", Name = "Less Than/Equal",
            Description = "Less than or Equal")]
        [TransformFunctionCompare(Compare = Filter.ECompare.LessThanEqual)]
        public bool LessThanEqual(object value, object compare)
        {
            var compareResult = DataType.Compare(null, value, compare);
            return compareResult == DataType.ECompareResult.Less || compareResult == DataType.ECompareResult.Equal;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "General Condition", Name = "Greater Than",
            Description = "Greater than")]
        [TransformFunctionCompare(Compare = Filter.ECompare.GreaterThan)]
        public bool GreaterThan(object value, object compare)
        {
            return DataType.Compare(null, value, compare) == DataType.ECompareResult.Greater;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "General Condition",
            Name = "Greater Than/Equal", Description = "Greater or Equal")]
        [TransformFunctionCompare(Compare = Filter.ECompare.GreaterThanEqual)]
        public bool GreaterThanEqual(object value, object compare)
        {
            var compareResult = DataType.Compare(null, value, compare);
            return compareResult == DataType.ECompareResult.Greater || compareResult == DataType.ECompareResult.Equal;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "General Condition", Name = "Equal",
            Description = "The list of values are equal.")]
        [TransformFunctionCompare(Compare = Filter.ECompare.IsEqual)]
        public bool IsEqual(object[] values)
        {
            for (var i = 1; i < values.Length; i++)
            {
                if (!Equals(values[0], values[i])) return false;
            }

            return true;
        }
        
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "General Condition", Name = "Array Contains",
            Description = "The value is equal to at least one of the values in the values array")]
        [TransformFunctionCompare(Compare = Filter.ECompare.IsEqual)]
        public bool ArrayContains(object value, object[] values)
        {
            return values.Contains(value);
        }
        
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "General Condition", Name = "Strings Equal",
            Description = "The list of string values are equal.")]
        [TransformFunctionCompare(Compare = Filter.ECompare.IsEqual)]
        public bool StringIsEqual(string[] values, bool ignoreCase) 
        {
            for (var i = 1; i < values.Length; i++)
            {
                var compare = string.Equals(values[0], values[i], ignoreCase
                    ? StringComparison.InvariantCultureIgnoreCase
                    : StringComparison.CurrentCulture);
                if (!compare) return false;
            }

            return true;
        }
        
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Date Condition", Name = "Is Date",
            Description =
                "Return boolean if the value is a valid date.")]
        public bool IsDate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return DateTime.TryParse(value, out _);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition", Name = "Is Number",
            Description = "Value is a valid number")]
        public bool IsNumber(string value)
        {
            return decimal.TryParse(value, out _);
        }



        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "General Condition", Name = "Is Null",
            Description = "Value is null")]
        public bool IsNull(object value)
        {
            return value == null || value is DBNull;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Is Null Or Empty",
            Description = "Value is null or any empty string")]
        public bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }
        
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Is Null Or Whitespace",
            Description = "Value is null or a string with only whitespaces")]
        public bool IsNullOrWhitespace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition", Name = "Is Between",
            Description = "Value is between the specified values but not equal to them.")]
        public bool IsBetween(object value, object lowRange, object highRange)
        {
            return GreaterThan(value, lowRange) && LessThan(value, highRange);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition",
            Name = "Is Between Inclusive", Description = "Value is equal or between the specified values")]
        public bool IsBetweenInclusive(object value, object lowRange, object highRange)
        {
            return GreaterThanEqual(value, lowRange) && LessThanEqual(value, highRange);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition",
            Name = "Is Regular Expression", Description = "Value matches the specified regular expression.")]
        public bool RegexMatch(string input, string pattern)
        {
            return Regex.Match(input, pattern).Success;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Contains",
            Description = "Returns a value indicating whether a specified substring occurs within this string.")]
        public bool Contains(string value, string contains, bool ignoreCase = false)
        {
            return value.IndexOf(contains,
                       ignoreCase
                           ? StringComparison.InvariantCultureIgnoreCase
                           : StringComparison.CurrentCulture) >= 0;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Ends With",
            Description = "Determines whether the end of this string instance matches the specified string.")]
        public bool EndsWith(string value, string endsWith, bool ignoreCase = false)
        {
            return value.EndsWith(endsWith, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCulture);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Starts With",
            Description = "Determines whether the beginning of this string instance matches the specified string.")]
        public bool StartsWith(string value, string startsWith, bool ignoreCase = false)
        {
            return value.StartsWith(startsWith, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCulture);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Is Uppercase",
            Description = "All character values are upper case.")]
        public bool IsUpper(string value, bool skipNonAlpha)
        {
            foreach (var t in value)
            {
                if ((skipNonAlpha && char.IsLetter(t) && !char.IsUpper(t)) ||
                    (skipNonAlpha == false && !char.IsUpper(t)))
                    return false;
            }

            return true;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Is Lowercase",
            Description = "All character values are lower case.")]
        public bool IsLower(string value, bool skipNonAlpha)
        {
            foreach (var t in value)
            {
                if ((skipNonAlpha && char.IsLetter(t) && !char.IsLower(t)) ||
                    (skipNonAlpha == false && !char.IsLower(t)))
                    return false;
            }

            return true;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Is Letters",
            Description = "Only contains letters")]
        public bool IsAlpha(string value)
        {
            foreach (var t in value)
            {
                if (!char.IsLetter(t))
                    return false;
            }

            return true;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "IsLetter/Digit",
            Description = "Only contains letters and digits (number).")]
        public bool IsAlphaNumeric(string value)
        {
            foreach (var t in value)
            {
                if (!char.IsLetter(t) && !char.IsNumber(t))
                    return false;
            }
            return true;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Is Pattern",
            Description =
                "Matches the pattern where A=upper character, a=lower character, 9=number and other values match precisely.")]
        public bool IsPattern(string value, string pattern)
        {
            if (value.Length != pattern.Length) return false;
            for (var i = 0; i < pattern.Length; i++)
            {
                if ((pattern[i] == '9' && !char.IsNumber(value[i])) ||
                    (pattern[i] == 'A' && !char.IsUpper(value[i])) ||
                    (pattern[i] == 'a' && !char.IsLower(value[i])) ||
                    (pattern[i] == 'Z' && !char.IsLetter(value[i])))
                    return false;
            }

            return true;
        }
        
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Date Condition", Name = "Is Daylight Saving",
            Description =
                "Indicates whether this instance of DateTime is within the daylight saving time range for the current time zone.")]
        public bool IsDaylightSavingTime(DateTime dateValue)
        {
            return TimeZoneInfo.Local.IsDaylightSavingTime(dateValue);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Date Condition", Name = "Is Leap Year",
            Description = "Returns an indication whether the specified year is a leap year.")]
        public bool IsLeapYear(DateTime dateValue)
        {
            return DateTime.IsLeapYear(dateValue.Year);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Date Condition", Name = "Is Weekend",
            Description = "Date falls on a weekend")]
        public bool IsWeekend(DateTime dateValue)
        {
            return dateValue.DayOfWeek == DayOfWeek.Saturday || dateValue.DayOfWeek == DayOfWeek.Sunday;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Date Condition", Name = "Is Weekday",
            Description = "Date falls on a week day")]
        public bool IsWeekDay(DateTime dateValue)
        {
            return !(dateValue.DayOfWeek == DayOfWeek.Saturday || dateValue.DayOfWeek == DayOfWeek.Sunday);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "String Condition", Name = "Is In",
            Description = "Value is one of the elements in an array")]
        public bool IsIn(object value, object[] compareTo)
        {
            var returnValue = false;
            foreach (var t in compareTo)
            {
                returnValue = Equals(value, t);
                if(returnValue)
                {
                    break;
                }
            }
            return returnValue;
        }
        
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Date Condition", Name = "DateTime Equals",
            Description = "Are the specified datetime values equal.")]
        [TransformFunctionCompare(Compare = Filter.ECompare.IsEqual)]
        public bool IsDateTimeEqual(DateTime[] values)
        {
            for (var i = 1; i < values.Length; i++)
            {
                if (values[0] != values[i]) return false;
            }

            return true;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition", Name = "Numeric Equals",
            Description = "Are the specified decimal values equal with the specified precision.")]
        [TransformFunctionCompare(Compare = Filter.ECompare.IsEqual)]
        public bool IsDecimalEqual(decimal[] values, int precision = 6)
        {
            if (values.Length == 0) return true;
            
            var value1 = decimal.Round(values[0], precision);
            
            for (var i = 1; i < values.Length; i++)
            {
                if (value1 != decimal.Round(values[i], precision)) return false;
            }

            return true;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Boolean Condition", Name = "Boolean Equals",
            Description = "Are the specified boolean conditions equal.")]
        [TransformFunctionCompare(Compare = Filter.ECompare.IsEqual)]
        public bool IsBooleanEqual(bool[] values)
        {
            for (var i = 1; i < values.Length; i++)
            {
                if (values[0] != values[i]) return false;
            }

            return true;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Boolean Condition", Name = "Is True",
            Description = "Is the specified value equal to true.")]
        public bool IsTrue(bool value) => value;


        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Date Condition", Name = "Is Now Between Dates",
            Description = "The date now is between start/end date ")]
        public bool IsDateBetweenNow(DateTime lowRange, DateTime highRange)
        {
            return DateTime.Now > lowRange && DateTime.Now < highRange;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Date Condition",
            Name = "Is Now Between Dates(inclusive)",
            Description = "The date now is equal or between the start/end date")]
        public bool IsDateBetweenInclusiveNow(DateTime lowRange, DateTime highRange)
        {
            return DateTime.Now >= lowRange && DateTime.Now <= highRange;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition", Name = "Range Intersect",
            Description = "The two ranges (lowRange1-highRange1 & lowRange2-highRange2) intersect.")]
        public bool RangeIntersect(object lowRange1, object highRange1, object lowRange2, object highRange2)
        {
            return LessThan(lowRange1, highRange2) && LessThan(lowRange2, highRange1);
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition", Name = "Is Even", Description = "The specific number is even")]
        public bool IsEven(long number)
        {
            return (number & 1) == 0;
        }

        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition", Name = "Is Odd", Description = "The specific number is odd")]
        public bool IsOdd(long number)
        {
            return (number & 1) == 1;
        }
        
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition", Name = "Is Power Of Two", Description = "The specific number is a power of two")]
        public static bool IsPowerOfTwo(long number)
        {
            if (number > 0L)
                return (number & number - 1L) == 0L;
            return false;
        }

        /// <summary>
        /// Find out whether the provided 32 bit integer is a perfect square, i.e. a square of an integer.
        /// </summary>
        /// <param name="number">The number to very whether it's a perfect square.</param>
        /// <returns>True if and only if it is a perfect square.</returns>
        [TransformFunction(FunctionType = EFunctionType.Condition, Category = "Numeric Condition", Name = "Is Perfect Square", Description = "The specific number is a perfect square (i.e. square of an integer)")]
        public static bool IsPerfectSquare(int number)
        {
            if (number < 0)
                return false;
            switch (number & 15)
            {
                case 0:
                case 1:
                case 4:
                case 9:
                    var num = (int) Math.Floor(Math.Sqrt((double) number) + 0.5);
                    return num * num == number;
                default:
                    return false;
            }
        }

    }
}