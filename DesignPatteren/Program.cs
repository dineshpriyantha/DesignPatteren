using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DesignPatteren
{
    #region Single Responsibility Principle

    public class Journal
    {
        private readonly List<string> entries = new List<string>();

        private static int count = 0;

        public int AddEntry(string text)
        {
            entries.Add($"{++count}: {text}");
            return count;
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }

        //public void Save(string filename)
        //{
        //    File.WriteAllText(filename, ToString());
        //}

        //public static Journal Load(string filename)
        //{

        //}

        //public void Load(Uri uri)
        //{

        //}

    }

    public class Persistence
    {
        public void SaveToFile(Journal j, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
                File.WriteAllText(filename, j.ToString());
        }
    }

    #endregion

    #region Open Close Principle
    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Yuge
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            if (name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }

            Name = name;
            Color = color;
            Size = size;
        }
    }

    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products,
            Size size)
        {
            foreach (var p in products)
                if (p.Size == size)
                    yield return p;
        }

        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products,
            Color color)
        {
            foreach (var p in products)
                if (p.Color == color)
                    yield return p;
        }


        public IEnumerable<Product> FiltereBySizeandColor(IEnumerable<Product> products,
            Color color, Size size)
        {
            foreach (var p in products)
                if (p.Color == color && p.Size == size)
                    yield return p;
        }
    }

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first; //??  :throw new ArgumentNullException(paramName: nameof(first));
            this.second = second;//?? : throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
                if (spec.IsSatisfied(i))
                    yield return i;
        }
    }

    #endregion

    #region Liskov Substitution Principle

    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangle()
        {

        }

        public Rectangle(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public override string ToString()
        {
            // C# NameOf operator is used to get name of a variable, class or method.
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class Square : Rectangle
    {
        public override int Width
        {
            set { base.Width = base.Height = value; }
        }

        public override int Height
        {
            set { base.Width = base.Height = value; }
        }

    }

    #endregion

    #region Interface Segregation Principle

    public class Document
    {

    }

    public interface IMachine
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }

    public class MultiFunctionPrinter : IMachine
    {
        public void Fax(Document d)
        {
            //
        }

        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    // dosen't need fax, print, scan
    public class OldFashionedPrinter : IMachine
    {
        public void Print(Document d)
        {
            //
        }

        // OldFashionedPrinter doesn't need the Fax, Scan
        // they don't need to pay for this functions.
        // then need to divide an intereface into seperate parts 
        public void Fax(Document d)
        {
            throw new NotImplementedException();//
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }
    }

    // Segregate the Interface
    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    public interface IMultiFunctionDevice : IScanner, IPrinter //..
    {

    }

    public class MultiFfunctionMachine : IMultiFunctionDevice
    {
        private IPrinter printer;
        private IScanner scanner;

        public MultiFfunctionMachine(IPrinter printer, IScanner scanner)
        {
            this.printer = printer;
            this.scanner = scanner;
        }

        // doing delegate
        public void Print(Document d)
        {
            printer.Print(d);
        }

        public void Scan(Document d)
        {
            scanner.Scan(d);
        }
    }

    #endregion

    #region Dependency Inversion Principle

    public enum Relationship
    {
        Parent,
        Child,
        Sibiling
    }

    public class Person
    {
        public string Name;

    }

    // low level
    //public class Relationships
    //{
    //    private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

    //    public void AddParentAndChild(Person parent, Person child)
    //    {
    //        //relations.Add((parent, Relationship.Parent))
    //    }

    //    public List<(Person, Relationship, Person)> Relations => relations;
    //}



    #endregion

    #region Builder

    public class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> elements = new List<HtmlElement>();
        private const int indentSize = 2;

        public HtmlElement()
        {

        }

        public HtmlElement(string name, string text)
        {
            this.Name = name;
            this.Text = text;
        }

        private string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);
            sb.AppendLine($"{i}<{Name}>");
            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var e in elements)
            {
                sb.Append(e.ToStringImpl(indent + 1));
            }

            sb.AppendLine($"{i} <{Name}>");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    public class HtmlBuilder
    {
        private readonly string rootName;
        HtmlElement root = new HtmlElement();

        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName;
            root.Name = rootName;
        }

        public HtmlBuilder AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.elements.Add(e);
            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement { Name = rootName };
        }
    }

    #endregion

    #region Fluent Builder Inheritance with Recursive Generics

    public class Person1
    {
        public string Name;
        public string Position;

        public class Builder : PersonJobBuilder<Builder>
        {

        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(Name)} : {Name}, {nameof(Position)} : {Position}";
        }

    }

    public abstract class PersonBulider
    {
        protected Person1 person = new Person1();

        public Person1 Build()
        {
            return person;
        }
    }

    public class PersonInfoBuilder<SELF>
        : PersonBulider
        where SELF : PersonInfoBuilder<SELF>
    {
        //protected Person1 person = new Person1();

        public SELF Called(string name)
        {
            person.Name = name;
            return (SELF)this;
        }
    }


    public class PersonJobBuilder<SELF>
        : PersonInfoBuilder<PersonJobBuilder<SELF>>
        where SELF : PersonJobBuilder<SELF>
    {
        public SELF WorksAsA(string position)
        {
            person.Position = position;

            return (SELF)this;
        }
    }

    #endregion

    #region Functional Builder



    #endregion


    #region Factories

    #region Point

    //public enum CoordinateSystem
    //{
    //    Cartesian,
    //    Polar
    //}

    //public static class PointFactory
    //{
    //    // factory methos
    //    public static Point NewCartesianPoint(double x, double y)
    //    {
    //        return new Point(x, y);
    //    }

    //    public static Point NewPolarPoint(double rho, double theta)
    //    {
    //        return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
    //    }

    //}

    public class Point
    {
        private double x, y;

        /// <summary>
        /// Initialize a point from EITHER cartesian or polar 
        /// </summary>
        /// <param name="a">x if cartesian, rho if polar</param>
        /// <param name="b"></param>
        /// <param name="system"></param>
        //public Point(double a, double b,
        //    CoordinateSystem system = CoordinateSystem.Cartesian)
        //{
        //    switch (system)
        //    {
        //        case CoordinateSystem.Cartesian:
        //            x = a;
        //            y = b;
        //            break;
        //        case CoordinateSystem.Polar:
        //            x = a * Math.Cos(b);
        //            y = a * Math.Sin(b);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        // public Point(double rho, double theta) we can't do this


        // apply Factory method instead of above method
        //
        //
        //


        //// factory methos
        //public static Point NewCartesianPoint(double x, double y)
        //{
        //    return new Point(x, y);
        //}

        //public static Point NewPolarPoint(double rho, double theta)
        //{
        //    return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        //}

        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{nameof(x)} : {x}, {nameof(y)} : {y}";
        }

        public static Point Origin => new Point(0, 0);
        public static Point Origin2 = new Point(0, 0);

        public static class Factory
        {
            // factory method
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }

        }
    }

    #endregion

    #region Abstract Factory

    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This tea is nice but I'd prefere it with milk.");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This coffee is sensational!");
        }
    }


    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Put in a teat bag, boil water , pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    internal class CofeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Grind some beans, boil water, pour {amount} ml,, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        public enum AvailableDrink
        {
            Coffee, Tea
        }

        private Dictionary<AvailableDrink, IHotDrinkFactory> factories =
            new Dictionary<AvailableDrink, IHotDrinkFactory>();

        public HotDrinkMachine()
        {
            foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            {
                var factory = (IHotDrinkFactory)Activator.CreateInstance(
                    Type.GetType("DesignPatterns." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory")
                    );
                factories.Add(drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        {
            return factories[drink].Prepare(amount);
        }

    }

    #endregion

    #endregion

    #region Composite

    public class GraphicObject
    {
        public virtual string Name { get; set; } = "Group";
        public string Color;

        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        public List<GraphicObject> Children => children.Value;

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }

        private void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth))
              .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color}")
              .AppendLine(Name);

            foreach (var child in Children)
            {
                child.Print(sb, depth + 1);
            }
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square1 : GraphicObject
    {
        public override string Name => "Square";
    }

    #endregion

    #region Neural Networks

    public static class ExtensionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self,
            IEnumerable<Neuron> other)
        {
            foreach (var from in self)
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
        }
    }

    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        public List<Neuron> In, Out;

        public void ConnectTo(Neuron other)
        {
            Out.Add(other);
            other.In.Add(this);
        }

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NeuroneLayer : Collection<Neuron>
    {

    }


    #endregion

    class Program
    {
        //public Program(Relationships relationships)
        //{
        //    //var relations = relationships
        //}

        //public Research(IRelationshipBrowser )
        //{

        //}
        static public int Area(Rectangle r) => r.Width * r.Height;
        static public int AreaMxM(Rectangle r) => r.Width + r.Height;

        static void Main(string[] args)
        {
            #region SOLID Design Principles

            // one responsinble 
            // One thing to have change
            #region Single Responsibility Principle

            //var j = new Journal();
            //j.AddEntry("I cried today");
            //j.AddEntry("I ate a bug");

            //Console.WriteLine(j);

            //var p = new Persistence();
            //var filename = @"c:\test\journal.txt";
            //p.SaveToFile(j, filename, true);

            //Process.Start(filename);


            #endregion

            // open for extend cloase for modification
            #region Open Close Principle

            //var apple = new Product("Apple", Color.Green, Size.Small);
            //var tree = new Product("Tree", Color.Green, Size.Large);
            //var house = new Product("House", Color.Blue, Size.Large);

            //Product[] products = { apple, tree, house };

            //var pf = new ProductFilter();
            //Console.WriteLine("Green products (old):");

            //foreach (var p in pf.FilterByColor(products, Color.Green))
            //{
            //    Console.WriteLine($" - {p.Name} is Green");
            //}

            //var bf = new BetterFilter();
            //Console.WriteLine("Green products (new)");
            //foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
            //{
            //    Console.WriteLine($" - {p.Name} is green");
            //}

            //Console.WriteLine("Large blue items");
            //foreach (var p in bf.Filter(
            //    products,
            //    new AndSpecification<Product>(
            //        new ColorSpecification(Color.Blue),
            //        new SizeSpecification(Size.Large)
            //        )))
            //{
            //    Console.WriteLine($" - {p.Name} is big and blue");
            //}


            #endregion

            //The Liskov substitution principle (LSP) is a collection of guidelines for creating inheritance 
            // hierarchies in which a client can reliably use any class or subclass without compromising the expected behavior.
            #region Liskov Substitution Principle

            //Rectangle rc = new Rectangle(2, 3);
            //Console.WriteLine($"{rc} has area {Area(rc)}");

            ////Square sq = new Square();
            //Rectangle sq = new Square();
            //sq.Width = 4;
            //Console.WriteLine($"{sq} has area {Area(sq)}");

            #endregion

            // Segregate of the Interface
            #region Interface Segregation Principle

            #endregion

            // What is the best description of the Dependency Inversion principle?
            // The Dependency Inversion Principle (DIP)states that high level modules should not depend on low level modules; 
            // both should depend on abstractions.Abstractions should not depend on details. Details should depend upon abstractions.
            #region Dependency Inversion Principle




            #endregion

            #endregion

            #region Builder

            #region Gamma Categorization

            // life without builder

            //var hello = "hello";
            //var sb = new StringBuilder();
            //sb.Append("<p>");
            //sb.Append(hello);
            //sb.Append("</p>");
            //Console.WriteLine(sb);

            //var words = new[] { "hello", "world" };
            //sb.Append("<ul>");

            //foreach (var item in words)
            //{
            //    sb.Append($"<ol>{item}</ol>");
            //}
            //sb.Append("</ol>");
            //Console.WriteLine(sb);

            //var builder = new HtmlBuilder("ul");
            //builder.AddChild("li", "hello").AddChild("li", "world");
            //Console.WriteLine(builder.ToString());

            #endregion

            #endregion

            #region Fluent Builder Inheritance with Recursive Generics

            //var builder1 = new PersonJobBuilder();
            //builder1.Called("Dinesh")
            //    .WorksAsA() // can't access this then create new "abstract class name is 'PersonBulider'"

            //var me = Person1.New
            //    .Called("Amal")
            //    .WorksAsA("Wije")
            //    .Build();

            //Console.WriteLine(me);


            #endregion


            #region Functional Builder

            #endregion


            #region Factories

            #region Point

            //var point = Point.Factory.NewPolarPoint(1.0, Math.PI / 2);
            //Console.WriteLine(point);


            #endregion

            #region Abstract Factory

            //var machine = new HotDrinkMachine();
            //var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, 12);
            //drink.Consume();

            #endregion


            #endregion

            #region Composite

            //var drawing = new GraphicObject { Name = "My Drawing" };
            //drawing.Children.Add(new Square1 { Color = "Red" });
            //drawing.Children.Add(new Circle { Color = "Yellow" });

            //var group = new GraphicObject();
            //group.Children.Add(new Circle { Color = "Blue" });
            //group.Children.Add(new Square1 { Color = "Blue" });
            //drawing.Children.Add(group);
            //WriteLine(drawing);

            #endregion

            #region Neural Networks

            var neuron1 = new Neuron();
            var neuron2 = new Neuron();

            //neuron1.ConnectTo(neuron2);

            var layer1 = new NeuroneLayer();
            var layer2 = new NeuroneLayer();

            //neuron1.ConnectTo(layer1);
            //layer1.ConnectTo(layer2);
            #endregion



            Main1 observer = new Main1();
            observer.Main11();

            Console.ReadKey();
        }
    }
}
