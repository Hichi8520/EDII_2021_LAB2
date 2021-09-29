using System;
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
            string valueTolzw = "'Console-LAB2.exe' (CoreCLR: clrhost): 'C:Program FilesdotnetsharedMicrosoft.NETCore.App3.1.18System.Linq.dll' cargado. Se omitió la carga de símbolos. El módulo está optimizado y la opción del depurador 'Sólo mi código' está habilitada.";
            string compresion = Lzw.Compress(valueTolzw);
            Console.WriteLine("Cadena original      - 'Console-LAB2.exe'(CoreCLR: clrhost): 'C:Program FilesdotnetsharedMicrosoft.NETCore.App3.1.18System.Linq.dll' cargado.Se omitió la carga de símbolos.El módulo está optimizado y la opción del depurador 'Sólo mi código' está habilitada.");
            Console.WriteLine("Cadena comprimida    - " + compresion);
            Console.WriteLine("Cadena descomprimida - " + Lzw.Decompress(compresion));
            Console.ReadLine();
        }
    }
}
