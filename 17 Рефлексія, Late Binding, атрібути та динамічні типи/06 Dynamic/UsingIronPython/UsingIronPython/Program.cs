using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

void RunPythonScript()
{
    ScriptEngine engine = Python.CreateEngine();
    engine.Execute("print('Hi, warrior')");
    engine.ExecuteFile("D://hi.py");
}
//RunPythonScript();

void UseScriptScope()
{
    int a = 10;

    ScriptEngine engine = Python.CreateEngine();
    ScriptScope scope = engine.CreateScope();

    scope.SetVariable("x", a);
    engine.ExecuteFile("D://square.py", scope);
    
    dynamic y = scope.GetVariable("y");
    Console.WriteLine(y);
}
//UseScriptScope();

void CallPythonFunction()
{

    ScriptEngine engine = Python.CreateEngine();
    ScriptScope scope = engine.CreateScope();

    engine.ExecuteFile("D://squares.py", scope);

    dynamic square = scope.GetVariable("square");
    dynamic result = square(10);

    Console.WriteLine(result);
}
CallPythonFunction();