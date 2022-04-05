namespace jsonserver.Data.Entities
{
    public class Json
    {
        public int JsonId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
