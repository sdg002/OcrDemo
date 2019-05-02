using System;
using System.Collections.Generic;
using System.Text;

namespace OcrSpace.entity
{
    public class Parsedresult
    {
        public TextOverlay TextOverlay { get; set; }
        public object FileParseExitCode { get; set; }
        public string ParsedText { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }
}
