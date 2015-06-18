using Sonos.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sonos.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SonosClient sonosClient;
                string ip = ConfigurationSettings.AppSettings["SONOS_IP"];
                int port;

                if (Int32.TryParse(ConfigurationSettings.AppSettings["SONOS_PORT"], out port))
                {
                    sonosClient = new SonosClient(ip, port);
                }
                else
                {
                    sonosClient = new SonosClient(ip);
                }

                var deviceDescription = sonosClient.GetDeviceDescription().Result;

                var playing = sonosClient.IsPlaying().Result;
                var volume = sonosClient.GetVolume().Result;

                var result = sonosClient.Play().Result;

                Thread.Sleep(5000);

                result = sonosClient.Next().Result;

                Thread.Sleep(5000);

                result = sonosClient.Previous().Result;

                Thread.Sleep(5000);

                result = sonosClient.SetVolume(0).Result;

                Thread.Sleep(5000);

                result = sonosClient.SetVolume(25).Result;

                Thread.Sleep(5000);

                result = sonosClient.Pause().Result;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
