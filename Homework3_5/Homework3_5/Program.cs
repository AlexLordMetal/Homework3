using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Homework3_5
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int[]> variantBank = GenerateList();           //List of variant numbers
            List<int[]> attemptBank = new List<int[]>();        //List of attempt numbers
            List<int[]> bullscowsBank = new List<int[]>();      //List of bulls, cows, herd (bulls + cows) for each attempt

            int attemptCount = 0;
            bool isGuessed = false;

            while (isGuessed != true)
            {
                Random rnd = new Random();

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

                Console.Title = "Быки и коровы v.0.13";
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{{Осталось {variantBank.Count} вариантов}} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Число {ElementToString(attemptBank[attemptCount])}");
                Console.Write("Быков: ");
                int bulls = ConsoleReadToInt();
                Console.Write("Коров: ");
                int cows = ConsoleReadToInt(bulls);
                Console.WriteLine();

                int herd = bulls + cows;
                bullscowsBank.Add(new int[] { bulls, cows, herd });

                if (bulls != 4)
                {
                    variantBank.Remove(attemptBank[attemptCount]);
                    variantBank = RemoveIncorrect(variantBank, attemptBank[attemptCount], bulls, herd);

                    if (variantBank.Count == 0)                 //If user made a mistake somewhere
                    {
                        Console.WriteLine("Вы где-то допустили ошибку! Числа с такими условиями не существует.");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }

                }
                else isGuessed = true;

                //PrintList(variantBank);           //For debugging
            }

            Console.WriteLine($"Ваше число {ElementToString(attemptBank[attemptCount])} угадано c {attemptCount + 1} попытки!");
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
        /// Reads the number of bulls or cows from the keyboard and checks if it correct.
        /// </summary>
        /// <param name="correctionNum">By default - 0 - If method reads the number of bulls. Number of bulls - if method reads the number of cows</param>
        /// <returns></returns>
        static int ConsoleReadToInt(int correctionNum=0)
        {
            var isCorrectRead = false;
            int BullsOrCows = -1;
            while (isCorrectRead != true)
            {
                isCorrectRead = Int32.TryParse(Console.ReadLine(), out BullsOrCows);
                if (isCorrectRead == true && (BullsOrCows < 0 || (BullsOrCows + correctionNum) > 4))
                {
                    isCorrectRead = false;
                }
                if (isCorrectRead == false)
                {
                    Console.Write("Еще раз, сколько? ");
                }
            }
            return BullsOrCows;
        }

        /// <summary>
        /// Chooses the random number with numerics different from the previous (first) attempt.
        /// </summary>
        /// <param name="tempList">All possible answer variants.</param>
        /// <param name="attemptElementPrevious">Previous answer attempt.</param>
        /// <returns></returns>
        static int[] SecondTry(List<int[]> tempList, int[] attemptElementPrevious)
        {
            Random rnd1 = new Random();
            bool isNotEqual = false;
            int[] attemptElement = tempList[0];
            while (isNotEqual != true)
            {
                isNotEqual = true;
                attemptElement = tempList[rnd1.Next(tempList.Count)];
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
        /// Convert the attempt number from array of int to string.
        /// </summary>
        /// <param name="elementArray"></param>
        /// <returns></returns>
        static string ElementToString(int[] elementArray)
        {
            string elementString = null;
            foreach (var i in elementArray)
            {
                elementString += i;
            }
            return elementString;
        }

        /// <summary>
        /// Write List of arrays of int to txt file and open it.
        /// </summary>
        /// <param name="tempList">All possible answer variants.</param>
        static void PrintList(List<int[]> tempList)
        {
            StreamWriter write = new StreamWriter("List.txt");
            foreach (int[] i in tempList)
            {
                write.WriteLine(ElementToString(i));
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

    }
}