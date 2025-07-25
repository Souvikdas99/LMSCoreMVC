﻿using System.ComponentModel.DataAnnotations;

namespace LMSCoreMVC.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
