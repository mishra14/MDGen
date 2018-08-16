using NuGet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MDGen.Library
{
	public class MD
	{
		public LogLevel Level { get; set; }

		public NuGetLogCode Code { get; set; }

		public string Path { get; private set; }

		public Header Header { get; set; }

		public IList<Scenario> Scenarios { get; set; }

		public void Save(string path, bool overwrite = true)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("---");
			builder.Append(Header.ToString());
			builder.AppendLine("---");
			builder.AppendLine();
			builder.AppendLine($"# {Header.Title}");

			if (Scenarios != null)
			{
				if (Scenarios.Count < 2)
				{
					foreach (Scenario scenario in Scenarios)
					{
						builder.AppendLine();
						builder.AppendLine(scenario.ToString());
					}
				}
				else
				{
					for (int i = 0; i < Scenarios.Count; i++)
					{
						var scenario = Scenarios[i];
						builder.AppendLine();
						builder.AppendLine($"## Scenario {i+1}");
						builder.AppendLine();
						builder.AppendLine(scenario.ToString());
					}
				}
			}

			foreach (Scenario scenario in Scenarios)
			{
				builder.AppendLine();
				builder.AppendLine(scenario.ToString());
			}

			if (builder.Length > 0)
			{
				if (File.Exists(path) && !overwrite)
				{
					throw new InvalidOperationException($"File {path} already exists.");
				}

				File.WriteAllText(path, builder.ToString());
				Path = path;
			}
		}
	}
}
