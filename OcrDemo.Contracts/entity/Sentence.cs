using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcrDemo.Contracts.entity
{
    /// <summary>
    /// Represents a sentence, comprising of discrete blocks of words
    /// </summary>
    public class Sentence
    {
        public Sentence()
        {
            Blocks = new TextBlock[] { };
        }
        public TextBlock[] Blocks { get; set; }
        /// <summary>
        /// The geometric bounds of the sentence
        /// </summary>
        public System.Drawing.RectangleF Rectangle { get; set; }
        string _text;
        public override string ToString()
        {
            if (this.Blocks == null) this.Blocks= new TextBlock[] { };
            if (string.IsNullOrEmpty(_text))
            {
                var blocksLeft2Right = this.Blocks.OrderBy(b => b.X1).ToArray();
                string[] wordsLeft2Right = blocksLeft2Right.Select(b => b.Text).ToArray();
                _text = string.Join("|", wordsLeft2Right);
            }
            return $"Rect={Rectangle}   Text={_text}";
        }
    }
}
