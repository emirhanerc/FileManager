using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project2.Models;
using Project2.Services;
using System.Text;
using System.Text.Json.Nodes;

namespace Project2.Pages
{
    public class AuthModel : PageModel
    {
        private UserService _userService;

        public AuthModel(Project2.DAL.AppDbContext context)
        {
            _userService = new UserService(context);
        }

        public IActionResult OnPostLogin(Dictionary<String,String> val)
        {
            string username = val["username"];
            string password = val["password"];

            User? u = _userService.GetUserByUsernameAndPassword(username, password);

            if (u==null)
            {
                return new JsonResult("Username veya password hatalý!");
            }

            return new JsonResult("succesed");
        }

        public IActionResult OnPostRegister(Dictionary<String, String> val)
        {
            string username = val["username"];
            string password = val["password"];
            string email = val["email"];

            User user = new User();
            user.Name = username;
            user.Password = password;
            user.Email = email;
            user.IsAdmin = 0;
            string token = _GenerateRandomString();
            user.Token = token;

            _userService.SetUser(user);

            
            //burda 200 döndürüyoruz. yani OK demek
            return new JsonResult("200");
        }

        private string _GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            StringBuilder randomStringBuilder = new StringBuilder(15);
            Random random = new Random();

            for (int i = 0; i < 15; i++)
            {
                randomStringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return randomStringBuilder.ToString();
        }


    }
}
