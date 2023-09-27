using System.Text;
using rinha_de_compiler_csharp.Models;

namespace rinha_de_compiler_csharp.Services 
{
    public class Interpreter
    {        
        private readonly Dictionary<string, dynamic> fnCache = new();
        public Interpreter() {}

        public void InterpretAST(AST ast) => Evaluate(ast.Expression, new Dictionary<string, dynamic>());

        private dynamic? Evaluate(Term expression,  Dictionary<string, dynamic> memory) {
            switch (expression.Kind.ToString()) {
                case "Let":
                    var let = expression as Let;
                    if (let!.Value.Kind == "Function") {
                        memory[let.Name.Text] = let.Value;
                        break;
                    }
                    memory[let.Name.Text.ToString()] = Evaluate(let.Value, memory)!;
                    
                    break;
                case "Function":
                     var fn = (Function)expression;
                     fn.LocalMemory = memory;
                     return fn;
                case "Call":
                    var call = expression as Call;
                    Function? functionCallee;
                    if (call!.Callee.Kind.Equals("Var"))
                        functionCallee = Evaluate(call.Callee, memory) as Function;
                    else
                        functionCallee = call.Callee as Function;

                    if (functionCallee!.Parameters.Count != call.Arguments.Count)
                        throw new Exception($"Invalid number of parameters for function.");

                    var localMemory = new Dictionary<string, dynamic>();

                    foreach (var a in memory)
                        localMemory[a.Key] =  a.Value;
                    
                    foreach (var mem in functionCallee.LocalMemory)
                        localMemory[mem.Key] = mem.Value;
                    StringBuilder functionKey = new();
                    if (functionCallee.IsPure) 
                    {
                        if (call!.Callee.Kind.Equals("Var"))
                            functionKey.Append(((Var)call!.Callee).Text);
                        functionKey.Append(functionCallee.Kind);
                    }
                    for (var index = 0; index < functionCallee.Parameters.Count; index++)
                    {
                        var argEval = Evaluate(call.Arguments[index], memory);
                        var paramenterKey = functionCallee.Parameters[index].Text;
                        localMemory[paramenterKey] = argEval;
                        if (functionCallee.IsPure)
                            functionKey.Append(paramenterKey + argEval);
                    }

                    if (functionCallee.IsPure) 
                    {
                        var resultKey = functionKey.ToString();
                        if (fnCache.ContainsKey(resultKey))
                            return fnCache[resultKey];

                        var resultFn = Evaluate(functionCallee.Value, localMemory);
                        fnCache.Add(resultKey, resultFn);
                        return resultFn;
                    }
                    else
                    {
                        return Evaluate(functionCallee.Value, localMemory);
                    }
                case "Print":
                    var print = expression as Print;
                    var content = Evaluate(print!.Value, memory);

                    string output;
                    if (content is bool)
                        output = content.ToString().ToLower();
                    else
                        output = content!.ToString();
                    Console.WriteLine(output);
                    // TO DO
                    // if (memory.TryGetValue("output", out dynamic sb))
                    //     ((StringBuilder)sb).AppendLine(output);
                    return content;
                case "First":
                    var first = expression as First;
                    var res = Evaluate(first!.Value, memory);
                    try 
                    {
                        var tupleFirst = res as Tuple<dynamic, dynamic>;
                    }
                    catch(Exception ex) 
                    {
                        throw new Exception("Invalid argument for First function. Expected a tuple.", ex);
                    }
                    return res!.Item1;
                case "Second":
                    var second = expression as Second;
                    var resp = Evaluate(second!.Value, memory);
                    try 
                    {
                        var tupleSecond = resp as Tuple<dynamic, dynamic>;
                    }
                    catch(Exception ex) 
                    {
                        throw new Exception("Invalid argument for First function. Expected a tuple.", ex);
                    }
                    return resp!.Item2;
                case "Str":
                    return ((Str)expression).Value;
                case "Bool":
                    var b = expression as Bool;
                    return b!.Value;
                case "Int":
                    return ((Int)expression).Value;
                case "Tuple":
                    var tup = expression as TupleRinha;
                    return new Tuple<dynamic, dynamic>(Evaluate(tup.First, memory), Evaluate(tup.Second, memory));
                case "If":
                    var ifBlock = expression as If;
                    var result = Evaluate(ifBlock!.Condition, memory);
                    if (result) 
                        return Evaluate(ifBlock.Then, memory);
                    else 
                        return Evaluate(ifBlock.Otherwise, memory);
                case "Var":
                    var v = expression as Var;
                    return memory[v!.Text];
                case "Binary":
                    var binary = expression as Binary;
                    var lhs = Evaluate(binary!.Lhs, memory);
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
            if (exp!.Next is not null)
                return Evaluate(exp.Next, memory);

            return "";
        }
    }
}

