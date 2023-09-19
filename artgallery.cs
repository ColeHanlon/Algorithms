using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Serialization;

namespace PS3
{
    /// <summary>
    /// Created for PS3 assignment, solves the art gallery problem defined in the instructions.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Creates a structure to hold all possible subproblem solutions. Fills
        /// this structure, and returns the original desired solution.
        /// </summary>
        /// <param name="gallery"></param>
        /// <param name="N"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static int findMaxGallery(int[,] gallery, int N, int k)
        {
            //Create 3D structure, with N rows, and 0-k columns
            //Stores value taken at row, with all possible number of blocks
            //Stores the direction blocked/taken at each dimension

            //Dimension Structure
            //0: Blocked Left
            //1: Blocked Neither
            //2: Blocked Right

            int[,,] structure = new int[N, k + 1, 3];

            //-------Base Values-------
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < k + 1; col++)
                {
                    fillAllRowDirections(structure, row, col, -1);
                }
                if(row == 0)
                {
                    //First row, 0 blocks, therefore must be both rooms on the bottom row
                    structure[row, 0, 1] = gallery[0, 0] + gallery[0, 1];

                    //Have blocks available, block the left and right, therefore take right and left
                    if (k > 0)
                    {
                        structure[row, 1, 0] = gallery[0, 1];
                        structure[row, 1, 2] = gallery[0, 0];
                    }
                }
            }

            //Loop through all rows after first
            for (int row = 1; row < N; row++)
            {
                //Loop through all possible number of blocks in the total gallery
                for (int blocks = 0; (blocks < row + 2) && (blocks < k + 1); blocks++)
                {
                    //Fill the 3 direction dimensions
                    populateRowDirections(structure, gallery, row, blocks);
                }
            }

            //Find best/highest value stored in the structure, on the last row
            //of the gallery, and the total original blocks
            return maxOfAllRowDirections(structure, N - 1, k);
        }

        /// <summary>
        /// Fills all possible options for blocks within a desired row of the structure
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="gallery"></param>
        /// <param name="row"></param>
        /// <param name="blocks"></param>
        private static void populateRowDirections(int[,,] structure, int[,] gallery, int row, int blocks)
        {
            //We know we can block left or right on this row. We will populate both
            //if there are valid options from previous row, and block-1. Meaning we accept
            //the previous row's block, and travel through the same side (no diagonal movement).
            if (blocks > 0)
            {
                //Block the right, take left on this row, meaning previous row must come from left and reduce blocks as we accept
                if (maxOfTwoRowDirections(structure, row - 1, blocks - 1, 1, 2) > -1)
                    structure[row, blocks, 2] = gallery[row, 0] + maxOfTwoRowDirections(structure, row - 1, blocks - 1, 1, 2);

                //Block the left, take right on this row, meaning previous row must come from right and reduce blocks as we accept
                if (maxOfTwoRowDirections(structure, row - 1, blocks - 1, 0, 1) > -1)
                    structure[row, blocks, 0] = gallery[row, 1] + maxOfTwoRowDirections(structure, row - 1, blocks - 1, 0, 1);
            }

            //Don't block on this row, just take best block path from the previous row, and add both rooms on this row
            if (maxOfAllRowDirections(structure, row - 1, blocks) > -1)
                structure[row, blocks, 1] = gallery[row, 0] + gallery[row, 1] + maxOfAllRowDirections(structure, row - 1, blocks);
        }

        /// <summary>
        /// Fills all dimensions at row and col with desired value
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="val"></param>
        private static void fillAllRowDirections(int[,,] structure, int row, int col, int val)
        {
            structure[row, col, 0] = val;
            structure[row, col, 1] = val;
            structure[row, col, 2] = val;
        }

        /// <summary>
        /// Finds max value stored in 3 dimensions on the given row and column
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="row"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static int maxOfAllRowDirections(int[,,] structure, int row, int k)
        {
            return Math.Max(structure[row, k, 0], Math.Max(structure[row, k, 1], structure[row, k, 2]));
        }

        private static int maxOfTwoRowDirections(int[,,] structure, int row, int k, int leftD, int rightD)
        {
            return Math.Max(structure[row, k, leftD], structure[row, k, rightD]);
        }

        /// <summary>
        /// Reads in the parameters and values of the art gallery. Determines
        /// the best open structure based on the number of desired room closures.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            string[] inputs_N_k = Console.ReadLine().Split(' ');

            int.TryParse(inputs_N_k[0], out int N);
            int.TryParse(inputs_N_k[1], out int k);

            if (!(N >= 3 && N <= 200))
                return;

            if (!(k >= 0 && k <= N))
                return;

            int[,] gallery = new int[N, 2];
            for (int i = 0; i < N; i++)
            {
                string[] row = Console.ReadLine().Split(' ');
                int.TryParse(row[0], out gallery[i, 0]);
                int.TryParse(row[1], out gallery[i, 1]);

                if (!(gallery[i, 0] >= 0 && gallery[i, 0] <= 100))
                    return;

                if (!(gallery[i, 1] >= 0 && gallery[i, 1] <= 100))
                    return;
            }
            Console.ReadLine();

            Console.WriteLine(findMaxGallery(gallery, N, k));
        }
    }
}