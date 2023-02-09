using BidProjectsManager.DataLayer;
using BidProjectsManager.Logic;
using BidProjectsManager.Mappings;
using BidProjectsManager.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "App.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    Array.Empty<string>()
                }
                });
});

builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddMappings();
builder.Services.AddValidation();
builder.Services.AddLogic();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.Authority = builder.Configuration["Jwt:Authority"];
    o.Audience = builder.Configuration["Jwt:Audience"];
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = ClaimTypes.Role
    };

    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();
            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";

            if (builder.Environment.IsDevelopment())
            {
                return c.Response.WriteAsync(c.Exception.ToString());
            }

            return c.Response.WriteAsync("An error occured processing your authentication.");
        }
    };
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("Administrator", policy =>
       policy.RequireAssertion(c =>
            JsonSerializer.Deserialize<Dictionary<string, string[]>>(c.User?.FindFirst((claim) => claim?.Type == "realm_access")?.Value ?? "{}")?
                 .FirstOrDefault().Value?.Any(v => v == "Administrator") ?? false));

    options.AddPolicy("Reader", policy =>
       policy.RequireAssertion(c =>
            JsonSerializer.Deserialize<Dictionary<string, string[]>>(c.User?.FindFirst((claim) => claim?.Type == "realm_access")?.Value ?? "{}")?
                 .FirstOrDefault().Value?.Any(v => v == "Reader" || v == "Administrator" || v == "Editor" || v == "Reviewer") ?? false));

    options.AddPolicy("Editor", policy =>
        policy.RequireAssertion(c =>
            JsonSerializer.Deserialize<Dictionary<string, string[]>>(c.User?.FindFirst((claim) => claim?.Type == "realm_access")?.Value ?? "{}")?
                 .FirstOrDefault().Value?.Any(v => v == "Editor" || v == "Administrator") ?? false));

    options.AddPolicy("Reviewer", policy =>
        policy.RequireAssertion(c =>
            JsonSerializer.Deserialize<Dictionary<string, string[]>>(c.User?.FindFirst((claim) => claim?.Type == "realm_access")?.Value ?? "{}")?
                .FirstOrDefault().Value?.Any(v => v == "Reviewer" || v == "Administrator") ?? false));
});

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials()
    );
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var databaseInitializer = app.Services.GetService<IDataInitializer>();
if(databaseInitializer!= null)
{
    await databaseInitializer.MigrateAsync();
    await databaseInitializer.SeedAsync();
}

app.Run();
