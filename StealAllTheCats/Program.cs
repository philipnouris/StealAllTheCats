using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using StealAllTheCats.Data;
using StealAllTheCats.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Win32;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

//Register SQL Server
builder.Services.AddDbContext<StealAllTheCatsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddHttpClient();
builder.Services.AddScoped<CatService>();
//Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Steal All The Cats API");
        c.RoutePrefix = "swagger"; // Ensures Swagger is available at swagger
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();