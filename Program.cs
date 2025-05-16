using webApiC.Middlewares;
using webApiC.Repositories;
using webApiC.Services;

namespace webApiC;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        // builder.Services.AddSwaggerGen();
        
        builder.Services.AddScoped<IVisitRepository, VisitRepository>();
        builder.Services.AddScoped<IVisitService, VisitService>();

        var app = builder.Build();
        
        app.UseGlobalExceptionHandling();

        if (app.Environment.IsDevelopment())
        {
            // app.UseSwagger();
            // app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}