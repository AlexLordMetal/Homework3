using System;

namespace Homework3_3
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfDigits = 4;                 //Bonus, game modifier :)

            bool isContinue = true;
            while (isContinue == true)
            {
                Console.Clear();
                int[] compArray = CreateArray(numberOfDigits);

                Console.WriteLine("БЫКИ И КОРОВЫ (ver.1.1)\n");

                //GameHint(compArray);              //For debugging

                Console.WriteLine($"Отгадайте {numberOfDigits}-значное число, которое я загадал.");

                bool isGuessed = false;             //Start the game
                while (isGuessed != true)
                {
                    Console.Write("Введите число: ");
                    string userString = Console.ReadLine();
                    bool isParseSuccess = Int32.TryParse(userString, out int userNum);
                    if ((isParseSuccess == true) && (userString.Length == numberOfDigits))
                    {
                        int[] userArray = ConvertToArray(userNum, userString.Length);           //Костыль со вторым параметром, т.к. TryParse обрезает нули в начале
                        bool isUnique = IsArrayUnique(userArray);                               //Тоже нужно, иначе коров неправильно считает
                        if (isUnique == true)
                        {
                            int[] cowArray = CompareArrays(compArray, userArray);
                            if (cowArray[0] == numberOfDigits)
                            {
                                isGuessed = true;
                                Console.WriteLine("Красава, отгадал! Еще партейку? (\"y\" - Yes, other - No)");
                                isContinue = (Console.ReadLine() == "y") ? true : false;
                            }
                            else
                            {
                                Console.WriteLine($"{cowArray[0]} {BullDeclension(cowArray[0])}, {cowArray[1]} {CowDeclension(cowArray[1])}\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ввод, попробуйте ввести число еще раз");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ввод, попробуйте ввести число еще раз");
                    }
                }
            }
        }

        /// <summary>
        /// Create an Array of integer numbers from 0 to 9.
        /// </summary>
        /// <param name="ArrayLength" >Must be more or equal than 0 and less or equal than 10</param>
        /// <returns></returns>
        static int[] CreateArray(int ArrayLength)
        {
            Random random = new Random();
            int[] compArray = new int[ArrayLength];
            for (int i = 0; i < ArrayLength; i++)
            {
                compArray[i] = random.Next(10);
                if (Array.IndexOf(compArray, compArray[i]) != i)
                {
                    i--;
                }
            }
            return compArray;
        }

        /// <summary>
        /// Convert an integer number to Array of integer numbers from 0 to 9.
        /// </summary>
        /// <param name="intNumber"></param>
        /// <param name="NumberLength"></param>
        /// <returns></returns>
        static int[] ConvertToArray(int intNumber, int NumberLength)
        {
            int[] userArray = new int[NumberLength];
            for (int i = 0; i < NumberLength; i++)
            {
                int tempNum = (int)Math.Pow(10, NumberLength - 1 - i);
                userArray[i] = intNumber / tempNum;
                intNumber = intNumber - userArray[i] * tempNum;
            }
            return userArray;
        }

        /// <summary>
        /// Check, if all numbers in Array are unique.
        /// </summary>
        /// <param name="userArray"></param>
        /// <returns></returns>
        static bool IsArrayUnique(int[] userArray)
        {
            bool isUnique = true;
            int i = userArray.Length - 1;
            do
            {
                if (Array.IndexOf(userArray, userArray[i]) != i)
                {
                    isUnique = false;
                }
                i--;
            } while ((isUnique == true) && (i >= 0));
            return isUnique;
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
        /// Return the right declension of word Bull according to the number of bulls.
        /// </summary>
        /// <param name="NumberOfBulls"></param>
        /// <returns></returns>
        static string BullDeclension(int NumberOfBulls)
        {
            string bulls = "";
            switch (NumberOfBulls)
            {
                case 1:
                    bulls = "бык";
                    break;
                case 2:
                    bulls = "быка";
                    break;
                case 3:
                    bulls = "быка";
                    break;
                case 4:
                    bulls = "быка";
                    break;
                default:
                    bulls = "быков";
                    break;
            }
            return bulls;
        }

        /// <summary>
        /// Return the right declension of word Cow according to the number of cows.
        /// </summary>
        /// <param name="NumberOfCows"></param>
        /// <returns></returns>
        static string CowDeclension(int NumberOfCows)
        {
            string cows = "";
            switch (NumberOfCows)
            {
                case 1:
                    cows = "корова";
                    break;
                case 2:
                    cows = "коровы";
                    break;
                case 3:
                    cows = "коровы";
                    break;
                case 4:
                    cows = "коровы";
                    break;
                default:
                    cows = "коров";
                    break;
            }
            return cows;
        }

        /// <summary>
        /// Write Array to console
        /// </summary>
        /// <param name="compArray"></param>
        static void GameHint(int[] compArray)
        {
            Console.Write("Подсказка: ");
            foreach (var i in compArray)
            {
                Console.Write($"{i}");
            }
            Console.WriteLine("\n");
        }

    }
}
