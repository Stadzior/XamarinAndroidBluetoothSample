using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothClient
{
    [Activity(Label = "BluetoothClient", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            TextView input = FindViewById<TextView>(Resource.Id.InputStream);

            button.Click += delegate 
            {
                var uuid = Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                try
                {
                    var serverSocket = BluetoothAdapter.DefaultAdapter.ListenUsingRfcommWithServiceRecord("SerialPort", uuid);
                    var socket = serverSocket.Accept();
                    serverSocket.Close();

                    var timerTask = Task.Factory.StartNew(() => Task.Delay(10000));
                    button.Text = $"Connected{socket.RemoteDevice.Name}";
                    while (timerTask.IsCompleted)
                    {
                        var receivedValue = (char)socket.InputStream.ReadByte();
                        if (receivedValue > 0)
                            input.Text += receivedValue;
                    };
                }
                catch (Exception ex)
                {
                    var hue = ex;
                }
            };
        }

    }
}

