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
                    Console.WriteLine("Let:" + expression.name.text);
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
                    return expression.value;
                case "If":
                    Console.WriteLine("Condition: " + expression.condition.kind);
                    return expression.value;
                case "Var":
                    return memory[expression.text.ToString()];
                case "Binary":
                    var lhs = Evaluate(expression.lhs, memory);
                    var rhs = Evaluate(expression.rhs, memory);
                    if (expression.op == "Add") 
                    {
                        return lhs + rhs;
                    }
                    return "";
            }

            if (expression.next is not null)
                Evaluate(expression.next, memory);

            return "";
        }
    }
}

