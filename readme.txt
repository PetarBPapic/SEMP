# SemProg – TV Program Rating

ASP.NET Core 8 MVC aplikacija za ocenjivanje TV emisija.  
Radi sa LocalDB-om; nije potreban pun SQL Server.

---

## 1. Preduslovi

- .NET 8 SDK  
- SQL Server Express LocalDB (dolazi s VS 2022)  
- (Opciono) Visual Studio / VS Code

Provjeri LocalDB:
```powershell
sqllocaldb info MSSQLLocalDB

sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB

dotnet ef database update --project SemProg.DAL --startup-project SemProg.Web

sqlcmd -S "(LocalDB)\MSSQLLocalDB" -d SemProgDB -Q "INSERT INTO Users (Username, Password, Role) VALUES ('admin','admin','admin')"

sqlcmd -S "(LocalDB)\MSSQLLocalDB" -d SemProgDB -Q "INSERT INTO Users (Username, Password, Role) VALUES ('user','user','user')"


cd SemProg.Web
dotnet run

