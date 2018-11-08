using System;
using System.Diagnostics;

namespace Homework3_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var ArrayLength = 0;                                //Read array length from console
            var isCorrect = false;
            while (isCorrect != true || ArrayLength == 0)
            {
                Console.Write("Задайте длину массива: ");
                isCorrect = Int32.TryParse(Console.ReadLine(), out ArrayLength);
                if (isCorrect == false || ArrayLength == 0)
                {
                    Console.WriteLine("Некоректный ввод, введите еще раз.");
                }
            }

            Random random = new Random();                       //Create random array
            int[] rndArray = new int[ArrayLength];
            for (int i = 0; i < ArrayLength; i++)
            {
                rndArray[i] = random.Next(1000);
            }

            Console.WriteLine("\nПолучившийся массив:");        //Write array to console
            foreach (var i in rndArray)
            {
                Console.Write($"{i} ");
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            CombSort(rndArray);                                //Sort array
            sw.Stop();
            Console.WriteLine($"\n\nМассив отсортирован алгоритмом \"Расческа\". Сортировка заняла {sw.Elapsed}.");

            Console.WriteLine("\nОтсортированный массив:");    //Write array to console
            foreach (var i in rndArray)
            {
                Console.Write($"{i} ");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Method sorts array of Int32 with Comb sort algorithm
        /// </summary>
        /// <param name="sortedArray"></param>
        /// <returns></returns>
        static int[] CombSort(int[] sortedArray)
        {
            double reductionFactor = 1.247;
            int sortStep = (int)(sortedArray.Length / reductionFactor);
            while (sortStep >= 1)
            {
                for (int i = sortStep; i < sortedArray.Length; i++)
                {
                    if (sortedArray[i] < sortedArray[i - sortStep])
                    {
                        int tempNum = sortedArray[i];
                        sortedArray[i] = sortedArray[i - sortStep];
                        sortedArray[i - sortStep] = tempNum;
                    }
                }
                sortStep = (int)(sortStep / reductionFactor);
            }
            return sortedArray;
        }

    }
}
