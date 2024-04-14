using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProcureHub_ASP.NET_Core.Filters;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Respositories;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using ProcureHub_ASP.NET_Core.SignalR;
using System.Text;

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHangfire((sp, config) =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseStorage(
                new MySqlStorage(
                    "server=localhost;database=procurehub;uid=root;pwd=Golla@189;Allow User Variables=True",
                    new MySqlStorageOptions
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(10),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 25000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "Hangfire",
                    }
                )
            );
    //config.UseSqlServerStorage(@"Data Source=127.0.0.1:3306;Initial Catalog=procurehub;User Id=root;Password=Golla@189; Integrated Security=SSPI;Trusted_Connection=True;TrustServerCertificate=True;");
});

builder.Services.AddHangfireServer();


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
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddSingleton<IConnectionDriver, Neo4jDriver>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddTransient<ISupplierService, SupplierService>();
builder.Services.AddTransient<ILiveService, LiveService>();
builder.Services.AddTransient<ISupplierRepository, SupplierRepository>();
builder.Services.AddTransient<IEventService,EventService>();
builder.Services.AddTransient<IEventRepository, EventRepository>();
builder.Services.AddTransient<ILotsRepository, LotsRepository>();
builder.Services.AddTransient<ILotsService, LotsService>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddTransient<ExtractInfoFilter>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Angular",
     builder =>
     {
         builder.
          AllowAnyMethod()
         .AllowAnyHeader()
         .AllowAnyOrigin();

     });
});
builder.Services.AddProblemDetails();
builder.Services.AddRouting(options=> options.LowercaseUrls = true);
builder.Services.AddSignalR();


//builder.Services.AddDistributedRedisCache(options =>
//{
//    options.Configuration = "127.0.0.1:6379";
//});

builder.Services.AddSingleton<IRedisConnection, RedisConnection>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHangfireDashboard("/hangfire");
app.UseCors(builder =>
    builder.WithOrigins("*")
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AuctionHub>("/auction");
app.Run();

