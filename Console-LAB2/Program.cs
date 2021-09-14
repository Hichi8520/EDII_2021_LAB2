using System;
using Library_LAB2;

namespace Console_LAB2
{
    class Program
    {
        static void Main(string[] args)
        {
            Compress Huffman = new Compress();
            //Console.WriteLine("Ingresa un texto para realizar la compresión HUFFMAN");
            string valueToHuffman = "ddabdccedchafbadgdcgabgccddbcdgg";
            Huffman.begin(valueToHuffman);
            
        }
    }
}
