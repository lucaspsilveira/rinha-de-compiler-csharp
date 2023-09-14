using rinha_de_compiler_csharp.Models;

namespace rinha_de_compiler_csharp.Services 
{
    public class Interpreter
    {
        public Interpreter() {}

        public static void Interpret(dynamic ast) {
            //Console.WriteLine("Expression: "+ ast.expression);
            Evaluate(ast.expression, new Dictionary<string, dynamic>());
        }

        private static dynamic? Evaluate(dynamic expression, Dictionary<string, dynamic> memory) {
            switch (expression.kind.ToString()) {
                case "Let":
                    memory[expression.name.text.ToString()] = Evaluate(expression.value, memory);
                    break;
                case "Function":
                    Console.WriteLine("Parametro: " + expression.parameters[0].text);
                    return Evaluate(expression.value, memory);
                case "Print":
                    var content = Evaluate(expression.value, memory);
                    Console.WriteLine(content.ToString());
                    return null;
                case "Str":
                    return expression.value.ToString();
                case "Bool":
                    return bool.Parse(expression.value.ToString());
                case "Int":
                    return int.Parse(expression.value.ToString());
                case "If":
                    Console.WriteLine("Condition: " + expression.condition.kind);
                    return expression.value;
                case "Var":
                    return memory[expression.text.ToString()];
                case "Binary":
                    var lhs = Evaluate(expression.lhs, memory);
                    var rhs = Evaluate(expression.rhs, memory);
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

