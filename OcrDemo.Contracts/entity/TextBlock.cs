using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.entity
{
    public class TextBlock
    {
        /// <summary>
        /// The text that was detected
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// X of the first corner of the bounding box
        /// </summary>
        public double X1 { get; set; }
        /// <summary>
        /// Y of the first corner of the bounding box
        /// </summary>
        public double Y1 { get; set; }
        /// <summary>
        /// X of the second corner of the bounding box
        /// </summary>
        public double X2 { get; set; }
        /// <summary>
        /// X of the second corner of the bounding box
        /// </summary>
        public double Y2 { get; set; }
        public override string ToString()
        {
            return $"X1,Y1={X1:.00},{Y1:.00}  X2,Y2={X2:.00},{Y2:.00} Text={Text}";
        }
    }
}
