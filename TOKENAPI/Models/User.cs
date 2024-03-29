﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models;
using BackEndDevelopment.Models.OrderProps;

namespace TOKENAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public bool Status { get; set; } = false;
        public DateTime? LastLoginTime { get; set; }
        public string? AvatarUrl { get; set; }

        //Navigation props
        public ICollection<Image>? Image { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
