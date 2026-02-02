
//// Example1
//// Make use of types defined the MyShapes namespace.
//using CustomNamespaces.MyShapes;
//void Example1()
//{
//    Hexagon h = new Hexagon();
//    Circle c = new Circle();
//    Square s = new Square();
//}

//// Example2

//void Example2()
//{
//    // Note we are not importing CustomNamespaces.MyShapes anymore!
//    CustomNamespaces.MyShapes.Hexagon h = new CustomNamespaces.MyShapes.Hexagon();
//    CustomNamespaces.MyShapes.Circle c = new CustomNamespaces.MyShapes.Circle();
//    CustomNamespaces.MyShapes.Square s = new CustomNamespaces.MyShapes.Square();
//}


//// Example3
//// Ambiguities abound!
//using CustomNamespaces.MyShapes;
//using CustomNamespaces.My3DShapes;
//void Example3()
//{
//    //// Which namespace do I reference?
//    //Hexagon h = new Hexagon(); // Compiler error!
//    //Circle c = new Circle();   // Compiler error!
//    //Square s = new Square();   // Compiler error!

//    // We have now resolved the ambiguity.
//    CustomNamespaces.My3DShapes.Hexagon h = new CustomNamespaces.My3DShapes.Hexagon();
//    CustomNamespaces.My3DShapes.Circle c = new CustomNamespaces.My3DShapes.Circle();
//    CustomNamespaces.MyShapes.Square s = new CustomNamespaces.MyShapes.Square();
//}

//// Example4
//using CustomNamespaces.MyShapes;
//using CustomNamespaces.My3DShapes;
//using The3DHexagon = CustomNamespaces.My3DShapes.Hexagon;
//void Example4()
//{
//    // This is really creating a My3DShapes.Hexagon class.
//    The3DHexagon h2 = new The3DHexagon();
//}


////Example5
//using CustomNamespaces.MyShapes;
//using CustomNamespaces.My3DShapes;
//using TreeDShapes = CustomNamespaces.My3DShapes;

//void Example5()
//{
//    TreeDShapes.Hexagon h = new();
//}
