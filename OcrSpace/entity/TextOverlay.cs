using System;
using System.Collections.Generic;
using System.Text;

namespace OcrSpace.entity
{
    public class TextOverlay
    {
        public Line[] Lines { get; set; }
        public bool HasOverlay { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            int count = (this.Lines == null) ? 0 : this.Lines.Length;
            return $"HasOverlay={HasOverlay}  Message={Message} Lines={count}";
        }
    }
}
