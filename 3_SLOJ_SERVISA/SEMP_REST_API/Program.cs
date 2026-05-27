var graditelj = WebApplication.CreateBuilder(args);

graditelj.Services.AddControllers();
graditelj.Services.AddEndpointsApiExplorer();

// Dodajemo WebRootPath podrsku
graditelj.WebHost.UseWebRoot("wwwroot");
graditelj.WebHost.UseStaticWebAssets();

var aplikacija = graditelj.Build();

if (aplikacija.Environment.IsDevelopment())
{
    aplikacija.UseDeveloperExceptionPage();
}

aplikacija.UseHttpsRedirection();
aplikacija.UseStaticFiles();
aplikacija.UseAuthorization();
aplikacija.MapControllers();
aplikacija.Run();