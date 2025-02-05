using ApiNotification.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer("ConnectionString"));

        builder.Services.AddSingleton(new KafkaProducer("localhost:8080")); // Configure seu servidor Kafka aqui
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        app.UseRouting();
        

        var cancellationTokenSource = new CancellationTokenSource();
        var consumer = new NotificationConsumer("notifications", "localhost:8080");
        Task.Run(() => consumer.StartConsuming(cancellationTokenSource.Token));

    }
}
