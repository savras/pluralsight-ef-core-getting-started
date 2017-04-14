namespace SamuraiApp.Domain
{
    public class SecretIdentity
    {
        public int Id { get; set; }
        public string RealName { get; set; }

        // indicate one-one relationship with Id and object property
        public int SamuraiId { get; set; }
        public Samurai Samurai { get; set; }
        
    }
}