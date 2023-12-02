using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project2.Models;
using Project2.Public;
using Project2.Services;

namespace Project2.Pages
{
	public class ManagerModel : PageModel
	{
		private FileService _fileService;
		private UserService _userService;
		private LogService _logService;

		public ManagerModel(Project2.DAL.AppDbContext context)
		{
			_fileService = new FileService(context);
			_userService = new UserService(context);
			_logService = new LogService(context);
		}

		public IActionResult OnPostFiles(Dictionary<String, String> val)
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();

			string token = val["token"];

			if (string.IsNullOrEmpty(token))
			{
				return new JsonResult("300");
			}

			var files = _fileService.GetFilesByToken(token);

			var fileDictionaryList = files.Select(file =>
			{
				string s = Utils.ConvertBytesToHumanReadable(file.Size);
				return new Dictionary<string, object>
				{
					{ "name", file.Name },
					{ "size", s },
					{ "createdAt", file.CreatedAt.ToShortDateString() },
					{ "type",file.Type.Trim() }
				};
			}).ToList();

			dict["files"] = fileDictionaryList;

			var sizes = _calculateSizes(files);

			dict["sizes"] = sizes;

			User? me = _userService.GetUserByToken(token);

			if (me != null && me.IsAdmin == 1)
			{
				dict["isAdmin"] = "true";
			}
			else
			{
				dict["isAdmin"] = "false";
			}

			return new JsonResult(dict);
		}

		public IActionResult OnPostAddFile(Dictionary<String, String> val)
		{
			string name = val["name"];
			string token = val["token"];
			string size = val["size"];

			int sizeInt = int.Parse(size);

			FileModel file = new FileModel();

			file.Name = name;
			file.Size = sizeInt;
			file.UserToken = token;
			file.CreatedAt = DateTime.Now;

			string type = Utils.GetFileType(name);

			file.Type = type;

			_fileService.AddFile(file);

			Dictionary<String, String> r = new Dictionary<string, String>();
			r["code"] = "200";
			r["type"] = type;
			r["size"] = Utils.ConvertBytesToHumanReadable(sizeInt);

			User? me = _userService.GetUserByToken(token);

			if (me != null)
			{
				_logService.SetLog("Dosya ekledi", me.Name);
			}

			return new JsonResult(r);
		}

		public IActionResult OnPostDeleteFile(Dictionary<string, string> val)
		{
			string rowIndexString = val["rowIndex"];
			int rowIndex = int.Parse(rowIndexString);
			//Burada -1 yapýyoruz çünkü List 0 dan baþlar fakat Javascpripte silinen dosyanýn sýralamasý geliyor o da 1 den baþlýyor
			rowIndex = rowIndex - 1;

			string token = val["token"];

			_fileService.DeleteFileAt(token, rowIndex);

			User? me = _userService.GetUserByToken(token);

			if (me != null)
			{
				_logService.SetLog("Dosya sildi", me.Name);
			}

			return new JsonResult("200");
		}

		private Dictionary<string, object> _calculateSizes(List<FileModel> files)
		{
			Dictionary<string, object> sizes = new Dictionary<string, object>();

			int maxSize = 0;
			int maxLength = files.Count;

			int imageSize = 0;
			int imageLength = 0;

			int docSize = 0;
			int docLength = 0;

			int otherSize = 0;
			int otherLength = 0;

			foreach (var file in files)
			{
				string type = file.Type.Trim();

				if (type == "document")
				{
					docLength++;
					docSize += file.Size;
				}
				else if (type == "image")
				{
					imageLength++;
					imageSize += file.Size;
				}
				else
				{
					otherLength++;
					otherSize += file.Size;
				}
			}

			maxSize = imageSize + docSize + otherSize;

			var percantages = _CalculatePercentages(docLength, imageLength, otherLength);

			double docPercant = percantages["val1"];
			double imagePercant = percantages["val2"];
			double otherPercant = percantages["val3"];

			sizes["max"] = new Dictionary<string, object>
			{
				{"size" , Utils.ConvertBytesToHumanReadable(maxSize) },
				{"length", maxLength.ToString() }
			};

			sizes["doc"] = new Dictionary<string, object>
			{
				{"size" , Utils.ConvertBytesToHumanReadable(docSize) },
				{"length", docLength.ToString() },
				{"percant", docPercant.ToString()+"%" }
			};

			sizes["image"] = new Dictionary<string, object>
			{
				{"size" , Utils.ConvertBytesToHumanReadable(imageSize) },
				{"length", imageLength.ToString() },
				{"percant", imagePercant.ToString()+"%" }
			};

			sizes["other"] = new Dictionary<string, object>
			{
				{"size" , Utils.ConvertBytesToHumanReadable(otherSize) },
				{"length", otherLength.ToString() },
				{"percant", otherPercant.ToString() + "%" }
			};

			return sizes;
		}

		private Dictionary<string, double> _CalculatePercentages(double val1, double val2, double val3)
		{
			double max = val1 + val2 + val3;

			double percentageVal1 = (val1 / max) * 100;
			double percentageVal2 = (val2 / max) * 100;
			double percentageVal3 = (val3 / max) * 100;

			Dictionary<string, double> percentages = new Dictionary<string, double>
		{
			{ "val1", percentageVal1 },
			{ "val2", percentageVal2 },
			{ "val3", percentageVal3 }
		};

			return percentages;
		}
	}
}
