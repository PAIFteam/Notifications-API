using Notifications.API.Extensions;
using Notifications.Infra.Extensions;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes("abc123");

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ClockSkew = TimeSpan.Zero
//    };
//});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfraestructure();
builder.Services.AddRabbitMq(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger(c =>
    {
    c.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        var scheme = httpReq.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? httpReq.Scheme;
        var host = httpReq.Headers["X-Forwarded-Host"].FirstOrDefault() ?? httpReq.Host.Value;

        var prefix = httpReq.Headers["X-Forwarded-Prefix"].FirstOrDefault();

        // fallback se não vier do ingress
        if (string.IsNullOrWhiteSpace(prefix))
        {
            prefix = "/notifications";
        }

        swagger.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer>
        {
            new() { Url = $"{scheme}://{host}{prefix}" }
        };
        });
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapUserEndpoints();

app.Run();
