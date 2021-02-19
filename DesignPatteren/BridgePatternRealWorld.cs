using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DesignPatteren
{
    public interface IMessageSender
    {
        void SendMessage(string message);
    }

    // Implementation part
    public class SmsMessageSender : IMessageSender
    {
        public void SendMessage(string message)
        {
            WriteLine($"{message} this message send by SMS");
        }
    }

    public class EmailMessageSender : IMessageSender
    {
        public void SendMessage(string message)
        {
            WriteLine($"{message} this message send by Email ");
        }
    }

    public abstract class AbstractMessage
    {
        protected IMessageSender messageSender;
        public AbstractMessage(IMessageSender messageSender)
        {
            this.messageSender = messageSender;
        }
        public abstract void SendMessage(string message);
    }


    public class LongMessage : AbstractMessage
    {
        public LongMessage(IMessageSender messageSender) : base(messageSender)
        {
        }

        public override void SendMessage(string message)
        {
            messageSender.SendMessage(message);
        }
    }

    public class ShortMessage : AbstractMessage
    {
        public ShortMessage(IMessageSender messageSender) : base(messageSender)
        {
        }

        public override void SendMessage(string message)
        {
            if (message.Length < 10)
            {
                messageSender.SendMessage(message);
            }
            else
            {
                WriteLine("Unable to send message , length size > 10");
            }

        }
    }

    class BridgePatternRealWorldMain
    {
        static void BridgePatternRealWorldMainMethod()
        {
            WriteLine("Please enter 2 or 1");
            int messageType = Convert.ToInt32(Console.ReadLine());

            WriteLine("Please write a message that you want send");
            string message = Console.ReadLine();

            // short message
            if (messageType == 1)
            {
                AbstractMessage longMessage = new LongMessage(new SmsMessageSender());
                longMessage.SendMessage(message);
            }
            else
            {
                AbstractMessage shortMessage = new ShortMessage(new EmailMessageSender());
                shortMessage.SendMessage(message);
            }


        }
    }
}
