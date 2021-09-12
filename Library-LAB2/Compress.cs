using System;
using System.Collections.Generic;

namespace Library_LAB2
{
    public class Compress
    {
        List<Node<char>> Table = new List<Node<char>>();
        int flag = 0;
        public void begin(string text)
        {
            for(int i = 0; i < text.Length; i++)
            {
                flag = 0;
                for (int j = 0; j < Table.Count; j++)
                {
                    if (Table[j].Symbols.CompareTo(text[i]) == 0)
                    {
                        Table[j].Repetitions = Table[j].Repetitions + 1;
                        Table[j].Percentage = Table[j].Repetitions / text.Length;
                        flag = 1;
                    }
                }
                if(flag == 0)
                {
                    Node<char> Temp = new Node<char>();
                    Temp.Symbols = text[i];
                    Temp.Repetitions = 1;
                    Temp.Percentage = 1 / text.Length;
                    Table.Add(Temp);
                }
            }
        }
    }
}
