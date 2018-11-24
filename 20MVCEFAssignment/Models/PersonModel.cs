using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _20MVCEFAssignment.Models
{
    public class PersonModel
    {
        [Required]
        public string User_Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public System.DateTime Joined { get; set; }
        public bool Active { get; set; }
    }
}