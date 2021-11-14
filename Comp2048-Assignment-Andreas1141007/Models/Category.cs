using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comp2048_Assignment_Andreas1141007.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category ID is required")]
        public string GameName { get; set; }
    }
}
