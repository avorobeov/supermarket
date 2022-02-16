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
            List<Buyer> buyers = new List<Buyer>{new Buyer("Dima", 100),
                                                 new Buyer("Vasa", 10),
                                                 new Buyer("Kiril", 25),
                                                 new Buyer("Den", 30) };
            List<Commodity> showcase = new List<Commodity> { new Commodity("Рыба", 10),
                                                             new Commodity("Мясо", 20),
                                                             new Commodity("Колбаса", 15),
                                                             new Commodity("Печение", 20)};

            Supermarket supermarket = new Supermarket(buyers, showcase);

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

        public Supermarket(List<Buyer> buyers, List<Commodity> showcase)
        {
            _buyers = buyers;
            _showcase = showcase;
        }

        public void ServeCustomers()
        {
            Buyer buyer;

            while (_queueBuyers.Count != 0)
            {
                buyer = _queueBuyers.Dequeue();

                bool havePurchase = false;

                while (havePurchase == false)
                {
                    int amountPurchases = buyer.GetAmountPurchases(buyer);

                    if (buyer.Money >= amountPurchases && buyer.CountPurchases != 0)
                    {
                        buyer.DecreaseBalance(amountPurchases);

                        buyer.ShowPurchases();

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
                      
                        buyer.DeletePurchase();
                    }
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        public void CreatePurchase()
        {
            int numberPurchases;
            int maximumNumberPurchases = 10;
            int minimalNumberPurchases = 1;

            for (int i = 0; i < _buyers.Count; i++)
            {
                numberPurchases = _random.Next(0, maximumNumberPurchases);

                for (int p = 0; p < numberPurchases; p++)
                {
                    _buyers[i].TakePurchase(_showcase[_random.Next(minimalNumberPurchases, _showcase.Count())]);
                }

                _queueBuyers.Enqueue(_buyers[i]);
            }
        }

        public void FillShowcase(Commodity commodity)
        {
            _showcase.Add(commodity);
        }

        public void FillingListBuyers(Buyer buyer)
        {
            _buyers.Add(buyer);
        }

        private void ShowMessage(string message, ConsoleColor color)
        {
            ConsoleColor preliminaryColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message);

            Console.ForegroundColor = preliminaryColor;
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
        public int CountPurchases => _commodities.Count;
      
        public Buyer(string name, int money)
        {
            Name = name;
            Money = money;
        }

        public void TakePurchase(Commodity commodity)
        {
            _commodities.Add(commodity);
        }

        public void DecreaseBalance(int sum)
        {
            Money -= sum;
        }

        public void DeletePurchase()
        {
            Random random = new Random();

            int noItems = 0;

            if (_commodities.Count != noItems) 
            {
                _commodities.RemoveAt(random.Next(0, _commodities.Count));
            }
        }

        public void ShowPurchases()
        {
            Console.WriteLine("Список ваших покупок:\n");

            for (int i = 0; i < _commodities.Count; i++)
            {
                Console.WriteLine(_commodities[i].Title + "\n");
            }
        }

        public int GetAmountPurchases(Buyer buyer)
        {
            int sum = 0;

            for (int i = 0; i < _commodities.Count; i++)
            {
                sum += _commodities[i].Price;
            }

            return sum;
        }
    }
}
