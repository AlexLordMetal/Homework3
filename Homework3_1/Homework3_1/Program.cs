using System;

namespace Homework3_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var ArrayLength = 0;                            //Read array length from console
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

            Random random = new Random();                   //Create random array
            int[] rndArray = new int[ArrayLength];
            for (int i = 0; i < ArrayLength; i++)
            {
                rndArray[i] = random.Next(1000);
            }

            Console.WriteLine("\nПолучившийся массив:");    //Write array to console
            foreach (var i in rndArray)
            {
                Console.Write($"{i} ");
            }

            var min = 0;                                    //Find min and max elements of array
            var max = 0;
            for (int i = 1; i < ArrayLength - 1; i++)
            {
                if (rndArray[i] < rndArray[min])
                {
                    min = i;
                }
                if (rndArray[i] > rndArray[max])
                {
                    max = i;
                }
            }

            Console.WriteLine($"\n\nНаименьший элемент массива - {rndArray[min]} (порядковый номер в массиве - {min})");
            Console.WriteLine($"Наибольший элемент массива - {rndArray[max]} (порядковый номер в массиве - {max})");
            Console.ReadKey();
        }
    }
}
