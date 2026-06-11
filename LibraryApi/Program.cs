using BusinessLayer.Services;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("LibraryDB")));
//BookRepository and BookService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookService>();
//AuthorRepository and AuthorService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<AuthorService>();
//CategoryRepository and CategoryService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<CategoryService>();
//UserRepository and UserService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
//UserProfileRepository and UserProfileService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<UserProfileRepository>();
builder.Services.AddScoped<UserProfileService>();
//MemberRepository and MemberService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<MemberService>();
//BorrowingRepository and BorrowingService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<BorrowingRepository>();
builder.Services.AddScoped<BorrowingService>();
//Dashbaord
builder.Services.AddScoped<DashbaordService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
