using System;
using System.IO;
using IEnumerable.ForEach;
using MDGen.Library;

namespace MDGen.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			var src = @"F:\validation\demo\codes.xlsx";
			var dest = @"F:\validation\demo\docs";

			if (Directory.Exists(dest))
			{
				Console.WriteLine("Deleting old md files...");
				Directory.GetFiles(dest).ForEach(e => File.Delete(e));

			}
			else
			{
				Directory.CreateDirectory(dest);
			}

			Converter.FromCSV(src, "Pack", dest);
		}
	}
}
