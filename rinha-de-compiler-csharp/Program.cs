using System.Diagnostics;
using Newtonsoft.Json;
using rinha_de_compiler_csharp.Models;
using rinha_de_compiler_csharp.Services;

var stopWatch = new Stopwatch();
stopWatch.Start();

var fileName = "source.rinha.json";
if (args.Length > 0 && args[0] is not null)
    fileName = args[0];

var file = File.ReadAllText($"/var/rinha/{fileName}");
var astJson = JsonConvert.DeserializeObject<dynamic>(file);
if (astJson is null)
{
    Console.WriteLine("Não foi possível ler o seu arquivo.");
    Environment.Exit(1);
}

var ast = new AST(astJson);

var interpreter = new Interpreter();
interpreter.InterpretAST(ast);

stopWatch.Stop();

Console.WriteLine($"Tempo de execução: {stopWatch.ElapsedMilliseconds} milissegundos.");
Console.WriteLine($"Tempo de execução: {stopWatch.ElapsedMilliseconds / 1000f} segundos.");
