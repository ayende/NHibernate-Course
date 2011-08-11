namespace NHibernateCourse.QuickStart.Model
{
    public class Test
    {
        public virtual int Id { get; set; }
        public virtual Student Student { get; set; }
        public virtual int Score { get; set; }
        public virtual byte[] Timestamp { get; set; }
    }
}