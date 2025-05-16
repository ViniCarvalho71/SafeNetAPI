using Microsoft.AspNetCore.Authentication.Cookies;
using SafeNetAPI.Data;
using Microsoft.EntityFrameworkCore;
using SafeNetAPI.Services.Request;
using SafeNetAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeNetAPI.Services.User;
using SafeNetAPI.Services.Token;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Porta onde o React está rodando
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Se você usar cookies ou autenticação baseada em sessão
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRequestInterface, RequestService>();
builder.Services.AddScoped<IUserInterface, UserService>();



builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // ajuste conforme sua rota
        options.Cookie.SameSite = SameSiteMode.None; // ESSENCIAL em dev entre domínios diferentes
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // coloque Always em produção com HTTPS
    });

//builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddIdentityApiEndpoints<UserModel>().AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");
app.UseHttpsRedirection();
app.UseMiddleware<ApiTokenAuthenticationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<UserModel>();

app.MapPost("/logout", async (SignInManager<UserModel> SignInManager, [FromBody]object empty ) => {
    await SignInManager.SignOutAsync();
    return Results.Ok();
});
app.Run();
