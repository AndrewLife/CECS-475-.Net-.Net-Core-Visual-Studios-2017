using Cecs475.War;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace CECS475.Poker.Cards
{
    public class PokerHandClassifier
    {
        /**
         * Helper Method / Overload Constructor
         * Takes in an IEnumerable list of Cards
         * This Method is used to classify if the list of cards has a pair or not
         * Returns PokerHand
         * */
        public static PokerHand ClassifyHand(IEnumerable<Card> hand)
        {
            /**
             * Condition is used to determine if the hand is a: One Pair, Two Pair, Three of a Kind, Four of a Kind, Full House
             * */
            if(HasPair(hand) == true)
            {
                return Pair(hand);
            }
            /**
             * Condition is used to determine if the hand is a: High Card, Flush, Straight, Straight Flush, Royal Flush
             * */
            else
            {
                return NoPair(hand);
            }
        }

        /**
         * Constructor
         * Takes in an array of Cards
         * Converts the array into an IEnumerable list of cards
         * Returns PokerHand
         * */
        public static PokerHand ClassifyHand(Card[] card)
        {
            IEnumerable<Card> player_card = (IEnumerable<Card>)card;
            return ClassifyHand(player_card);
        }
        /**
         * Method is used to determine if the list of cards contains a pair
         * I used a dictionary to look for a two or more similar Card.Kind
         * Returns true if there is a pair within the list
         * Returns false if there are no pairs within the list
         * */
        public static bool HasPair(IEnumerable<Card> hand)
        {
            //Used to check if there are similar Card.Kind, key = Card.Kind, value = amount of that Kind
            Dictionary<Card.CardKind, int> hand_card = new Dictionary<Card.CardKind, int>();

            //Convert IEnumerable to List
            List<Card> player_hand = hand.ToList<Card>();

            //Iterate through the list to be placed in the Dictionary
            for(int i = 0; i < player_hand.Count; i++)
            {
                //If Card.CardKind does not exist in the Dictionary yet
                if (!hand_card.ContainsKey(player_hand[i].Kind)){
                    hand_card.Add(player_hand[i].Kind, 1);
                }

                //Increment the amound of that Card.CardKind if it does exist in the Dictionary
                else
                {
                    int temp = hand_card[player_hand[i].Kind];
                    hand_card.Remove(player_hand[i].Kind);
                    hand_card.Add(player_hand[i].Kind, temp + 1);
                }
            }

            //Returns true if Dictionary contains similar Card.CardKind
            if(hand_card.ContainsValue(2) || hand_card.ContainsValue(3) || hand_card.ContainsValue(4))
            {
                return true;
            }
            //Returns false if Dictionary contains unique Card.CardKind
            else
            {
                return false;
            }
        }
        /**
         * Method is used to determine if the list is a: High Card, Flush, Straight, Straight Flush, Royal Flush
         * Calls another method to see if the list has characteristics of a Straight
         * Calls another method to see if the list has characteristics of a Flush
         * Based on the characteristics that are true, determines the type of hand
         * Returns PokerHand
         * */
        public static PokerHand NoPair(IEnumerable<Card> hand)
        {
            //Determine if the list of the Cards have a straight characteristic
            bool Straight_Check = IsStraight(hand);

            //Determine if the list of the Cards have a flush charactertistic
            bool Flush_Check = IsFlush(hand);

            //Convert IEnumerable to List
            List<Card> player_hand = hand.ToList<Card>();

            //Sorts the Cards in increasing order
            player_hand = SortHand(hand);

            //If the list of cards have both straight and flush characteristics
            if (Straight_Check && Flush_Check)
            {
                //If the first card is a Ten, then the PokerHand is a Royal Flush
                if(player_hand[0].Kind == Card.CardKind.Ten)
                {
                    PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.RoyalFlush);
                    return classified_hand;
                }

                //If the first card is not a Ten, then the PokerHand is a Straight Flush
                else
                {
                    PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.StraightFlush);
                    return classified_hand;
                }
            }

            //If the list of cards has only the straight characteristic
            else if(Straight_Check && !Flush_Check)
            {
                //PokerHand is a Straight
                PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.Straight);
                return classified_hand;
            }

            //If the list of cards has only the flush characteristic
            else if(!Straight_Check && Flush_Check)
            {
                //PokerHand is a Flush
                PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.Flush);
                return classified_hand;
            }

            //If the list does not have either the straight and flush characteristic then it is a HighCard
            else
            {
                //PokerHand is a HighCard
                PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.HighCard);
                return classified_hand;
            }
        }
        /**
         * Method is used to determine if the list is a: One Pair, Two Pair, Three of a Kind, Four of a Kind, Full House
         * Uses a Dictionary to determine what kind of pair the list has, key = Card.CardKind, value = amount of CardKind
         * If the Dictionary contains a key = 4, then the list is a Four of a Kind
         * If the Dictionary contains a key = 3 and key = 2, then the list is a Full House
         * If the Dictionary contains a key = 3 only, then the list is a Three of a Kind
         * If the Dictionary has length of 4, then the list is a One Pair
         * If none of the above, then the list is a Two Pair
         * Returns PokerHand
         * */
        public static PokerHand Pair(IEnumerable<Card> hand)
        {
            //Convert IEnumerable to List
            List<Card> player_hand = hand.ToList<Card>();

            //Sorts the list in increasing order
            player_hand = SortHand(hand);

            //Dictionary used to determine what kind of pair the list of card has
            Dictionary<Card.CardKind, int> pair_hand = new Dictionary<Card.CardKind, int>();

            //Iterates through the list to be placed in the dictionary
            for (int i = 0; i < player_hand.Count; i++)
            {
                //If Dictionary does not currently contain the CardKind 
                if (!pair_hand.ContainsKey(player_hand[i].Kind))
                {
                    pair_hand.Add(player_hand[i].Kind, 1);
                }

                //Increment the value if the dictionary does contain the CardKind
                else
                {
                    int temp = pair_hand[player_hand[i].Kind];
                    pair_hand.Remove(player_hand[i].Kind);
                    pair_hand.Add(player_hand[i].Kind, temp + 1);
                }
            }

            //If the Dictionary contains a key = 4, the PokerHand is a Four of a Kind
            if (pair_hand.ContainsValue(4))
            {
                PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.FourOfKind);
                return classified_hand;
            }

            //If the Dictionary contains a key = 3 and key = 2, the PokerHand is a Full House
            else if(pair_hand.ContainsValue(3) && pair_hand.ContainsValue(2))
            {
                PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.FullHouse);
                return classified_hand;
            }

            //If the Dictionary only contains a key = 3, the PokerHand is a Three of a Kind
            else if(pair_hand.ContainsValue(3) && !pair_hand.ContainsValue(2))
            {
                PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.ThreeOfKind);
                return classified_hand;
            }

            //If the Dictionary count is equal to 4, the PokerHand is a One Pair
            else if(pair_hand.ContainsValue(2) && pair_hand.Count == 4)
            {
                PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.Pair);
                return classified_hand;
            }

            //If none of the above, the 
            else
            {
                PokerHand classified_hand = new PokerHand(player_hand, PokerHand.HandType.TwoPair);
                return classified_hand;
            }
        }
        /**
         * Method determines if the list of cards has the straight characteristic
         * Sorts the list of cards in increasing order
         * Checks to see if the cards increment by one to the next card
         * if CardKind is sequential, then return true
         * if CardKind is not sequential, then return false
         * */
        public static bool IsStraight(IEnumerable<Card> hand)
        {
            //Convert IEnumberable to List
            List<Card> player_hand = hand.ToList<Card>();

            //Sort list of cards to increasing order
            player_hand = SortHand(player_hand);

            //If the cards are in sequential order then return true
            if (    player_hand[4].Kind - player_hand[3].Kind == 1
                && player_hand[3].Kind - player_hand[2].Kind == 1
                && player_hand[2].Kind - player_hand[1].Kind == 1
                && player_hand[1].Kind - player_hand[0].Kind == 1)
            {
                return true;
            }

            //If the cards are not in sequential order then return false
            else
            {
                return false;
            }
        }
        /**
         * Method determines if the list of cards has the flush characteristic
         * Uses a Dictionary to check if the list of cards has 5 of the same CardSuit, key = CardSuit, value = amount of suits
         * if Dictionary contains key = 5, return true;
         * if Dictionary does not contain key = 5, return false;
         * */
        public static bool IsFlush(IEnumerable<Card> hand)
        {
            //Dictionary used to determine if there are 5 of the same suit in the list of cards
            Dictionary<Card.CardSuit, int> is_Flush_Hand = new Dictionary<Card.CardSuit, int>();

            //Convert IEnumerable to List
            List<Card> player_hand = hand.ToList<Card>();

            //Iterate through the list to be placed in Dictionary
            for (int i = 0; i < player_hand.Count; i++)
            {
                //If suit does not currently exist in the dictionary
                if (!is_Flush_Hand.ContainsKey(player_hand[i].Suit))
                {
                    is_Flush_Hand.Add(player_hand[i].Suit, 1);
                }
                
                //Increment value if suit exists in dictionary
                else
                {
                    int temp = is_Flush_Hand[player_hand[i].Suit];
                    is_Flush_Hand.Remove(player_hand[i].Suit);
                    is_Flush_Hand.Add(player_hand[i].Suit, temp + 1);
                }
            }

            //If dictionary contains key = 5, return true
            if (is_Flush_Hand.ContainsValue(5))
            {
                return true;
            }

            //if dictionary does not contain key = 5, return false
            else
            {
                return false;
            }

        }

        /**
         * Method is used to Sort the list of cards in increasing order
         * Using Insertion Sort, smallest card goes first, highest card goes last, based on CardKind
         * Returns sorted list of cards
         * */
        public static List<Card> SortHand(IEnumerable<Card> hand)
        {
            //Convert IEnumerable to List
            List<Card> player_hand = hand.ToList<Card>();

            //Interate through position for the next smallest card
            for (int i = 0; i < player_hand.Count; i++)
            {
                Card lowest = player_hand[i];
                for (int j = i; j < player_hand.Count; j++)
                {
                    int compare = player_hand[i].CompareTo(player_hand[j]);
                    if (compare > 0)
                    {
                        Card temp = player_hand[j];
                        player_hand[j] = player_hand[i];
                        player_hand[i] = temp;
                    }

                }
            }
            return player_hand;
        }

    }

}
