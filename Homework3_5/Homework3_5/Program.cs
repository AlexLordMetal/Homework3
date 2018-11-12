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
            List<int[]> variantBank = GenerateList();
            List<int[]> attemptBank = new List<int[]>();
            List<int[]> bullscowsBank = new List<int[]>();

            int attemptCount = 0;
            bool isGuessed = false;
            while (isGuessed != true)
            {
                Random rnd = new Random();

                if (attemptBank.Count == 0)                                                 //Special condition for first attempt     
                {
                    attemptBank.Add(variantBank[rnd.Next(variantBank.Count)]);
                    attemptBank.Add(SecondTry(variantBank, attemptBank[0]));                //Create on first step because this variant may be exclude before second attempt
                    attemptCount = 0;
                }
                else if (attemptBank.Count == 2 && attemptCount == 0)                       //Special condition for second attempt
                    if (bullscowsBank[0][2] == 4)
                    {
                        attemptBank.RemoveAt(attemptBank.Count - 1);                        //Remove because on first attempt we finded all numbers
                        attemptBank.Add(variantBank[rnd.Next(variantBank.Count)]);
                        attemptCount++;
                    }
                    else attemptCount++;
                else                                                                        //Normal condition for third and further attempts
                {
                    attemptBank.Add(variantBank[rnd.Next(variantBank.Count)]);
                    //attemptBank.Add(variantBank[0]);
                    attemptCount++;
                }

                Console.Title = "Быки и коровы v.0.13";
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{{Осталось {variantBank.Count} вариантов}} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Число {ElementToString(attemptBank[attemptCount])}");
                Console.Write("Быков: ");
                int bulls = Int32.Parse(Console.ReadLine());
                Console.Write("Коров: ");
                int cows = Int32.Parse(Console.ReadLine());
                Console.WriteLine();
                int herd = bulls + cows;
                bullscowsBank.Add(new int[] { bulls, cows, herd });
                if (bulls != 4)
                {
                    variantBank.Remove(attemptBank[attemptCount]);
                    variantBank = IfAnyBulls(variantBank, attemptBank[attemptCount], bulls);
                    variantBank = IfAnyHerd(variantBank, attemptBank[attemptCount], herd);
                }
                else isGuessed = true;
                //PrintList(variantBank);
            }
            Console.WriteLine($"Ваше число {ElementToString(attemptBank[attemptCount])} угадано c {attemptCount + 1} попытки!");
            Console.ReadKey();
        }

        static List<int[]> GenerateList()
        {
            List<int[]> numList = new List<int[]>();
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    for (int k = 0; k < 10; k++)
                        for (int l = 0; l < 10; l++)
                        {
                            if (IsCorrect(i, j, k, l) == true)
                            {
                                numList.Add(new int[] { i, j, k, l });
                            }
                        }
            return numList;
        }

        static bool IsCorrect(int i, int j, int k, int l)
        {
            bool isCorrect = true;
            if (i == j || i == k || i == l || j == k || j == l || l == k)
            {
                isCorrect = false;
            }
            return isCorrect;
        }

        static string ElementToString(int[] elementArray)
        {
            string elementString = null;
            foreach (var i in elementArray)
            {
                elementString += i;
            }
            return elementString;
        }

        static int[] SecondTry(List<int[]> variantBank, int[] attemptElementPrevious)
        {
            Random rnd1 = new Random();
            bool isNotEqual = false;
            int[] attemptElement = variantBank[0];
            while (isNotEqual != true)
            {
                isNotEqual = true;
                attemptElement = variantBank[rnd1.Next(variantBank.Count)];
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

        static List<int[]> IfAnyBulls(List<int[]> tempList, int[] tempElement, int bulls)      //Работает
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                int bullsCount = 0;
                for (int j = 0; j < tempElement.Length; j++)
                {
                    if (Array.IndexOf(tempList[i], tempElement[j]) == j)
                    {
                        bullsCount++;
                    }
                }
                if (bulls != bullsCount)
                {
                    tempList.RemoveAt(i);
                    i--;
                }
            }
            return tempList;
        }

        static List<int[]> IfAnyHerd(List<int[]> tempList, int[] tempElement, int herd)         //Вроде работает
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                int herdCount = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (Array.IndexOf(tempList[i], tempElement[j]) != -1)
                    {
                        herdCount++;
                    }
                }
                if (herd != herdCount)
                {
                    tempList.RemoveAt(i);
                    i--;
                }
            }
            return tempList;
        }

        static void PrintList(List<int[]> tempList)             //Работает как надо!!!
        {
            StreamWriter write = new StreamWriter("List.txt");
            foreach (int[] i in tempList)
            {
                foreach (int j in i)
                {
                    write.Write(j);
                }
                write.WriteLine();
            }
            write.Close();
            Process.Start("List.txt");
        }


    }
}