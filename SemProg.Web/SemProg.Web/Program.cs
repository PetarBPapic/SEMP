using Microsoft.EntityFrameworkCore;
using SemProg.DAL;
using SemProg.BLL.Interfejsi;
using SemProg.BLL.Servisi;

var graditelj = WebApplication.CreateBuilder(args);

// Dodaj servise
graditelj.Services.AddControllersWithViews();
graditelj.Services.AddDbContext<BazaPodatakaKontekst>(opcije =>
    opcije.UseSqlServer(graditelj.Configuration.GetConnectionString("PodrazumevanaKonekcija")));

graditelj.Services.AddSession();
graditelj.Services.AddScoped<IServisKorisnika, ServisKorisnika>();
graditelj.Services.AddScoped<IServisEpizoda, ServisEpizoda>();
graditelj.Services.AddScoped<IServisOcena, ServisOcena>();

var aplikacija = graditelj.Build();

// Konfiguriši pipeline
if (!aplikacija.Environment.IsDevelopment())
{
    aplikacija.UseExceptionHandler("/Pocetna/Greska");
    aplikacija.UseHsts();
}

aplikacija.UseHttpsRedirection();
aplikacija.UseStaticFiles();
aplikacija.UseRouting();
aplikacija.UseSession();
aplikacija.UseAuthorization();

aplikacija.MapControllerRoute(
    name: "podrazumevano",
    pattern: "{controller=Pocetna}/{action=Index}/{id?}");

aplikacija.Run();
