using System;
using MDGen.Library;

namespace MDGen.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			var src = @"C:\Users\anmishr\OneDrive - Microsoft\codes.xlsx";
			var dest = @"C:\Users\anmishr\OneDrive - Microsoft\docs";

			Converter.FromCSV(src, "Pack", dest);
		}
	}
}
