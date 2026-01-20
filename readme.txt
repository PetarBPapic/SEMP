# SemProg – TV Program Rating
ASP.NET Core 8 MVC aplikacija za ocenjivanje TV emisija.  
Radi sa LocalDB-om; nije potreban pun SQL Server.

---

## 1. Preduslovi
- .NET 8 SDK  
- SQL Server Express LocalDB (dolazi s VS 2022)  

Provjeri LocalDB:
```powershell
sqllocaldb info MSSQLLocalDB

-------------------------------------------------------------------

Ako ne postoji:

sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB

Ako treba:

cd SemProg
dotnet restore
dotnet ef database update --project SemProg.DAL --startup-project SemProg.Web

---------------------------------------------------------------------

Seedani korisnici:

Admin: admin admin

User1: user user

User2: pera pera

---------------------------------------------------------------------

cd SemProg.Web
dotnet run