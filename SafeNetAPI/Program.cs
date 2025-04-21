using SafeNetAPI.Data;
using Microsoft.EntityFrameworkCore;
using SafeNetAPI.Services.Request;
using SafeNetAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeNetAPI.Services.User;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

//builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.MapIdentityApi<User>();

app.MapPost("/logout", async (SignInManager<User> SignInManager, [FromBody]object empty ) => {
    await SignInManager.SignOutAsync();
    return Results.Ok();
});
app.Run();
