using DAL.EF;
using Microsoft.EntityFrameworkCore;
using WebApi.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddSignalR();
// Можно вынести в расширение
BLL.StartupBLL.Configure(builder.Services);
DAL.StartupDAL.Configure(builder.Services);

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/chat");

app.Run();
