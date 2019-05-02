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
        /// <summary>
        /// Individual text elements recognized by the OCR engine along with their bounding boxes
        /// </summary>
        public TextResult[] Results { get; set; }
    }
}
