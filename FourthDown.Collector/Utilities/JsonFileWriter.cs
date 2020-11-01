using System;
using System.Collections.Generic;
using System.IO;
using FourthDown.Shared.Models;
using Newtonsoft.Json;

namespace FourthDown.Collector.Utilities
{
    public static class JsonFileWriter
    {
        public static void Write(IEnumerable<PlayByPlay> data, string fileName)
        {
            const string DestinationPath = @"../../../../FourthDown.API/wwwroot/data/";
            var folderPath = StringParser.GetAbsolutePath(DestinationPath);

            // Create directory if it doesn't exist
            if (Directory.Exists(folderPath))
                return;

            Directory.CreateDirectory(folderPath);
            Console.WriteLine($"Directory created: {folderPath}");

            var path = Path.Combine(folderPath, fileName);
            var file = File.CreateText($"{path}.json");
            var serializer = new JsonSerializer();

            serializer.Serialize(file, data);
            file.Close();
        }
    }
}