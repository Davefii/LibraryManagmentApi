using BusinessLayer.Services;
using BusinessLayer.Services.Interface;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using LibraryApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;
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
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["access_token"];

                Console.WriteLine(
                    $"Access Token Cookie Exists: {!string.IsNullOrEmpty(token)}"
                );
                context.Token =
                    context.Request.Cookies["access_token"];

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(
                    $"JWT Authentication Failed: {context.Exception.Message}"
                );

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

//RateLimiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
});

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
builder.Services.AddScoped<IBookRepository,BookRepository>();
builder.Services.AddScoped<IBookService,BookService>();
//AuthorRepository and AuthorService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<IAuthorRepository,AuthorRepository>();
builder.Services.AddScoped<IAuthorService,AuthorService>();
//CategoryRepository and CategoryService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
//UserRepository and UserService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
//RefreshToken
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
//UserProfileRepository and UserProfileService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
//MemberRepository and MemberService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();
//BorrowingRepository and BorrowingService are added to the DI container with Scoped lifetime, which means a new instance will be created for each HTTP request.
builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();
//Dashbaord
builder.Services.AddScoped<IDashboardRepository, DashbaordRepositry>();
builder.Services.AddScoped<IDashboardService, DashbaordService>();
// Auditing
builder.Services.AddScoped<IAudit, AuditRepository>();
builder.Services.AddScoped<IAuditService,AuditService>();
//ImageService
builder.Services.AddScoped<ImageService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Librarypolicy",
        policy =>
        {
            policy.WithOrigins(
                "https://127.0.0.1:5500",
                "http://127.0.0.1:5500"
            )
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("Librarypolicy");

app.UseRateLimiter();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
    {
        await context.Response.WriteAsync("Too many login attempts. Please try again later.");
    }
});

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
