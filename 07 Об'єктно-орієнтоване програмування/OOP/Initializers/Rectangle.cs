namespace Initializers
{
    internal class Rectangle
    {
        private Point topLeft =new();
        private Point bottomRight =new();

        public Point TopLeft 
        { 
            get { return topLeft; } 
            set { topLeft = value; } 
        }
        public Point BottomRight 
        { 
            get { return bottomRight; } 
            set { bottomRight = value; } 
        }

        public void ToConsole()
        {
            Console.WriteLine($"Rectangle [{TopLeft.X},{TopLeft.Y}],[{BottomRight.X},{BottomRight.Y}] \n");
        }   

    }
}
