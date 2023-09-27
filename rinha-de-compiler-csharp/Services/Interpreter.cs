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
                    InterpretLet(expression, memory);
                    break;
                case "Call":
                    return InterpretCall(expression, memory);
                case "Print":
                    return InterpretPrint(expression, memory);
                case "First":
                    return InterpretFirst(expression, memory);
                case "Second":
                    return InterpretSecond(expression, memory);
                case "If":
                    return InterpretIf(expression, memory);
                case "Binary":
                    return InterpretBinary(expression, memory);
                case "Function":
                     var fn = (Function)expression;
                     fn.LocalMemory = memory;
                     return fn;
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
                case "Var":
                    var v = expression as Var;
                    return memory[v!.Text];
            }
            
            var exp = expression as Let;
            if (exp!.Next is not null)
                return Evaluate(exp.Next, memory);

            return "";
        }

        private void InterpretLet(Term expression, Dictionary<string, dynamic> memory)
        {
            var let = expression as Let;
            if (let!.Value.Kind == "Function")
                memory[let.Name.Text] = let.Value;
            else
                memory[let.Name.Text.ToString()] = Evaluate(let.Value, memory)!;
        }

        private dynamic InterpretCall(Term expression, Dictionary<string, dynamic> memory)
        {
            var call = expression as Call;
            Function? functionCallee;
            if (call!.Callee.Kind.Equals("Function"))
                functionCallee = call.Callee as Function;
            else
                functionCallee = Evaluate(call.Callee, memory) as Function;

            if (functionCallee!.Parameters.Count != call.Arguments.Count)
                throw new Exception($"Invalid number of parameters for function.");

            Dictionary<string, dynamic> localMemory = BuildLocalMemory(memory, functionCallee);

            if (functionCallee.IsPure)
                return InterpretPureFunction(call, functionCallee, memory, localMemory);
            return InterpretInpureFunction(call, functionCallee, memory, localMemory);
        }

        private static Dictionary<string, dynamic> BuildLocalMemory(Dictionary<string, dynamic> memory, Function? functionCallee)
        {
            var localMemory = new Dictionary<string, dynamic>();

            foreach (var mem in memory)
                localMemory[mem.Key] = mem.Value;

            foreach (var mem in functionCallee.LocalMemory)
                localMemory[mem.Key] = mem.Value;
            return localMemory;
        }

        private dynamic InterpretInpureFunction( Call? call, Function? functionCallee, Dictionary<string, dynamic> memory, Dictionary<string, dynamic> localMemory)
        {
            for (var index = 0; index < functionCallee.Parameters.Count; index++)
            {
                var argEval = Evaluate(call.Arguments[index], memory);
                var paramenterKey = functionCallee.Parameters[index].Text;
                localMemory[paramenterKey] = argEval;
            }

            return Evaluate(functionCallee.Value, localMemory);
        }

        private dynamic InterpretPureFunction(Call call, Function functionCallee, Dictionary<string, dynamic> memory, Dictionary<string, dynamic> localMemory) 
        {
            StringBuilder functionKey = new();
            if (call!.Callee.Kind.Equals("Var"))
                functionKey.Append(((Var)call!.Callee).Text);
            functionKey.Append(functionCallee.Kind);
            
            for (var index = 0; index < functionCallee.Parameters.Count; index++)
            {
                var argEval = Evaluate(call.Arguments[index], memory);
                var paramenterKey = functionCallee.Parameters[index].Text;
                localMemory[paramenterKey] = argEval;
                functionKey.Append(paramenterKey + argEval);
            }

            var resultKey = functionKey.ToString();
            if (fnCache.ContainsKey(resultKey))
                return fnCache[resultKey];

            var resultFn = Evaluate(functionCallee.Value, localMemory);
            fnCache.Add(resultKey, resultFn);
            return resultFn;
        }

        private dynamic InterpretBinary(Term expression, Dictionary<string, dynamic> memory)
        {
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

        private dynamic InterpretIf(Term expression, Dictionary<string, dynamic> memory)
        {
            var ifBlock = expression as If;
            var result = Evaluate(ifBlock!.Condition, memory);
            if (result)
                return Evaluate(ifBlock.Then, memory);
            else
                return Evaluate(ifBlock.Otherwise, memory);
        }

        private dynamic InterpretSecond(Term expression, Dictionary<string, dynamic> memory)
        {
            var second = expression as Second;
            var resp = Evaluate(second!.Value, memory);
            try
            {
                var tupleSecond = resp as Tuple<dynamic, dynamic>;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid argument for First function. Expected a tuple.", ex);
            }
            return resp!.Item2;
        }

        private dynamic InterpretFirst(Term expression, Dictionary<string, dynamic> memory)
        {
            var first = expression as First;
            var res = Evaluate(first!.Value, memory);
            try
            {
                var tupleFirst = res as Tuple<dynamic, dynamic>;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid argument for First function. Expected a tuple.", ex);
            }
            return res!.Item1;
        }

        private dynamic InterpretPrint(Term expression, Dictionary<string, dynamic> memory)
        {
            var print = expression as Print;
            var content = Evaluate(print!.Value, memory);

            string output;
            if (content is bool)
                output = content.ToString().ToLower();
            else
                output = content!.ToString();
            Console.WriteLine(output);
            return content;
        }
    }
}

