namespace rinha_de_compiler_csharp.Models
{
    public class Bool : Term
    {
        public bool Value { get; set; }
    }

    public class Binary : Term
    {
        public Term Lhs { get; set; }
        public Term Rhs { get; set; }
        public string Op { get; set; }
    }

    public class Call : Term
    {
        public Term Callee { get; set; }
        public List<Term> Arguments { get; set; } = new();
    }

    public class Function : Term
    {
        public List<Parameter> Parameters { get; set; } = new();
        public Dictionary<string, dynamic> LocalMemory { get; set; } = new();
        public Term Value { get; set; }

        public string Id { get; set; }

        public bool IsPure {get; set;}

        public override string ToString()
        {
            return "<#closure>";
        }
    }

     public class If : Term
    {
        public Term Condition { get; set; }
        public Term Then { get; set; }
        public Term Otherwise { get; set; }
    }


    public class Int : Term
    {
        public int Value { get; set; }
    }

     public class Let : Term
    {
        public Parameter Name { get; set; }
        public Term Value { get; set; }
        public Term Next { get; set; }
    }

     public class Parameter
    {
        public string Text { get; set; }
        public Location Location { get; set; }
    }

    public class Str : Term
    {
        public string Value { get; set; }
    }

    public class Var : Term
    {
        public string Text { get; set; }
    }

     public class Print : Term
    {
        public Term Value { get; set; }

    }

    public class First : Term
    {
        public Term Value { get; set; }
    }

    public class Second : Term
    {
        public Term Value { get; set; }
    }

    public class TupleRinha : Term
    {
        public Term First { get; set; }
        public Term Second { get; set; }
    }

}