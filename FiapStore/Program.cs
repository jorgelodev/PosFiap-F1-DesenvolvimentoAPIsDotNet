using FiapStore.Interface;
using FiapStore.Logging;
using FiapStore.Repository;
using FiapStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    //CONFIGURANDO ARQUIVO DE DOCUMENTAÇAO DO SUMMARY
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FiapStore", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    //CONFIGURANDO O SWAGGER PARA RECEBER O JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header - utilizado com Bearer Authentication." + Environment.NewLine +
        "Digite 'Bearer' [espaço] e então seu token no campo abaixo" + Environment.NewLine +
        "Exemplo (informar sem as aspas): 'Bearer 1234abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
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
            Array.Empty<string>()
        }
    });
});



//builder.Services.AddSingleton<IUsuarioRepository, UsuarioRepository>(); DAPPER
builder.Services.AddScoped<IUsuarioRepository, EFUsuarioRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Scoped);

// CONFIGURAÇÃO LOGGING
builder.Logging.ClearProviders();
builder.Logging.AddProvider(
    new CustomLoggerProvider(
        new CustomLoggerProviderConfiguration()
        {
            LogLevel = LogLevel.Information
        }
        ));

// CONFIGURAÇÃO JWT
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Secret"));

builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseReDoc(c =>
{
    c.DocumentTitle = "REDOC FiapStore API";
    c.RoutePrefix = "";
});

app.UseHttpsRedirection();

// AUTENTICAÇÃO TEM QUE SER SEMPRE ANTES DE AUTORIZAÇÃO
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
