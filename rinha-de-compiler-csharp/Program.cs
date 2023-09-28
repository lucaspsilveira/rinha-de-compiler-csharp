using Newtonsoft.Json;
using rinha_de_compiler_csharp.Models;
using rinha_de_compiler_csharp.Services;

var fileName = "source.rinha.json";
if (args.Length > 0 && args[0] is not null)
    fileName = args[0];

var file = File.ReadAllText($"/var/rinha/{fileName}");
var astJson = JsonConvert.DeserializeObject<dynamic>(file, settings: new JsonSerializerSettings { MaxDepth = null });
if (astJson is null)
{
    Console.WriteLine("Não foi possível ler o seu arquivo.");
    Environment.Exit(1);
}

var ast = new AST(astJson);
var interpreter = new Interpreter();
try 
{
    interpreter.InterpretAST(ast);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Erro ao executar .rinha.json: {ex.Message}");
}