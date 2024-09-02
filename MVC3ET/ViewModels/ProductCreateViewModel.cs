using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MVC3ET.Models;

namespace MVC3ET.ViewModels
{
    public class ProductCreateViewModel
    {
        public Product Products { get; set; }
        [ValidateNever]
        public List<Category> Categories { get; set; }
    }
}
