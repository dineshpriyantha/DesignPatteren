using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

/// <summary>
/// Bridge Design Pattern
/// </summary>
namespace DesignPatteren
{
    public interface ILEDTv
    {
        void SwitchOn();
        void SwitchOff();

        void SetChannel(int channelNumber);
    }


    // Implementation part of SamsungTV
    public class SamsungLedTv : ILEDTv
    {
        public void SetChannel(int channelNumber)
        {
            WriteLine($"Setting channel number : {channelNumber} on samsung Tv");
        }

        public void SwitchOff()
        {
            WriteLine("Switch off : Samsung Tv");
        }

        public void SwitchOn()
        {
            WriteLine("Switch on : Samsing Tv");
        }
    }

    // Implementation part of Sony tv
    public class SonyLEDTv : ILEDTv
    {
        public void SetChannel(int channelNumber)
        {
            WriteLine($"Setting channel number : {channelNumber} on Sony TV");
        }

        public void SwitchOff()
        {
            WriteLine("Switch off : Sony TV");
        }

        public void SwitchOn()
        {
            WriteLine("Switch ON : Sony Tv");
        }
    }


    public class HitachiLEDTv : ILEDTv
    {
        public void SetChannel(int channelNumber)
        {
            WriteLine($"Setting channel number : {channelNumber} on Hitachi Tv");
        }

        public void SwitchOff()
        {
            WriteLine($"Switch off : Hitachi TV");
        }

        public void SwitchOn()
        {
            WriteLine($"Switch on : Hitachi Tv");
        }
    }

    // Creating Abstract Remote Control

    public abstract class AbstractRemoteControl
    {
        protected ILEDTv ledTv;
        public AbstractRemoteControl(ILEDTv ledTv)
        {
            this.ledTv = ledTv;
        }

        public abstract void SwitchOn();
        public abstract void SwitchOff();
        public abstract void SetChannel(int channelNumber);
    }

    // create concrete remote control

    public class SamsungRemoteControl : AbstractRemoteControl
    {
        public SamsungRemoteControl(ILEDTv ledTv) : base(ledTv)
        {
        }

        public override void SetChannel(int channelNumber)
        {
            ledTv.SetChannel(channelNumber);
        }

        public override void SwitchOff()
        {
            ledTv.SwitchOff();
        }

        public override void SwitchOn()
        {
            ledTv.SwitchOn();
        }
    }


    public class SonyRemoteControl : AbstractRemoteControl
    {
        public SonyRemoteControl(ILEDTv ledTv) : base(ledTv)
        {
        }

        public override void SetChannel(int channelNumber)
        {
            ledTv.SetChannel(channelNumber);
        }

        public override void SwitchOff()
        {
            ledTv.SwitchOff();
        }

        public override void SwitchOn()
        {
            ledTv.SwitchOn();
        }
    }


    public class HitachiRemoteControl : AbstractRemoteControl
    {
        public HitachiRemoteControl(ILEDTv ledTv) : base(ledTv)
        {
        }

        public override void SetChannel(int channelNumber)
        {
            ledTv.SetChannel(channelNumber);
        }

        public override void SwitchOff()
        {
            ledTv.SwitchOff();
        }

        public override void SwitchOn()
        {
            ledTv.SwitchOn();
        }
    }

    class BridgeMain
    {
        static void BridgeMainMethod()
        {
            SonyRemoteControl sonyRemote = new SonyRemoteControl(new SonyLEDTv());
            sonyRemote.SwitchOn();
            sonyRemote.SetChannel(1);
            sonyRemote.SwitchOff();


            SamsungRemoteControl samsunRemote = new SamsungRemoteControl(new SamsungLedTv());
            samsunRemote.SwitchOn();
            samsunRemote.SetChannel(3);
            samsunRemote.SwitchOff();

        }

    }
}
