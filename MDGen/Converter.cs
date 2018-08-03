﻿using ExcelInterop;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MDGen.Library
{
	public static class Converter
	{
		public static IList<MD> FromCSV(string sourceFilePath, string worksheetName, string outputPath)
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

			var data = ExcelUtility.ReadFromExcel(sourceFilePath, worksheetName);
			var sectionTitles = data[0];

			var header = new Header()
			{
				Author = "mishra14",
				MsAuthor = "anmishr",
				Manager = "rrelyea",
				MsDate = DateTimeOffset.Now.ToString("d"),
				MsTopic = "reference",
				MsReviewer = "anangaur"
			};

			var mdFiles = new List<MD>();

			for (var i = 1; i < data.Count; i++)
			{
				var row = data[i];
				var code = string.Empty;
				var level = string.Empty;
				var pretext = string.Empty;
				var sections = new List<Section>();

				for (var j = 0; j < row.Count; j++)
				{
					var title = sectionTitles[j];
					if (!SkipSection(title))
					{
						if (title.Equals("loglevel", StringComparison.OrdinalIgnoreCase))
						{
							level = row[j];
						}
						else if (title.Equals("NuGet Log Code", StringComparison.OrdinalIgnoreCase))
						{
							code = row[j];
						}
						else if (title.Equals("log message", StringComparison.OrdinalIgnoreCase))
						{
							pretext = row[j];
						}
						else
						{
							var section = new Section()
							{
								Title = title,
								Content = row[j]
							};

							sections.Add(section);
						}
					}
				}

				header.Title = $"NuGet {level} {code}";
				header.Description = $"{code} {level} code";
				header.F1Keywords = new List<string>() { code };
				Enum.TryParse(code, out NuGetLogCode logCode);
				Enum.TryParse(level, out LogLevel logLevel);

				var md = new MD()
				{
					Header = header,
					Pretext = pretext,
					Sections = sections,
					Code = logCode,
					Level = logLevel
				};

				if (SaveMD(outputPath, md))
				{
					mdFiles.Add(md);
				}
			}

			return mdFiles;
		}

		private static bool SaveMD(string outputPath, MD md)
		{
			// write only the ones that have description and solution sections
			if (md.Sections.Any(section => section.Title == "Issue" && !string.IsNullOrEmpty(section.Content) &&
				md.Sections.Any(anotherSection => anotherSection.Title == "Solution" && !string.IsNullOrEmpty(anotherSection.Content))))
			{
				md.Save($@"{outputPath}\{md.Code}.md", overwrite: true);
				return true;
			}

			return false;
		}

		private static bool SkipSection(string title)
		{
			return title.StartsWith("[SKIP]", StringComparison.OrdinalIgnoreCase);
		}
	}
}
