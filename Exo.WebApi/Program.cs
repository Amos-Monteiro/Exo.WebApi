using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme = "jwBearer";
    options.DefaultChallengeScheme = "jwtBearer";
})
 .AddJwtBearer("JwBearer", options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,

        ValidateAudience = true, 
        ValidateLifetime = true,
        IssuerSigningKey = new 
        SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("Exoapi-chave-autenticacao")),
        ClockSkew =TimeSpan.FromMinutes(30),
        ValidAudience = "exoapi.wbapi"


    };
 });


builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
