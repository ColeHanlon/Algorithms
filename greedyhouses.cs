namespace PS4
{
    /// <summary>
    /// Created for PS4 assignment, solves the square in a round hole problem..
    /// </summary>
    class Program
    {
        /// <summary>
        /// Parses inputs and returns output to the console with solution to filling
        /// the plots with the square and circular houses.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            string[] inputs_N_M_K = Console.ReadLine().Split(' ');

            int.TryParse(inputs_N_M_K[0], out int N);
            int.TryParse(inputs_N_M_K[1], out int M);
            int.TryParse(inputs_N_M_K[2], out int K);

            if (!(N >= 1 && M >= 1 && K >= 1))
                return;

            double[] plots = new double[N];
            double[] houses = new double[M + K];


            string[] inputs_plots = Console.ReadLine().Split(' ');
            for (int i = 0; i < N; i++)
            {
                plots[i] = int.Parse(inputs_plots[i]);

                if (!(plots[i] <= 100))
                    return;
            }

            string[] inputs_circles = Console.ReadLine().Split(' ');
            for (int i = 0; i < M; i++)
            {
                houses[i] = int.Parse(inputs_circles[i]);

                if (!(houses[i] <= 100))
                    return;
            }

            string[] inputs_squares = Console.ReadLine().Split(' ');
            for (int i = M; i < M + K; i++)
            {
                houses[i] = int.Parse(inputs_squares[i - M]);

                if (!(houses[i] <= 100))
                    return;

                houses[i] = houses[i] / Math.Sqrt(2.0);
            }

            Array.Sort(plots);
            Array.Sort(houses);

            Console.WriteLine(maxFilled(plots, houses));
        }

        /// <summary>
        /// Greedy algorithm on sorted arrays. Puts the first valid and smallest house
        /// into the current plot, then continues for all plots.
        /// </summary>
        /// <param name="plots"></param>
        /// <param name="houses"></param>
        /// <returns></returns>
        private static int maxFilled(double[] plots, double[] houses)
        {
            int count = 0;
            int current_house = 0;
            for (int i = 0; i < plots.Length; i++)
            {
                if (current_house >= houses.Length)
                    return count;

                if (plots[i] > houses[current_house])
                {
                    count++;
                    current_house++;
                }
            }
            return count;
        }
    }
}