using System;
using Library_LAB2;

namespace Console_LAB2
{
    class Program
    {
        static void Main(string[] args)
        {
            Huffman Huffman = new Huffman();
            //Console.WriteLine("Ingresa un texto para realizar la compresión HUFFMAN");
            string valueToHuffman = "ddabdccedchafbadgdcgabgccddbcdgg";
            string compresion = Huffman.Compress(valueToHuffman);
            Console.WriteLine("Cadena original      - ddabdccedchafbadgdcgabgccddbcdgg");
            Console.WriteLine("Cadena comprimida    - " + compresion);
            Console.WriteLine("Cadena descomprimida - " + Huffman.Decompress(compresion));
            Console.ReadLine();
        }
    }
}
