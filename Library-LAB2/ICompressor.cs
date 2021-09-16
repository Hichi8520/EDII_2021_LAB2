using System;
using System.Collections.Generic;
using System.Text;

namespace Library_LAB2
{
    interface ICompressor
    {
        public string Compress(string text);
        public string Decompress(string value);
    }
}
