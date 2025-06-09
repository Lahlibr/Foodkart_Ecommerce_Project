namespace Foodkart.Models.Entities.Base
{
    public class TimeStamp
    {
        public abstract class TimeStampEntity
        {
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        }
    }
}
