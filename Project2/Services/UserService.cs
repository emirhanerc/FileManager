using Project2.DAL;
using Project2.Models;

namespace Project2.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly LogService _logService;

        public UserService(AppDbContext context)
        {
            _context = context;
            _logService = new LogService(context);
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Name == username && u.Password == password);
        }

		public User? GetUserByToken(string token)
		{
			return _context.Users.FirstOrDefault(u => u.Token == token);
		}


		public int GetFileCount(string token)
        {
            var list = _context.Files.Where(f => f.UserToken == token).ToList();

            return list.Count;
		}

        public void SetUser(User user) { 
            _context.Users.Add(user);
            _Save();

            _logService.SetLog("Kayıt oldu",user.Name);
        }

        public void DeleteUser(string token)
        {
            User? user = GetUserByToken(token);

            if (user == null)
            {
                return;
            }

            _context.Users.Remove(user);
            _Save();
        }

        private void _Save() {
            _context.SaveChanges();
        }
    }
}
