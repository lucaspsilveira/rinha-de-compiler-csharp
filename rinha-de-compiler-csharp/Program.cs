using rinha_de_compiler_csharp.Models;
using System.Text.Json;

Console.WriteLine("Hello, Rinha!");
var fileName = "fib.json";
var file = File.ReadAllText($"../../../../var/rinha/files/{fileName}");
Console.WriteLine(file);
var ast = JsonSerializer.Deserialize<AST>(file, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
if (ast is null)
{
    Console.WriteLine("Não foi possível ler o seu arquivo.");
    Environment.Exit(1);
}
Console.WriteLine($"Lendo arquivo: {ast.Name}");