using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace jsonserver.Web.ViewModels
{
    public class EditJsonViewModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(10)]
        [RegularExpression("^[a-zA-Z0-9]{4,10}$")]
        public string Name { get; set; }

        [Required]
        public int JsonId { get; set; }
    }
}
