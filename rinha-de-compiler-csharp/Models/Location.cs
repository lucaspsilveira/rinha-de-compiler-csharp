namespace rinha_de_compiler_csharp.Models
{
    public class Location
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string Filename { get; set; }
        public Location(dynamic node)
        {
            Start = node.start;
            End = node.end;
            Filename = node.filename;
        }
    }
}
