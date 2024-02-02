Zainstaluj Pakiety :

Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter,

Microsoft.AspNetCore.Identity.EntityFrameworkCore,

Microsoft.AspNetCore.Identity.UI,

Microsoft.EntityFrameworkCore,

Microsoft.EntityFrameworkCore.Design,

Microsoft.EntityFrameworkCore.Relational,

Microsoft.EntityFrameworkCore.Sqlite,

Microsoft.EntityFrameworkCore.SqlServer,

Microsoft.EntityFrameworkCore.Tools,

Microsoft.VisualStudio.Web.CodeGeneration.Design,


Przed uruchomieniem ustaw w pliku appsetings.json połączenie z serwerem bazodanowym, następnie utwórz migracje. Komendy do utworzenia migracji w konsoli NuGet Package Manager:

        add-migration "Initial Create"
        
        update-database

Admin oraz testowi użytkownicy powinni zostac automatycznie utworzeni po uruchomieniu projektu.

Dane logowania(email, hasło, rola):

        ("admin@example.com", "Admin123!", "Administrator"),
        ("user1@example.com", "User123!", "Użytkownik"),
        ("user2@example.com", "User123!", "Użytkownik"),
        ("user3@example.com", "User123!", "Użytkownik"),
        ("user4@example.com", "User123!", "Użytkownik")

Niezalogowani użytkownicy moga przeglądać projekty, komentarze oraz zadania.

Zalogowani użytkownicy mogą także sprawdzać szczegóły.

Administratorzy mogą wykonywać wszystkie czynności CRUD.
