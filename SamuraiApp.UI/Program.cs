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

            ExplicitLoadWithFilter();
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
                new Battle {Name = "Samurai Wars", StartDAte = DateTime.Now, EndDate = DateTime.Now.AddYears(25)},
                new Battle {Name = "Return of the Samurai", StartDAte = DateTime.Now, EndDate = DateTime.Now.AddYears(25)},
                new Battle {Name = "The Samurai Strikes Back", StartDAte = DateTime.Now, EndDate = DateTime.Now.AddYears(25)}
            });

        }

        private static void AddManyToManyWithFks()
        {
            var sb = new SamuraiBattle {SamuraiId = 1, BattleId = 1}; // Only need the two main Ids.
            _context.SamuraiBattles.Add(sb);
            _context.SaveChanges();
        }

        #endregion

        #region Eager loading (the .Include). Cannot be used with filters, sorting .etc

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
                .ThenInclude(sb => sb.Battle) // This is new to EF Core
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

        #region Projection query - use LINQ Select function to pick and choose properties of result.

        private static void AnonymousTypeViaProjection()
        {
            _context = new SamuraiContext();
            var quotes = _context.Quotes
                .Select(q => new {q.Id, q.Text})
                .ToList();
        }

        private static void AnonymousTypeViaProjectionWithRelated()
        {
            _context = new SamuraiContext();
            var samurais = _context.Samurais
                .Select(s => new
                {
                    s.Id,
                    s.SecretIdentity.RealName,
                    QuoteCount = s.Quotes.Count() // N + 1 because EF will construct query to get each quote and count them
                })
                .
                ToList();
        }

        private static void RelatedDataFixUp()
        {
            _context = new SamuraiContext();
            var samurai = _context.Samurais.Find(1);

            // samurai variable above will also have Quotes because change tracker will see that it is requesting Quotes of a 'Related data', so it will 'Fix up' the entity.
            var quotes = _context.Quotes.Where(q => q.SamuraiId == 1).ToList();
        }

        #endregion

        #region Explicit loading - using .Entry() from Change Tracking API. Will track objects and navigation fixup will happen.

        private static void ExplicitLoad()
        {
            _context = new SamuraiContext();
            var samurai = _context.Samurais.First();
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.SecretIdentity).Load();
        }

        private static void ExplicitLoadWithFilter()
        {
            _context = new SamuraiContext();
            var samurai = _context.Samurais.First(s => s.Id == 15);

            // Does't work.
            //_context.Entry(samurai)
            //    .Collection(s => s.Quotes
            //        .Where(q => q.Text.Contains("happy")))
            //    .Load();

            // Use this instead.
            _context.Entry(samurai)
                .Collection(s => s.Quotes)
                .Query()
                .Where(q => q.Text.Contains("save"))
                .Load();
        }
        #endregion

        #region Use Related Data for filtering - without pulling down the related data.

        private static void UsingRelatedDataForFilterAndMore()
        {
            _context = new SamuraiContext();
            var samurai = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))    // We are not pulling down the Quotes 'related data'.
                .ToList();
        }
        #endregion
    }
}
