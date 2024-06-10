using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Net8_JWT.WebAPI.Middlewares;
using Net8_JWT.WebAPI.Services;
using Net8_JWT.WebAPI.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT_TokenSettings"));

var provider = builder.Services.BuildServiceProvider();
var jwtSettings = provider.GetRequiredService<IOptionsMonitor<JwtOptions>>();

builder.Services.AddControllers();


#region Swagger with JWT configuration
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Bearer Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
      {
        new OpenApiSecurityScheme
         {
           Reference = jwtSecuritySheme.Reference,
         },
         new string[] {}
      }
     });
});
#endregion


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.CurrentValue.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.CurrentValue.Audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.CurrentValue.SecretKey))
    };
});

builder.Services.AddScoped<JwtProvider>();

var app = builder.Build();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMyCustomMiddlewares();

app.MapControllers();

app.Run();
