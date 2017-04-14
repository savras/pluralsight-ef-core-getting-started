using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public Samurai()
        {
            Quotes = new List<Quote>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes { get; set; }

        // indicate many-many relationship
        public List<SamuraiBattle> SamuraiBattles { get; set; }

        // indicate one-one relationship
        public SecretIdentity SecretIdentity { get; set; }
    }
}
