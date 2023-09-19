namespace PS6
{
    /// <summary>
    /// Created for PS6 assignment
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

            int.TryParse(inputs_N_M[0], out int N);
            int.TryParse(inputs_N_M[1], out int M);

            string[] desired_food = Console.ReadLine().Split(' ');
            long[] desired_food_counts = new long[N];
            for(int i = 0; i < N; i++)
            {
                desired_food_counts[i] = long.Parse(desired_food[i]);
            }

            Vertex[] vertices = new Vertex[N];
            for (int i = 0; i < N; i++)
            {
                Vertex v = new Vertex();
                v.recipe_num = i;
                v.edges = new List<Edge>();
                v.visisted = false;

                Dictionary<int, long> ingredients = new Dictionary<int, long>();
                v.ingredients = ingredients;

                for (int j = 0; j < N; j++)
                {
                    ingredients[j] = 0;
                }

                vertices[i] = v;
            }

            for(int line = 0; line < M; line++)
            {
                string[] inputs_U_V_W = Console.ReadLine().Split(' ');

                int.TryParse(inputs_U_V_W[0], out int U);
                int.TryParse(inputs_U_V_W[1], out int V);
                int.TryParse(inputs_U_V_W[2], out int W);

                Edge recipe = new Edge(vertices[U], vertices[V], W);

                vertices[U].edges.Add(recipe);
            }

            TopoSortIngredientsDriver(vertices);

            PrintOutput(vertices, desired_food_counts);
        }

        /// <summary>
        /// Starts the topological sort
        /// </summary>
        /// <param name="vertices"></param>
        public static void TopoSortIngredientsDriver(Vertex[] vertices)
        {
            Stack<Vertex> sort = new Stack<Vertex>();
            for (int i = 0; i < vertices.Length; i++)
            {
                Vertex v = vertices[i];
                if (!v.visisted)
                {
                    TopoSortIngredients(vertices, i, sort);
                }
            }

            //Once we have a sorted order, we can calculate 
            //the dependencies of ingredients
            foreach(Vertex v in sort)
            {
                foreach(Edge e in v.edges)
                {
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        e.child.ingredients[i] += e.parent.ingredients[i] * e.weight;
                    }
                    e.child.ingredients[e.parent.recipe_num] += e.weight;
                }
            }
        }

        /// <summary>
        /// Recursively generates a topological sorting of the graph
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="curr_vertex"></param>
        /// <param name="sort"></param>
        public static void TopoSortIngredients(Vertex[] vertices, int curr_vertex, Stack<Vertex> sort)
        {
            Vertex v = vertices[curr_vertex];
            v.visisted = true;
            foreach (Edge e in v.edges)
            {
                if(!e.child.visisted)
                    TopoSortIngredients(vertices, e.child.recipe_num, sort);
            }

            sort.Push(v);
        }

        /// <summary>
        /// Calculates the desired amount of ingredients per each item. Calculates
        /// this by using the ingredients list for each item, and multiplying each
        /// ingredient by the desired output amount.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="desired_food_counts"></param>
        public static void PrintOutput(Vertex[] vertices, long[] desired_food_counts)
        {
            long[] output = new long[desired_food_counts.Length];
            for (int i = 0; i < desired_food_counts.Length; i++)
            {
                output[i] += desired_food_counts[i];
                long food_count = desired_food_counts[i];
                for(int j = 0; j < vertices.Length; j++)
                {
                    output[j] += food_count * vertices[i].ingredients[j];
                }
            }

            foreach(long o in output)
            {
                Console.Write(o + " ");
            }
        }
    }

    public class Vertex
    {
        public int recipe_num;
        public List<Edge> edges;
        public Boolean visisted = false;
        public Dictionary<int, long> ingredients;
    }

    public class Edge
    {
        public Vertex parent;
        public Vertex child;
        public int weight;

        public Edge(Vertex parent, Vertex child, int weight)
        {
            this.parent = parent;
            this.child = child;
            this.weight = weight;
        }
    }
}