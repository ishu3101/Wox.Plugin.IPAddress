using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace Wox.Plugin.IPAddress
{
    public class Program : IPlugin
    {
        public void Init(PluginInitContext context) { }
        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();

            String hostname = Dns.GetHostName();

            // Get the Local IP Address
            String IP = Dns.GetHostByName(hostname).AddressList[0].ToString();

            // Get the External IP Address
            String externalip = new WebClient().DownloadString("http://ipecho.net/plain");

            String icon = "ipaddress.png";

            results.Add(Item(IP, "Local IP Address ", icon, Action(IP)));
            results.Add(Item(externalip, "External IP Address ", icon, Action(externalip)));

            return results;
        }
        // relative path to your plugin directory
        private static Result Item(String title, String subtitle, String icon, Func<ActionContext, bool> action)
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
        private static Func<ActionContext, bool> Action(String text)
        {
            return e =>
            {
                CopyToClipboard(text);

                // return false to tell Wox don't hide query window, otherwise Wox will hide it automatically
                return false;
            };
        }

        public static void CopyToClipboard(String text)
        {
            Clipboard.SetText(text);
        }
    }
}
