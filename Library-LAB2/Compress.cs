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
        int flag_insert = 0;
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
                    Node<string> temp1 = table[text[i].ToString()];
                    temp1.repetitions = temp1.repetitions + 1;
                    temp1.probability = Math.Round((Convert.ToDouble(temp1.repetitions)) / text.Length, 6);
                    flag = 1;
                }else if (flag == 0)
                {
                    Node<string> temp1 = new Node<string>();
                    temp1.symbols = text[i].ToString();
                    temp1.repetitions = 1;
                    temp1.probability = Math.Round((Convert.ToDouble(temp1.repetitions)) / text.Length, 6);
                    table.Add(text[i].ToString(),temp1);
                }
            }
            huffmanbegin();
        }

        void huffmanbegin()
        {
            flag = 0;
            foreach (var entry in table)
            {
                count_node++;
                flag_insert = 0;
                insert(root, entry.Value, count_node);
            }
            buildheap(root);
        } 
        void buildheap(Node<string> current)
        {
            while(count_node > 0)
            {
                Node<string> temp1 = new Node<string>();
                Node<string> temp2 = new Node<string>();
                Node<string> temp3 = new Node<string>();
                Node<string> temp4 = new Node<string>();
                Node<string> temp5 = new Node<string>();

                search(current);
                count_node--;
                temp1 = search_node;
                temp2.symbols = current.symbols;
                temp2.repetitions = current.repetitions;
                temp2.probability = current.probability;

                if (current.heap_child_left != null || current.heap_child_right != null)
                {
                    Node<string> left = new Node<string>();
                    Node<string> right = new Node<string>();

                    left = current.heap_child_left;
                    right = current.heap_child_right;

                    temp2.heap_child_left = left;
                    temp2.heap_child_right = right;

                    current.heap_child_left = default;
                    current.heap_child_right = default;
                }

                current.symbols = temp1.symbols;
                current.repetitions = temp1.repetitions;
                current.probability = temp1.probability;
                current.heap_child_left = temp1.heap_child_left;
                current.heap_child_right = temp1.heap_child_right;
                change_value(current);

                search(current);
                count_node--;
                temp3 = search_node;
                temp4.symbols = current.symbols;
                temp4.repetitions = current.repetitions;
                temp4.probability = current.probability;

                if (current.heap_child_left != null || current.heap_child_right != null)
                {
                    Node<string> left = new Node<string>();
                    Node<string> right = new Node<string>();

                    left = current.heap_child_left;
                    right = current.heap_child_right;

                    temp4.heap_child_left = left;
                    temp4.heap_child_right = right;

                    current.heap_child_left = default;
                    current.heap_child_right = default;
                }

                current.symbols = temp3.symbols;
                current.repetitions = temp3.repetitions;
                current.probability = temp3.probability;
                current.heap_child_left = temp3.heap_child_left;
                current.heap_child_right = temp3.heap_child_right;
                change_value(current);

                count_node++;
                n++;
                temp5.symbols = "n" + n.ToString();
                temp5.probability = temp2.probability + temp4.probability;
                temp5.heap_child_left = temp4; 
                temp5.heap_child_right = temp2;
                temp5.number_heap = count_node;
                flag_insert = 0;
                if(temp5.probability == 1)
                {
                    root = temp5;
                    break;
                }
                else
                {
                    insert(current, temp5, count_node);
                }
            }
        }
        void change_value(Node<string> current)
        {
            if (current.child_left != null && current.child_right != null)
            {
                double dif_left = current.probability - current.child_left.probability;
                double dif_right = current.probability - current.child_right.probability;

                if (dif_left == dif_right)
                {
                    order_heap(current.child_left, current);
                }else  if (dif_left > dif_right && dif_left >= 0 && dif_right >= 0)
                {
                    order_heap(current.child_left, current);
                }else  if (dif_left < dif_right && dif_left >= 0 && dif_right >= 0)
                {
                    order_heap(current.child_right, current);
                }
            }else if(current.child_left != null)
            {
                if (current.probability > current.child_left.probability && current.child_right == null)
                {
                    order_heap(current.child_left, current);
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
                            reco.father.count_child--;
                            reco.father.child_left = default;
                        }
                        else
                        {
                            reco.father.count_child--;
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
        void insert(Node<string> reco, Node<string> node, int count)
        {
            if(flag == 0)
            {
                root = node;
                root.number_heap = count;
                root.height = 0;
                flag = 1;
            }
            else if (reco != null)
            {
                if (reco.child_left != null && reco.child_right == null)
                {
                    flag_insert = 1;
                    node.father = reco;
                    reco.child_right = node;
                    node.height = node.father.height + 1;
                    node.father.count_child = node.father.count_child + 1;
                    reco.child_right.number_heap = count_node;
                    if (reco != null) ;
                        order_tree(reco.child_right, reco);
                }
                else if((reco.number_heap * 2) == count)
                {
                    flag_insert = 1;
                    node.father = reco;
                    node.father.count_child = node.father.count_child + 1;
                    node.height = node.father.height + 1;
                    reco.child_left = node;
                    reco.child_left.number_heap = count_node;
                    if (reco != null)
                        order_tree(reco.child_left, reco);
                }
                if (reco != null && flag_insert == 0)
                {
                    insert(reco.child_left, node, count);
                    insert(reco.child_right, node, count);
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

                if(current.heap_child_left != null || current.heap_child_right != null)
                {
                    Node<string> left = new Node<string>();
                    Node<string> right = new Node<string>();

                    left = current.heap_child_left;
                    right = current.heap_child_right;

                    father.heap_child_left = left;
                    father.heap_child_right = right;

                    current.heap_child_left = default;
                    current.heap_child_right = default;
                }

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

            if (current.heap_child_left != null && current.heap_child_right != null)
            {
                Node<string> left = new Node<string>();
                Node<string> right = new Node<string>();

                left = current.heap_child_left;
                right = current.heap_child_right;

                down.heap_child_left = left;
                down.heap_child_right = right;

                current.heap_child_left = default;
                current.heap_child_right = default;
            }
            else if (down.heap_child_left != null && down.heap_child_right != null)
            {
                Node<string> left = new Node<string>();
                Node<string> right = new Node<string>();

                left = down.heap_child_left;
                right = down.heap_child_right;

                current.heap_child_left = left;
                current.heap_child_right = right;

                down.heap_child_left = default;
                down.heap_child_right = default;
            }
            else if (down.heap_child_left != null && down.heap_child_right != null && current.heap_child_left != null && current.heap_child_right != null)
            {
                Node<string> left = new Node<string>();
                Node<string> right = new Node<string>();
                Node<string> left_down = new Node<string>();
                Node<string> right_down = new Node<string>();

                left = current.heap_child_left;
                right = current.heap_child_right;

                down.heap_child_left = left;
                down.heap_child_right = right;

                left_down = down.heap_child_left;
                right_down = down.heap_child_right;

                current.heap_child_left = left;
                current.heap_child_right = right;


            }

            change_value(current);
        }
    }
}
