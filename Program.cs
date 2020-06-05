using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using TAPWin;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System.Threading;
using System.Data.Common;
using System.IO.Ports;
using System.Diagnostics;

namespace TapVirtualXBox
{
    class Program
    {

        static void Main(string[] args)
        {
            Messenger middleman = new Messenger();
            middleman.Activate();
            Console.ReadLine();
        }

    }
    class Messenger {
        static ViGEmClient gamer = new ViGEmClient();
        IXbox360Controller gamepad = gamer.CreateXbox360Controller();
        public void Activate()   {
            TAPManager.Instance.OnTapConnected += InitializeXbox;
            TAPManager.Instance.SetTapInputMode(TAPInputMode.ControllerWithMouseHID());
            TAPManager.Instance.OnAirGestured += AirInput;
            TAPManager.Instance.OnTapped += OnTapped;
            TAPManager.Instance.OnTapDisconnected += OnTapDisconnected;
            // TAPManager.Instance.OnMoused += OnMoused;
            TAPManager.Instance.Start();
            //  = 
            gamepad.Connect();
        }
        
        //private void OnMoused(string identifier, int vx, int vy, bool isMouse)
        //{
        //    Console.WriteLine(identifier + " moves " + vx.ToString() + "." + vy.ToString());
        //}

        private void OnTapDisconnected(string identifier)
        {
            Console.WriteLine(identifier + " was disconnected.");
        }

        private void OnTapped(string identifier, int TapCode)
        {
            Console.WriteLine(identifier + " tapped " + TapCode.ToString());
            TAPManager.Instance.Vibrate(new int[] { 50, 100, 50 });
        }

        void InitializeXbox(string identifier, string name, int fw)
        {
            Console.WriteLine("Tap " + identifier + " connected!!!!!!");
        }

        bool MoveToggle;
        bool Consumed;
        static Stopwatch stopwatch = new Stopwatch();
        TimeSpan stopwatchElapsed = stopwatch.Elapsed;


        void AirInput(string identifier, TAPAirGesture airGesture)
        {
            Consumed = false;
            while (MoveToggle == true)
            {
                if ((int)airGesture == 2)
                {
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbY, 32767);
                    MoveToggle = false;
                    Consumed = true;
                }
               if ((int)airGesture == 4)
                {
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbY, -32768);
                    MoveToggle = false;
                    Consumed = true;
                }
                if ((int)airGesture == 6)
                {
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbX, -32768);
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                    MoveToggle = false;
                    Consumed = true; 
                }
                if ((int)airGesture == 8)
                {
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbX, 32767);
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                    MoveToggle = false;
                    Consumed = true; 
                }

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
                    gamepad.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                    MoveToggle = false;
                    Consumed = false;
                    stopwatch.Reset();
                }
                Thread.Sleep(50);
                
            }
            if (Consumed == false)
            {
                // OneFingerUp
                if ((int)airGesture == 2)
                {
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbX, 0);
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbY, 32767);
                    gamepad.SetButtonState(Xbox360Button.RightShoulder, true);
                    Thread.Sleep(200);
                    gamepad.SetButtonState(Xbox360Button.RightShoulder, false);
                }
                // OneFingerDown
                if ((int)airGesture == 4)
                {
                    MoveToggle = true;
                    TAPManager.Instance.Vibrate(new int[] { 150, 250, 50, 550, 200 });
                    stopwatch.Reset();
                    stopwatch.Start();
                    Console.WriteLine("MoveToggle invoked");
                }
                // OneFingerLeft
                if ((int)airGesture == 6)
                {
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbX, -32768);
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbY, 0);
                    gamepad.SetButtonState(Xbox360Button.RightShoulder, true);
                    Thread.Sleep(200);
                    gamepad.SetButtonState(Xbox360Button.RightShoulder, false);
                }
                // OneFingerRight
                if ((int)airGesture == 8)
                {
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbX, 32767);
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbY, 0);
                    gamepad.SetButtonState(Xbox360Button.RightShoulder, true);
                    Thread.Sleep(200);
                    gamepad.SetButtonState(Xbox360Button.RightShoulder, false);
                }

                // TwoFingersUp
                if ((int)airGesture == 3)
                {
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbX, 0);
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbY, 32767);
                    gamepad.SetSliderValue(Xbox360Slider.RightTrigger, 255);
                    Thread.Sleep(200);
                    gamepad.SetSliderValue(Xbox360Slider.RightTrigger, 0);
                }
                // TwoFingersDown
                if ((int)airGesture == 5)
                {
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbX, 0);
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbY, 0);
                    gamepad.SetButtonState(Xbox360Button.B, true);
                    Thread.Sleep(200);
                    gamepad.SetButtonState(Xbox360Button.B, false);
                }
                // TwoFingersLeft
                if ((int)airGesture == 7)
                {
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbX, -32768);
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbY, 0);
                    gamepad.SetSliderValue(Xbox360Slider.RightTrigger, 255);
                    Thread.Sleep(200);
                    gamepad.SetSliderValue(Xbox360Slider.RightTrigger, 0);
                }
                // TwoFingersRight
                if ((int)airGesture == 9)
                {
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbX, 32767);
                    gamepad.SetAxisValue(Xbox360Axis.RightThumbY, 0);
                    gamepad.SetSliderValue(Xbox360Slider.RightTrigger, 255);
                    Thread.Sleep(200);
                    gamepad.SetSliderValue(Xbox360Slider.RightTrigger, 0);
                }
                // 
            }
            
            // if ((int)airGesture == -1000)
            // {

            // }
            Console.WriteLine("AirGesture registered!");

            // impelement an algorithm to set delay
            // by setting consumed to true a certain time after initialization
        }


    }


}
