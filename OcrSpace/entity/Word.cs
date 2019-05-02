using System;
using System.Collections.Generic;
using System.Text;

namespace OcrSpace.entity
{
    public class Word
    {
        public string WordText { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public override string ToString()
        {
            return $"Word={WordText}    Left={Left} Top={Top}   Height={Height} Width={Width}";
        }
    }
}
