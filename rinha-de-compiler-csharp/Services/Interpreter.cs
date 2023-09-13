using rinha_de_compiler_csharp.Models;

namespace rinha_de_compiler_csharp.Services 
{
    public class Interpreter
    {
        public Interpreter() {}

        public static void Interpret(dynamic ast) {
            //Console.WriteLine("Expression: "+ ast.expression);
            Evaluate(ast.expression);
        }

        private static dynamic? Evaluate(dynamic expression) {
            switch (expression.kind.ToString()) {
                case "Let":
                    Console.WriteLine("Let:" + expression.name.text);
                    return Evaluate(expression.value);
                case "Function":
                    Console.WriteLine("Parametro: " + expression.parameters[0].text);
                    return Evaluate(expression.value);
                case "Print":
                    var content = Evaluate(expression.value);
                    Console.WriteLine(content.ToString());
                    return null;
                case "Str":
                    return expression.value;
                case "If":
                    Console.WriteLine("Condition: " + expression.condition.kind);
                    return expression.value;
            }
            return "";
        }
    }
}

