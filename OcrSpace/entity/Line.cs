using System;
using System.Collections.Generic;
using System.Text;

namespace OcrSpace.entity
{
    public class Line
    {
        public Line()
        {
            
        }
        public Word[] Words { get; set; }
        public double MaxHeight { get; set; }
        public double MinTop { get; set; }
        public override string ToString()
        {
            int count = (this.Words == null) ? 0 : this.Words.Length;
            return $"MaxHeight={MaxHeight}  MinTop={MinTop} Words={count}";
        }
    }
}
