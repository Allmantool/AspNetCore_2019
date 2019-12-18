using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Northwind.Web.MVC.Models.Categories
{
    public class CategoryEditViewModel
    {
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        [Display(Name = "Add a picture")]
        public IFormFile Picture { get; set; }
    }
}
