1. Unzip files
2. Open .sln file
3. File will open in Visual Studio
4. Run code by pressing green play button

Packages Required

    Install MySql.EntityFrameworkCore using Nuget Package Installer
    Install Microsoft.EntityFrameworkCore.InMemory using Nuget Package Installer
    Install SQLite Database for storing database
    Install iTextSharp for PDF report download

Change the OnConfiguration settings of the context to be like this

 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
     optionsBuilder.UseInMemoryDatabase(databaseName: "DatabaseDB");
 }

