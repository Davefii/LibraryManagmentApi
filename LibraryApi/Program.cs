using BusinessLayer.Services;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using LibraryApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

var jwtSecret =
    Environment.GetEnvironmentVariable(
        "LIBRARY_JWT_SECRET");

if (string.IsNullOrEmpty(jwtSecret))
{
    throw new Exception(
        "JWT Secret Not Found");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TokenValidationParameters define how incoming JWTs will be validated.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Ensures the token was issued by a trusted issuer.
            ValidateIssuer = true,


            // Ensures the token is intended for this API (audience check).
            ValidateAudience = true,


            // Ensures the token has not expired.
            ValidateLifetime = true,


            // Ensures the token signature is valid and was signed by the API.
            ValidateIssuerSigningKey = true,


            ValidIssuer = "LibraryApi",

            ValidAudience = "LibraryUsers",

            IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // ===============================
    // 1) Define the JWT Bearer security scheme
    // ===============================
    //
    // This tells Swagger that our API uses JWT Bearer authentication
    // through the HTTP Authorization header.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // The name of the HTTP header where the token will be sent.
        Name = "Authorization",


        // Indicates this is an HTTP authentication scheme.
        Type = SecuritySchemeType.Http,


        // Specifies the authentication scheme name.
        // Must be exactly "Bearer" for JWT Bearer tokens.
        Scheme = "Bearer",


        // Optional metadata to describe the token format.
        BearerFormat = "JWT",


        // Specifies that the token is sent in the request header.
        In = ParameterLocation.Header,


        // Text shown in Swagger UI to guide the user.
        Description = "Enter: Bearer {your JWT token}"
    });


    // ===============================
    // 2) Require the Bearer scheme for secured endpoints
    // ===============================
    //
    // This tells Swagger that endpoints protected by [Authorize]
    // require the Bearer token defined above.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                // Reference the previously defined "Bearer" security scheme.
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },


            // No scopes are required for JWT Bearer authentication.
            // This array is empty because JWT does not use OAuth scopes here.
            new string[] {}
        }
    });
});

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
    options.AddPolicy("Librarypolicy",
        policy =>
        {
            policy.WithOrigins(
                " https://localhost:7010",
                "http://localhost:5155"
            )
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
app.UseCors("Librarypolicy");
app.UseHttpsRedirection();

//app.UseMiddleware<ApiKeyMiddleware>();


app.UseAuthorization();

app.MapControllers();

app.Run();
