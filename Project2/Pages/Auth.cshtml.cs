using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project2.Models;
using Project2.Services;
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

        public IActionResult OnPostGetAjax(Dictionary<String,String> val)
        {
            User? u = _userService.GetUserByUsernameAndPassword(val["username"], val["password"]);

            if (u==null)
            {
                return new JsonResult("error");
            }

            return new JsonResult("succesed");
        }

        
    }
}
