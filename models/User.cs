using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project2.models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Login { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Role {  get; set; } = "Admin";
        [MaxLength(200)]
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
