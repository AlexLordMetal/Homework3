using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Homework3_5
{
    class Program
    {
        public static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.Write("Задайте количество игр: ");
            int NumberOfGames = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Ожидайте, идут игры...");
            var answerList = new List<int[]>();
            var iterationArray = new int[NumberOfGames];

            for (int i = 0; i < NumberOfGames; i++)
            {
                List<int[]> variantBank = GenerateList();           //List of variant numbers
                List<int[]> attemptBank = new List<int[]>();        //List of attempt numbers
                List<int[]> bullscowsBank = new List<int[]>();      //List of bulls, cows, herd (bulls + cows) for each attempt

                int numberOfDigits = 4;
                answerList.Add(CreateArray(numberOfDigits));
                //answerList.Add(variantBank[i]);
                int attemptCount = 0;
                bool isGuessed = false;

                while (isGuessed != true)
                {
                    if (attemptBank.Count == 0)                                                 //Special condition for the first attempt     
                    {
                        attemptBank.Add(variantBank[rnd.Next(variantBank.Count)]);
                        attemptBank.Add(SecondTry(variantBank, attemptBank[0]));                //Creates on the first attempt because this variant may be exclude before the second attempt
                        attemptCount = 0;
                    }

                    else if (attemptBank.Count == 2 && attemptCount == 0)                       //Special condition for the second attempt
                        if (bullscowsBank[0][2] == 4)
                        {
                            attemptBank.RemoveAt(attemptBank.Count - 1);                        //Removes incorrect second attempt because on the first attempt were found all numbers
                            attemptBank.Add(variantBank[rnd.Next(variantBank.Count)]);
                            attemptCount++;
                        }
                        else attemptCount++;

                    else                                                                        //Normal condition for third and further attempts
                    {
                        attemptBank.Add(variantBank[rnd.Next(variantBank.Count)]);
                        attemptCount++;
                    }

                    int[] cowArray = CompareArrays(answerList[i], attemptBank[attemptCount]);
                    if (cowArray[0] == numberOfDigits)
                    {
                        isGuessed = true;
                        iterationArray[i] = attemptCount + 1;
                    }

                    else
                    {
                        int bulls = cowArray[0];
                        int cows = cowArray[1];
                        int herd = bulls + cows;
                        bullscowsBank.Add(new int[] { bulls, cows, herd });

                        variantBank.Remove(attemptBank[attemptCount]);
                        variantBank = RemoveIncorrect(variantBank, attemptBank[attemptCount], bulls, herd);
                    }
                }
            }

            Console.WriteLine($"Всего сыграно {NumberOfGames} игр.");
            PrintToFile(answerList, iterationArray);
            IterationsCounter(iterationArray);
            Console.WriteLine("Подробнее в открывшемся файле.");
            Console.ReadKey();
        }


        /// <summary>
        /// Generates all possible attempts of four-digit numbers with non-repeating numerics.
        /// </summary>
        /// <returns></returns>
        static List<int[]> GenerateList()
        {
            List<int[]> tempList = new List<int[]>();
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    for (int k = 0; k < 10; k++)
                        for (int l = 0; l < 10; l++)
                        {
                            if (IsCorrect(i, j, k, l) == true)
                            {
                                tempList.Add(new int[] { i, j, k, l });
                            }
                        }
            return tempList;
        }

        /// <summary>
        /// Cheks if the numerics are not repeat.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        static bool IsCorrect(int i, int j, int k, int l)
        {
            bool isCorrect = true;
            if (i == j || i == k || i == l || j == k || j == l || l == k)
            {
                isCorrect = false;
            }
            return isCorrect;
        }

        /// <summary>
        /// Chooses the random number with numerics different from the previous (first) attempt.
        /// </summary>
        /// <param name="tempList">All possible answer variants.</param>
        /// <param name="attemptElementPrevious">Previous answer attempt.</param>
        /// <returns></returns>
        static int[] SecondTry(List<int[]> tempList, int[] attemptElementPrevious)
        {
            bool isNotEqual = false;
            int[] attemptElement = tempList[0];
            while (isNotEqual != true)
            {
                isNotEqual = true;
                attemptElement = tempList[rnd.Next(tempList.Count)];
                for (int i = 0; i < 4; i++)
                {
                    if (Array.IndexOf(attemptElementPrevious, attemptElement[i]) != -1)
                    {
                        isNotEqual = false;
                    }
                }
            }
            return attemptElement;
        }

        /// <summary>
        /// Write list of finded numbers and number of iterations to txt file.
        /// </summary>
        /// <param name="tempList"></param>
        static void PrintToFile(List<int[]> tempList, int[] tempNum)
        {
            StreamWriter write = new StreamWriter("List.txt");
            for (int i = 0; i < tempList.Count; i++)
            {
                write.WriteLine($"Число {tempList[i][0]}{tempList[i][1]}{tempList[i][2]}{tempList[i][3]} угадано с {tempNum[i]} попытки");
            }
            write.Close();
            Process.Start("List.txt");
        }

        /// <summary>
        /// Removes the answer variants, which do not fit the current attempt.
        /// </summary>
        /// <param name="tempList">All possible answer variants.</param>
        /// <param name="attemptElement">Current answer attempt.</param>
        /// <param name="bulls">Number of bulls in current answer attempt.</param>
        /// <param name="herd">Herd number (bulls and cows together) in current answer attempt.</param>
        /// <returns></returns>
        static List<int[]> RemoveIncorrect(List<int[]> tempList, int[] attemptElement, int bulls, int herd)
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                int bullsCount = 0;
                int herdCount = 0;
                for (int j = 0; j < attemptElement.Length; j++)
                {
                    if (Array.IndexOf(tempList[i], attemptElement[j]) == j)
                    {
                        bullsCount++;
                    }
                    if (Array.IndexOf(tempList[i], attemptElement[j]) != -1)
                    {
                        herdCount++;
                    }
                }

                if (bulls != bullsCount)        //If the number of bulls in current element is not equal to the number of bulls in attempt element, removes this element
                {
                    tempList.RemoveAt(i);
                    i--;
                }
                else if (herd != herdCount)     //If the herd number (of bulls and cows together) in current element is not equal to the herd number in attempt element, removes this element
                {
                    tempList.RemoveAt(i);
                    i--;
                }
            }
            return tempList;
        }

        /// <summary>
        /// Create an Array of integer numbers from 0 to 9.
        /// </summary>
        /// <param name="ArrayLength" >Must be more or equal than 0 and less or equal than 10</param>
        /// <returns></returns>
        static int[] CreateArray(int ArrayLength)
        {
            int[] compArray = new int[ArrayLength];
            for (int i = 0; i < ArrayLength; i++)
            {
                compArray[i] = rnd.Next(10);
                if (Array.IndexOf(compArray, compArray[i]) != i)
                {
                    i--;
                }
            }
            return compArray;
        }

        /// <summary>
        /// Compare 2 arrays, are there bulls and cows.
        /// </summary>
        /// <param name="compArray">array with which is compare</param>
        /// <param name="userArray">Array that is compare</param>
        /// <returns></returns>
        static int[] CompareArrays(int[] compArray, int[] userArray)
        {
            int[] cowArray = { 0, 0 };
            for (int i = 0; i < userArray.Length; i++)
            {
                int isInArray = Array.IndexOf(compArray, userArray[i]);
                if (isInArray == i)
                {
                    cowArray[0]++;
                }
                else if (isInArray != -1)
                {
                    cowArray[1]++;
                }
            }
            return cowArray;
        }

        /// <summary>
        /// Counts how often definite number of iterations happen.
        /// </summary>
        /// <param name="tempArray"></param>
        static void IterationsCounter(int[] tempArray)
        {
            var NumberOfIterations = new int[10];
            for (int i = 0; i < NumberOfIterations.Length; i++)
            {
                NumberOfIterations[i] = 0;
            }

            foreach (int i in tempArray)
            {
                NumberOfIterations[i]++;
            }

            for (int i = 1; i < NumberOfIterations.Length; i++)
            {
                Console.WriteLine($"С {i} попытки угадано {NumberOfIterations[i]} раз.");
            }
        }


    }
}