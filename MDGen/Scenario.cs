using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDGen.Library
{
	public class Scenario
	{
		public string Pretext { get; set; }

		public IList<Section> Sections { get; set; }

		public override string ToString()
		{
			var builder = new StringBuilder();

			if (!string.IsNullOrEmpty(Pretext))
			{
				builder.AppendLine($"<pre>{Pretext}</pre>");
			}

			foreach (var section in Sections)
			{
				builder.AppendLine();
				builder.AppendLine(section.ToString());
			}

			return builder.ToString();
		}
	}
}
