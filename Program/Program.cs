using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
 // Importação essencial para os modelos do OpenAPI
using ProjetoMIragnum;
using ProjetoMIragnum.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Serviços
builder.Services.AddControllers();

// CORREÇÃO 1: .AddEndpointsApiExplorer() foi removido pois não é mais necessário no .NET 10

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Digite: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // CORREÇÃO 2: Atualização do OpenApiReference utilizando a nova sintaxe lambda exigida no .NET 10
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(DefaultConnection));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
             options.Events = new JwtBearerEvents
             {
                 OnForbidden = async context =>
                 {
                     context.Response.StatusCode = StatusCodes.Status403Forbidden;
                     context.Response.ContentType = "application/json";

                     await context.Response.WriteAsJsonAsync(new
                     {
                         mensagem = "Você não tem permissão para acessar este recurso."
                     });
                 }
             };

        });
    


builder.Services.AddAuthorization();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LoggingMiddleware>();
app.MapControllers();

app.Run();
