using System.Collections.Generic;

namespace NHibernateCourse.QuickStart.Model
{
    public class Test : IWantValidationOldStyleWay
    {
        public virtual int Id { get; set; }
        public virtual Student Student { get; set; }
        public virtual int Score { get; set; }
        public virtual int ClientId { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual State State { get; set; }
        public virtual bool IsValid
        {
            get { return Score > 0; }
        }
    }

    public interface IWantValidationOldStyleWay
    {
        bool IsValid { get; }
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