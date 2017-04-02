using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    class Program
    {
        private static readonly SamuraiContext Context = new SamuraiContext();

        static void Main()
        {
            Context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
            InsertSamurai();
            InsertMultipleSamurais();
            SimpleSamuraiQuery();
            MoreQueries();
        }

        public static void MoreQueries()
        {
            var name = "Vincent";
            var samurais = Context.Samurais.FirstOrDefault(s => s.Name == name);
        }

        public static void SimpleSamuraiQuery()
        {
            var samurais = Context.Samurais.ToList(); // LINQ execution method to excute against DB.
            // Others include: First(), SingleOrDefault(), Count(), Min(), Max(), Last(), Average(), Find().

            // Enumeration without LINQ execution will keep DB connection open!!!
        }

        public static void InsertMultipleSamurais()
        {
            var samurai = new Samurai {Name = "Vincent"};
            var kensai = new Samurai {Name = "Gim"};
            Context.Samurais.AddRange(samurai, kensai);
            Context.SaveChanges();
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai {Name = "Vincent"};
            Context.Samurais.Add(samurai);
            Context.SaveChanges();
        }
    }
}
