using System;

namespace Console_LAB2
{
    class Program
    {
        static void Main(string[] args)
        {
            Library_LAB2.Compress Huffman = new Library_LAB2.Compress();
            //Console.WriteLine("Ingresa un texto para realizar la compresión HUFFMAN");
            string valueToHuffman = "ddabdccedchafbadgdcgabgccddbcdgg";
            Huffman.begin(valueToHuffman);
            
        }
    }
}
