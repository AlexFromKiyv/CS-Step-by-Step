
using InterfaceHierarchy;

void InterfaceHierarchies1()
{
    // Call from object level.
    BitmapImage image = new BitmapImage();
    image.Draw();
    image.DrawInBoundingBox(1, 1, 10, 10);
    image.DrawUpsideDown();

    // Get IAdvancedDraw explicitly.
    if (image is IAdvancedDraw draw)
    {
        draw.DrawUpsideDown();
    }
}
//InterfaceHierarchies1();

void InterfaceHierarchiesWithDefaultImplementations()
{
    BitmapImage image = new BitmapImage();
    //This does not compile
    //image.TimeToDraw();

    if (image is IAdvancedDraw draw)
    {
        Console.WriteLine(draw.TimeToDraw());
    }
}
//InterfaceHierarchiesWithDefaultImplementations();

void InterfaceHierarchiesWithDefaultImplementations1()
{
    BitmapImage image = new BitmapImage();

    Console.WriteLine(image.TimeToDraw());
    Console.WriteLine(((IDrawable)image).TimeToDraw());
    Console.WriteLine(((IAdvancedDraw)image).TimeToDraw());
}
//InterfaceHierarchiesWithDefaultImplementations1();