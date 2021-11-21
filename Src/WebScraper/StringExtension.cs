using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WebScraper
{

    public static class StringExtension
    {
        public static void WriteToHtmFile(this string data, string path, string fileName)
        {
            string docName = $"{path}\\{fileName.RemoveInvalidCharsFileName()}.htm";
            using (StreamWriter streamWriter = new StreamWriter(docName))
                streamWriter.WriteLine(data);
        }
        public static string RemoveInvalidCharsFileName(this string value)
        {
            string illegal = value;
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            illegal = r.Replace(illegal, "");

            return illegal;
        }

        public static void WriteToLogFile(this string data, string path, string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter($"{path}\\{fileName}.log"))
                streamWriter.WriteLine(data);
        }

        public static string ExtractOrDefault(this string source, string start, string end)
        {
            var num1 = source.IndexOf(start, StringComparison.Ordinal);
            if (num1 == -1)
                return "";
            var startIndex = num1 + start.Length;
            int num2 = source.IndexOf(end, startIndex, StringComparison.Ordinal);
            return num2 == -1 ? "" : source.Substring(startIndex, num2 - startIndex);
        }

        public static string ExtractOrDefault(this string source, int startIndex, string start, string end)
        {
            var num1 = source.IndexOf(start, startIndex, StringComparison.Ordinal);
            if (num1 == -1 || num1 >= source.Length)
                return "";
            var startIndex1 = num1 + start.Length;
            var num2 = source.IndexOf(end, startIndex1, StringComparison.Ordinal);
            if (num2 == -1 || num2 >= source.Length)
                return "";
            return source.Substring(startIndex1, num2 - startIndex1);
        }

        public static string ExtractOrDefault(this string source, string find, string start, string end)
        {
            var num1 = source.IndexOf(find, StringComparison.Ordinal);
            if (num1 == -1 || num1 >= source.Length)
                return "";
            var num2 = source.IndexOf(start, num1 + find.Length, StringComparison.Ordinal);
            if (num2 == -1 || num2 >= source.Length)
                return "";
            var startIndex = num2 + start.Length;
            var num3 = source.IndexOf(end, startIndex, StringComparison.Ordinal);
            if (num3 == -1 || num3 >= source.Length)
                return "";
            return source.Substring(startIndex, num3 - startIndex);
        }

    }
}
