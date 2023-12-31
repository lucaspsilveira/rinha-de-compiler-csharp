using System.Diagnostics;
using Newtonsoft.Json;
using rinha_de_compiler_csharp.Models;
using rinha_de_compiler_csharp.Services;

namespace rinha_de_compiler_csharp.UnitTests;

public class InterpreterTests
{
    [Fact]
    public void Interpret_ReadSumFile()
    {
        using StringWriter sw = new();
        Console.SetOut(sw);
        var file = File.ReadAllText("Resources/sum.json");
        var astJson = JsonConvert.DeserializeObject<dynamic>(file);
        var ast = new AST(astJson);
        var stopWatch = new Stopwatch();
        
        stopWatch.Start();
        var interpreter = new Interpreter();
        interpreter.InterpretAST(ast);
        stopWatch.Stop();

        Assert.Equal("3\n", sw.ToString());
        Console.WriteLine($"It took {stopWatch.ElapsedMilliseconds} milliseconds to run.");
    }

    [Fact]
    public void Interpret_ReadPrintFile()
    {
        using StringWriter sw = new();
        Console.SetOut(sw);
        var file = File.ReadAllText("Resources/print.json");
        var astJson = JsonConvert.DeserializeObject<dynamic>(file);
        var ast = new AST(astJson);
        var stopWatch = new Stopwatch();
        
        stopWatch.Start();
        var interpreter = new Interpreter();
        interpreter.InterpretAST(ast);
        stopWatch.Stop();
        
        Assert.Equal("Hello, Lucas!\n", sw.ToString());
        Console.WriteLine($"It took {stopWatch.ElapsedMilliseconds} milliseconds to run.");
    }

    [Fact]
    public void Interpret_ReadFibFile()
    {
        using StringWriter sw = new();
        Console.SetOut(sw);
        var file = File.ReadAllText("Resources/fib.json");
        var astJson = JsonConvert.DeserializeObject<dynamic>(file);
        var ast = new AST(astJson);
        
        var stopWatch = new Stopwatch();
        
        stopWatch.Start();
        var interpreter = new Interpreter();
        interpreter.InterpretAST(ast);
        stopWatch.Stop();

        Assert.Equal("2\n", sw.ToString());
        Console.WriteLine($"It took {stopWatch.ElapsedMilliseconds} milliseconds to run.");
    }

    [Fact]
    public void Interpret_ReadCombinationFile()
    {
        using StringWriter sw = new();
        Console.SetOut(sw);
        var file = File.ReadAllText("Resources/combination.json");
        var astJson = JsonConvert.DeserializeObject<dynamic>(file);
        var ast = new AST(astJson);
        var stopWatch = new Stopwatch();
        
        stopWatch.Start();
        var interpreter = new Interpreter();
        interpreter.InterpretAST(ast);
        stopWatch.Stop();
        
        Assert.Equal("6\n", sw.ToString());
        Console.WriteLine($"It took {stopWatch.ElapsedMilliseconds} milliseconds to run.");
    }
}