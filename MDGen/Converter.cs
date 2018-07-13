using System;
using System.IO;

namespace MDGen.Library
{
	public static class Converter
	{
		public static void FromCSV(string sourceFilePath, string worksheetName, string outputPath)
		{
			if (!File.Exists(sourceFilePath))
			{
				throw new InvalidDataException($"File {sourceFilePath} does not exist.");
			}

			if (!Directory.Exists(outputPath))
			{
				Console.WriteLine($"Creating output directory {outputPath}");
				Directory.CreateDirectory(outputPath);
			}

			var data = ExcelInterop.ExcelUtility.ReadFromExcel(sourceFilePath, worksheetName);
			var sectionTitles = data[0];

			for (var i = 1; i < data.Count; i++)
			{
				var row = data[i];

			}
		}
	}
}
