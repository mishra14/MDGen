using ExcelInterop;
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

			IList<IList<string>> data = ExcelUtility.ReadFromExcel(sourceFilePath, worksheetName);
			IList<string> sectionTitles = data[0];
			List<MD> mdFiles = new List<MD>();
			MD mdFile = null;

			for (int i = 1; i < data.Count; i++)
			{
				IList<string> row = data[i];
				string code = string.Empty;
				string level = string.Empty;
				string pretext = string.Empty;
				List<Section> sections = new List<Section>();

				for (int j = 0; j < row.Count; j++)
				{
					string title = sectionTitles[j];
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
							Section section = new Section()
							{
								Title = title,
								Content = row[j]
							};

							sections.Add(section);
						}
					}
				}

				Enum.TryParse(code, out NuGetLogCode logCode);
				Enum.TryParse(level, out LogLevel logLevel);

				if (logCode == NuGetLogCode.Undefined)
				{
					mdFile.Scenarios.Add(new Scenario() { Pretext = pretext, Sections = sections });
				}
				else
				{
					if (mdFile != null && SaveMD(outputPath, mdFile))
					{
						mdFiles.Add(mdFile);
					}

					Header header = new Header()
					{
						Author = "mishra14",
						MsAuthor = "anmishr",
						Manager = "rrelyea",
						MsDate = DateTimeOffset.Now.ToString("d"),
						MsTopic = "reference",
						MsReviewer = "anangaur",
						Title = $"NuGet {level} {code}",
						Description = $"{code} {level} code",
						F1Keywords = new List<string>() { code },
					};

					mdFile = new MD()
					{
						Header = header,
						Scenarios = new List<Scenario>() { new Scenario() { Pretext = pretext, Sections = sections } },
						Code = logCode,
						Level = logLevel
					};
				}
			}

			return mdFiles;
		}

		private static bool SaveMD(string outputPath, MD md)
		{
			// write only the ones that have description and solution sections
			if (md.Scenarios.Any(scenario => scenario.Sections.Any(s => s.Title == "Issue" && !string.IsNullOrEmpty(s.Content))) &&
				md.Scenarios.Any(scenario => scenario.Sections.Any(s => s.Title == "Solution" && !string.IsNullOrEmpty(s.Content))))
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
