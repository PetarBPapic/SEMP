using BibliotekaKlasa.KlasePodatakaEF.KontekstEF;
using BibliotekaKlasa.Servisi;
using Microsoft.EntityFrameworkCore;
using PoslovnaLogika.PoslovniProcessi;

var graditelj = WebApplication.CreateBuilder(args);

graditelj.Services.AddControllersWithViews();
graditelj.Services.AddSession();

// EF DbContext (za EF repozitorijume - treci pristup u DAL-u)
graditelj.Services.AddDbContext<AppDbContext>(opcije =>
    opcije.UseSqlServer(graditelj.Configuration.GetConnectionString("PodrazumevanaKonekcija")));

// XML servis za top liste (singleton - deli se kroz celu aplikaciju)
graditelj.Services.AddSingleton<XmlTopListeServis>(provider =>
{
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    return new XmlTopListeServis(env.WebRootPath);
});

// Poslovni Proces (scoped - po zahtevu)
graditelj.Services.AddScoped<PoslovniProces>(provider =>
{
    var konfiguracija = provider.GetRequiredService<IConfiguration>();
    var xmlServis = provider.GetRequiredService<XmlTopListeServis>();
    string konekcioniString = konfiguracija.GetConnectionString("PodrazumevanaKonekcija")!;
    return new PoslovniProces(konekcioniString, xmlServis);
});

var aplikacija = graditelj.Build();

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
