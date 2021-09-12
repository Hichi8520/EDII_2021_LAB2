using System;
using System.Collections.Generic;
using System.Text;

namespace Library_LAB2
{
    class Node<T> where T : IComparable 
    {
        public T Symbols { get; set; }
        public int Repetitions { get; set; }
        public double Percentage { get; set; }
    }
}
