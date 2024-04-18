using System.Reflection;
using System.Reflection.Emit;

void UsingCreateHelloWorldAssemblyBuilder()
{
    AssemblyBuilder assemblyBuilder = CreateHelloWorldAssemblyBuilder();
    Console.WriteLine($"\nI have {assemblyBuilder.GetType()} {assemblyBuilder.GetName()}\n");

    Type typeHelloWorld = assemblyBuilder.GetType("HelloWorldAssembly.HelloWorld")!;
    Console.WriteLine($"\nI have {typeHelloWorld.GetType()} {typeHelloWorld}\n");

    string msg = "Hi girl!";
    object[] constrArgs = [msg];

    object helloWorldObject = Activator.CreateInstance(typeHelloWorld, constrArgs )!;
    Console.WriteLine($"\nI have {helloWorldObject.GetType()} \n");

    MethodInfo methodInfoSayHello = typeHelloWorld.GetMethod("SayHello")!;
    methodInfoSayHello.Invoke(helloWorldObject, null);

    MethodInfo methodInfoPropertyGetMsg = typeHelloWorld.GetMethod("GetMsg")!;
    Console.WriteLine(methodInfoPropertyGetMsg.Invoke(helloWorldObject,null));
}
UsingCreateHelloWorldAssemblyBuilder();



static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{
    AssemblyName assemblyName = new AssemblyName
    {
        Name = "HelloWorldAssembly",
        Version = new Version("1.0.0.0")
    };
    // Create new assembly.
    var builder = AssemblyBuilder.DefineDynamicAssembly(assemblyName,
        AssemblyBuilderAccess.Run);

    // Define the name of the module.
    ModuleBuilder moduleBuilder = builder.DefineDynamicModule("HelloWorldAssembly");

    // Define a public class
    TypeBuilder typeHelloWordBuilder =
        moduleBuilder.DefineType("HelloWorldAssembly.HelloWorld", TypeAttributes.Public);

    // Define a private String
    FieldBuilder fieldMessageBuilder =
        typeHelloWordBuilder.DefineField(
            "theMessage",
            typeof(string),
            FieldAttributes.Private);

    // Create the custom ctor taking single string arg.
    Type[] constructorArgs = [typeof(string)];
    ConstructorBuilder constructorBuilder = typeHelloWordBuilder.DefineConstructor(
        MethodAttributes.Public,
        CallingConventions.Standard,
        constructorArgs);

    ILGenerator constructorIl = constructorBuilder.GetILGenerator();

    //Emit the necessary CIL into the ctor
    constructorIl.Emit(OpCodes.Ldarg_0);
    Type objectClass = typeof(object);
    ConstructorInfo superConstructor =
        objectClass.GetConstructor(new Type[0])!;
    constructorIl.Emit(OpCodes.Call, superConstructor);
    //Load this pointer onto the stack
    constructorIl.Emit(OpCodes.Ldarg_0);
    constructorIl.Emit(OpCodes.Ldarg_1);
    //Load argument on virtual stack and store in msdField
    constructorIl.Emit(OpCodes.Stfld, fieldMessageBuilder);
    constructorIl.Emit(OpCodes.Ret);

    // Create the default constructor.
    typeHelloWordBuilder.DefineDefaultConstructor(
      MethodAttributes.Public);

    // Create the SayHello method.
    MethodBuilder methodSayHiBuilder = typeHelloWordBuilder.
        DefineMethod("SayHello", MethodAttributes.Public,null,null);

    ILGenerator methodIl = methodSayHiBuilder.GetILGenerator();
    methodIl.EmitWriteLine("Hello from the HelloWorld class!");
    methodIl.Emit(OpCodes.Ret);


    // Now create the GetMsg() method.
    MethodBuilder getMsgMethod = typeHelloWordBuilder.DefineMethod(
      "GetMsg",
      MethodAttributes.Public,
      typeof(string),
      null);
    ILGenerator methodGetMsgIl = getMsgMethod.GetILGenerator();
    methodGetMsgIl.Emit(OpCodes.Ldarg_0);
    methodGetMsgIl.Emit(OpCodes.Ldfld, fieldMessageBuilder);
    methodGetMsgIl.Emit(OpCodes.Ret);

    // Create
    typeHelloWordBuilder.CreateType();

    return builder;
}






