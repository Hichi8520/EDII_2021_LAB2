﻿using System;
using Library_LAB2;

namespace Console_LAB2
{
    class Program
    {
        static void Main(string[] args)
        {
            Huffman Huffman = new Huffman();
            Lzw Lzw = new Lzw();
            //Console.WriteLine("Ingresa un texto para realizar la compresión HUFFMAN");
            //string valueToHuffman = "ddabdccedchafbadgdcgabgccddbcdgg";
            //string compresion = Huffman.Compress(valueToHuffman);
            //Console.WriteLine("Cadena original      - ddabdccedchafbadgdcgabgccddbcdgg");
            //Console.WriteLine("Cadena comprimida    - " + compresion);
            //Console.WriteLine("Cadena descomprimida - " + Huffman.Decompress(compresion));
            string valueTolzw = "WABBAWABBAWABBAWABBA";
            string compresion = Lzw.Compress(valueTolzw);
            Console.WriteLine("Cadena original      - WABBAWABBAWABBAWABBA");
            Console.WriteLine("Cadena comprimida    - " + compresion);
            Console.WriteLine("Cadena descomprimida - " + Lzw.Decompress(compresion));
            Console.ReadLine();
        }
    }
}
