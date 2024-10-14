using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Mapping;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var secretKey = builder.Configuration["Jwt:SecretKey"];//Get SercretKey from appsetting.json

// Add services to the container.
var connection = builder.Configuration
                        .GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection String not found");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Config token validate
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RoleClaimType = ClaimTypes.Role,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

//cho vuejs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddHttpClient();
builder.Services.AddControllers();
//Register dependency service
builder.Services.AddScoped<StartupService>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<ReportTypeService>();
builder.Services.AddScoped<ReportTypeRepository>();
builder.Services.AddScoped<ReportTypeService>();
builder.Services.AddScoped<AmenityRepository>();
builder.Services.AddScoped<AmenityService>();
builder.Services.AddScoped<PostAmenityRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CarTypeDetailRepository>();
builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CarTypeRepository>();
builder.Services.AddScoped<CarTypeService>();
builder.Services.AddScoped<IUserRepository, AuthenticationRepository>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<SalePromotionRepository>();
builder.Services.AddScoped<SalePromotionTypeRepository>();
builder.Services.AddScoped<SalePromotionService>();
builder.Services.AddScoped<RatingRepository>();
builder.Services.AddScoped<RatingService>();
builder.Services.AddScoped<FavoriteRepository>();
builder.Services.AddScoped<FavoriteService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Product APIs", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
// Add NewtonsoftJSON for serializing/deserializing JSON
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});


var app = builder.Build();

//Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await ApplicationRole.InitialRoles(services);//Initial default role
    var repository = services.GetRequiredService<SalePromotionTypeRepository>();
    await repository.SeedSalePromotionTypeAsync();//Initial default promotion type
}

//Startup Service
using (var scope = app.Services.CreateScope())
{
    var startupService = scope.ServiceProvider.GetRequiredService<StartupService>();
    await startupService.UpdateBookingsOnStartup();
    await startupService.UpdatePostOnStartup();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();