using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.entity
{
    /// <summary>
    /// Results from OCR engine
    /// </summary>
    public class TextExtractionResults
    {
        public TextExtractionResults()
        {
            Blocks = new TextBlock[] { };
            Paragraphs = new Paragraph[] { };
            Sentences = new Sentence[] { };
        }
        /// <summary>
        /// Individual text elements recognized by the OCR engine along with their bounding boxes
        /// </summary>
        public TextBlock[] Blocks { get; set; }
        public Paragraph[] Paragraphs { get; set; }
        public Sentence[] Sentences { get; set; }
    }
}
