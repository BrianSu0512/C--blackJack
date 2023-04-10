using System;
using System.Collections.Generic;
using System.Linq;

namespace AsyncBlackJack
{
    class BlackJack
    {
        public string? cards { get; set; } = "";

        static IEnumerable<string> Suits()
        {
            yield return "clubs";
            yield return "diamons";
            yield return "hearts";
            yield return "spades";
        }
        static IEnumerable<string> Ranks()
        {
            yield return "two";
            yield return "three";
            yield return "four";
            yield return "five";
            yield return "six";
            yield return "seven";
            yield return "eight";
            yield return "nine";
            yield return "ten";
            yield return "eight";
            yield return "nine";
            yield return "ten";
            yield return "jack";
            yield return "queen";
            yield return "king";
            yield return "ace";
        }

        static Dictionary<string, int> numbermap = new Dictionary<string, int>()
        {
            {"two",2},
            {"three",3},
            {"four",4},
            {"five",5},
            {"six",6},
            {"seven",7},
            {"eight",8},
            {"nine",9},
            {"ten",10},
            {"jack",10},
            {"queen",10},
            {"king",10},
            {"ace",1},
        };

        public static List<BlackJack> cardList { get; set; } = new List<BlackJack>();
        public static List<BlackJack> userCardList { get; set; } = new List<BlackJack>();
        public static List<BlackJack> sysCardList { get; set; } = new List<BlackJack>();

        public static Random random { get; set; } = new Random();
        public static int userCardValue { get; set; } = 0;
        public static int sysCardValue { get; set; } = 0;
        public static int randomNumber { get; set; } = 0;
        public static string[] separators = { "Suit =", ", Rank =", "}" };
        public static int countAce { get; set; } = 0;
        public static int sysCountAce { get; set; } = 0;




        private static void Main(string[] args)
        {
            var allcards = from s in Suits()
                           from r in Ranks()
                           select new { Suit = s, Rank = r };

            foreach (var card in allcards)
            {
                cardList.Add(new BlackJack
                {
                    cards = card.ToString()
                });

            }
            // cardList.ForEach(p => Console.WriteLine(p.cards));

            startingGame();
        }

        static void startingGame()
        {
            for (var i = 1; i < 3; i++)
            {
                randomNumber = random.Next(0, cardList.Count - 1);
                userCardList.Add(new BlackJack
                {
                    cards = cardList.ElementAt(randomNumber).cards
                });
                cardList.RemoveAt(randomNumber);
            }

            userCardList.ForEach(p => Console.WriteLine($"Your Cards:" + p.cards));


            string[] cardsYouGot = new string[userCardList.Count];
            for (var i = 0; i < 2; i++)
            {
                cardsYouGot[i] = userCardList[i].cards.Split(separators, StringSplitOptions.None)[2].ToString().Trim();
            }

            Console.WriteLine("_________");

            if (cardsYouGot[0].ToString().Trim() == "ace" && cardsYouGot[1].ToString().Trim() == "ace")
            {
                userCardValue += 12;
                countAce += 2;
            }
            else if (cardsYouGot[0].ToString().Trim() == "ace")
            {
                userCardValue = 11 + numbermap[cardsYouGot[1].ToString().Trim()];
                countAce++;
            }
            else if (cardsYouGot[1].ToString().Trim() == "ace")
            {
                userCardValue = numbermap[cardsYouGot[0].ToString().Trim()] + 11;
                countAce++;
            }
            else
            {
                userCardValue = numbermap[cardsYouGot[0].ToString().Trim()] + numbermap[cardsYouGot[1].ToString().Trim()];
            }
            Console.WriteLine("You got:" + userCardValue);
            Console.WriteLine("_________");

            randomNumber = random.Next(0, cardList.Count - 1);

            sysCardList.Add(new BlackJack
            {
                cards = cardList.ElementAt(randomNumber).cards
            });
            cardList.RemoveAt(randomNumber);

            sysCardList.ForEach(card => Console.WriteLine("System Card" + card.cards));
            string[] sysFirstCard = sysCardList[0].cards.Split(separators, StringSplitOptions.None);
            if (sysFirstCard[2].ToString().Trim() == "ace")
            {
                sysCardValue += 11;
                sysCountAce++;
            }
            else
            {
                sysCardValue = numbermap[sysFirstCard[2].ToString().Trim()];
            }
            Console.WriteLine("system got: " + sysCardValue);

            Console.WriteLine("Cards remain: " + cardList.Count);
            Console.WriteLine("_________");

            if (userCardValue < 21)
            {
                Console.WriteLine("Add card or not please enter 1 for add 2 for skip");
                string? input = Console.ReadLine();
                addCards(input);
            }
            else
            {
                Console.WriteLine("Black Jack!! well done");
            }

        }

        private static void addCards(string userInput)
        {
            switch (userInput)
            {
                case "1":
                    randomNumber = random.Next(0, cardList.Count - 1);
                    userCardList.Add(new BlackJack
                    {
                        cards = cardList.ElementAt(randomNumber).cards
                    });

                    cardList.RemoveAt(randomNumber);
                    string[] addCard = userCardList.ElementAt(userCardList.Count - 1).cards.Split(separators, StringSplitOptions.None);

                    if (countAce == 0)
                    {
                        if (addCard[2].ToString().Trim() == "ace")
                        {
                            if (userCardValue < 11)
                            {
                                userCardValue += 11;
                            }
                            else
                            {
                                userCardValue++;
                            }
                        }
                        else
                        {
                            userCardValue += numbermap[addCard[2].ToString().Trim()];
                        }
                    }
                    else if (countAce > 1 && (userCardValue + numbermap[addCard[2].ToString().Trim()]) <= 21)
                    {
                        userCardValue += numbermap[addCard[2].ToString().Trim()];
                    }
                    else
                    {
                        userCardValue = (userCardValue - 10) + numbermap[addCard[2].ToString().Trim()];
                    }

                    userCardList.ForEach(card => Console.WriteLine("you card:" + card.cards));
                    Console.WriteLine("You got: " + userCardValue);

                    if (userCardValue > 21)
                    {
                        Console.WriteLine("go over 21 you loosse");
                        Console.WriteLine("Cards remain:" + cardList.Count);
                    }
                    else
                    {
                        Console.WriteLine("Cards remain:" + cardList.Count);
                        Console.WriteLine("Add card or not !! please enter 1 for add. 2 for skip");
                        string userinput = Console.ReadLine();
                        addCards(userinput);
                    }
                    break;

                case "2":
                    while (sysCardValue < 17)
                    {
                        randomNumber = random.Next(0, cardList.Count - 1);
                        sysCardList.Add(new BlackJack
                        {
                            cards = cardList.ElementAt(randomNumber).cards
                        });
                        cardList.RemoveAt(randomNumber);

                        string[] addsysCard = sysCardList[(sysCardList.Count - 1)].cards.Split(separators, StringSplitOptions.None);

                        if (addsysCard[2].ToString().Trim() == "ace" && sysCardValue <= 21)
                        {
                            if (sysCardValue < 11)
                            {
                                sysCardValue += 11;
                            }
                            else
                            {
                                sysCardValue++;
                            }
                        }
                        else
                        {
                            sysCardValue += numbermap[addsysCard[2].ToString().Trim()];
                        }

                        sysCardList.ForEach(card => Console.WriteLine("system Card" + card.cards));
                        Console.WriteLine("System got: " + sysCardValue);
                    }

                    if (sysCardValue > 21)
                    {
                        Console.WriteLine("You win this game");
                    }
                    else
                    {
                        compareCards();
                    }
                    break;
            }
        }
        private static void compareCards()
        {
            if (userCardValue == sysCardValue)
            {
                Console.WriteLine("____________");
                Console.WriteLine("Tie this game!!");
                Console.WriteLine("____________");
            }
            else if (userCardValue > sysCardValue)
            {
                Console.WriteLine("____________");
                Console.WriteLine("Winer Winer Chicken dinner!!");
                Console.WriteLine("____________");
            }
            else
            {
                Console.WriteLine("haha!!! Bad Luck try again");
            }


        }

        static void playAgain(string continuePlay)
        {
            switch (continuePlay)
            {
                case "1":
                    startingGame();
                    break;

                case "2":
                    break;
            }
        }
    }
}
