using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;

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
            var externalip = new WebClient().DownloadString("http://ipecho.net/plain");

            const string icon = "ipaddress.png";

            // Get the Local IP Address
            foreach (var ip in Dns.GetHostEntry(hostname).AddressList)
            {

                results.Add(Result(ip.ToString(), ip.AddressFamily.ToString(), icon, Action(ip.ToString())));
            }

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
