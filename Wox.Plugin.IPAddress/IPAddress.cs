using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace Wox.Plugin.IPAddress
{
    public class Program : IPlugin
    {
        public void Init(PluginInitContext context) { }
        public List<Result> Query(Query query)
        {
            var results = new List<Result>();

            var hostname = Dns.GetHostName();

            // Get the External IP Address


            const string icon = "ipaddress.png";
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                    || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    //Console.WriteLine(ni.Name);
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            //Console.WriteLine(ip.Address.ToString());
                            results.Add(
                                Result(ip.Address.ToString(),
                                ni.Name,
                                icon, Action(ip.Address.ToString())));
                        }
                    }
                }
            }


            var externalip = new WebClient().DownloadString("http://ipecho.net/plain");
            results.Add(Result(externalip, "External IP Address ", icon, Action(externalip)));

            return results;
        }
        // relative path to your plugin directory
        private static Result Result(string title, string subtitle, string icon, Func<ActionContext, bool> action)
        {
            return new Result()
            {
                Title = title,
                SubTitle = subtitle,
                IcoPath = icon,
                Action = action
            };
        }

        // The Action method is called after the user selects the item
        private static Func<ActionContext, bool> Action(string text)
        {
            return e =>
            {
                CopyToClipboard(text);

                // return false to tell Wox don't hide query window, otherwise Wox will hide it automatically
                return false;
            };
        }

        public static void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
