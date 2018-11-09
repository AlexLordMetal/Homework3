using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3_5
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int[]> variantBank = GenerateList();
            List<int[]> attemptBank = new List<int[]>();
            List<int[]> amountBank = new List<int[]>();
            //List<int[]> varBank = new List<int[]>();
            //varBank.Add(new int[] { 0, 1, 2, 3 });
            //varBank.Add(new int[] { 0, 1, 2, 4 });
            //varBank.Add(new int[] { 5, 7, 2, 8 });
            //varBank.Add(new int[] { 5, 6, 7, 8 });

            bool isGuessed = false;
            while (isGuessed != true)
            {
                attemptBank.Add(variantBank[0]);
                int attemptCount = attemptBank.Count - 1;
                Console.WriteLine($"Осталось {variantBank.Count}, число {ElementToString(attemptBank[attemptCount])}");
                Console.Write("Быков: ");
                int bulls = Int32.Parse(Console.ReadLine());
                Console.Write("Коров: ");
                int cows = Int32.Parse(Console.ReadLine());
                amountBank.Add(new int[] { bulls, cows });
                if (bulls != 4)
                {
                    int herd = bulls + cows;
                    if (bulls != 4) variantBank.RemoveAt(0);
                    if (bulls == 0) variantBank = IfNoBulls(variantBank, attemptBank[attemptCount]);
                    if (bulls != 0) variantBank = IfAnyBulls(variantBank, attemptBank[attemptCount]);
                    if (herd == 0) variantBank = IfNoBullsNoCows(variantBank, attemptBank[attemptCount]);
                    if (herd == 4) variantBank = IfFullHerd(variantBank, attemptBank[attemptCount]);
                    if (herd < (attemptBank[attemptCount][0] + attemptBank[attemptCount][1])) variantBank = IfHerdBecameLess(variantBank, attemptBank[attemptCount], attemptBank[attemptCount - 1]);
                }
                else isGuessed = true;
                PrintList(variantBank);
            }








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

        static List<int[]> IfNoBulls(List<int[]> tempList, int[] tempElement)           //Вроде работает
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Array.IndexOf(tempList[i], tempElement[j]) == j)
                    {
                        tempList.RemoveAt(i);
                        i--;
                        j = 4;
                    }
                }
            }
            return tempList;
        }

        static List<int[]> IfNoBullsNoCows(List<int[]> tempList, int[] tempElement)         //Работает как надо
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Array.IndexOf(tempList[i], tempElement[j]) != -1)
                    {
                        tempList.RemoveAt(i);
                        i--;
                        j = 4;
                    }
                }
            }
            return tempList;
        }

        static List<int[]> IfAnyBulls(List<int[]> tempList, int[] tempElement)      //Работает как надо!!!
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                if ((Array.IndexOf(tempList[i], tempElement[0]) != 0) && (Array.IndexOf(tempList[i], tempElement[1]) != 1) && (Array.IndexOf(tempList[i], tempElement[2]) != 2) && (Array.IndexOf(tempList[i], tempElement[3]) != 3))
                {
                    tempList.RemoveAt(i);
                    i--;
                }
            }
            return tempList;
        }

        static List<int[]> IfFullHerd(List<int[]> tempList, int[] tempElement)          //Вроде работает     
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                if ((Array.IndexOf(tempList[i], tempElement[0]) == -1) || (Array.IndexOf(tempList[i], tempElement[1]) == -1) || (Array.IndexOf(tempList[i], tempElement[2]) == -1) || (Array.IndexOf(tempList[i], tempElement[3]) == -1))
                {
                    tempList.RemoveAt(i);
                    i--;
                }
            }
            return tempList;
        }

        static List<int[]> IfHerdBecameLess(List<int[]> tempList, int[] tempElement, int[] tempElementPrevious)   
        {
            for (int j = 0; j < 4; j++)
            {
                if (Array.IndexOf(tempElement, tempElementPrevious[j]) != -1)
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (Array.IndexOf(tempList[i], tempElementPrevious[j]) == -1) //Херня пока что
                        {
                            tempList.RemoveAt(i);
                            i--;
                        }
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