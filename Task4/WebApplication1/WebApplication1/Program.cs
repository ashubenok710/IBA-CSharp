global using Microsoft.EntityFrameworkCore;
global using WebApplication1.Model;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

//DI https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-6.0&tabs=visual-studio
builder.Services.AddDbContext<DBMoviesContext>();

//Is the repository pattern useful with Entity Framework Core?
//https://www.thereformedprogrammer.net/is-the-repository-pattern-useful-with-entity-framework-core/

//https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application

//EntityFramework vs EntityFramework Core???

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
