using System.ComponentModel.DataAnnotations;
namespace ApiNotification.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public bool Seen { get; set; } = false;
    }
}
