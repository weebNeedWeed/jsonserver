using System.ComponentModel.DataAnnotations;

namespace jsonserver.Data.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
