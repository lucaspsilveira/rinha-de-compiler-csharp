﻿using Newtonsoft.Json;
using rinha_de_compiler_csharp.Services;
using System.Diagnostics;

Console.WriteLine("Hello, Rinheiros!");

var stopWatch = new Stopwatch();
stopWatch.Start();

var fileName = "test_binary_options.json";
var file = File.ReadAllText($"var/rinha/files/{fileName}");
//var file = File.ReadAllText($"../var/rinha/files/{fileName}");
var ast = JsonConvert.DeserializeObject<dynamic>(file);
if (ast is null)
{
    Console.WriteLine("Não foi possível ler o seu arquivo.");
    Environment.Exit(1);
}
Console.WriteLine($"Lendo arquivo: {ast.name}");

Interpreter.Interpret(ast);

stopWatch.Stop();

Console.WriteLine($"Tempo de execução: {stopWatch.ElapsedMilliseconds} milissegundos.");
Console.WriteLine($"Tempo de execução: {stopWatch.ElapsedMilliseconds / 1000f} segundos.");
