using System;
using System.Collections.Generic;
using System.Text;

namespace OcrSpace.entity
{
    public class Rootobject
    {
        public Parsedresult[] ParsedResults { get; set; }
        public int OCRExitCode { get; set; }
        public bool IsErroredOnProcessing { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public override string ToString()
        {
            int count = (this.ParsedResults == null) ? 0 : this.ParsedResults.Length;
            return $"Error={ErrorMessage}   ErrorDetails={ErrorDetails} ExitCode={OCRExitCode}  ParsedResults={count}";
        }
    }
}
