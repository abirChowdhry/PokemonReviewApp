using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//For Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IPokemonRepo, PokemonRepo>(); // For Interface & Repository of Pokemon
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>(); // For Interface & Repository of Category
builder.Services.AddScoped<ICountryRepo, CountryRepo>(); // For Interface & Repository of Country
builder.Services.AddScoped<IOwnerRepo, OwnerRepo>(); // For Interface & Repository of Owner
builder.Services.AddScoped<IReviewRepo, ReviewRepo>(); // For Interface & Repository of Review
builder.Services.AddScoped<IReviewerRepo, ReviewerRepo>(); // For Interface & Repository of Reviewer

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//For Sql Server
builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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