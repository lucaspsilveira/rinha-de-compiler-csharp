using System.Dynamic;
using rinha_de_compiler_csharp.Models;

namespace rinha_de_compiler_csharp.Services 
{
    public class Interpreter
    {
        public static void Interpret(dynamic ast) => Evaluate(ast.expression, new Dictionary<string, dynamic>());

        public static void InterpretAST(AST ast) => Evaluate(ast.Expression, new Dictionary<string, dynamic>());

        private static dynamic? Evaluate(Term expression,  Dictionary<string, dynamic> memory) {
            switch (expression.Kind.ToString()) {
                case "Let":
                    var let = expression as Let;
                    if (let.Value.Kind == "Function") {
                        memory[let.Name.Text] = let.Value;
                        break;
                    }
                    memory[let.Name.Text.ToString()] = Evaluate(let.Value, memory);
                    
                    break;
                case "Function":
                    return Evaluate(((Function)expression).Value, memory);
                case "Call":
                    var call = expression as Call;
                    var functionCallee = Evaluate(call.Callee, memory) as Function;
                    var localMemory = new Dictionary<string, dynamic>();
                    
                    if (functionCallee.Parameters.Count != call.Arguments.Count)
                        throw new Exception($"Invalid number of parameters for function.");

                    foreach (var a in memory)
                        localMemory.Add(a.Key, a.Value);

                    for (var index = 0; index < functionCallee.Parameters.Count; index++)
                        localMemory[functionCallee.Parameters[index].Text.ToString()] = Evaluate(call.Arguments[index], memory);
                    
                    return Evaluate(functionCallee, localMemory);
                case "Print":
                    var print = expression as Print;
                    var content = Evaluate(print.Value, memory);
                    if (content is bool)
                        Console.WriteLine(content.ToString().ToLower());
                    else
                        Console.WriteLine(content.ToString());
                    return content;
                case "First":
                    var first = expression as First;
                    var res = Evaluate(first.Value, memory);
                    try 
                    {
                        var tupleFirst = res as Tuple<dynamic, dynamic>;
                    }
                    catch(Exception ex) 
                    {
                        throw new Exception("Invalid argument for First function. Expected a tuple.", ex);
                    }
                    return res.Item1;
                case "Second":
                    var second = expression as Second;
                    var resp = Evaluate(second.Value, memory);
                    try 
                    {
                        var tupleSecond = resp as Tuple<dynamic, dynamic>;
                    }
                    catch(Exception ex) 
                    {
                        throw new Exception("Invalid argument for First function. Expected a tuple.", ex);
                    }
                    return resp.Item2;
                case "Str":
                    return ((Str)expression).Value;
                case "Bool":
                    var b = expression as Bool;
                    return b.Value;
                case "Int":
                    return ((Int)expression).Value;
                case "Tuple":
                    var tup = expression as TupleRinha;
                    return new Tuple<dynamic, dynamic>(Evaluate(tup.First, memory), Evaluate(tup.Second, memory));
                case "If":
                    var ifBlock = expression as If;
                    var result = Evaluate(ifBlock.Condition, memory);
                    if (result) 
                        return Evaluate(ifBlock.Then, memory);
                    else 
                        return Evaluate(ifBlock.Otherwise, memory);
                case "Var":
                    var v = expression as Var;
                    return memory[v.Text];
                case "Binary":
                    var binary = expression as Binary;
                    var lhs = Evaluate(binary.Lhs, memory);
                    var rhs = Evaluate(binary.Rhs, memory);

                    if (lhs is string && rhs is bool)
                        rhs = rhs.ToString().ToLower();
                    if (rhs is string && lhs is bool)
                        lhs = lhs.ToString().ToLower();

                    return binary.Op switch
                    {
                        "Add" => lhs + rhs,
                        "Sub" => lhs - rhs,
                        "Mul" => lhs * rhs,
                        "Div" => lhs / rhs,
                        "Rem" => lhs % rhs,
                        "Eq" => lhs == rhs,
                        "Neq" => lhs != rhs,
                        "Lt" => lhs < rhs,
                        "Gt" => lhs > rhs,
                        "Lte" => lhs <= rhs,
                        "Gte" => lhs >= rhs,
                        "And" => lhs && rhs,
                        "Or" => lhs || rhs,
                        _ => "",
                    };
            }
            
            var exp = expression as Let;
            if (exp.Next is not null)
                return Evaluate(exp.Next, memory);

            return "";
        }
    }
}

