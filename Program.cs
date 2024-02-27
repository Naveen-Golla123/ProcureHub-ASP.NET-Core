using Microsoft.OpenApi.Models;
using ProcureHub_ASP.NET_Core.Respositories;
using ProcureHub_ASP.NET_Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Middleware;
using ProcureHub_ASP.NET_Core.Helper;

var builder = WebApplication.CreateBuilder(args);

// -------Add services to the container.---------


// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ProcureHUB", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
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
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddSingleton<IConnectionDriver, Neo4jDriver>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddSingleton<ISupplierService, SupplierService>();
builder.Services.AddSingleton<ISupplierRepository, SupplierRepository>();
builder.Services.AddSingleton<IEventService,EventService>();
builder.Services.AddSingleton<IEventRepository, EventRepository>();
//builder.Services.Add
//builder.Services.AddScoped<InfoExtracter>();

builder.Services.AddProblemDetails();
builder.Services.AddRouting(options=> options.LowercaseUrls = true);


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("http://example.com",
//                                              "http://www.contoso.com");
//                      });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder =>
    builder.WithOrigins("*")
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
