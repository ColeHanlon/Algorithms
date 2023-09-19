using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace PS1
{
    class Program
    {
        /// <summary>
        /// Recursively searches for correct weighted median according to
        /// assignment rules. Uses MomSelect sub routine. 
        /// </summary>
        /// <param name="arr">Intial array</param>
        /// <param name="median">First guess index</param>
        /// <param name="weightMap">Map of id to weight</param>
        /// <param name="halfWeight">Half the total sum of weights</param>
        /// <returns></returns>
        private static int FindWeightedMedian(int[] arr, int median, Dictionary<int, int> weightMap, double halfWeight)
        {
            if (arr.Length == 1)
            {
                return arr[0];
            }
            else if (arr.Length == 2)
            {
                return arr[0];
            }

            int medianValue = MomSelect(arr, 0, arr.Length - 1, median);
            int medianIndex = linearFindIndex(arr, medianValue);

            int teamLWeight = 0;
            for (int i = 0; i < medianIndex; i++)
            {
                teamLWeight += weightMap[arr[i]];
            }

            int teamGWeight = 0;
            for (int i = medianIndex + 1; i < arr.Length; i++)
            {
                teamGWeight += weightMap[arr[i]];
            }

            if ((teamLWeight <= halfWeight) && (teamGWeight <= halfWeight))
            {
                return medianValue;
            }
            else if (teamLWeight > halfWeight)
            {
                return FindWeightedMedian(arr, median - 1, weightMap, halfWeight);
            }
            else if (teamGWeight > halfWeight)
            {
                return FindWeightedMedian(arr, median + 1, weightMap, halfWeight);
            }

            return -1;
        }

        /// <summary>
        /// MomSelect algorithm inspired by textbook
        /// </summary>
        /// <param name="arr">Array to analyze</param>
        /// <param name="low">Low index of the subarray</param>
        /// <param name="high">High index of the subarray</param>
        /// <param name="k">median index</param>
        /// <returns></returns>
        static int MomSelect(int[] arr, int low, int high, int k)
        {
            int n = high - low + 1;

            if (n <= 25)
            {
                Array.Sort(arr, low, n);
                return arr[k];
            }

            int m = n / 5;
            int m_remainder = n % 5;

            int[] M;

            if (m_remainder == 0)
            {
                M = new int[m];

                for (int i = 0; i < m; i++)
                {
                    M[i] = MedianOfN(arr, low + i * 5, 5);
                }
            }
            else
            {
                M = new int[m + 1];

                for (int i = 0; i < m; i++)
                {
                    M[i] = MedianOfN(arr, low + i * 5, 5);
                }

                M[M.Length - 1] = MedianOfN(arr, low + m * 5, m_remainder);
            }
            int mom = M[0];

            if (m > 1)
            {
                mom = MomSelect(M, 0, m - 1, m / 2);
            }
            else
            {
                return mom;
            }

            int r = partition(arr, low, high, mom);

            if (k < r)
            {
                return MomSelect(arr, low, r - 1, k);
            }
            else if (k > r)
            {
                return MomSelect(arr, r + 1, high, k);
            }
            else
            {
                return mom;
            }
        }

        /// <summary>
        /// Partition method similar to what is found in quicksort
        /// </summary>
        /// <param name="arr">Array to partition</param>
        /// <param name="low">Low index of subarray</param>
        /// <param name="high">High index of subarray</param>
        /// <param name="x">Pivot index</param>
        /// <returns></returns>
        static int partition(int[] arr, int low, int high, int x)
        {
            swap(arr, linearFindIndex(arr, x), high);
            int loc = low - 1;

            for (int curr = low; curr <= high - 1; curr++)
            {
                if (arr[curr] < arr[high])
                {
                    loc++;
                    swap(arr, loc, curr);
                }
            }
            swap(arr, loc + 1, high);
            return (loc + 1);
        }

        /// <summary>
        /// Determines median for n values
        /// </summary>
        /// <param name="arr">Array of values</param>
        /// <param name="low">Start of median values</param>
        /// <param name="n">Number of values for median</param>
        /// <returns></returns>
        private static int MedianOfN(int[] arr, int low, int n)
        {
            Array.Sort(arr, low, n);
            return arr[low + (n / 2)];
        }

        /// <summary>
        /// Finds the target in a linear fashion
        /// </summary>
        /// <param name="arr">Array to search</param>
        /// <param name="target">Target value to find</param>
        /// <returns></returns>
        private static int linearFindIndex(int[] arr, int target)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == target)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Swaps array elements at two indexes
        /// </summary>
        /// <param name="arr">Array to swap elements</param>
        /// <param name="left">Left element</param>
        /// <param name="right">Right element</param>
        private static void swap(int[] arr, int left, int right)
        {
            int temp = arr[left];
            arr[left] = arr[right];
            arr[right] = temp;
        }

        /// <summary>
        /// Driver method to collect inputs and call the
        /// weighted mean algorithm
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int num_par;
            int.TryParse(Console.ReadLine(), out num_par);

            if (num_par < 1 || num_par > 1000000)
                return;

            int[] ids = new int[num_par];
            int[] weights = new int[num_par];
            
            string[] inputs_id = Console.ReadLine().Split(' ');
            try
            {
                for (int i = 0; i < num_par; i++)
                {
                    ids[i] = int.Parse(inputs_id[i]);

                   if (ids[i] < 1 || ids[i] > 1000000)
                        return;
                }
            }
            catch (Exception)
            {
                return;
            }

            string[] inputs_weight = Console.ReadLine().Split(' ');
            try
            {
                for (int i = 0; i < num_par; i++)
                {
                    weights[i] = int.Parse(inputs_weight[i]);

                    if (weights[i] < 1 || weights[i] > 1000000)
                        return;
                }
            }
            catch (Exception)
            {
                return;
            }

            int totalWeight = 0;
            for (int i = 0; i < num_par; i++)
            {
                totalWeight += weights[i];
            }

            double halfWeight = totalWeight / 2;

            Dictionary<int, int> weightMap = new Dictionary<int, int>();

            for (int i = 0; i < num_par; i++)
            {
                weightMap.Add(ids[i], weights[i]);
            }


            Console.WriteLine(FindWeightedMedian(ids, ids.Length / 2, weightMap, halfWeight));
        }
    }
}