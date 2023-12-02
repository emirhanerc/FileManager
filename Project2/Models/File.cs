namespace Project2.Models
{
	public class FileModel
	{
		public int Id { get; set; }
		public string UserToken { get; set; }
		public int Size { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
