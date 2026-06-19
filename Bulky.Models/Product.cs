using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        [DataType(DataType.Currency)]
        public double? ListPrice { get; set; }

        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        [DataType(DataType.Currency)]
        public double? Price { get; set; }

        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        [DataType(DataType.Currency)]
        public double? Price50 { get; set; }

        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        [DataType(DataType.Currency)]
        public double? Price100 { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category ? Category { get; set; }

        public string? ImageUrl { get; set; }
    }
}