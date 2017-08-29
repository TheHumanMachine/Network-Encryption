using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption
{
    /// <summary>
    /// provides manipulation to file Extentions
    /// </summary>
    public class FileExtentionFuncts
    {
        /// <summary>
        /// function compares the file extentions of two files
        /// <para/>returns true if they hold the same extention 
        /// </summary>
        /// <param name="fileOne"> file one to check </param>
        /// <param name="fileTwo">file two to check</param>
        /// <returns> returns true if the two files contain the same extention </returns>
        public static bool checkExtention(string fileOne, string fileTwo)
        {
            bool areSame = false;
            if (fileOne.Count() >= 4 && fileTwo.Count() >= 4) // 4 is the minimal len for a valid extention
            {
                try
                {
                    if (getExtention(fileOne) == getExtention(fileTwo))
                    {
                        areSame = true;
                    }
                }
                catch (Exception exception)
                {
                    if (exception is FormatException)
                    {
                        return false;
                    }
                    throw; // rethrow any other exception
                }
            }
            return areSame;
        }
        /// <summary>
        /// function finds the extention type of a given file name
        /// <para/>returns a string with the extention type of the file
        /// </summary>
        /// <param name="file"> name of the file</param>
        /// <returns> returns a string associated with the file extention type</returns>
        public static string getExtention(string file)
        {
            int index = file.Count() - 1;
            string extention = "";
            char token = ' ';
            while (token != '.' && index > 0)
            {
                token = file[index];
                extention = extention.Insert(0, token.ToString());
                index--;
            }
            if (!extention.Contains('.'))
            {
                throw new FormatException("not a valid extention");
            }
            return extention;
        }
        /// <summary>
        /// function checks given string has a valid file extention
        /// <para/>returns true if string has an extention
        /// </summary>
        /// <param name="FileName"> string to check</param>
        /// <returns>returns a bool whether or not file has an extention</returns>
        public static bool checkHasExtention(string FileName)
        {
            bool hasExtention = true;
            try
            {
                getExtention(FileName);
            }
            catch (Exception)
            {
                hasExtention = false;
            }
            return hasExtention;
        }
        /// <summary>
        /// function remove \ & . from the begining of the file
        /// <para>returns the file name without . or \ in the begining of the name</para>
        /// </summary>
        /// <param name="File">file to prepend</param>
        /// <returns>a file name</returns>
        public static string prependFile(string File)
        {
            int index = 0;
            char atIndex = new char();
            do
            {
                atIndex = File[index];
                index++;
            } while (index < File.Length && atIndex == '.' || atIndex == '\\');
            return File.Substring(index - 1);
        }
    }
}
