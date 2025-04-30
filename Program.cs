/*
 * C# Sorting template
 * 1) Read and understand the existing code and structure
 * 2) Comment the existing code
 * 3) Write and test the bubble() and merge() sort algorithms.
 *    You may have to write/modify this in an exam
 * 4) Write and test the insertion() sort algorithm.
 *    You may have to write/modify this in an exam
 * 5) Write and test the quick sort alogorithm - this is a quite a bit harder.
 *    You may have to describe this algo in
 * 6) Write and test the heap sort alogorithm - not part of the currciulum.
 *    This may be needed for your NEA.
 * Ensure that all code is commented.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    abstract class Sorter
    {
        protected String name { get; }

        public String GetName()
        {
            return name;
        }

        public Sorter(String name)
        {
            this.name = name;
        }

        public abstract int[] sort(int[] array);
    }

    class BubbleSort : Sorter
    {
        public BubbleSort()
            : base("Bubble") { }

        public override int[] sort(int[] array)
        {
            //
            // Implement the bubble sort algo here.
            //
            for (int i = 0; i < array.Length; i += 1)
            {
                for (int j = 0; j < array.Length - i - 1; j += 1)
                {
                    if (array[j] > array[j + 1])
                    {
                        // Swap values as the value after is less than the one before
                        // We want the values to be ascending
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                }
            }

            return array;
        }
    }

    class NextSmallestIter(int[] a, int[] b)
    {
        readonly int[] A = a;
        readonly int[] B = b;
        int AIndex = 0;
        int BIndex = 0;

        public int Next()
        {
            // Check if A is exhausted
            int aLength = A.Length;
            int nextB;
            if (AIndex >= aLength)
            {
                // A is exhausted
                nextB = B[BIndex];
                BIndex += 1;

                return nextB;
            }

            // Check if B is exhausted
            int bLength = B.Length;
            int nextA;
            if (BIndex >= bLength)
            {
                // B is exhausted
                nextA = A[AIndex];
                AIndex += 1;

                return nextA;
            }

            // Both A and B still have values
            nextA = A[AIndex];
            nextB = B[BIndex];

            // Check which one is smaller and return it
            if (nextA < nextB)
            {
                AIndex += 1;
                return nextA;
            }
            else
            {
                BIndex += 1;
                return nextB;
            }
        }
    }

    class MergeSort : Sorter
    {
        public MergeSort()
            : base("Merge") { }

        public override int[] sort(int[] array)
        {
            //
            // Implement the merge sort algo here.
            //
            // Check if we have reached a cell size of 1
            if (array.Length <= 1)
            {
                return array;
            }

            // Split this array down the middle
            int arrLength = array.Length;
            int middle = arrLength / 2;

            int[] left = new int[middle];
            Array.Copy(array, left, middle);

            int[] right = new int[arrLength - middle];
            Array.Copy(array, middle, right, 0, arrLength - middle);

            left = sort(left);
            right = sort(right);

            // Console.WriteLine("{0} - {1} - {2}", string.Join(',', array), string.Join(',', left), string.Join(',', right));

            NextSmallestIter iter = new(left, right);
            int[] result = new int[arrLength];

            for (int i = 0; i < arrLength; i++)
            {
                int item = iter.Next();

                result[i] = item;
            }

            return result;
        }
    }

    class InsertionSort : Sorter
    {
        public InsertionSort()
            : base("Insertion") { }

        public override int[] sort(int[] array)
        {
            //
            // Implement the merge sort algo here.
            //
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (array[j] < array[j - 1])
                    {
                        // Swap values
                        (array[j - 1], array[j]) = (array[j], array[j - 1]);
                    }
                }
            }

            return array;
        }
    }

    class QuickSort : Sorter
    {
        public QuickSort()
            : base("Quick") { }

        public override int[] sort(int[] array)
        {
            //
            // Implement the quick sort algo here.
            //
            return Inner(array, 0, array.Length - 1);
        }

        private static int[] Inner(int[] array, int low, int high)
        {
            if (low < high)
            {
                int partition = Partition(array, low, high);
                Inner(array, low, partition - 1);
                Inner(array, partition + 1, high);
            }

            return array;
        }

        private static int Partition(int[] array, int low, int high)
        {
            int pivot = array[high];

            int i = low - 1;

            // Move all elements that are smaller to the left side
            for (int j = low; j <= high - 1; j++)
            {
                if (array[j] < pivot)
                {
                    i += 1;

                    (array[i], array[j]) = (array[j], array[i]);
                }
            }

            // Move pivot are smaller elements
            (array[i + 1], array[high]) = (array[high], array[i + 1]);
            return i + 1;
        }
    }

    class PigeonHoleSort : Sorter
    {
        public PigeonHoleSort()
            : base("PigeonHole") { }

        public override int[] sort(int[] array)
        {
            int min = array.Min();
            int max = array.Max();
            int length = array.Length;

            int index = 0;
            int range = max - min + 1;
            int[] transfer_arr = new int[range];

            for (int i = 0; i < length; i += 1)
            {
                transfer_arr[array[i] - min] += 1;
            }

            for (int i = 0; i < range; i += 1)
            {
                while (transfer_arr[i] > 0)
                {
                    array[index] = i + min;

                    index += 1;
                    transfer_arr[i] -= 1;
                }
            }

            return array;
        }
    }

    public static int[] DeepCopy(int[] d)
    {
        int[] c = new int[d.Length];

        for (int i = 0; i < d.Length; i++)
        {
            c[i] = d[i];
        }
        return c;
    }

    public static void Main(string[] args)
    {
        Stopwatch sw = new();

        List<Sorter> sorters = [new BubbleSort(), new MergeSort(), new InsertionSort()];
        //
        // Three sets of data to test with your sorter algorithms courtesy of random.com
        //

        // csharpier-ignore-start
        int[][] data =
        [
            /* 10 items = short */
            [14, 1, 15, 17, 20, 13, 2, 8, 5, 3],
            /* 50 items = a bit longer */
            [
                83, 8, 133, 156, 199, 92, 194, 52,
                152, 197, 66, 154, 170, 138, 47,
                130, 163, 106, 172, 128, 113, 181,
                135, 15, 69, 182, 160, 140, 159,
                200, 112, 169, 91, 65, 55, 131, 33,
                63, 40, 150, 161, 9, 39, 62, 78,
                145, 20, 32, 178, 94
            ],
            /* 100 = should sort out the algos */
            [
                488, 243, 78, 486, 463, 418, 175, 306,
                59, 90, 331, 13, 298, 50, 257, 448,
                218, 464, 467, 356, 1, 120, 434, 98,
                371, 154, 493, 270, 164, 96, 302, 237,
                457, 299, 361, 38, 292, 60, 262, 128,
                312, 136, 122, 310, 153, 80, 167, 93,
                52, 296, 408, 11, 482, 39, 106, 475,
                174, 181, 289, 31, 73, 274, 411, 178,
                244, 316, 368, 201, 63, 221, 57, 236,
                14, 235, 461, 47, 79, 10, 112, 421,
                349, 211, 182, 319, 226, 375, 176,
                111, 314, 108, 209, 238, 103, 304,
                190, 255, 452, 422, 7, 500
            ]
        ];
        // csharpier-ignore-end

        //
        // Dual Core - Implement Quick sort
        sorters.Add(new QuickSort());

        //
        // Quad Core - Implement PigeonHole sort
        //
        sorters.Add(new PigeonHoleSort());

        // Iterate through all the sort routines on the three sets of data to compare the alogorithm speed

        foreach (Sorter s in sorters)
        {
            for (int x = 0; x < data.Length; x++)
            {
                // Deep copy the test data before attempting to sort it
                int[] copy = DeepCopy(data[x]);
                sw.Reset();
                sw.Start();
                copy = s.sort(copy);
                sw.Stop();
                Console.WriteLine(
                    "{1} sort, list length {2}, Elapsed={0}",
                    sw.Elapsed,
                    s.GetName(),
                    data[x].Length
                );

                // Check if the array is sorted
                for (int i = 0; i < copy.Length - 1; i += 1)
                {
                    if (copy[i] > copy[i + 1])
                    {
                        // This array is not sorted in ascending order
                        Console.WriteLine(
                            "--- Failed to order in ascending order: [{0}]",
                            string.Join(", ", copy)
                        );
                        break;
                    }
                }
            }
        }
    }
}
