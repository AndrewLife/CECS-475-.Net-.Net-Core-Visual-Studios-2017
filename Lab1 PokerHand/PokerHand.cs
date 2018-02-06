using Cecs475.War;
using System;
using System.Collections.Generic;
using System.Text;

namespace CECS475.Poker.Cards
{
    public class PokerHand : IComparable<PokerHand>
    {
        //List of cards
        private List<Card> player_hand;

        //Type of Hand (High Card, One Pair, Two Pair, Three of a Kind, Straight, Flush, Full House, Four of a Kind, Straight Flush, Royal Flush)
        private HandType hand_kind;

        //ranking of types of Hands
        public enum HandType
        {
            HighCard, //0
            Pair,
            TwoPair,
            ThreeOfKind,
            Straight,
            Flush,
            FullHouse,
            FourOfKind,
            StraightFlush,
            RoyalFlush
        }

        /**
         * Auto-property for the list of cards
         * */
        public List<Card> Hand
        {
            get { return player_hand; }
        }
        /**
         * Auto-property for the type of hand
         * */
        public HandType PokerHandType
        {
            get { return hand_kind; }
        } 
        /**
         * Constructor
         * Instantiates new PokerHand with list of cards and the type of hand
         * */
        public PokerHand(List<Card> hand, HandType type)
        {
            player_hand = hand;
            hand_kind = type;
        }
        /**
         * Method is used to compare the hand types between two different Poker Hands
         * Checks to see if the hand types are different
         * If not, then checks the high card of each Poker Hand
         * returns positive integer, if first poker hand is greater
         * return 0, if both hands are the same
         * returns negative integer, if second poker hand is greater
         * */
        public int CompareTo(PokerHand other)
        {

            //If both hand types are different
            if(this.PokerHandType.CompareTo(other.PokerHandType) != 0)
            {
                return this.PokerHandType.CompareTo(other.PokerHandType);
            }
            //If both hand types are the same
            else
            {
                //Iterrate through both hands to see which has the higher card
                for(int i = (this.Hand.Count - 1); i >= 0; i--)
                {
                    if(this.player_hand[i].CompareTo(other.player_hand[i]) != 0){
                        return this.player_hand[i].CompareTo(other.player_hand[i]);
                    }
                }
                //both hands are the same
                return 0;
            }
        }
    }
}
