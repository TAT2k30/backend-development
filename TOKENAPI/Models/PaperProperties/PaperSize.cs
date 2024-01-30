﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models.OrderProps;

namespace BackEndDevelopment.Models.PaperProperties
{
    public class PaperSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Dimensions { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Navigation Property
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
