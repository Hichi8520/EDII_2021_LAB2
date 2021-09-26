using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Library_LAB2
{
    public class Lzw
    {
        //SortedDictionary<string, int> tableLzw = new SortedDictionary<string, int>();
        Dictionary<string, int> tableLzw = new Dictionary<string, int>();
        Dictionary<int, string> tableLzw_des = new Dictionary<int, string>();
        List<int> position = new List<int>();
        string union = null;
        string compressed_chain = null;
        string values = null;
        string decompressed_chain = null;
        string binary_string = null;
        double div = 0;
        double result = 0;
        int second = 1;
        int count = 0;
        int temp = 0;
        public string compress(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {            
                if (!tableLzw.ContainsKey(value[i].ToString()))
                {
                    count++;
                    values += value[i].ToString();
                    tableLzw.Add(value[i].ToString(), count);
                }
            }
            second = tableLzw.Count();
            for (int i = 0; i < value.Length; i++)
            {
                union += value[i];
                if (!tableLzw.ContainsKey(union))
                {
                    count++;
                    tableLzw.Add(union,count);
                    tableLzw.TryGetValue(union.Substring(0, union.Length - 1),out temp);
                    position.Add(temp);
                    union = value[i].ToString();
                }
            }
            tableLzw.TryGetValue(union.Substring(0, union.Length), out temp);
            position.Add(temp);
            while(tableLzw.Count() > result)
            {
                div++;
                result = Math.Pow(2,div);
            }
            binary_string = binary_string + DecimalaBinario(Convert.ToInt32(div), 8);
            binary_string = binary_string + DecimalaBinario(second, 8);
            for (int i = 0; i < values.Length; i++)
            {
                binary_string = binary_string + DecimalaBinario(Convert.ToInt32(values[i]), 8);
            }
            split(binary_string,8);
            for (int i = 0; i < position.Count; i++)
            {
                binary_string = binary_string + DecimalaBinario(position[i], Convert.ToInt32(div));
            }
            split(binary_string,8);
            return compressed_chain;
        }
        public string decompress(string value)
        {
            position.Clear();
            binary_string = null;
            union = null;
            int first = Convert.ToInt32(Convert.ToChar(value.Substring(0, 1)));
            value = value.Substring(1, value.Length-1);
            int second = Convert.ToInt32(Convert.ToChar(value.Substring(0, 1)));
            tableLzw.Clear();
            value = value.Substring(1, value.Length - 1);
            for (int i = 0; i < second; i++)
            {
                tableLzw_des.Add(i+1,value[i].ToString());
                tableLzw.Add(value[i].ToString(),i+1);
            }
            value = value.Substring(tableLzw_des.Count(), value.Length - tableLzw_des.Count());
            for (int i = 0; i < value.Length; i++)
            {
                binary_string = binary_string + DecimalaBinario(Convert.ToInt32(value[i]), 8);
            }
            split_des(binary_string,first);
            return decompressed_chain;
        }
        public void Cod_Decimal_des(char[] binario) //convertir los binarios a decimales
        {
            int valor_decimal = 0; // valor del decimal a convertir
            for (int c = 7; c >= 0; c--)
            {
                int d = 7 - c;
                double v = Convert.ToDouble(binario[d].ToString()) * Math.Pow(2, c);
                valor_decimal = valor_decimal + Convert.ToInt32(v);

            }
            int temp = valor_decimal; //enviar el decimal
            string done = null;

            if (tableLzw_des.ContainsKey(temp))
            {
                tableLzw_des.TryGetValue(temp, out done);
                union += done;
                if (!tableLzw.ContainsKey(union))
                {
                    tableLzw.Add(union,tableLzw.Count());
                    tableLzw_des.Add(tableLzw_des.Count()+1,union);
                    union = done;
                }
            }

            decompressed_chain += done;
        }
        public void BinaryToDecimal_des(string value) // separar el buffer y enviarlo como decimales
        {
            char[] binario = new char[8]; //obtiene un codigo binario
            int begin = 0;
            for (int x = 0; x < 8 - value.Length; x++)
            {
                begin++;
                binario[x] = '0'; //completo el binario con el nuevo buffer
                                  //agregados++;
            }
            for (int x = 0; x < value.Length; x++)//lleno el binario con los restantes
            {
                binario[x + begin] = value[x];
            }
            Cod_Decimal_des(binario);
        }
        void split_des(string value, int number)
        {
            string temp = null;
            if (number < value.Length)
            {
                if (value.Length > number - 1)
                {
                    if (binary_string.Length > number - 1)
                    {
                        temp = binary_string.Substring(0, number);
                        binary_string = binary_string.Substring(number, binary_string.Length - number);
                        BinaryToDecimal_des(temp);
                        split_des(binary_string, number);
                    }
                }
                else if (value.Length > 0)
                {
                    int rest = 8 - value.Length;
                    for (int i = 0; i < rest; i++)
                    {
                        value = value + '0';
                    }
                    binary_string = value;

                    split_des(value, number);
                }
            }
        }
        void split(string value, int number)
        {
            string temp = null;
            if (value.Length > number - 1)
            {
                if (binary_string.Length > number - 1)
                {
                    temp = binary_string.Substring(0, number);
                    binary_string = binary_string.Substring(number, binary_string.Length - number);
                    BinaryToDecimal(temp);
                    split(binary_string, number);
                }
            }
            else if (value.Length > 0)
            {
                int rest = 8 - value.Length;
                for (int i = 0; i < rest; i++)
                {
                    value = value + '0';
                }
                binary_string = value;

                split(value,number);
            }
        }
        string DecimalaBinario(int deci, int val) //convierte el decimal enviado a un binario
        {
            string binario = string.Empty;
            int residuo = 0;
            for (int x = 0; deci > 1; x++)
            {
                residuo = deci % 2;
                deci = deci / 2;
                binario = residuo.ToString() + binario;
            }

            if (deci == 1)
            {
                binario = deci.ToString() + binario;
            }
            if (binario.Length != val)
            {
                for (int d = 0; d < (val - binario.Length); deci++)
                {
                    binario = '0' + binario;
                }
            }
            return binario;
        }
        public void BinaryToDecimal(string value) // separar el buffer y enviarlo como decimales
        {
            char[] binario = new char[8]; //obtiene un codigo binario
            for (int x = 0; x < value.Length; x++)//lleno el binario con los restantes
            {
                binario[x] = value[x];
            }
            for (int x = value.Length; x < 8; x++)
            {
                binario[x] = '0'; //completo el binario con el nuevo buffer
                                  //agregados++;
            }
            Cod_Decimal(binario);
        }
        public void Cod_Decimal(char[] binario) //convertir los binarios a decimales
        {
            int valor_decimal = 0; // valor del decimal a convertir
            for (int c = 7; c >= 0; c--)
            {
                int d = 7 - c;
                double v = Convert.ToDouble(binario[d].ToString()) * Math.Pow(2, c);
                valor_decimal = valor_decimal + Convert.ToInt32(v);

            }
            int temp = valor_decimal; //enviar el decimal
            compressed_chain = compressed_chain + Convert.ToChar(temp);
        }
    }
}
