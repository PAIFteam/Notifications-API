namespace Notifications.API.Extensions;

public static class UserEndpointsExtensions
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");

        api.MapGet("/health", () => Results.Ok(new
        {
            Service = "notifications-api",
            Status = "Healthy"
        }))
        .WithName("GetNotificationsHealth")
        .WithSummary("Health check do serviço de notificações")
        .Produces(StatusCodes.Status200OK);
    }
}
