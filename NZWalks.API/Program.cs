using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using NZWalks.API.Repositories.WalksRepositories;
using System.Text;

using NZWalks.API.Repositories.AccountRepos;
using System.Reflection.Metadata;
using Microsoft.OpenApi.Models;
using NZWalks.API.Repositories.ImagesRepos;
using Microsoft.Extensions.FileProviders;
using Serilog;
using NZWalks.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//adding error logger
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    //.WriteTo.File("Logs/NzWlaks_Log.txt",rollingInterval:RollingInterval.Day)
    .WriteTo.File("Logs/NzWlaks_Log.txt",rollingInterval:RollingInterval.Minute)
    //.MinimumLevel.Information()
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
//Adding for images
builder.Services.AddHttpContextAccessor();

//for injecting databases
builder.Services.AddDbContext<NZWalksDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

// For passing the token like postman but its not needed (only a for devloper for debuing)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});



//injecting repos
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();


//automapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

//Adding identities
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequiredLength = 6;
});
//Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudiences = new[] { builder.Configuration["Jwt:Audience"] },
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//for exception of middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
//Also add these
app.UseAuthentication();
app.UseAuthorization();

//for image
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});


app.MapControllers();

app.Run();