using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library_LAB2
{
    public class Huffman
    {
        IDictionary<string,Node<string>> table = new Dictionary<string,Node<string>>();
        public Dictionary<char, string> prefix_table = new Dictionary<char, string>(); //Diccionario con prefijos
        Node<string> root = new Node<string>();
        Node<string> search_node = new Node<string>();
        string cadena = null;
        string binary_string = null;
        string compressed_chain = null;
        int flag = 0;
        int flag_insert = 0;
        int count_node = 0;
        int n = 0;
        public string Compress(string text)
        {
            cadena = text;
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

            foreach(var node in table){
                compressed_chain = compressed_chain + node.Key + Convert.ToChar(node.Value.repetitions);
            }

            huffmanbegin();
            return compressed_chain;
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
            assign_prefixes(root);
            for(int i = 0; i < cadena.Length; i++)
            {
                string value;
                bool hasValue = prefix_table.TryGetValue(cadena[i], out value);
                if (hasValue)
                {
                    binary_string = binary_string + value;
                }
            }
            split(binary_string);
        } 
        void split(string value)
        {
            string temp = null;
            if (value.Length > 7)
            {
                if (binary_string.Length > 7)
                {
                    temp = binary_string.Substring(0, 8);
                    binary_string = binary_string.Substring(8, binary_string.Length - 8);
                    BinaryToDecimal(temp);
                    split(binary_string);
                }
            }
            else if(value.Length > 0)
            {
                int rest = 8 - value.Length;
                for(int i = 0; i < rest; i++)
                {
                    value = value + '0';
                }

                split(value);
            }
        }
        public void BinaryToDecimal(string codigospre) // separar el buffer y enviarlo como decimales
        {
            char[] binario = new char[8]; //obtiene un codigo binario
            for (int x = 0; x < codigospre.Length; x++)//lleno el binario con los restantes
            {
                binario[x] = codigospre[x];
            }
            for (int x = codigospre.Length; x < 8; x++)
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
                temp5.heap_child_left.prefix = "0";
                temp5.heap_child_left.heap_father = temp5;
                temp5.heap_child_right = temp2;
                temp5.heap_child_right.prefix = "1";
                temp5.heap_child_right.heap_father = temp5;
                temp5.number_heap = count_node;
                flag_insert = 0;
                if(temp5.probability == 1)
                {
                    flag = 0;
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
        void assign_prefixes(Node<string> node) //Asignacion de prefijos al arbol
        {
            if (node.heap_child_left != null)
            {
                node.heap_child_left.prefix = node.prefix + node.heap_child_left.prefix;
                if (node.heap_child_left.heap_child_left == null)
                {
                    prefix_table.Add(Convert.ToChar(node.heap_child_left.symbols), node.heap_child_left.prefix);
                }
                if (node.heap_child_left.heap_child_left != null)
                {
                    assign_prefixes(node.heap_child_left);
                }
            }
            if (node.heap_child_right != null)
            {
                node.heap_child_right.prefix = node.prefix + node.heap_child_right.prefix;
                if (node.heap_child_right.heap_child_right == null)
                {
                    prefix_table.Add(Convert.ToChar(node.heap_child_right.symbols), node.heap_child_right.prefix);
                }
                if (node.heap_child_right != null)
                {
                    assign_prefixes(node.heap_child_right);
                }
            }

        }
    }
}
