using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace supermarket
{
    class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket();

            supermarket.FillingListBuyers(new Buyer("Dima", 100));
            supermarket.FillingListBuyers(new Buyer("Vasa", 10));
            supermarket.FillingListBuyers(new Buyer("Kiril", 25));
            supermarket.FillingListBuyers(new Buyer("Den", 30));

            supermarket.FillShowcase();
            supermarket.CreatePurchase();
            supermarket.ServeCustomers();
        }
    }

    class Supermarket
    {
        private Random _random = new Random();

        private Queue<Buyer> _queueBuyers = new Queue<Buyer>();
        private List<Commodity> _showcase = new List<Commodity>();
        private List<Buyer> _buyers = new List<Buyer>();

        public void ServeCustomers()
        {
            Buyer buyer;

            while (_queueBuyers.Count != 0)
            {
                buyer = _queueBuyers.Dequeue();

                bool havePurchase = false;

                while (havePurchase == false)
                {
                    int amountPurchases = GetAmountPurchases(buyer);

                    if (buyer.Money >= amountPurchases && buyer.CountPurchases != 0)
                    {
                        buyer.BalanceReduction(amountPurchases);

                        ShowPurchases(buyer);

                        ShowMessage($"\nПокупка совершена успешно вот ваша сдача: {buyer.Money}\n", ConsoleColor.Green);

                        havePurchase = true;
                    }
                    else if (buyer.CountPurchases == 0)
                    {
                        ShowMessage("\nВам не хватило денег на покупку тех товаров, что вы выбрали\n", ConsoleColor.DarkMagenta);
                      
                        havePurchase = true;
                    }
                    else
                    {
                        ShowMessage("\nНедостаточно денег для покупки\nВложите какой-либо  товар\n", ConsoleColor.Red);
                      
                        buyer.Deleteurchase();
                    }
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        public void CreatePurchase()
        {
            int maximumNumberPurchases = 10;
            int minimalNumberPurchases = 1;

            for (int i = 0; i < _buyers.Count; i++)
            {
                for (int p = 0; p < _random.Next(0,maximumNumberPurchases); p++)
                {
                    _buyers[i].TakePurchase(_showcase[_random.Next(minimalNumberPurchases, _showcase.Count())]);
                }

                _queueBuyers.Enqueue(_buyers[i]);
            }
        }

        public void FillShowcase()
        {
            _showcase.Add(new Commodity("Рыба", 10));
            _showcase.Add(new Commodity("Мясо", 20));
            _showcase.Add(new Commodity("Колбаса", 15));
            _showcase.Add(new Commodity("Печение", 20));
        }

        public void FillingListBuyers(Buyer buyer)
        {
            _buyers.Add(buyer);
        }

        private int GetAmountPurchases(Buyer buyer)
        {
            List<Commodity> Purchase = buyer.GetPurchaseList();

            int sum = 0;

            for (int i = 0; i < Purchase.Count; i++)
            {
                sum += Purchase[i].Price;
            }

            return sum;
        }

        private void ShowMessage(string message, ConsoleColor color)
        {
            ConsoleColor preliminaryColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message);

            Console.ForegroundColor = preliminaryColor;
        }
      
        private void ShowPurchases(Buyer buyer)
        {
            List<Commodity> shopping = buyer.GetPurchaseList();

            ShowMessage("Список ваших покупок:\n", ConsoleColor.Blue);

            for (int i = 0; i < shopping.Count; i++)
            {
                ShowMessage(shopping[i].Title + "\n", ConsoleColor.Yellow);
            }
        }
    }

    class Commodity
    {
        public string Title { get; private set; }
        public int Price { get; private set; }

        public Commodity(string title,int price)
        {
            Title = title;
            Price = price;
        }
    }

    class Buyer
    {
        private List<Commodity> _commodities = new List<Commodity>();

        public string Name { get; private set; }
        public int Money { get; private set; }
        public int CountPurchases
        {
            get
            {
                return _commodities.Count;
            }
        }
      
        public Buyer(string name, int money)
        {
            Name = name;
            Money = money;
        }

        public void TakePurchase(Commodity commodity)
        {
            _commodities.Add(commodity);
        }

        public List<Commodity> GetPurchaseList()
        {
            return _commodities;
        }

        public void BalanceReduction(int sum)
        {
            Money -= sum;
        }

        public void Deleteurchase()
        {
            Random random = new Random();

            int noItems = 0;

            if (_commodities.Count != noItems) 
            {
                _commodities.RemoveAt(random.Next(0, _commodities.Count));
            }
        }
    }
}
