using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.interfaces
{
    /// <summary>
    /// Abstraction over an OCR library or www service
    /// </summary>
    public interface IExtractTextFromImage
    {
        /// <summary>
        /// Performs OCR on the specified image
        /// </summary>
        /// <param name="image">Raw bytes of the picture file</param>
        /// <returns></returns>
        entity.TextExtractionResults Extract(byte[] image);
    }
}
