using FluentValidation;
using FluentValidation.AspNetCore;
using GestionSucursalesAPI.Infraestructure.Repository;
using GestionSucursalesAPI.Infraestructure.Services.AuditService;
using GestionSucursalesAPI.Infraestructure.Services.IdentityService;
using GestionSucursalesAPI.Infraestructure.Services.JWT;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


if (Convert.ToBoolean(builder.Configuration["EnableSwagger"].ToString()))
{

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = builder.Configuration["IdentificadorApp"].ToString(), Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Escriba la palabra 'Bearer', luego un espacio y luego el token JWT",
            Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                     },
                    new List<string>()
                }
            });
    });
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
                ValidIssuer = builder.Configuration["Jwt:Issuer"]
            };
        });

builder.Services.AddDbContext<SucursalesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SucursalesDB")));


//builder.Services.AddDbContext<SucursalesDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SucursalesDB"));
//    // Verificar si la migración es necesaria
//    using (var scope = builder.Services.BuildServiceProvider().CreateScope())
//    {
//        var serviceProvider = scope.ServiceProvider;
//        var dbContext = serviceProvider.GetRequiredService<SucursalesDbContext>();
//        dbContext.Database.Migrate();
//    }
//});

Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.Enrich.WithProperty("Application", builder.Configuration["IdentificadorApp"].ToString())
.Enrich.FromLogContext()
.WriteTo.Console(new JsonFormatter())
.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<IUserIdentity, UserIdentity>();

builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
