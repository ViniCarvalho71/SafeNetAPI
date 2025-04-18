namespace SafeNetAPI.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string Agent { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
        public string Ip { get; set; }
        public DateTime Date { get; set; }

        /*        public string UserId { get; set; }  // FK para Identity
                public ApplicationUser User { get; set; }  // navegação*/
    }
}
