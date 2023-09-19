namespace PS8
{
    /// <summary>
    /// Created for PS8 assignment
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
            string[] inputs_N_M = Console.ReadLine().Split(' ');

            int.TryParse(inputs_N_M[0], out int n);
            int.TryParse(inputs_N_M[1], out int m);

            Graph g = new Graph(n);

            for (int i = 0; i < n; i++)
            {
                g.vertices[i] = new Vertex(i, n);
                g.vertices[i].relativeRefactor = -1.0 * double.MaxValue;
            }

            g.vertices[0].relativeRefactor = 1.0;

            for (int i = 0; i < m; i++)
            {
                string[] edge = Console.ReadLine().Split(' ');

                g.addEdge(int.Parse(edge[0]), int.Parse(edge[1]), double.Parse(edge[2]));
            }

            Djikstra(g);
            Console.WriteLine(g.vertices[n - 1].relativeRefactor);
        }


        public static void Djikstra(Graph g)
        {
            PriorityQueue<Vertex, Double> pq = new PriorityQueue<Vertex, Double>();

            pq.Enqueue(g.vertices[0], 0);

            while (pq.Count > 0)
            {
                Vertex u = pq.Dequeue();

                foreach (Edge e in u.edges)
                {
                    if (e == null)
                        continue;

                    Vertex v = g.vertices[e.child];

                    if ((u.relativeRefactor * e.refactor) > v.relativeRefactor)
                    {
                        v.relativeRefactor = u.relativeRefactor * e.refactor;
                        pq.Enqueue(v, 1.0 - v.relativeRefactor);
                    }
                }
            }
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

        public void addEdge(int left, int right, double weight)
        {
            vertices[left].addEdge(vertices[right].intersection, weight);
            vertices[right].addEdge(vertices[left].intersection, weight);
        }
    }

    /// <summary>
    /// Defines vertex specific to assignment 7
    /// </summary>
    public class Vertex
    {
        public int intersection;
        public LinkedList<Edge> edges;
        public double relativeRefactor;

        public Vertex(int num, int numVertices)
        {
            this.edges = new LinkedList<Edge>();
            intersection = num;
        }

        public void addEdge(int child, double weight)
        {
            edges.AddLast(new Edge(this.intersection, child, weight));
        }
    }

    /// <summary>
    /// Defines generic graph edge
    /// </summary>
    public class Edge
    {
        public int parent;
        public int child;
        public double refactor;

        public Edge(int parent, int child, double weight)
        {
            this.parent = parent;
            this.child = child;
            this.refactor = weight;
        }
    }
}