using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MDGen.Library
{
	internal class MD
	{
		public Header Header { get; set; }

		public string Pretext { get; set; }

		public IList<Section> Sections { get; set; }

		public void Save(string path, bool overwrite = true)
		{
			var builder = new StringBuilder();
			builder.AppendLine("---");
			builder.Append(Header.ToString());
			builder.AppendLine("---");
			builder.AppendLine();
			builder.AppendLine($"# {Header.Title}");

			if (!string.IsNullOrEmpty(Pretext))
			{
				builder.AppendLine($"<pre>{Pretext}</pre>");
			}

			foreach (var section in Sections)
			{
				builder.AppendLine();
				builder.AppendLine(section.ToString());
			}

			if (builder.Length > 0)
			{
				if (File.Exists(path) && !overwrite)
				{
					throw new InvalidOperationException($"File {path} already exists.");
				}

				File.WriteAllText(path, builder.ToString());
			}			
		}
	}
}
