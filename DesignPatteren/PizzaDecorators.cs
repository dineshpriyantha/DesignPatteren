using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DesignPatteren
{
    public interface IPizza
    {
        string MakePizza();
    }

    public class PlainPizza : IPizza
    {
        public string MakePizza()
        {
            return "Plain Pizza";
        }
    }

    // create a pizza decorator
    public abstract class PizzaDecorator : IPizza
    {
        protected IPizza pizza;

        public PizzaDecorator(IPizza pizza)
        {
            this.pizza = pizza;
        }
        public virtual string MakePizza()
        {
            return pizza.MakePizza();
        }
    }


    // create chiken pizza decorator

    public class ChickenPizzaDecotator : PizzaDecorator
    {
        public ChickenPizzaDecotator(IPizza pizza) : base(pizza)
        {
        }

        public override string MakePizza()
        {
            return pizza.MakePizza() + AddChicken();
        }

        private string AddChicken()
        {
            return ", Chicken Added";
        }
    }


    // create Veg pizza decorator

    public class VegPizzaDecorator : PizzaDecorator
    {
        public VegPizzaDecorator(IPizza pizza) : base(pizza)
        {
        }

        public override string MakePizza()
        {
            return pizza.MakePizza() + AddVegitables();
        }

        private string AddVegitables()
        {
            return " Vegibales added";
        }
    }


    // create Fish decorator

    public class FishPizzaDecorator : PizzaDecorator
    {
        public FishPizzaDecorator(IPizza pizza) : base(pizza)
        {
        }

        public override string MakePizza()
        {
            return pizza.MakePizza() + AddFish();
        }

        private string AddFish()
        {
            return ", Fish added";
        }
    }


    // create Egg piza decorator

    public class EggPizzaDecorator : PizzaDecorator
    {
        public EggPizzaDecorator(IPizza pizza) : base(pizza)
        {
        }

        public override string MakePizza()
        {
            return pizza.MakePizza() + AddEgg();
        }

        private string AddEgg()
        {
            return ", Egg added";
        }
    }


    // Client code

    class program1
    {
        static void Main1()
        {
            PlainPizza plainPizza = new PlainPizza();
            WriteLine(plainPizza.MakePizza());

            PizzaDecorator chikenDecorator = new ChickenPizzaDecotator(plainPizza);
            WriteLine($"\n {chikenDecorator.MakePizza()}");


            PizzaDecorator vegDecorator = new VegPizzaDecorator(plainPizza);
            WriteLine($"\n {vegDecorator.MakePizza()}");

            Read();
        }

    }
}
