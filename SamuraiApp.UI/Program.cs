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
        private static readonly SamuraiContext Context = new SamuraiContext();

        static void Main()
        {
            Context.Database.EnsureCreated();
            Context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
            //InsertSamurai();
            //InsertMultipleSamurais();
            //SimpleSamuraiQuery();
            //MoreQueries();
        }
    }
}
