namespace SamuraiApp.Domain
{
    public class SamuraiBattle
    {
        // many-many will need at least the ID and the object property for each entity.
        public int SamuraiId { get; set; }
        public Samurai Samurai { get; set; } 
        public int BattleId { get; set; }
        public Battle Battle { get; set; }
    }
}
