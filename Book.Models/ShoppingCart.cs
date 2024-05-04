using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId"), ValidateNever]
        public Product Product { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }
        [NotMapped]
        public double Price
        {
            get;
            set;
        }
    }
}
