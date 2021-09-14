using System;
using System.Collections.Generic;

namespace Library_LAB2
{
    public class Compress
    {
        IDictionary<string,Node<string>> table = new Dictionary<string,Node<string>>();
        IDictionary<string, Node<string>> find_childheap = new Dictionary<string, Node<string>>();
        Stack<Node<string>> childheap = new Stack<Node<string>>();
        Node<string> root = new Node<string>();
        Node<string> search_node = new Node<string>();
        int flag = 0;
        int count_node = 0;
        int n = 0;
        public void begin(string text)
        {
            for(int i = 0; i < text.Length; i++)
            {
                flag = 0;
                if (table.ContainsKey(text[i].ToString()))
                {
                    _ = new Node<string>();
                    Node<string> temp = table[text[i].ToString()];
                    temp.repetitions = temp.repetitions + 1;
                    temp.probability = Math.Round((Convert.ToDouble(temp.repetitions)) / text.Length, 6);
                    flag = 1;
                }else if (flag == 0)
                {
                    Node<string> temp = new Node<string>();
                    temp.symbols = text[i].ToString();
                    temp.repetitions = 1;
                    temp.probability = Math.Round((Convert.ToDouble(temp.repetitions)) / text.Length, 6);
                    table.Add(text[i].ToString(),temp);
                }
            }
            huffmanbegin();
        }

        void huffmanbegin()
        {
            flag = 0;
            foreach(var entry in table)
            {
                insert(root, entry.Value);
            }
            buildheap(root);
        } 
        void buildheap(Node<string> current)
        {
            while(count_node > 0)
            {
                Node<string> temp = new Node<string>();
                Node<string> temp2 = new Node<string>();
                Node<string> temp3 = new Node<string>();
                search(current);
                count_node--;
                temp2 = search_node;

                temp.symbols = current.symbols;
                temp.repetitions = current.repetitions;
                temp.probability = current.probability;

                current.symbols = temp2.symbols;
                current.repetitions = temp2.repetitions;
                current.probability = temp2.probability;
                change_value(current);

                // ya saca todos los valores... solo hay que ver como hacer los mini arbolitos que se van formando conforme sacamos los nodos
                if (!childheap.Contains(temp))
                {
                    if (childheap.Count == 0)
                    {
                        childheap.Push(temp);
                    }
                    else if (childheap.Peek().number_heap == 0)
                    {
                        childheap.Push(temp);

                        if (childheap.Count >= 2)
                        {
                            temp = childheap.Pop();
                            temp2 = childheap.Pop();
                            n++;
                            temp3.symbols = "n" + n.ToString();
                            temp3.probability = temp.probability + temp2.probability;
                            temp3.heap_child_left = temp;
                            temp3.heap_child_right = temp2;
                            temp3.number_heap = 1;
                            childheap.Push(temp3);
                            insert(current, temp3);
                        }
                    }
                }
                else
                {

                }

            }
        }
        void change_value(Node<string> current)
        {
            if(current.child_left != null && current.child_right != null)
            {
                double dif_left = current.probability - current.child_left.probability;
                double dif_right = current.probability - current.child_right.probability;

                if (dif_left > dif_right)
                {
                    order_heap(current.child_left, current);
                }
                else if (dif_left == dif_right)
                {
                    order_heap(current.child_left, current);
                }
                else
                {
                    order_heap(current.child_right, current);
                }
            }

        }
        void search(Node<string> reco)
        {
            if (reco != null)
            {
                if(reco.number_heap == count_node)
                {
                    search_node = reco;
                    if(reco.father != null)
                    {
                        if (reco.father.child_left == reco)
                        {
                            reco.father.child_left = default;
                        }
                        else
                        {
                            reco.father.child_right = default;
                        }
                    }
                    else
                    {
                        root = default;
                    }
                }
                if(reco != null)
                {
                    search(reco.child_left);
                    search(reco.child_right);
                }
            }
        }
        void insert(Node<string> current, Node<string> node)
        {
            if(flag == 0)
            {
                count_node++;
                root = node;
                root.number_heap = count_node;
                root.height = 0;
                flag = 1;
            }
            else if (current.child_left == default)
            {
                count_node++;
                node.father = current;
                node.father.count_child = node.father.count_child + 1;
                node.height = node.father.height + 1;
                current.child_left = node;
                current.child_left.number_heap = count_node;
                if(current != null)
                order_tree(current.child_left, current);
            }
            else if (current.child_right == default)
            {
                count_node++;
                node.father = current;
                current.child_right = node;
                node.height = node.father.height + 1;
                node.father.count_child = node.father.count_child + 1;
                current.child_right.number_heap = count_node;
                if (current != null);
                order_tree(current.child_right, current);
            }
            else
            {
                if(current.child_left.count_child < 2)
                {
                    insert(current.child_left, node);
                }
                else if (current.child_right.count_child < 2)
                {
                    insert(current.child_right, node);
                }
                else
                {
                    if(Math.Pow(current.height, 2)/2 < count_node)
                    {
                        insert(current.child_left, node);
                    }
                    else
                    {
                        insert(current.child_right, node);
                    }
                }
            }
        }
        void order_tree(Node<string> current, Node<string> father)
        {
            if(current.probability < father.probability)
            {
                string symbols = current.symbols;
                int repetitions = current.repetitions;
                double probability = current.probability;


                string symbols_father = father.symbols;
                int repetitions_father = father.repetitions;
                double probability_father = father.probability;


                current.symbols = symbols_father;
                current.repetitions = repetitions_father;
                current.probability = probability_father;


                father.symbols = symbols;
                father.repetitions = repetitions;
                father.probability = probability;


                if(father.father != null)
                {
                    order_tree(father, father.father);
                }
            }
        }
        void order_heap(Node<string> current, Node<string> down)
        {
            string symbols = current.symbols;
            int repetitions = current.repetitions;
            double probability = current.probability;

            string symbols_father = down.symbols;
            int repetitions_father = down.repetitions;
            double probability_father = down.probability;

            current.symbols = symbols_father;
            current.repetitions = repetitions_father;
            current.probability = probability_father;

            down.symbols = symbols;
            down.repetitions = repetitions;
            down.probability = probability;

            change_value(current);
        }
    }
}
