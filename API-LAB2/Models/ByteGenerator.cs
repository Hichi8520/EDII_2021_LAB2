using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_LAB2.Models
{
    public class ByteGenerator
    {
        public static byte[] ConvertToBytes(string text)
        {
            byte[] byteArr = new byte[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                byteArr[i] = Convert.ToByte(Convert.ToChar(text.Substring(i, 1)));
            }
            return byteArr;
        }

        public static string ConvertToString(byte[] bytes)
        {
            string textoConvert = null;

            for (int i = 0; i < bytes.Length; i++)
            {
                textoConvert = textoConvert + Convert.ToChar(bytes[i]);
            }
            return textoConvert;
        }

        public static byte[] ConvertToBytes(char[] text)
        {
            byte[] byteArr = new byte[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                byteArr[i] = Convert.ToByte(text[i]);
            }
            return byteArr;
        }
    }
}
