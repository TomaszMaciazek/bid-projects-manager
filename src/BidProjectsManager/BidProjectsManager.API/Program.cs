using BidProjectsManager.DataLayer;
using BidProjectsManager.Logic;
using BidProjectsManager.Mappings;
using BidProjectsManager.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddMappings();
builder.Services.AddValidation();
builder.Services.AddLogic();

builder.Services.AddCors();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.Authority = builder.Configuration["Jwt:Authority"];
    o.Audience = builder.Configuration["Jwt:Audience"];
    o.Events = new JwtBearerEvents()
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
