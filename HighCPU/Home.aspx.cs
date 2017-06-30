using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HighCPU
{
    public partial class Home : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Regexoperations()
        {
            string uri = "https://blogs.msdn.microsoft.com/";
            //Get request
            HttpWebRequest Getrequest;
            string html = string.Empty;
            HttpWebResponse response;
            try
            {
                Getrequest = (HttpWebRequest)WebRequest.Create(uri);
                Getrequest.AutomaticDecompression = DecompressionMethods.GZip;
                html = string.Empty;
                using (response = (HttpWebResponse)Getrequest.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Get request, exception : " + ex.ToString());
            }


            Regex regIp = new Regex(@"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\." +
                                    @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4]" +
                                    @"[0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}" +
                                    @"[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])\:[0-9]{1,5}",
                                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

            Regex slowRegex = new Regex("([a - z] +) *=");
            MatchCollection matchCol;
            Match m;


            int percentage = Convert.ToInt32(TextBox1.Text);
            TextBox1.Text = "done....";
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                (new Thread(() =>
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    while (true)
                    {
                        // Make the loop go on for "percentage" milliseconds then sleep the 
                        // remaining percentage milliseconds. So 40% utilization means work 40ms and sleep 60ms
                        if (watch.ElapsedMilliseconds > percentage)
                        {
                            Thread.Sleep(100 - percentage);
                            watch.Reset();
                            watch.Start();
                            matchCol = regIp.Matches(html);
                            m = slowRegex.Match(html);
                        }
                    }
                })).Start();

            }

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Regexoperations();
        }


    }
}