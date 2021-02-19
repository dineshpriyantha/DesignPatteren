using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatteren
{

    class Salary
    {
        List<Observer> observers = new List<Observer>();

        public void Attach(Observer ob)
        {
            observers.Add(ob);
        }

        private int val;

        public int Val
        {
            set
            {
                val = value;
                NotifyAllObservers();
            }
            get
            {
                return this.val;
            }
        }

        private void NotifyAllObservers()
        {
            foreach (Observer ob in observers)
            {
                ob.Update();
            }
        }
    }

    abstract class Observer
    {
        protected Salary sal;

        public abstract void Update();
    }


    class ManagerBonus : Observer
    {
        public ManagerBonus(Salary sal)
        {
            this.sal = sal;
            this.sal.Attach(this);
        }
        public override void Update()
        {
            Console.WriteLine("Manager Bonus is " + sal.Val * 3);
        }
    }

    class EmployeeBonus : Observer
    {
        public EmployeeBonus(Salary sal)
        {
            this.sal = sal;
            this.sal.Attach(this);
        }
        public override void Update()
        {
            Console.WriteLine("Employee Bonus is " + sal.Val * 2);
        }
    }

    public class Main1
    {
        public void Main11()
        {
            Salary salary = new Salary();

            new ManagerBonus(salary);
            new EmployeeBonus(salary);

            salary.Val = 1000;
            salary.Val = 2000;

            Console.ReadLine();
        }
    }
}
