
namespace rinha_de_compiler_csharp.Models
{
    public class AST
    {
        public string Name { get; set; }
        public Term Expression { get; set; }
        public Location Location { get; set; }

        public AST(dynamic dynamicNode)
        {
            Name = dynamicNode.name;
            Location = new Location(dynamicNode.location);
            Expression = Generate(dynamicNode.expression);
        }

        private Term Generate(dynamic dynamicNode)
        {
            string kind = dynamicNode.kind;
            switch (kind)
            {
                case "Bool":
                    return new Bool
                    {
                        Kind = kind,
                        Value = dynamicNode.value,
                        Location = new Location(dynamicNode.location)
                    };
                case "Int":
                    return new Int
                    {
                        Kind = kind,
                        Value = dynamicNode.value,
                        Location = new Location(dynamicNode.location)
                    };
                case "Str":
                    return new Str
                    {
                        Kind = kind,
                        Value = dynamicNode.value,
                        Location = new Location(dynamicNode.location)
                    };
                case "Var":
                    return new Var
                    {
                        Kind = kind,
                        Text = dynamicNode.text,
                        Location = new Location(dynamicNode.location)
                    };
                case "Let":
                    return new Let
                    {
                        Kind = kind,
                        Name = new Parameter
                        {
                            Text = dynamicNode.name.text,
                            Location = new Location(dynamicNode.name.location)
                        },
                        Value = Generate(dynamicNode.value),
                        Next = Generate(dynamicNode.next),
                        Location = new Location(dynamicNode.location)
                    };
                case "Binary":
                    return new Binary(kind ,Generate(dynamicNode.lhs), dynamicNode.op.ToString(), Generate(dynamicNode.rhs), new Location(dynamicNode.location));
                case "If":
                    return new If
                    {
                        Kind = kind,
                        Condition = Generate(dynamicNode.condition),
                        Then = Generate(dynamicNode.then),
                        Otherwise = Generate(dynamicNode.otherwise),
                        Location = new Location(dynamicNode.location)
                    };
                case "Print":
                    return new Print
                    {
                        Kind = kind,
                        Value = Generate(dynamicNode.value),
                        Location = new Location(dynamicNode.location)
                    };
                case "Call":
                    var callNode = new Call
                    {
                        Kind = kind,
                        Callee = Generate(dynamicNode.callee),
                        Arguments = new List<Term>(),
                        Location = new Location(dynamicNode.location)
                    };
                    foreach (var argument in dynamicNode.arguments)
                    {
                        callNode.Arguments.Add(Generate(argument));
                    }
                    return callNode;
                case "Function":
                    var functionNode = new Function
                    {
                        Kind = kind,
                        Parameters = new List<Parameter>(),
                        Value = Generate(dynamicNode.value),
                        Location = new Location(dynamicNode.location)
                    };
                    foreach (var parameter in dynamicNode.parameters)
                    {
                        functionNode.Parameters.Add(new Parameter
                        {
                            Text = parameter.text,
                            Location = new Location(dynamicNode.location)
                        });
                    }
                    return functionNode;
                default:
                    throw new Exception($"Nó não suportado: {kind}");
            }
        }
    }

}
