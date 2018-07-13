
using System.Collections.Generic;
using System.Text;

namespace MDGen.Library
{
	internal class Header
	{
		public string Title { get; set; }

		public string Description { get; set; }

		public string Author { get; set; }

		public string MsAuthor { get; set; }

		public string Manager { get; set; }

		public string MsDate { get; set; }

		public string MsTopic { get; set; }

		public string MsReviewer { get; set; }

		public IList<string> F1Keywords { get; set; }

		public override string ToString()
		{
			var builder = new StringBuilder();

			builder.AppendLine($"title: {Title}");
			builder.AppendLine($"description: {Description}");
			builder.AppendLine($"author: {Author}");
			builder.AppendLine($"ms.author: {MsAuthor}");
			builder.AppendLine($"manager: {Manager}");
			builder.AppendLine($"ms.date: {MsDate}");
			builder.AppendLine($"ms.topic: {MsTopic}");
			builder.AppendLine($"ms.reviewer: {MsReviewer}");
			builder.AppendLine($"f1_keywords:");
			foreach (var keyword in F1Keywords)
			{
				builder.AppendLine($"  - {keyword}");
			}

			return builder.ToString();
		}
	}
}
