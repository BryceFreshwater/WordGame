using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriorityQueue;

namespace WordGameStarter
{
    class WordGame
    {
        public const int BAD_LENGTH = -2;
        public const int NO_SUCH_WORD = -1;
        private const string DICTIONARY_LOCATION = "C:\\wordlist.txt";
        private int wordLength;
        private int numWords;
        static private List<string> wordList = null;
        List<string> filteredWordList = new List<string>();
        IDictionary<string, int> wordLengthDictionary = new Dictionary<string, int>();
        List<string> guessList = new List<string>();
        List<int> guessCountList = new List<int>();
        private int priority = 0;
        public string Target { get; private set; }
        public int GuessCount { get; private set; }
        Random r = new Random();

        public WordGame(int length)
        {
            wordLength = length;
            if (wordList == null)
                LoadList();
            if (wordLengthDictionary.Count == 0)
                CreateDictionary();
        }

        public WordGame(int length, int numWords) : this(length)
        {
            this.numWords = numWords;
            this.wordLength = length;
            if (wordList == null)
                LoadList();
            if (wordLengthDictionary.Count == 0)
                CreateDictionary();
        }

        /*
         * Get a random word from the list; make certain it fits the criteria,
         * e.g. correct length, no repeated letters (for a 'C' only).
         */
        public void GenerateNextWord()
        {
            //foreach (var word in wordLengthDictionary)
            foreach (KeyValuePair<string, int> word in wordLengthDictionary)
            {
                if (word.Value == wordLength)
                {
                    filteredWordList.Add(word.Key);
                }
            }
           
            int index = r.Next(filteredWordList.Count);
            Target = filteredWordList[index];
        }

        public void setPriority()
        {
            int vowelCount = 0;
            int rareLetterCount = 0;
            int sameLetterCounter = 0;
            Dictionary<char, int> targetLetterDictionary = new Dictionary<char, int>();
            if (Target[Target.Length - 1] != 's')
            {
                priority++;
            }
            if (Target[0] == 'a' || Target[0] == 'e' || Target[0] == 'i' || Target[0] == 'o' || Target[0] == 'u')
            {
                priority++;
            }
            for (int i = 0; i< Target.Length; i++)
            {
                if (Target[i] == 'a' || Target[i] == 'e' || Target[i] == 'i' || Target[i] == 'o' || Target[i] == 'u')
                {
                    vowelCount++;
                }
            }
            if ( vowelCount >= 2 ) { priority++; }
            for (int i = 0; i < Target.Length; i++)
            {
                if (Target[i] == 'q' || Target[i] == 'v' || Target[i] == 'w' || Target[i] == 'x' || Target[i] == 'y' || Target[i] == 'z')
                {
                    rareLetterCount++;
                }
            }
            if ( rareLetterCount >= 1 ) { priority++; }
            for(int i = 0; i < Target.Length;i++)
            {
                if (targetLetterDictionary.ContainsKey(Target[i]))
                {
                    targetLetterDictionary[Target[i]]++;
                }
                else { targetLetterDictionary.Add(Target[i], 1); }   
            }
            foreach (var item in targetLetterDictionary)
            {
                if (item.Value >= 3)
                {
                    priority = priority + 4; break;
                }
            }
            foreach (var item in targetLetterDictionary)
            {
                if (item.Value == 2)
                {
                    priority = priority + 2;
                }
            }
        }

        public int GetPriority() { return priority; }

        private void LoadList()
        {
            StreamReader input = new StreamReader(DICTIONARY_LOCATION);
            wordList = new List<string>();
            while (!input.EndOfStream)
            {
                wordList.Add(input.ReadLine());
            }
        }

        private void CreateDictionary()
        {
            foreach (string word in wordList)
            {
                if (wordLengthDictionary.ContainsKey(word))
                {
                   

                }
                else { wordLengthDictionary.Add(word, word.Length); }
            }
        }

        /*
         * Is this the exact word?
         */
        public bool IsMatch(string guess)
        {
            if ( guess == Target)
            {
                GuessCount++;
                return true;
            }
            else
            {
                GuessCount++;
                return false;
            }    
        }
        
        /*
         * This code retuns the correct count for non-duplicated letters,
         * but it does not check that the word is valid for this game.
         * this includes length and the existence of the word guessed.
         * Add that code.  Check out wordExists below.
         */
        public int GetCount(string guess)
        {
            int count = 0;
            // For the 'C' code, we don't want to count duplicates in our
            // target, so the GenerateNextWord should keep duplicates out.
            // This will not be a problem for 'B' code.
            for (int i=0; i < guess.Length; i++)
            {
                if (guess.Substring(i + 1).Contains(guess[i]))
                {
                    guess = guess.Remove(i, 1);
                    i--;
                }

            }
            for (int i=0; i < guess.Length; i++)
            {
                if (Target.Contains(guess[i]))
                        count++;
            }

            return count;
        }

        /*
         * The word list is a little broken in that some plurals are missing,
         * as are the third person single entries for some verbs.  For this reason
         * I have provided this method to check whether the word exists.
         * Essentially, if a word seems to be plural, it checks to see whether the
         * singular form exists.
         * 
         * This algorithm allows non-existent words (e.g. filess, or larges)
         * but it is good enough for this program, unless you feel inspired.  It is
         * possible that there are words that are not caught if the construction is
         * not done by merely adding an 's', e.g. relaxes.
         * 
         * You should not need to modify this code.
         */
        private bool wordExists(string word)
        {
            word = word.ToLower();
            bool found = false;
            if (wordExistsInList(word))
                found = true;
            else if (word[word.Length - 1] == 's' && wordExistsInList(word.Substring(0, wordLength - 1)))
                found = true;
            return found;
        }

        public bool wordExistsInList(string word)
        {
            return wordLengthDictionary.ContainsKey(word);
        }

        public void addGuessList(string guess, int count)
        {
            guessList.Add(guess);
            guessCountList.Add(count);

        }
        public void displayGuessList()
        {
            for(int i = 0; i<guessList.Count; i++)
            {
                Console.WriteLine(guessList[i] + " - " + guessCountList[i] + " correct letters");
            }
        }

        public void Retrieve(string word)
        {
            for(int i = 0; i<guessList.Count; i++)
            {
                if (guessList[i] == word)
                {
                    Console.WriteLine(word + " - " + guessCountList[i]);
                    return;
                }
            }
            Console.WriteLine("Word was not found");
        }
    }
}
