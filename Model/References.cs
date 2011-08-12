namespace NHibernateCourse.QuickStart.Model
{
    public class State
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Abb { get; set; }

        public static  implicit operator State(StateNames x)
        {
            return new State{Id = (int)x};
        }
    }

    public enum StateNames
    {
        TX = 1,
        CA = 2,
        NY = 2
    }

    
}