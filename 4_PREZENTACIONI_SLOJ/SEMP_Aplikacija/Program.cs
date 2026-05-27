using BibliotekaKlasa.KlasePodatakaEF.KontekstEF;
using BibliotekaKlasa.Servisi;
using Microsoft.EntityFrameworkCore;
using PoslovnaLogika.PoslovniProcessi;

var graditelj = WebApplication.CreateBuilder(args);

graditelj.Services.AddControllersWithViews();
graditelj.Services.AddSession();

// HttpClient factory - koristi se u kontrolerima za pozivanje REST servisa
graditelj.Services.AddHttpClient();

// EF DbContext
graditelj.Services.AddDbContext<AppDbContext>(opcije =>
    opcije.UseSqlServer(graditelj.Configuration.GetConnectionString("PodrazumevanaKonekcija")));

// XML servis za top liste
graditelj.Services.AddSingleton<XmlTopListeServis>(provider =>
{
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    return new XmlTopListeServis(env.WebRootPath);
});

// Poslovni Proces - prosledjuje se URL REST servisa
graditelj.Services.AddScoped<PoslovniProces>(provider =>
{
    var konfiguracija = provider.GetRequiredService<IConfiguration>();
    var xmlServis = provider.GetRequiredService<XmlTopListeServis>();
    string konekcioniString = konfiguracija.GetConnectionString("PodrazumevanaKonekcija")!;
    string restApiUrl = konfiguracija["RestApiUrl"] ?? "https://localhost:7001";
    return new PoslovniProces(konekcioniString, xmlServis, restApiUrl);
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