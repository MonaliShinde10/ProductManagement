﻿using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models.DomainModel
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }


    }
}
