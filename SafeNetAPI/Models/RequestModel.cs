using Microsoft.AspNetCore.Identity;

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

        // Chave estrangeira para o Identity User
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
