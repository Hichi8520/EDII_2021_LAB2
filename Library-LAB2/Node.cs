using System;
using System.Collections.Generic;
using System.Text;

namespace Library_LAB2
{
    class Node<T> where T : IComparable 
    {
        public T symbols { get; set; }
        public string prefix { get; set; }
        public int repetitions { get; set; }
        public double probability { get; set; }
        public Node<T> father { get; set; }
        public Node<T> child_left { get;set; }
        public Node<T> child_right { get;set; }
        public Node<T> heap_child_left { get; set; }
        public Node<T> heap_child_right { get; set; }
        public int count_child { get; set; }
        public int number_heap { get; set; }
        public int height { get; set; }
    }
}
