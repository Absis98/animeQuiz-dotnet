using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MyWebAPI.Data; // Import necessary namespaces
using MyWebAPI.Services; // Import necessary namespaces
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<QuizSessionService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin() // Allow requests from any origin (not recommended for production)
           .AllowAnyMethod() // Allow any HTTP method
           .AllowAnyHeader(); // Allow any HTTP header
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
