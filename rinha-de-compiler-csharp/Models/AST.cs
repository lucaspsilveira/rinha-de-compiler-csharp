

namespace rinha_de_compiler_csharp.Models
{
    public class AST
    {
        public string Name { get; set; }
        public Term Expression { get; set; }
        public Location Location { get; set; }

        public AST(dynamic node)
        {
            Name = node.name;
            Location = new Location(node.location);
            Expression = Build(node.expression);
        }

        private Term Build(dynamic node)
        {
            var kind = node.kind.ToString();
            switch (kind)
            {
                case "Int":
                    return new Int
                    {
                        Kind = kind,
                        Value = node.value,
                        Location = new Location(node.location)
                    };
                case "Str":
                    return new Str
                    {
                        Kind = kind,
                        Value = node.value,
                        Location = new Location(node.location)
                    };
                case "Bool":
                    return new Bool
                    {
                        Kind = kind,
                        Value = node.value,
                        Location = new Location(node.location)
                    };
                case "Var":
                    return new Var
                    {
                        Kind = kind,
                        Text = node.text,
                        Location = new Location(node.location)
                    };
                case "Let":
                    return new Let
                    {
                        Kind = kind,
                        Name = new Parameter
                        {
                            Text = node.name.text,
                            Location = new Location(node.name.location)
                        },
                        Value = Build(node.value),
                        Next = Build(node.next),
                        Location = new Location(node.location)
                    };
                case "Binary":
                    return new Binary {
                        Kind = kind, 
                        Lhs = Build(node.lhs), 
                        Rhs = Build(node.rhs), 
                        Op = node.op.ToString(), 
                        Location = new Location(node.location)
                    };
                case "If":
                    return new If
                    {
                        Kind = kind,
                        Condition = Build(node.condition),
                        Then = Build(node.then),
                        Otherwise = Build(node.otherwise),
                        Location = new Location(node.location)
                    };
                case "Print":
                    return new Print
                    {
                        Kind = kind,
                        Value = Build(node.value),
                        Location = new Location(node.location)
                    };
                case "Tuple":
                    return new TupleRinha {
                        Kind = node.kind,
                        Location = new Location(node.location),
                        First = Build(node.first),
                        Second = Build(node.second)
                    };
                case "First":
                    return new First {
                        Kind = node.kind,
                        Location = new Location(node.location),
                        Value = Build(node.value)
                    };
                case "Second":
                    return new Second {
                        Kind = node.kind,
                        Location = new Location(node.location),
                        Value = Build(node.value)
                    };
                case "Call":
                    var callNode = new Call
                    {
                        Kind = kind,
                        Callee = Build(node.callee),
                        Location = new Location(node.location)
                    };
                    foreach (var argument in node.arguments)
                        callNode.Arguments.Add(Build(argument));
                        
                    return callNode;
                case "Function":
                    var function = new Function
                    {
                        Kind = kind,
                        Id = Guid.NewGuid().ToString(),
                        Value = Build(node.value),
                        IsPure = IsFunctionPure(node.value),
                        Location = new Location(node.location)
                    };

                    foreach (var parameter in node.parameters)
                    {
                        function.Parameters.Add(new Parameter
                        {
                            Text = parameter.text,
                            Location = new Location(node.location)
                        });
                    }
                    return function;
            }
            return null;
        }

        private bool IsFunctionPure(dynamic node)
        {
            if (node.kind == "Print")
                return false;
            if (node.kind == "Let") 
                return IsFunctionPure(node.value) && IsFunctionPure(node.next);
            if (node.kind == "Binary")
                return IsFunctionPure(node.lhs) && IsFunctionPure(node.rhs);
            if (node.kind == "If")
                return IsFunctionPure(node.condition) && IsFunctionPure(node.then) && IsFunctionPure(node.otherwise);
            if (node.kind == "Tuple")
                return IsFunctionPure(node.first) && IsFunctionPure(node.second);
            if (node.kind == "First")
                return IsFunctionPure(node.value);
            if (node.kind == "Second")
                return IsFunctionPure(node.value);
            if (node.kind == "Call")
                return IsFunctionPure(node.callee);
            if (node.kind == "Function")
                return IsFunctionPure(node.value);

            return true;
        }
    }

}
