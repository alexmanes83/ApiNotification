using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[ApiController]
[Route("notifications")]
public class NotificationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly KafkaProducer _kafkaProducer;
    private const string NotificationTopic = "notifications";

    public NotificationController(AppDbContext context, KafkaProducer kafkaProducer)
    {
        _context = context;
        _kafkaProducer = kafkaProducer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        await _kafkaProducer.SendNotificationAsync(NotificationTopic, notification.Message);
        return CreatedAtAction(nameof(CreateNotification), new { id = notification.Id }, notification);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotifications(int userId)
    {
        var notifications = await _context.Notifications.Where(n => n.UserId == userId && !n.Seen).ToListAsync();
        return Ok(notifications);
    }
}
