namespace rinha_de_compiler_csharp.Services 
{
    public class Interpreter
    {
        public static void Interpret(dynamic ast) => Evaluate(ast.expression, new Dictionary<string, dynamic>());

        private static dynamic? Evaluate(dynamic expression,  Dictionary<string, dynamic> memory) {
            switch (expression.kind.ToString()) {
                case "Let":
                    if (expression.value.kind == "Function") {
                        memory[expression.name.text.ToString()] = expression.value;
                        break;
                    }
                    memory[expression.name.text.ToString()] = Evaluate(expression.value, memory);
                    break;
                case "Function":
                    return Evaluate(expression.value, memory);
                case "Call":
                    var functionCallee = Evaluate(expression.callee, memory);
                    var localMemory = new Dictionary<string, dynamic>();
                    
                    if (functionCallee.parameters.Count != expression.arguments.Count)
                        throw new Exception($"Invalid number of parameters for function {expression.callee.text}. Expected {functionCallee.parameters.Count} but received {expression.arguments.Count}.");

                    foreach (var a in memory)
                        localMemory.Add(a.Key, a.Value);

                    for (var index = 0; index < functionCallee.parameters.Count; index++)
                        localMemory[functionCallee.parameters[index].text.ToString()] = Evaluate(expression.arguments[index], memory);
                    
                    return Evaluate(functionCallee, localMemory);
                case "Print":
                    var content = Evaluate(expression.value, memory);
                    if (content is bool)
                        Console.WriteLine(content.ToString().ToLower());
                    else
                        Console.WriteLine(content.ToString());
                    return content;
                case "First":
                    if (expression.value.kind != "Tuple")
                        throw new Exception("Invalid argument for First function. Expected a tuple.");
                    return Evaluate(expression.value.first, memory);
                case "Second":
                    if (expression.value.kind != "Tuple")
                        throw new Exception("Invalid argument for Second function. Expected a tuple.");
                    return Evaluate(expression.value.second, memory);
                case "Str":
                    return expression.value.ToString();
                case "Bool":
                    return bool.Parse(expression.value.ToString());
                case "Int":
                    return int.Parse(expression.value.ToString());
                case "Tuple":
                    return (Evaluate(expression.first, memory), Evaluate(expression.second, memory));
                case "If":
                    if (Evaluate(expression.condition, memory)) 
                        return Evaluate(expression.then, memory);
                    else 
                        return Evaluate(expression.otherwise, memory);
                case "Var":
                    return memory[expression.text.ToString()];
                case "Binary":
                    var lhs = Evaluate(expression.lhs, memory);
                    var rhs = Evaluate(expression.rhs, memory);

                    if (lhs is string && rhs is bool)
                        rhs = rhs.ToString().ToLower();
                    if (rhs is string && lhs is bool)
                        lhs = lhs.ToString().ToLower();

                    return expression.op.ToString() switch
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

            if (expression.next is not null)
                Evaluate(expression.next, memory);

            return "";
        }
    }
}

