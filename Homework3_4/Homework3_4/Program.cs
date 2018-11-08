using System;
using System.Collections.Generic;
using System.IO;

namespace Homework3_4
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<char, char> psinaDictionary = new Dictionary<char, char>();
            StreamReader DictionaryFile = new StreamReader("Psina.dic");
            string DictionaryLine = "";
            while (DictionaryLine != null)
            {
                DictionaryLine = DictionaryFile.ReadLine();
                if (DictionaryLine != null)
                {
                    char[] tempCharArray = DictionaryLine.ToCharArray();
                    psinaDictionary.Add(tempCharArray[0], tempCharArray[2]);
                }
            }
            DictionaryFile.Close();

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
            for (int i = 0; i < files.Length; i++)
            {
                StreamReader psinaFile = new StreamReader(files[i]);
                StreamWriter normalFile = new StreamWriter(files[i].Remove((files[i].Length) - 5) + "-normal.txt");
                var stringConvert = psinaFile.ReadToEnd();
                foreach (KeyValuePair<char, char> keyValue in psinaDictionary)
                {
                    stringConvert = stringConvert.Replace(keyValue.Key, keyValue.Value);
                }
                normalFile.Write(stringConvert);
                psinaFile.Close();
                normalFile.Close();
            }
        }
    }
}
