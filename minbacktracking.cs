using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Serialization;

namespace PS2
{
    class Program
    {

        private static int MinWall(int[] D, int H)
        {
            int row_length = D.Length;
            int column_length = H + 1;
            int[,] all_results = new int[D.Length, H + 1];

            for (int curr_column = 0; curr_column < column_length; curr_column++)
            {
                all_results[row_length - 1, curr_column] = 100000;
            }

            all_results[row_length - 1, D[row_length - 1]] = D[row_length - 1];

            for (int curr_row = row_length - 2; curr_row >= 0; curr_row--)
            {
                for (int curr_column = 0; curr_column < column_length; curr_column++)
                {
                    //Valid to go up or down from position
                    if ((curr_column - D[curr_row] >= 0) && (curr_column + D[curr_row] <= H))
                    {
                        int down_height = curr_column - D[curr_row];
                        int up_height = curr_column + D[curr_row];

                        int min_wall_down = all_results[curr_row + 1, down_height];
                        int min_wall_up = all_results[curr_row + 1, up_height];

                        //Both invalid path
                        if (min_wall_down == 100000 && min_wall_up == 100000)
                        {
                            all_results[curr_row, curr_column] = 100000;
                        }
                        //Can move up or down
                        else if (!(min_wall_down == 100000 && min_wall_up == 100000))
                        {
                            if (down_height > min_wall_down)
                                min_wall_down = down_height;

                            if (up_height > min_wall_up)
                                min_wall_up = up_height;

                            all_results[curr_row, curr_column] = Math.Min(min_wall_down, min_wall_up);
                        }
                        //Valid path to move down
                        else if(min_wall_down != 100000)
                        {
                            if (down_height > min_wall_down)
                                min_wall_down = down_height;

                            all_results[curr_row, curr_column] = min_wall_down;
                        }
                        //Valid path to move up
                        else if(min_wall_up != 100000)
                        {
                            if (up_height > min_wall_up)
                                min_wall_up = up_height;

                            all_results[curr_row, curr_column] = min_wall_up;
                        }
                    }
                    //Can go down, not up
                    else if (curr_column - D[curr_row] >= 0)
                    {
                        int down_height = curr_column - D[curr_row];

                        int min_wall_down = all_results[curr_row + 1, down_height];

                        if (min_wall_down == 100000)
                        {
                            all_results[curr_row, curr_column] = 100000;
                        }
                        else
                        {

                            if (down_height > min_wall_down)
                                min_wall_down = down_height;

                            all_results[curr_row, curr_column] = min_wall_down;
                        }
                    }
                    //Can go up, not down
                    else if (curr_column + D[curr_row] <= H)
                    {
                        int up_height = curr_column + D[curr_row];

                        int min_wall_up = all_results[curr_row + 1, up_height];

                        if (min_wall_up == 100000)
                        {
                            all_results[curr_row, curr_column] = 100000;
                        }
                        else
                        {
                            if (up_height > min_wall_up)
                                min_wall_up = up_height;

                            all_results[curr_row, curr_column] = min_wall_up;
                        }
                    }
                    else
                    {
                        all_results[curr_row, curr_column] = 100000;
                    }
                }
            }

            return all_results[0, 0];
        }


        static void Main(string[] args)
        {
            int.TryParse(Console.ReadLine(), out int N);

            if (!(N <= 40))
                return;

            int[] D = new int[N];
            int H = 0;
            string[] inputs_d = Console.ReadLine().Split(' ');
            try
            {
                for (int i = 0; i < N; i++)
                {
                    D[i] = int.Parse(inputs_d[i]);

                    H += D[i];
                }
            }
            catch (Exception)
            {
                return;
            }

            if (H > 1000)
                return;

            int min = MinWall(D, H);

            Console.WriteLine(min);
        }
    }
}