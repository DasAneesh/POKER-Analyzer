using System;
using System.Collections.Generic;
using System.Linq;

// Класс для представления карты
public class Card
{
    public string Suit { get; }
    public string Rank { get; }

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString() => $"{Rank} of {Suit}";

    public int GetValue()
    {
        int val = 0;
        if (Rank == "Jack")
        {
            val = 11;
            return val;
        }
        else if (Rank == "Queen")
        {
            val = 12;
            return val;
        }
        else if (Rank == "King")
        {
            val = 13;
            return val;
        }
        else if (Rank == "Ace")
        {
            val = 14;
            return val;
        }
        else
        {
            val = int.Parse(Rank);
            return val;
        }
    }
}

// Класс для представления колоды карт
public class Deck
{
    private List<Card> cards = new List<Card>();
    private Random rng = new Random();

    public Deck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                cards.Add(new Card(suit, rank));
            }
        }
        Shuffle();
    }

    // Метод для перетасовки колоды
    public void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            var value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    public Card Deal()
    {
        if (cards.Count == 0) throw new InvalidOperationException("No cards left in the deck.");
        var card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}

// Класс для представления игрока
public class Player
{
    public string Name { get; }
    public List<Card> Hand { get; private set; } = new List<Card>();

    public Player(string name)
    {
        Name = name;
    }

    public void TakeCard(Card card)
    {
        Hand.Add(card);
    }

    public HandRank GetHandRank()
    {
        var rankCounts = Hand.GroupBy(card => card.Rank).ToDictionary(g => g.Key, g => g.Count());
        var flush = Hand.All(card => card.Suit == Hand[0].Suit);
        var straight = IsStraight();

        if (Flush && Straight && Hand.Any(card => card.Rank == "Ace") && Hand.Any(card => card.Rank == "10"))
            return HandRank.RoyalFlush;

        if (Flush && Straight) return HandRank.StraightFlush;

        if (rankCounts.Values.Contains(4)) return HandRank.FourOfAKind;

        if (rankCounts.Values.Contains(3) && rankCounts.Values.Contains(2)) return HandRank.FullHouse;

        if (Flush) return HandRank.Flush;

        if (Straight) return HandRank.Straight;

        if (rankCounts.Values.Contains(3)) return HandRank.ThreeOfAKind;

        if (rankCounts.Values.Count(v => v == 2) == 2) return HandRank.TwoPair;

        if (rankCounts.Values.Contains(2)) return HandRank.OnePair;

        return HandRank.HighCard;
    }

    private bool IsStraight()
    {
        var values = Hand.Select(card => card.GetValue()).Distinct().OrderBy(x => x).ToList();
        return values.Zip(values.Skip(1), (a, b) => b - a).All(x => x == 1);
    }
}

// Enum для ранга руки
public enum HandRank
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    Straight,
    Flush,
    FullHouse,
    FourOfAKind,
    StraightFlush,
    RoyalFlush
}

// Класс для представления игры
public class Game
{
    public List<Player> Players { get; private set; } = new List<Player>();
    public Deck Deck { get; private set; } = new Deck();

    public void AddPlayer(string playerName)
    {
        Players.Add(new Player(playerName));
    }

    public void Deal()
    {
        foreach (var player in Players)
        {
            player.TakeCard(Deck.Deal());
            player.TakeCard(Deck.Deal());
        }
    }

    public void ShowHands()
    {
        foreach (var player in Players)
        {
            Console.WriteLine($"{player.Name}'s hand: {string.Join(", ", player.Hand)} - {player.GetHandRank()}");
        }
    }

    public string GetWinner()
    {
        var winner = Players.OrderByDescending(player => player.GetHandRank()).First();
        return $"{winner.Name} wins with a hand of {winner.GetHandRank()}!";
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.AddPlayer("Alice");
        game.AddPlayer("Bob");

        game.Deal();
        game.ShowHands();

        Console.WriteLine(game.GetWinner());
    }
}

