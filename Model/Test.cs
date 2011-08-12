using System.Collections.Generic;

namespace NHibernateCourse.QuickStart.Model
{
    public class Test
    {
        public virtual int Id { get; set; }
        public virtual Student Student { get; set; }
        public virtual int Score { get; set; }
        public virtual byte[] Timestamp { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }

    public class Question
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
    }
}