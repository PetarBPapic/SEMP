var graditelj = WebApplication.CreateBuilder(args);

graditelj.Services.AddControllers();
graditelj.Services.AddEndpointsApiExplorer();

var aplikacija = graditelj.Build();

if (aplikacija.Environment.IsDevelopment())
{
    aplikacija.UseDeveloperExceptionPage();
}

aplikacija.UseHttpsRedirection();
aplikacija.UseAuthorization();
aplikacija.MapControllers();
aplikacija.Run();
