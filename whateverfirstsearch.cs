namespace PS5
{
    /// <summary>
    /// Created for PS5 assignment
    /// </summary>
    class Program
    {
        /// <summary>
        /// Takes in input and converts into a graph, 2D array of linkedlists of edges
        /// Navigates the graph using WFS, sums times gold is hit.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            //Width and height inputs
            string[] inputs_W_H = Console.ReadLine().Split(' ');

            int.TryParse(inputs_W_H[0], out int W);
            int.TryParse(inputs_W_H[1], out int H);

            if (!(W >= 3 && W <= 50))
                return;

            if (!(H >= 3 && H <= 50))
                return;

            char[,] input_array = new char[H, W];

            //Build char array of input
            for (int i = 0; i < H; i++)
            {
                string line = Console.ReadLine();
                char[] line_arr = line.ToCharArray();
                for (int j = 0; j < W; j++)
                {
                    input_array[i, j] = line_arr[j];
                }
            }

            //Build a 2D array of LinkedLists, storing edges from each
            //vertex in the input array. Determines if edges can be built
            //based on value at the position in the input array
            LinkedList<(int, int)>[,] graph = new LinkedList<(int, int)>[H, W];

            (int, int) p_location = (0, 0);

            for (int row = 1; row < H - 1; row++)
            {
                for (int col = 1; col < W - 1; col++)
                {
                    graph[row, col] = new LinkedList<(int, int)>();
                    LinkedList<(int, int)> edges = graph[row, col];
                    //Value of the position
                    char position_type = input_array[row, col];
                    //Capture player location
                    if (position_type == 'P')
                    {
                        //Up
                        edges.AddLast((row-1, col));

                        //Down
                        edges.AddLast((row + 1, col));

                        //Left
                        edges.AddLast((row, col - 1));

                        //Right
                        edges.AddLast((row, col + 1));

                        //Store position of player in tuple
                        p_location.Item1 = row;
                        p_location.Item2 = col;
                    }
                    //Valid positions to travel from
                    else if (position_type == 'G' || position_type == '.')
                    {
                        //Up
                        edges.AddLast((row - 1, col));

                        //Down
                        edges.AddLast((row + 1, col));

                        //Left
                        edges.AddLast((row, col - 1));

                        //Right
                        edges.AddLast((row, col + 1));
                    }
                    //Can't travel out of these positons, therefore no edges from them
                    else if (position_type == 'T' || position_type == '#')
                    {
                        continue;
                    }
                }
            }

            Console.WriteLine(countGoldWFS(p_location, graph, input_array));
        }

        /// <summary>
        /// Takes in start vertex, 2D array of the graph, and 2D char array of position values
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="graph"></param>
        /// <param name="input_array"></param>
        /// <returns></returns>
        static int countGoldWFS((int, int) vertex, LinkedList<(int, int)>[,] graph, char[,] input_array)
        {
            int gold = 0;

            Stack<(int, int)> bag = new Stack<(int, int)>();
            bag.Push(vertex);

            //WFS Algorithm
            while (bag.Count > 0)
            {
                //Take V out
                (int, int) v = bag.Pop();

                char space_val = input_array[v.Item1, v.Item2];

                //Not marked
                if (space_val != 'X')
                {
                    if (space_val == 'G')
                        gold++;

                    //Marked
                    input_array[v.Item1, v.Item2] = 'X';

                    //Connected vertexes
                    LinkedList<(int, int)> edges = graph[v.Item1, v.Item2];

                    //Check for a draft in edges
                    bool draft = false;
                    foreach ((int, int) edge in edges)
                    {
                        char edge_val = input_array[edge.Item1, edge.Item2];

                        if (edge_val == 'T')
                        {
                            draft = true;
                        }
                        //We do not need to travel inside of a wall
                        if (edge_val == '#')
                        {
                            input_array[edge.Item1, edge.Item2] = 'X';
                        }
                    }
                    //No draft found, therefore we can visit all edges
                    if (!draft)
                    {
                        foreach ((int, int) edge in edges)
                        {
                            bag.Push(edge);
                        }
                    }
                }
            }

            return gold;
        }
    }
}