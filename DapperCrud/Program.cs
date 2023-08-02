using System.Data;
using System;
using DapperCrud;

class Program
{
    static void Main()
    {
        // Veritabanı bağlantı dizesi
        string connectionString = "Data Source=database.db;Version=3;";

        // SQLite bağlantısı oluştur
        using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
        {
            // Veritabanını aç
            dbConnection.Open();

            // Tabloyu oluştur (Örnek amaçlı, eğer tablo zaten varsa bu satırı yorum satırı yapabilirsiniz.)
            dbConnection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Person (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Age INTEGER NOT NULL
                    )
                ");

            // Yeni bir kişi ekleme
            Person newPerson = new Person { Name = "Ahmet", Age = 30 };
            dbConnection.Execute("INSERT INTO Person (Name, Age) VALUES (@Name, @Age)", newPerson);

            // Tüm kişileri getirme
            var people = dbConnection.Query<Person>("SELECT * FROM Person");

            Console.WriteLine("Tüm kişiler:");
            foreach (var person in people)
            {
                Console.WriteLine($"Id: {person.Id}, Name: {person.Name}, Age: {person.Age}");
            }

            // Bir kişiyi güncelleme
            Person personToUpdate = people.FirstOrDefault();
            if (personToUpdate != null)
            {
                personToUpdate.Age = 35;
                dbConnection.Execute("UPDATE Person SET Age = @Age WHERE Id = @Id", personToUpdate);
            }

            // Kişilerden birini silme
            Person personToDelete = people.LastOrDefault();
            if (personToDelete != null)
            {
                dbConnection.Execute("DELETE FROM Person WHERE Id = @Id", personToDelete);
            }

            // Güncellenmiş ve silinmiş kişileri gösterme
            people = dbConnection.Query<Person>("SELECT * FROM Person");
            Console.WriteLine("\nGüncellenmiş ve silinmiş kişiler:");
            foreach (var person in people)
            {
                Console.WriteLine($"Id: {person.Id}, Name: {person.Name}, Age: {person.Age}");
            }
        }
    }
}