using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SamuraiApp.UI
{
    public class DisconnectedMethods
    {
        private static void DisplayState(List<EntityEntry> es, string method)
        {
            es.ForEach(e => Console.WriteLine(
                $"{e.Entity.GetType().Name} : {e.State.ToString()}"));
            Console.WriteLine();
        }
    }
}
