using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using FourthDown.Api;

namespace FourthDown.Api.Utilities
{
    public static class StringParser
    {
        public static int ToInt(string number) =>
            int.Parse(number);

        public static int? ToNullableInt(string number) =>
            IsNa(number) ? (int?) null : int.Parse(number);

        public static int ToIntDefaultZero(string number) =>
            IsNa(number) ? 0 : int.Parse(number);

        public static double ToDouble(string number) =>
            double.Parse(number);

        public static double? ToNullableDouble(string number) =>
            IsNa(number) ? (double?) null : double.Parse(number);

        public static double ToDoubleDefaultZero(string number) =>
            IsNa(number) ? 0 : double.Parse(number);

        public static DateTime? ToDateTimeOrNull(string dateTime, string format) =>
            IsNa(dateTime) || dateTime == "0"
                ? (DateTime?) null
                : DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);

        public static DateTime ToDateTime(string dateTime, string format) =>
            DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);

        public static bool ToBool(string number) =>
            number == "1";

        public static bool? ToNullableBool(string number) =>
            IsNa(number) ? (bool?) null : number == "1";

        public static string ToString(string str) =>
            IsNa(str) ? "" : str;

        private static bool IsNa(string value) => value == "NA" || value == "";

        public static string GetAbsolutePath(string relativePath)
        {
            var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);

            Debug.Assert(_dataRoot.Directory != null);
            var assemblyFolderPath = _dataRoot.Directory.FullName;

            var fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}