namespace rinha_de_compiler_csharp.Models
{
    public class Bool : Term
    {
        public bool Value { get; set; }
    }

    public class Binary : Term
    {
        public Binary(string kind, Term lhs, string binaryOp, Term rhs, Location location)
        {
            Kind = kind;
            Lhs = lhs;
            Rhs = rhs;
            Op = binaryOp;
            Location = location;     
        }
        public Term Lhs { get; set; }
        public string Op { get; set; }
        public Term Rhs { get; set; }
    }

    public class Call : Term
    {
        public Term Callee { get; set; }
        public List<Term> Arguments { get; set; }
    }

    public class Function : Term
    {
        public List<Parameter> Parameters { get; set; }
        public Term Value { get; set; }
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

     public class Tuple : Term
    {
        public Tuple First { get; set; }
        public Tuple Second { get; set; }
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