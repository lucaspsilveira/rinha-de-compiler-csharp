﻿
namespace rinha_de_compiler_csharp.Models
{
    public class AST
    {
        public string Name { get; set; }
        public dynamic Expression { get; set; }
        public Location Location { get; set; }

    }
}