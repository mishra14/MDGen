using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IEnumerable.ForEach;
using MDGen.Library;
using NuGet.Common;

namespace MDGen.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			var src = @"F:\validation\demo\codes.xlsx";
			var dest = @"F:\validation\demo\docs";
			var docsDest = @"E:\docs.microsoft.com-nuget\docs\reference\errors-and-warnings";

			if (Directory.Exists(dest))
			{
				Console.WriteLine($"Deleting old md files from {dest}");
				Directory.GetFiles(dest).ForEach(e => File.Delete(e));
			}
			else
			{
				Directory.CreateDirectory(dest);
			}

			Console.WriteLine($"Generating new docs at {dest}");
			var mdFiles = Converter.FromCSV(src, "Pack", dest);

			Console.WriteLine($"Moving new docs from {dest} to {docsDest}");
			Directory.GetFiles(dest).ForEach(e => File.Copy(e, Path.Combine(docsDest, Path.GetFileName(e)), overwrite: true));

			var packErrorTableEntry = GenerateTableEntry(mdFiles, LogLevel.Error, $"Pack Errors");
			var packWarningTableEntry = GenerateTableEntry(mdFiles, LogLevel.Warning, $"Pack Warnings");
			var tocEntries = GenerateTocEntries(mdFiles);

			Console.WriteLine($"Table contents -");
			Console.WriteLine(packErrorTableEntry);
			Console.WriteLine();
			Console.WriteLine(packWarningTableEntry);

			Console.WriteLine($"TOC contents -");
			Console.WriteLine(tocEntries);
		}

		private static string GenerateTableEntry(IList<MD> mdFiles, LogLevel level, string column)
		{
			var builder = new StringBuilder();
			var group = column;
			builder.Append($"| {group} | ");
			mdFiles.Where(m => m.Level == level).ForEach(m => builder.Append($"[{m.Code}](./errors-and-warnings/{m.Code}.md)"));
			return builder.ToString();
		}

		private static string GenerateTocEntries(IList<MD> mdFiles)
		{
			var builder = new StringBuilder();
			mdFiles.ForEach(m => builder.AppendLine($"### [NuGet {m.Level} {m.Code}](reference/errors-and-warnings/{m.Code}.md)"));
			return builder.ToString();
		}
	}
}