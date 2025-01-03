using Infraestructure.Data;
using Microsoft.OpenApi.Models;
using Transversal.Common;
using Transversal.Logging;

public partial class Program{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "My API",
                Version = "v1"
            });

            // Definir el esquema de seguridad Bearer
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. 
                        \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.
                        \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Agregar requisito de seguridad global
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
            });
        });
        builder.Services.AddControllers();


        builder.Services.AddScoped<IRestClient, RestClient>();
        builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
        builder.Services.AddSingleton(typeof(IAppLogger), typeof(LoggerAdapter));

        builder.Services.AddHttpClient();


        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.MapControllers();


        app.Run();
    }
}

public partial class Program{ }