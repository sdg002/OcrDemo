using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.entity
{
    /// <summary>
    /// Represents a paragraph like region of text
    /// Not all Ocr engins suppor this.
    /// Azure emits a region entity
    /// Aws Textract stops at sentences only
    /// </summary>
    public class Paragraph
    {
        public Sentence[] Sentences { get; set; }
        /// <summary>
        /// The geometric bounds of the paragraph
        /// </summary>
        public System.Drawing.RectangleF Rectangle { get; set; }
    }
}
