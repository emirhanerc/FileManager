﻿namespace Project2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public int IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
