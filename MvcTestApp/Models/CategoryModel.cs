using System.ComponentModel.DataAnnotations;

namespace MvcTestApp.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

    }
}