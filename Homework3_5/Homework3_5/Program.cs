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
                    //all time
                    variantBank.Remove(attemptBank[attemptCount]);
                    variantBank = IfAnyBulls(variantBank, attemptBank[attemptCount], bulls);
                    variantBank = IfAnyHerd(variantBank, attemptBank[attemptCount], herd);
                    //Complex if
                    for (int i = 0; i < attemptCount; i++)
                    {
                        int bullsDifference = bulls - bullscowsBank[i][0];
                        int herdDifference = herd - bullscowsBank[i][2];
                        int herdSum = herd + bullscowsBank[i][2];
                        if (bullsDifference < 0 && cows == 0) variantBank = IfBullsDecreased(variantBank, attemptBank[attemptCount], attemptBank[i], bullsDifference);
                        if (herdDifference < 0) variantBank = IfHerdDecreased(variantBank, attemptBank[attemptCount], attemptBank[i], herdDifference);
                        if (herdDifference > 0) variantBank = IfHerdIncreased(variantBank, attemptBank[attemptCount], attemptBank[i], herdDifference);
                        if (herdSum == 4) variantBank = IfHerdFullInTwoSteps(variantBank, attemptBank[attemptCount], attemptBank[i]);
                        if (herdSum == 2) variantBank = IfHerdTwoInTwoSteps(variantBank, attemptBank[attemptCount], attemptBank[i]);
                    }
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

        static List<int[]> IfBullsDecreased(List<int[]> tempList, int[] tempElement, int[] tempElementPrevious, int bullsDifference)  //Вроде работает
        {
            int differenceCount = 0;
            List<int[]> foundNums = new List<int[]>();
            for (int j = 0; j < 4; j++)
            {
                if (Array.IndexOf(tempElement, tempElementPrevious[j]) == -1)
                {
                    differenceCount++;
                    foundNums.Add(new int[] { j, tempElementPrevious[j], tempElement[j] });
                }
            }

            if (differenceCount == Math.Abs(bullsDifference))
            {
                for (int j = 0; j < foundNums.Count; j++)
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (Array.IndexOf(tempList[i], foundNums[j][1]) != foundNums[j][0])
                        {
                            tempList.RemoveAt(i);
                            i--;
                        }
                        else if (Array.IndexOf(tempList[i], foundNums[j][2]) != -1)
                        {
                            tempList.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            return tempList;
        }

        static List<int[]> IfHerdDecreased(List<int[]> tempList, int[] tempElement, int[] tempElementPrevious, int herdDifference)  //Вроде работает
        {
            int differenceCount = 0;
            List<int> foundNums = new List<int>();
            for (int j = 0; j < 4; j++)
            {
                if (Array.IndexOf(tempElement, tempElementPrevious[j]) == -1)
                {
                    differenceCount++;
                    foundNums.Add(tempElementPrevious[j]);
                }
            }

            if (differenceCount == Math.Abs(herdDifference))
            {
                for (int j = 0; j < foundNums.Count; j++)
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (Array.IndexOf(tempList[i], foundNums[j]) == -1)
                        {
                            tempList.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            return tempList;
        }

        static List<int[]> IfHerdIncreased(List<int[]> tempList, int[] tempElement, int[] tempElementPrevious, int herdDifference)  //Вроде работает
        {
            int differenceCount = 0;
            List<int> foundNums = new List<int>();
            for (int j = 0; j < 4; j++)
            {
                if (Array.IndexOf(tempElementPrevious, tempElement[j]) == -1)
                {
                    differenceCount++;
                    foundNums.Add(tempElement[j]);
                }
            }

            if (differenceCount == herdDifference)
            {
                for (int j = 0; j < foundNums.Count; j++)
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (Array.IndexOf(tempList[i], foundNums[j]) == -1)
                        {
                            tempList.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            return tempList;
        }

        static List<int[]> IfHerdFullInTwoSteps(List<int[]> tempList, int[] tempElement, int[] tempElementPrevious)  //Вроде работает
        {
            List<int> foundNums = new List<int>();
            for (int j = 0; j < 4; j++)
            {
                foundNums.Add(tempElement[j]);
                if (Array.IndexOf(tempElement, tempElementPrevious[j]) == -1)
                {
                    foundNums.Add(tempElementPrevious[j]);
                }
            }

            if (foundNums.Count == 8)
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    int CountNums = 0;
                    for (int j = 0; j < foundNums.Count; j++)
                    {
                        if (Array.IndexOf(tempList[i], foundNums[j]) != -1)
                        {
                            CountNums++;
                        }
                    }
                    if (CountNums < 4)
                    {
                        tempList.RemoveAt(i);
                        i--;
                    }
                }

            }
            return tempList;
        }

        static List<int[]> IfHerdTwoInTwoSteps(List<int[]> tempList, int[] tempElement, int[] tempElementPrevious)  //Вроде работает
        {
            List<int> foundNums = new List<int>();
            for (int j = 0; j < 10; j++)
            {
                if (Array.IndexOf(tempElement, j) == -1 && Array.IndexOf(tempElementPrevious, j) == -1)
                {
                    foundNums.Add(j);
                }
            }

            if (foundNums.Count == 2)
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (Array.IndexOf(tempList[i], foundNums[0]) == -1 || Array.IndexOf(tempList[i], foundNums[1]) == -1)
                    {
                        tempList.RemoveAt(i);
                        i--;
                    }
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