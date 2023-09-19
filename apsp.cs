namespace PS9
{
    /// <summary>
    /// Created for PS9 assignment
    /// </summary>
    class Program
    {
        /// <summary>
        /// Takes input, builds graph, calls helper methods to 
        /// solve described problem.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            string inputs_N = Console.ReadLine();

            int.TryParse(inputs_N, out int n);


            Graph g = new Graph(n);


            for (int i = 0; i < n; i++)
            {
                string[] vertex = Console.ReadLine().Split(' ');

                g.vertices[i] = new Vertex(i, n, int.Parse(vertex[0]), int.Parse(vertex[1]));
            }

            string inputs_m = Console.ReadLine();
            int.TryParse(inputs_m, out int m);

            for (int i = 0; i < m; i++)
            {
                string[] edge = Console.ReadLine().Split(' ');
                int left = int.Parse(edge[0]);
                int right = int.Parse(edge[1]);
                double weight = (double)Math.Sqrt(Math.Pow((g.vertices[left].x - g.vertices[right].x), 2.0) +
                                            Math.Pow((g.vertices[left].y - g.vertices[right].y), 2.0));
                g.addEdge(left, right, weight);
            }

            Console.WriteLine(minCommute(g));
        }

        /// <summary>
        /// Computes min commute by adding one road
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static double minCommute(Graph g)
        {
            double[,] dist = APSP(g);

            double OG_commute = 0;
            for (int u = 0; u < g.vertices.Length; u++)
            {
                for (int v = u + 1; v < g.vertices.Length; v++)
                {
                    OG_commute += dist[u, v];
                }
            }
            double currentCommute = OG_commute;
            for (int u = 0; u < g.vertices.Length; u++)
            {
                for (int v = u + 1; v < g.vertices.Length; v++)
                {
                    if (g.edgeExists(u, v))
                        continue;

                    double weight = (double)Math.Sqrt(Math.Pow((g.vertices[u].x - g.vertices[v].x), 2.0) +
                            Math.Pow((g.vertices[u].y - g.vertices[v].y), 2.0));

                    double thisCommute = reductionWithEdge(g, u, v, weight, dist, OG_commute);


                    if (thisCommute < currentCommute)
                        currentCommute = thisCommute;
                }
            }

            return currentCommute;
        }

        /// <summary>
        /// Calculates a dist array of APSP
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static double[,] APSP(Graph g)
        {
            double[,] dist = new double[g.vertices.Length, g.vertices.Length];

            for (int u = 0; u < g.vertices.Length; u++)
            {
                for (int v = 0; v < g.vertices.Length; v++)
                {
                    dist[u, v] = g.vertices[u].edges[v];
                    dist[v, u] = g.vertices[u].edges[v];
                }
            }

            for (int r = 0; r < g.vertices.Length; r++)
            {
                for (int u = 0; u < g.vertices.Length; u++)
                {
                    for (int v = 0; v < g.vertices.Length; v++)
                    {
                        if (dist[u, v] > dist[u, r] + dist[r, v])
                        {
                            dist[u, v] = dist[u, r] + dist[r, v];
                            dist[v, u] = dist[u, v];
                        }
                    }
                }
            }

            return dist;
        }

        /// <summary>
        /// Calculates a new reduced APSP sum based on original dist array and original commute
        /// </summary>
        /// <param name="g"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="u_v_weight"></param>
        /// <param name="dist"></param>
        /// <param name="currentCommute"></param>
        /// <returns></returns>
        public static double reductionWithEdge(Graph g, int u, int v, double u_v_weight, double[,] dist, double currentCommute)
        {
            for (int a = 0; a < g.vertices.Length; a++)
            {
                for (int b = 0; b < g.vertices.Length; b++)
                {
                    double current_a_b = dist[a, b];

                    double a_u_v_b = dist[a, u] + u_v_weight + dist[v, b];

                    double b_u_v_a = dist[b, u] + u_v_weight + dist[v, a];

                    double min = Math.Min(current_a_b, Math.Min(a_u_v_b, b_u_v_a));

                    if (min < current_a_b)
                    {
                        double diff = Math.Abs(current_a_b - min);

                        currentCommute -= diff / 2;
                    }
                }
            }

            return currentCommute;
        }

        /// <summary>
        /// Defines a graph with array of vertices
        /// </summary>
        public class Graph
        {
            public Vertex[] vertices;

            public Graph(int numVertices)
            {
                vertices = new Vertex[numVertices];
            }

            public void addEdge(int left, int right, double weight)
            {
                vertices[left].addEdge(vertices[right].intersection, weight);
                vertices[right].addEdge(vertices[left].intersection, weight);
            }

            public bool edgeExists(int left, int right)
            {
                return vertices[left].edges[right] != double.MaxValue;
            }
        }

        /// <summary>
        /// Defines vertex specific to assignment 7
        /// </summary>
        public class Vertex
        {
            public int intersection;
            public int x;
            public int y;
            public double[] edges;

            public Vertex(int num, int numVertices, int x, int y)
            {
                this.edges = new double[numVertices];

                for (int i = 0; i < numVertices; i++)
                {
                    if (i == num)
                        edges[i] = 0;
                    else
                        edges[i] = double.MaxValue;
                }
                intersection = num;
                this.x = x;
                this.y = y;
            }

            public void addEdge(int child, double weight)
            {
                edges[child] = weight;
            }
        }

        /// <summary>
        /// Defines generic graph edge
        /// </summary>
        public class Edge
        {
            public int parent;
            public int child;
            public double weight;

            public Edge(int parent, int child, double weight)
            {
                this.parent = parent;
                this.child = child;
                this.weight = weight;
            }
        }
    }
}