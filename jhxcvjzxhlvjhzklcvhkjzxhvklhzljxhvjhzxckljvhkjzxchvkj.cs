using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Player
    {
        public string Name { get; private set; }
        public List<Cards.Card> Hand { get; private set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Cards.Card>();
        }

        // Метод для добавления карты в руку игрока
        public void AddCard(Cards.Card card)
        {
            if (Hand.Count >= 2)
            {
                throw new InvalidOperationException("Player already has 2 cards.");
            }
            Hand.Add(card);
        }

        // Метод для сброса руки игрока
        public void ClearHand()
        {
            Hand.Clear();
        }

        // Метод для определения типа руки игрока
        public string EvaluateHand()
        {
            Hand.Sort((x, y) => x.GetValue().CompareTo(y.GetValue())); // Сортируем карты
            var hand = new Hand(Hand); // Создаем объект Hand для оценки
            return hand.DetermineHandType();
        }

        // Метод для отображения карт в руке
        public override string ToString()
        {
            return $"{Name} holds: {string.Join(", ", Hand)}";
        }
    }

}
