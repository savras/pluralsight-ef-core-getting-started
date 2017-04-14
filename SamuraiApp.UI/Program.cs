using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    class Program
    {
        // Always use short-lived db context! but using here just for simplicity in this demo.
        private static readonly SamuraiContext Context = new SamuraiContext();

        static void Main()
        {
            Context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
            InsertSamurai();
            InsertMultipleSamurais();
            SimpleSamuraiQuery();
            MoreQueries();
        }

        private static void QueryAndUpdateSamuraiDisconnected()
        {
            var samurai = Context.Samurais.First(s => s.Name == "Vincent");
            samurai.Name += "-nan";
            using (var contextNewAppInstance = new SamuraiContext())
            {
                contextNewAppInstance.Samurais.Update(samurai);
                contextNewAppInstance.SaveChanges();
            }
        }

        private static void DeleteWhileTracked()
        {
            var samurai = Context.Samurais.First(s => s.Name == "Vincent");
            Context.Samurais.Remove(samurai);   // Always need full object to remove with change tracking.
            // Alternatives:
            // Context.Remove(samurai);
            // Context.Entry(samurai).State = EntityState.Deleted;
            // Context.Samurais.Remove(Context.Samurais.Find(1));
            Context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = Context.Samurais.ToList();
            samurais.ForEach(s => s.Name += "-san");
            Context.SaveChanges();
        }
        
        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = Context.Samurais.First();
            samurai.Name += "-san";
            Context.SaveChanges();
        }

        private static void MoreQueries()
        {
            var name = "Vincent";
            var samurais = Context.Samurais.FirstOrDefault(s => s.Name == name);    // EF will parameterize this
        }

        private static void SimpleSamuraiQuery()
        {
            var samurais = Context.Samurais.ToList(); // LINQ execution method to excute against DB.
            // Others include: First(), SingleOrDefault(), Count(), Min(), Max(), Last(), Average(), Find().

            // Enumeration the context without using LINQ execution will keep DB connection open!!!
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai {Name = "Vincent"};
            var kensai = new Samurai {Name = "Gim"};
            Context.Samurais.AddRange(samurai, kensai);
            Context.SaveChanges();
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai
            {
                Name = "Vincent",
                Quotes = new List<Quote>
                {
                    new Quote
                    {
                        Text = "I AM the one!"
                    }
                }
            };

            Context.Samurais.Add(samurai);
            Context.SaveChanges();
        }
    }
}
