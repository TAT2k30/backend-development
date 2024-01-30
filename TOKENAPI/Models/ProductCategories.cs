﻿namespace BackEndDevelopment.Models
{
    public class ProductCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
