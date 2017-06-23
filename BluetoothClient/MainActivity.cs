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
                //BluetoothAdapter.DefaultAdapter.StartDiscovery();
                var uuid = Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                //var uuid = Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                //var pcDevice = BluetoothAdapter.DefaultAdapter.BondedDevices.Single(d => d.Name.Equals("ASUS USB-BT400"));
                //var socket = pcDevice.CreateRfcommSocketToServiceRecord(uuid);
                try
                {
                    var serverSocket = BluetoothAdapter.DefaultAdapter.ListenUsingRfcommWithServiceRecord("SerialPort", uuid);
                    var socket = serverSocket.Accept();
                    serverSocket.Close();
                    button.Text = $"Connected{socket.RemoteDevice.Name}";
                    while (true)
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
                //var msg = Encoding.UTF8.GetBytes("huehuehue");
                //socket.OutputStream.Write(msg, 0, msg.Length);



                //var serverSocket = BluetoothAdapter.DefaultAdapter.ListenUsingRfcommWithServiceRecord("", uuid);
                //var socket = serverSocket.Accept();
                //button.Text = $"Connected{socket.RemoteDevice.Name}";
                //serverSocket.Close();

            };
        }

    }
}

