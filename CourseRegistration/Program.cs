using CourseRegistration.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services
    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddControllersWithViews();

//builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");

builder.Services
    .AddLocalization(opt => { opt.ResourcesPath = "Resources"; }); // add Localization and the Resouce folder

builder.Services
    .AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(); // needs for localization

builder.Services
    .Configure<RequestLocalizationOptions>(opt =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("sv"),
            new CultureInfo("fi"),
            new CultureInfo("en"),
            new CultureInfo("fr"),
            new CultureInfo("es"),
            new CultureInfo("it"),
            new CultureInfo("de")
        };
        opt.DefaultRequestCulture = new RequestCulture("en");
        opt.SupportedCultures = supportedCultures;
        opt.SupportedUICultures = supportedCultures;
    });

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//var supportedCultures = new[] { "en", "fi", "en", "fr", "es", "it", "de" };

//var localizationOptions = new RequestLocalizationOptions()
//    .AddSupportedCultures(supportedCultures)  // format of number and currencies.
//    .AddSupportedUICultures(supportedCultures) // resources
//    .SetDefaultCulture(supportedCultures[3]);  // if we request a culture that is not supported, use this default language

//app.UseRequestLocalization(localizationOptions);

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
