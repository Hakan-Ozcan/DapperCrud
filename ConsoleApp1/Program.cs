using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;


namespace DapperCRUDExample
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static void Main()
        {
      
            // Veritabanı bağlantı dizesi
            string connectionString = "Data Source=C:\\Users\\hakan.ozcan\\Desktop\\SQLiteDb\\mydatabase.db";

            // SQLite bağlantısı oluştur
            using (var connection = new SqliteConnection(connectionString))
            {
                // Veritabanını aç
                connection.Open();

                // Tabloyu oluştur (Örnek amaçlı, eğer tablo zaten varsa bu satırı yorum satırı yapabilirsiniz.)
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Person (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Age INTEGER NOT NULL
                    )
                ");

                // Yeni bir kişi ekleme
                Person newPerson = new Person { Name = "Ahmet", Age = 30 };
                connection.Execute("INSERT INTO Person (Name, Age) VALUES (@Name, @Age)", newPerson);

                // Tüm kişileri getirme
                var people = connection.Query<Person>("SELECT * FROM Person");

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
                    connection.Execute("UPDATE Person SET Age = @Age WHERE Id = @Id", personToUpdate);
                }

                // Kişilerden birini silme
                Person personToDelete = people.LastOrDefault();
                if (personToDelete != null)
                {
                    connection.Execute("DELETE FROM Person WHERE Id = @Id", personToDelete);
                }

                // Güncellenmiş ve silinmiş kişileri gösterme
                people = connection.Query<Person>("SELECT * FROM Person");
                Console.WriteLine("\nGüncellenmiş ve silinmiş kişiler:");
                foreach (var person in people)
                {
                    Console.WriteLine($"Id: {person.Id}, Name: {person.Name}, Age: {person.Age}");
                }
            }
        }
    }
}