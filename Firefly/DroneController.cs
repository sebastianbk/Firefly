using AR.Drone.Client;
using AR.Drone.Client.Command;
using AR.Drone.Data.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly
{
    class DroneController
    {
        public class DroneCommandChangedEventArgs
        {
            public string CommandText { get; set; }
        }

        public delegate void DroneCommandChangedDelegate(object sender, DroneCommandChangedEventArgs args);

        public event DroneCommandChangedDelegate DroneCommandChanged;

        private DroneClient _client;

        public DroneController()
        {
            _client = new DroneClient("192.168.1.1");
        }

        public DroneController(DroneClient client)
        {
            _client = client;
        }

        public void Start()
        {
            _client.Start();
            _client.FlatTrim();
        }

        public void Stop()
        {
            _client.Stop();
        }

        public void Hover()
        {
            _client.Hover();
        }

        public void Emergency()
        {
            _client.Emergency();
        }

        public void ResetEmergency()
        {
            _client.ResetEmergency();
        }

        public void SubscribeToGestures()
        {
            // Right Hand
            GestureDetection.RightHandUpDownChanged += GestureDetection_RightHandUpDownChanged;
            GestureDetection.RightHandLeftRightChanged += GestureDetection_RightHandLeftRightChanged;
            GestureDetection.RightHandBackForwardsChanged += GestureDetection_RightHandBackForwardsChanged;

            // Left Hand
            GestureDetection.LeftHandUpDownChanged += GestureDetection_LeftHandUpDownChanged;
            GestureDetection.LeftHandLeftRightChanged += GestureDetection_LeftHandLeftRightChanged;
            GestureDetection.LeftHandBackForwardsChanged += GestureDetection_LeftHandBackForwardsChanged;
        }

        void GestureDetection_LeftHandBackForwardsChanged(object sender, HandPositionChangedArgs args)
        {
            switch (args.Position)
            {
                case HandPosition.Center:
                    break;
                case HandPosition.Backwards:
                    if (_client.NavigationData.State == (NavigationState.Landed | NavigationState.Command))
                        _client.FlatTrim();
                        DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Flat Trim " });
                    break;
                case HandPosition.Forwards:
                    _client.Hover();
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Hover" });
                    break;
            }
        }

        void GestureDetection_RightHandBackForwardsChanged(object sender, HandPositionChangedArgs args)
        {
            switch (args.Position)
            {
                case HandPosition.Center:
                    break;
                case HandPosition.Backwards:
                    _client.Progress(FlightMode.Progressive, pitch: 0.05f);
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Moving Backwards" });
                    break;
                case HandPosition.Forwards:
                    _client.Progress(FlightMode.Progressive, pitch: -0.05f);
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Moving Forwards" });
                    break;
            }
        }

        void GestureDetection_RightHandLeftRightChanged(object sender, HandPositionChangedArgs args)
        {
            switch (args.Position)
            {
                case HandPosition.Center:
                    break;
                case HandPosition.Left:
                    _client.Progress(FlightMode.Progressive, roll: -0.05f);
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Rolling to the Left" });
                    break;
                case HandPosition.Right:
                    _client.Progress(FlightMode.Progressive, roll: 0.05f);
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Rolling to the Right" });
                    break;
            }
        }

        void GestureDetection_LeftHandLeftRightChanged(object sender, HandPositionChangedArgs args)
        {
            switch (args.Position)
            {
                case HandPosition.Center:
                    break;
                case HandPosition.Left:
                    _client.Progress(FlightMode.Progressive, yaw: 0.25f);
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Turning Left" });
                    break;
                case HandPosition.Right:
                    _client.Progress(FlightMode.Progressive, yaw: -0.25f);
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Turning Right" });
                    break;
            }
        }

        void GestureDetection_RightHandUpDownChanged(object sender, HandPositionChangedArgs args)
        {
            switch (args.Position)
            {
                case HandPosition.Up:
                    _client.Progress(FlightMode.Progressive, gaz: 0.25f);
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Going Up" });
                    break;
                case HandPosition.Center:
                    break;
                case HandPosition.Down:
                    _client.Progress(FlightMode.Progressive, gaz: -0.25f);
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Going Down" });
                    break;
            }
        }

        void GestureDetection_LeftHandUpDownChanged(object sender, HandPositionChangedArgs args)
        {
            switch (args.Position)
            {
                case HandPosition.Up:
                    _client.Takeoff();
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Taking Off" });
                    break;
                case HandPosition.Center:
                    break;
                case HandPosition.Down:
                    _client.Land();
                    DroneCommandChanged(_client, new DroneCommandChangedEventArgs { CommandText = "Landing" });
                    break;
            }
        }
    }
}
