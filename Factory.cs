using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DPExamples.DrinkMachine;

namespace DPExamples
{
    public interface IDrink
    {
        void Consume();
    }
       public interface IHotDrink : IDrink
        {
            
        }

    public interface IColdDrink : IDrink
    {        
    }

        internal class Tea : IHotDrink
        {
            public void Consume()
            {
                Console.WriteLine("This tea is nice but I'd prefer it with milk.");
            }
        }

        internal class Coffee : IHotDrink
        {
            public void Consume()
            {
                Console.WriteLine("This coffee is delicious!");
            }
        }

          internal class Juice : IColdDrink
          {
                public void Consume()
               {
                Console.WriteLine("This Juice is delicious!");
               }
           }

    internal class ColdWater : IColdDrink
    {
        public void Consume()
        {
            Console.WriteLine("This Cold water refresh me !");
        }
    }

    public interface IDrinkFactory
    {
        IDrink Prepare(int amount);
    }

    public interface IHotDrinkFactory: IDrinkFactory
    {
        IDrink Prepare(int amount);
        }

    public interface IColdDrinkFactory: IDrinkFactory
    {
        IDrink Prepare(int amount);
    }
   
    internal class JuiceFactory : IColdDrinkFactory
    {
        public IDrink Prepare(int amount)
        {
            Console.WriteLine($"Put orange pour {amount} ml,squeeze , enjoy!");
            return new Juice();
        }
    }

    internal class ColdWaterFactory : IColdDrinkFactory
    {
        public IDrink Prepare(int amount)
        {
            Console.WriteLine($"Put water pour {amount} ml on the fredge for 20 min, enjoy drink it cold !");
            return new ColdWater();
        }
    }


    internal class TeaFactory : IHotDrinkFactory
        {
            public IDrink Prepare(int amount)
            {
                Console.WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
                return new Tea();
            }
        }

        internal class CoffeeFactory : IHotDrinkFactory
        {
            public IDrink Prepare(int amount)
            {
                Console.WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
                return new Coffee();
            }
        }

    public interface IDrinkMachine
    {
       public IDrink MakeDrink();

    }

    public class DrinkMachine : IDrinkMachine
    {


        private List<Tuple<string, string, IDrinkFactory>> namedFactories =
          new List<Tuple<string, string, IDrinkFactory>>();

        public DrinkMachine()
        {
            //foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            //{
            //  var factory = (IHotDrinkFactory) Activator.CreateInstance(
            //    Type.GetType("DotNetDesignPatternDemos.Creational.AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));
            //  factories.Add(drink, factory);
            //}

            foreach (var t in typeof(DrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    namedFactories.Add(Tuple.Create(
                      t.Name.Replace("Factory", string.Empty), t.GetInterfaces()[0].Name.Replace("Factory", "").Replace("I", ""), (IDrinkFactory)Activator.CreateInstance(t)));
                }
            }
        }

        public IDrink MakeDrink()
        {
            string cat;
            var catFactories = new List<Tuple<string, string, IDrinkFactory>>();

            Console.WriteLine("Available drinks Categories");

            var categories = namedFactories.Select(a => a.Item2).Distinct().ToList();

            for (var index = 0; index < categories.Count; index++)
            {
                Console.WriteLine($"{index}: {categories.ElementAt(index)}");
            }


            while (true)
            {

                cat = Console.ReadLine();

                if (cat != null
                    && int.TryParse(cat, out int i) // c# 7
                    && i >= 0
                    && i < categories.Count)
                {
                    catFactories = namedFactories.Where(a => a.Item2 == categories.ElementAt(i)).ToList();
                }


                Console.WriteLine("Available drinks");
                for (var index = 0; index < catFactories.Count; index++)
                {
                    var tuple = catFactories[index];
                    Console.WriteLine($"{index}: {tuple.Item1}");
                }


                string s;
                s = s = Console.ReadLine();

                if (s != null
                    && int.TryParse(s, out i) // c# 7
                    && i >= 0
                    && i < catFactories.Count)
                {
                    Console.Write("Specify amount: ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int amount)
                        && amount > 0)
                    {
                        return catFactories[i].Item3.Prepare(amount);
                    }
                }
                Console.WriteLine("Incorrect input, try again.");
            }
        }
    }
  
        //public class HotDrinkMachine : IDrinkMachine
        //{
        //    public enum AvailableDrink // violates open-closed
        //    {
        //        Coffee, Tea
        //    }

        //    private Dictionary<AvailableDrink, IHotDrinkFactory> factories =
        //      new Dictionary<AvailableDrink, IHotDrinkFactory>();

        //    private List<Tuple<string, IHotDrinkFactory>> namedFactories =
        //      new List<Tuple<string, IHotDrinkFactory>>();

        //    public HotDrinkMachine()
        //    {
        //        //foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
        //        //{
        //        //  var factory = (IHotDrinkFactory) Activator.CreateInstance(
        //        //    Type.GetType("DotNetDesignPatternDemos.Creational.AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));
        //        //  factories.Add(drink, factory);
        //        //}

        //        foreach (var t in typeof(IDrinkMachine).Assembly.GetTypes())
        //        {
        //            if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
        //            {
        //                namedFactories.Add(Tuple.Create(
        //                  t.Name.Replace("Factory", string.Empty), (IHotDrinkFactory)Activator.CreateInstance(t)));
        //            }
        //        }
        //    }

        //    public IDrink MakeDrink()
        //    {
        //        Console.WriteLine("Available drinks");
        //        for (var index = 0; index < namedFactories.Count; index++)
        //        {
        //            var tuple = namedFactories[index];
        //            Console.WriteLine($"{index}: {tuple.Item1}");
        //        }

        //        while (true)
        //        {
        //            string s;
        //            if ((s = Console.ReadLine()) != null
        //                && int.TryParse(s, out int i) // c# 7
        //                && i >= 0
        //                && i < namedFactories.Count)
        //            {
        //                Console.Write("Specify amount: ");
        //                s = Console.ReadLine();
        //                if (s != null
        //                    && int.TryParse(s, out int amount)
        //                    && amount > 0)
        //                {
        //                    return namedFactories[i].Item2.Prepare(amount);
        //                }
        //            }
        //            Console.WriteLine("Incorrect input, try again.");
        //        }
        //    }

        //    //public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        //    //{
        //    //  return factories[drink].Prepare(amount);
        //    //}
        //}

        public class Program
        {
            static void Main(string[] args)
            {
                var machine = new DrinkMachine();
                //var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, 300);
                //drink.Consume();

                IDrink drink = machine.MakeDrink();
                drink.Consume();
            }
        }

    }
    
