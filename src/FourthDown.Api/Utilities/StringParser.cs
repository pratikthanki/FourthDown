using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FourthDown.Api.Utilities
{
    public static class StringParser
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true,
            AllowTrailingCommas = true,
            IgnoreReadOnlyProperties = true
        };

        private const string DateFormat = "yyyy-MM-dd";

        public static int ToInt(string number)
        {
            int.TryParse(number, out var num);
            return num;
        }

        public static int? ToNullableInt(string number)
        {
            return IsNa(number) ? (int?) null : int.Parse(number);
        }

        public static int ToIntDefaultZero(string number)
        {
            return IsNa(number) ? 0 : int.Parse(number);
        }

        public static double ToDouble(string number)
        {
            return double.Parse(number);
        }

        public static double? ToNullableDouble(string number)
        {
            return IsNa(number) ? (double?) null : double.Parse(number);
        }

        public static double ToDoubleDefaultZero(string number)
        {
            return IsNa(number) ? 0 : double.Parse(number);
        }

        public static TimeSpan? ToTimeSpanOrNull(string time)
        {
            return IsNa(time) || time == "0" ? (TimeSpan?) null : TimeSpan.Parse(time);
        }

        public static DateTime? ToDateTimeOrNull(string dateTime)
        {
            return IsNa(dateTime) || dateTime == "0"
                ? (DateTime?) null
                : DateTime.ParseExact(dateTime, DateFormat, CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime(string dateTime, string format)
        {
            return DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
        }

        public static bool ToBool(string number)
        {
            return number == "1";
        }

        public static bool? ToNullableBool(string number)
        {
            return IsNa(number) ? (bool?) null : number == "1";
        }

        public static string ToString(string str)
        {
            return IsNa(str) ? "NA" : str;
        }

        private static bool IsNa(string value)
        {
            return value == "NA" || value == "";
        }

        private static string GetAbsolutePath(string relativePath)
        {
            var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);

            Debug.Assert(_dataRoot.Directory != null);
            var assemblyFolderPath = _dataRoot.Directory.FullName;

            var fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

        public static string GetDataFilePath(string file)
        {
            return GetAbsolutePath($@"../../../Data/{file}");
        }
        
        public static string[] SplitCsvLine(string line)
        {
            var result = new List<string>();
            var currentStr = new StringBuilder("");
            var inQuotes = false;

            foreach (var T in line)
                if (T == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (T == ',')
                {
                    if (!inQuotes)
                    {
                        result.Add(currentStr.ToString());
                        currentStr.Clear();
                    }
                    else
                    {
                        currentStr.Append(T);
                    }
                }
                else
                {
                    currentStr.Append(T);
                }

            result.Add(currentStr.ToString());

            return result.ToArray();
        }
    }
}