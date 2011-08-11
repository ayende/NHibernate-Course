namespace NHibernateCourse.QuickStart.Model
{
    public class File
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class Graphic : File
    {
        public virtual string Type { get; set; }
    }

    public class Final : Graphic
    {
        public virtual short ColorDepth { get; set; }
    }

    public class Rough : Graphic
    {
        public virtual int Height { get; set; }
        public virtual int Width { get; set; }
    }
}