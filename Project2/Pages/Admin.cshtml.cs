using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project2.Models;
using Project2.Services;

namespace Project2.Pages
{
	public class AdminModel : PageModel
	{
		private UserService _userService;
		private FileService _fileService;
		private LogService _logService;

		private List<User> _users;

		public AdminModel(Project2.DAL.AppDbContext context)
		{
			_userService = new UserService(context);
			_fileService = new FileService(context);
			_logService = new LogService(context);
		}

		public IActionResult OnPostLoadDatas(Dictionary<String,String> val)
		{
			string token = val["token"];

			_users = _userService.GetAllUsers();

			User? me = _users.FirstOrDefault(u => u.Token == token);

			if (me == null || me.IsAdmin==0)
			{
				return new JsonResult("500");
			}

			List<Dictionary<String,String>> names = _users.Select(u =>
			{
				Dictionary<String,String> dic = new Dictionary<String, String>();
				
				int count = _userService.GetFileCount(u.Token);

				dic["name"] = u.Name;
				dic["fileCount"] = count.ToString();
				dic["token"] = u.Token;

				return dic;
			}).ToList();

			Dictionary<String,Object> result = new Dictionary<String,Object>();

			result["users"] = names;
			result["name"] = me.Name;
			var logs = _GetLogs();
			result["logs"] = logs;

			result["userCount"] = _users.Count.ToString();

			string todayDate = DateTime.Now.ToShortDateString();
			var newUsers = _users.Select(u => u.CreatedAt.ToShortDateString() == todayDate).ToList();
			result["newUsersCount"] = newUsers.Count.ToString();


			string fileCount = _fileService.FilesCount();
			result["fileCount"] = fileCount;

			string fileSize = _fileService.FilesSize();
			result["fileSize"] = fileSize;

			return new JsonResult(result);
		}

		private List<String> _GetLogs()
		{
			var logs = _logService.GetLogs();

			var logsString = logs.Select(l =>
			{
				String txt = "";

				txt += l.CreatedAt.ToString() + " - " + l.Name + " - " + l.Message;

				return txt;
			}).ToList();

			return logsString;
		}

		public IActionResult OnPostDeleteUser(Dictionary<String, String> val)
		{
			string token = val["token"];

			_userService.DeleteUser(token);
			_fileService.RemoveUserFiles(token);

			return new JsonResult("200");
		}
	}
}
