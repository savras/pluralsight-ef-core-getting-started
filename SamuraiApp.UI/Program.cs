using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main()
        {
            _context.Database.EnsureCreated();
            _context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());

            InsertNewPkFkGraph();
            //InsertSamurai();
            //InsertMultipleSamurais();
            //SimpleSamuraiQuery();
            //MoreQueries();
        }

        #region Basic Inserts based on entity relationships
        // 1-to-1 relationship insert. EF will insert the Samurai, return the PK, then insert the Quotes (in batches if many) using the Samurai PK
        private static void InsertNewPkFkGraph()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote
                    {
                        Text = "I've come to save you!"
                    }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        // Same thing as InsertNewPkFkGraph(). Two requests to the DB.
        private static void InsertNewOneToOneGraph()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                SecretIdentity = new SecretIdentity
                {
                    RealName = "Vincent"
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        // Add to tracked object.
        private static void AddChildToExistingObject()
        {
            var samurai = _context.Samurais.First();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you are happy that I saved you!"
            });
            _context.SaveChanges();
        }

        // many-to-many inserts.
        private static void AddBattles()
        {
            _context.Battles.AddRange(new List<Battle>
            {
                new Battle { Name = "Samurai Wars", StartDAte = DateTime.Now, EndDate = DateTime.Now.AddYears(25)},
                new Battle { Name = "Return of the Samurai", StartDAte = DateTime.Now, EndDate = DateTime.Now.AddYears(25)},
                new Battle { Name = "The Samurai Strikes Back", StartDAte = DateTime.Now, EndDate = DateTime.Now.AddYears(25)}
            });
            
        }

        private static void AddManyToManyWithFks()
        {
            var sb = new SamuraiBattle { SamuraiId = 1, BattleId = 1};  // Only need the two main Ids.
            _context.SamuraiBattles.Add(sb);
            _context.SaveChanges();
        }
        #endregion

        #region Eager loading (the .Include)

        private static void EagerLoadingWithInclude()
        {
            _context = new SamuraiContext(); // Run without result of previous queries affecting the next queries from this point.
            var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
        }

        private static void EagerLoadingToManyAkaChildrenGrandchildren()
        {
            _context = new SamuraiContext();
            var samuraiWithBattles = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle)   // This is new to EF Core
                .ToList(); // End with LINQ execution method.
        }

        private static void EagerLoadingWithFind_NOPE()
        {
            // Can't do .Include().Find().ToList;..
            // Find() checks with context to see if the entity to find is already
            // tracked. Otherwise go retrieve the entity in the database.
            // Why can't use with .Include()?
        }
        private static void EagerLoadingWithMultipleBranches()
        {
            _context = new SamuraiContext();
            var samuraiWithBattles = _context.Samurais
                .Include(s => s.Quotes)
                .Include(s => s.SecretIdentity)
                .ToList();
        }

        #endregion
    }
}
