using System;
using MDGen.Library;

namespace MDGen.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			var src = @"F:\validation\demo\codes.xlsx";
			var dest = @"F:\validation\demo\docs";

			Converter.FromCSV(src, "Pack", dest);
		}
	}
}
