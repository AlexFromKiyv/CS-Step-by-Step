namespace Initializers
{
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; } = 0;
        public Point()
        {
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void ToConsole() =>  Console.WriteLine($"[{X},{Y},{Z}]\n");

        
    }
}
