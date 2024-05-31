using PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordGameStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<WordGame> wordGames = new List<WordGame>();
            char playAgainInput = '0';
            bool playAgain = true;
            int wordLength = 0;
            int numWords;
            while (playAgain == true)
            {
                Console.WriteLine("How many games would you like to play?");
                numWords = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("How many letters?");
                wordLength = Convert.ToInt16(Console.ReadLine());
                for (int i = 0; i < numWords; i++)
                {
                    WordGame game = new WordGame(wordLength, numWords);
                    game.GenerateNextWord();
                    game.setPriority();
                    wordGames.Add(game);
                }
                WordGame temp;
                for (int j = 0; j < wordGames.Count - 2; j++)
                {
                    for (int i = 0; i <= wordGames.Count -2; i++)
                    {
                        if (wordGames[i].GetPriority() > wordGames[i+1].GetPriority())
                        {
                            temp = wordGames[i + 1];
                            wordGames[i + 1] = wordGames[i];
                            wordGames[i] = temp;
                        }
                    }
                }
                PriorityQueue<WordGame> queue = new PriorityQueue<WordGame>();
                foreach (WordGame game in wordGames)
                {
                    queue.Add(game.GetPriority(), game);
                }

                while (queue.Count > 0)
                {
                    WordGame currGame = queue.Remove();
                    Console.WriteLine("This words difficulty is " + currGame.GetPriority());
                    char menuInput = '0';

                    while (menuInput != 'A' && menuInput != 'a' && menuInput != 'Q')
                    {
                        Console.WriteLine("G)uess");
                        Console.WriteLine("H)istory");
                        Console.WriteLine("R)etrieve");
                        Console.WriteLine("A)bandon");
                        menuInput = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                        switch (menuInput)
                        {
                            //prompts user to guess the word and displays the number of letter matches
                            case 'G':
                            case 'g':
                                string userGuess;
                                Console.WriteLine("Guess the " + wordLength + " letter word");
                                userGuess = Console.ReadLine();
                                while (userGuess.Count<char>() != wordLength || currGame.wordExistsInList(userGuess) == false)
                                {
                                    Console.WriteLine("Enter a valid " + wordLength + " letter word");
                                    userGuess = Console.ReadLine();
                                }

                                if (currGame.IsMatch(userGuess) == true)
                                {
                                    Console.WriteLine("You guessed the word! It took " + currGame.GuessCount + " guess");
                                    menuInput = 'Q';
                                }
                                else
                                {
                                    Console.WriteLine("Nope! " + currGame.GetCount(userGuess) + " letters correct");
                                    currGame.addGuessList(userGuess, currGame.GetCount(userGuess));
                                }
                                break;

                            //displays all the guesses and associated word counts in order guessed
                            case 'H':
                            case 'h':
                                currGame.displayGuessList();
                                break;

                            //prompts the user to enter a new word; returns the number of matches if the word
                            //has been guessed previously. If not, it informs the user that the word has not been selected
                            //before.Do not recalculate the number of matches; look them up.
                            case 'R':
                            case 'r':
                                Console.WriteLine("Enter a word to retrieve");
                                currGame.Retrieve(Console.ReadLine());
                                break;                                
                        }

                        if (menuInput == 'a' || menuInput == 'A')
                        {
                            Console.WriteLine("The word was " + currGame.Target);
                            break;
                        }    
                    }
                }
                Console.WriteLine("Would you like to play again?");
                Console.WriteLine("Y)es");
                Console.WriteLine("N)o");
                playAgainInput = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (playAgainInput)
                {
                    case 'Y':
                    case 'y':
                        playAgain = true;
                        break;

                    case 'N':
                    case 'n':
                        playAgain = false;
                        break;
                }
            }
        }
    }              
}
