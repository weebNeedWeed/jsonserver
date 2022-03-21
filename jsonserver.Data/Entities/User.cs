using System.Collections.Generic;

namespace jsonserver.Data.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public ICollection<Json> Jsons { get; set; }
    }
}
