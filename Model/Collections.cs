using System.Collections.Generic;

namespace NHibernateCourse.QuickStart.Model
{
    public class QuestionCollection : System.Collections.ArrayList
    {
         public void Renumber()
         {
             // logic
         }
    }

    public static class QuestionCollectionExtension
    {
        public static void Renumber(this ICollection<Question> self)
        {
            // logic
        }
    }
}