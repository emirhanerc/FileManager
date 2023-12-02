using Microsoft.VisualBasic.FileIO;

namespace Project2.Public
{
	public class Utils
	{
		public static string GetFileType(string fileName)
		{
			string extension = Path.GetExtension(fileName);

			// List of common document extensions
			string[] documentExtensions = { ".doc", ".docx", ".pdf", ".txt" };

			// List of common image extensions
			string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

			if (documentExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
			{
				return "document";
			}
			else if (imageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
			{
				return "image";
			}
			else
			{
				return "other";
			}
		}

		public static string ConvertBytesToHumanReadable(int size)
		{
			string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
			const int unit = 1024;

			int i = 0;

			while (size >= unit && i < sizes.Length - 1)
			{
				size /= unit;
				i++;
			}

			return $"{size:0.##} {sizes[i]}";
		}
	}
}
