using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MVC3ET.Models
{
    public class Product
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")] public string Name { get; set; }

        [Required(ErrorMessage = "Price is required"), Range(0, double.MaxValue)] public double Price { get; set; }

        [ValidateNever, DisplayName("Category")] public int CategoryId { get; set; }

        [ValidateNever,ForeignKey("CategoryId")] public Category Category { get; set; }
    }
}
