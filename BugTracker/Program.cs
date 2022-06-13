using BugTracker.DAL;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();;

// Add repository singleton
builder.Services.AddScoped<IRepositoryCRUD<Project>, ProjectRepository>();
builder.Services.AddScoped<IRepositoryCRD<ProjectUser>, ProjectUserRepository>();
builder.Services.AddScoped<IRepositoryCRU<Ticket>, TicketRepository>();
builder.Services.AddScoped<IRepositoryCR<TicketLogItem>, TicketLogItemRepository>();
builder.Services.AddScoped<IRepositoryCRUD<TicketAttachment>, TicketAttachmentRepository>();
builder.Services.AddScoped<IRepositoryCRUD<TicketComment>, TicketCommentRepository>();
builder.Services.AddScoped<IRepositoryCR<TicketHistory>, TicketHistoryRepository>();
builder.Services.AddScoped<IRepositoryCRUD<TicketNotification>, TicketNotificationRepository>();
builder.Services.AddScoped<IRepositoryCRUD<User>, UserRepository>();

// Add services to the container.

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using ( var scope = app.Services.CreateScope() )
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
} else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
