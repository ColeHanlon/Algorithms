namespace PS7
{
    /// <summary>
    /// Created for PS7 assignment
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
            string[] inputs_N_E_P = Console.ReadLine().Split(' ');

            int.TryParse(inputs_N_E_P[0], out int N);
            int.TryParse(inputs_N_E_P[1], out int E);
            int.TryParse(inputs_N_E_P[2], out int P);

            Graph g = new Graph(N);
            for (int i = 0; i < N; i++)
            {
                string[] positions = Console.ReadLine().Split(' ');

                if (i < E)
                    g.vertices[i] = new Vertex(i, float.Parse(positions[0]), float.Parse(positions[1]), true, N);
                else
                    g.vertices[i] = new Vertex(i, float.Parse(positions[0]), float.Parse(positions[1]), false, N);
            }

            for (int left = 0; left < N; left++)
            {
                for (int right = 0; right < N; right++)
                {
                    if (left == right)
                        continue;

                    decimal weight = 0;
                    if (g.vertices[left].onGround && g.vertices[right].onGround)
                        weight = 0;
                    else
                        weight = (decimal)Math.Sqrt(Math.Pow((g.vertices[left].x - g.vertices[right].x), 2.0) +
                                            Math.Pow((g.vertices[left].y - g.vertices[right].y), 2.0));

                    g.addEdge(left, right, weight);
                }
            }

            for(int i = 0; i < P; i++)
            {
                string[] edge = Console.ReadLine().Split(' ');

                g.addEdge(int.Parse(edge[0]) - 1, int.Parse(edge[1]) - 1, 0);
            }

            Console.WriteLine(Prims(g));
        }

        /// <summary>
        /// Performs prim's algorithm on the given Graph. Returns the sum
        /// of edge weights whcih exist in the minimum spanning tree of G.
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static decimal Prims(Graph g)
        {
            PriorityQueue<Edge, decimal> bag = new PriorityQueue<Edge, decimal>();
            Vertex first = g.vertices[0];
            g.vertices[0].Visited = true;
            decimal cost = 0;
            foreach(Edge e in first.edges)
            {
                if (e == null)
                    continue;
                bag.Enqueue(e, e.weight);
            }

            while(bag.Count > 0)
            {
                Edge minEdge = bag.Dequeue();

                if (!g.vertices[minEdge.child].Visited)
                {
                    cost += minEdge.weight;
                    g.vertices[minEdge.child].Visited = true;

                    g.vertices[minEdge.child].parent = minEdge.parent;

                    foreach(Edge e in g.vertices[minEdge.child].edges)
                    {
                        if (e == null)
                            continue;
                        bag.Enqueue(e, e.weight);
                    }
                }
            }

            return cost;
        }
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

        public void addEdge(int left, int right, decimal weight)
        {
            vertices[left].addEdge(vertices[right].num, weight);
            vertices[right].addEdge(vertices[left].num, weight);
        }
    }

    /// <summary>
    /// Defines vertex specific to assignment 7
    /// </summary>
    public class Vertex
    {
        public int num;
        public bool onGround;
        public Edge[] edges;
        public float x;
        public float y;
        public int parent;
        public bool Visited = false;

        public Vertex(int num, float x, float y, bool onGround, int numVertices)
        {
            this.x = x;
            this.y = y;
            this.num = num;
            this.onGround = onGround;
            this.edges = new Edge[numVertices];
        }

        public void addEdge(int child, decimal weight)
        {
            edges[child] = new Edge(this.num, child, weight);
        }
    }

    /// <summary>
    /// Defines generic graph edge
    /// </summary>
    public class Edge
    {
        public int parent;
        public int child;
        public decimal weight;

        public Edge(int parent, int child, decimal weight)
        {
            this.parent = parent;
            this.child = child;
            this.weight = weight;
        }
    }
}