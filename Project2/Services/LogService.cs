using Project2.DAL;
using Project2.Models;

namespace Project2.Services
{
	public class LogService
	{
		private readonly AppDbContext _context;

		public LogService(AppDbContext context)
		{
			_context = context;
		}

		public void SetLog(string message,string name) { 
			Log log = new Log();
			log.Name = name;
			log.Message = message;
			log.CreatedAt = DateTime.Now;

			_context.Logs.Add(log);
			_Save();
		}

		public List<Log> GetLogs() 
		{
			var logs = _context.Logs.ToList();
			logs.Reverse();

			return logs;
		}

		private void _Save() { 
			_context.SaveChanges();
		}
	}
}
