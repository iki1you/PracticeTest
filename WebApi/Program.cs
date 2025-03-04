using DAL.EF;
using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();

ServiceConfigurer.ConfigureServices(builder.Services);

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
