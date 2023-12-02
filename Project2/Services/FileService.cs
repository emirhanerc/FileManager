using Project2.DAL;
using Project2.Models;
using Project2.Public;

namespace Project2.Services
{
	public class FileService
	{
		private readonly AppDbContext _context;

		public FileService(AppDbContext context)
		{
			_context = context;
		}

		public string FilesCount()
		{
			int count = _context.Files.Count();

			return count.ToString();
		}

		public string FilesSize() {
			var files = _context.Files;

			int size = 0;

            foreach (var item in files)
            {
                size += item.Size;
            }

			string sizeString = Utils.ConvertBytesToHumanReadable(size);
			return sizeString;
        }

		public List<FileModel> GetFilesByToken(string token) { 
			return _context.Files.Where(f => f.UserToken == token).ToList();
		}

		public void DeleteFileAt(string token,int index) {
			var files = GetFilesByToken(token);
			var file = files[index];

			_context.Files.Remove(file);
			_Save();
		}

		public void RemoveUserFiles(string token)
		{
			var files = GetFilesByToken(token);

            foreach (var item in files)
            {
				_context.Files.Remove(item);
            }

			_Save();
		}

		public void AddFile(FileModel file) {
			_context.Files.Add(file);
			_Save();
		}

		private void _Save()
		{
			_context.SaveChanges();
		}
	}
}
