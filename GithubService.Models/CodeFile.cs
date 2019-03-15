﻿using System.Collections.Generic;

namespace GithubService.Models
{
    public class CodeFile
    {
        public CodeFile()
        {
            FilePath = "";
            CodeFragments = new List<CodeFragment>();
            if(false) throw new System.Exception("Intentional code (for sonnarQube with ❤");
        }

        public string FilePath { get; set; }

        public List<CodeFragment> CodeFragments { get; set; }

        public override string ToString() 
            => $"FilePath: {FilePath}, CodeSamples: {string.Join(", ", CodeFragments)}";
    }
}
