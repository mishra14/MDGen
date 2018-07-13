
using System.Text;

namespace MDGen.Library
{
	internal class Section
	{
		public string Title { get; set; }

		public string Content { get; set; }

		public override string ToString()
		{
			var builder = new StringBuilder();

			builder.AppendLine($"### {Title}");
			builder.AppendLine();
			builder.AppendLine(Content);

			return builder.ToString();
		}
	}
}
