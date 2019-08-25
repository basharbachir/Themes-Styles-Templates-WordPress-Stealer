using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
namespace ThemeStealer
{
   public static class Program
    {
        public static string PureUrl { get; set; }
        public static string Result { get; set; }
        public static string ThemeBasicUrl { get; set; }
        public static string ThemeName { get; set; }
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Credits goes to : Mr.Ali ");
            Console.WriteLine("Author,Coded By => Bashar Bachir");
            if (args[0].Contains("BasharBachir"))
            {
                var fl = File.ReadAllLines(args[0]);
                var strb = new StringBuilder();
                for (int i = 1; i < fl.Length; i++)
                {
                    var matches2 = Regex.Matches(fl[i], @"(?<=http://)(.+?)(?=/)");
                    foreach (var m in matches2)
                    {
                        strb.Append("http://" + m + " " + m + Environment.NewLine).Replace("www.", "").Replace("\"","");
                    }
                }
                File.WriteAllText("ProtrctionTips.txt", strb.ToString());
            }
            var goodUrls = new StringBuilder();
            var ListF = File.ReadAllLines("ProtrctionTips.txt");
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            if (!Directory.Exists(Urls))
            {
                Directory.CreateDirectory(Urls);
            }
            for (int i = 0; i < ListF.Length; i++)
            {
                var x = ListF[i].Split(' ')[0];
                var xx = ListF[i].Split(' ')[1];
                if (!x.EndsWith("/"))
                {
                    PureUrl = x + "/";
                    if (x.Contains("https"))
                    {
                        var replace = PureUrl.Replace("https", "http");
                    }
                }
                else
                {
                    if (x.Contains("https"))
                    {
                        PureUrl = x.Replace("https", "http");
                        if (!x.EndsWith("/"))
                        {
                            PureUrl = x + "/";
                        }
                    }
                }

                if (x.EndsWith("/") && !x.Contains("https"))
                {
                    PureUrl = x;
                }

                try
                {
                    var task = Task.Run(async () => await Microsoft(PureUrl.Replace("https", "http").Replace(" ", "").Replace("\"", "")));
                    Result = task.Result;
                }
                catch 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{PureUrl.Replace("https", "http").Replace(" ", "").Replace("\"", "")} is Down");
                    Console.ForegroundColor = ConsoleColor.White;
                    goto Down;
                }

                var matches = Regex.Matches(Result, @"(?<=wp-content/themes/)(.+?)(?=/)");
                foreach (var m in matches)
                {
                    ThemeName = m.ToString();
                    break;
                }
                var matches2 = Regex.Matches(Result, @"(?<=://)(.+?)(?=/)");
                foreach (var m in matches2)
                {
                    if (!m.ToString().Contains(xx)) continue;
                    ThemeBasicUrl = m.ToString();
                    break;
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Target Url Generating ..... ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                FinalUrl = $"http://{ThemeBasicUrl}/wp-content/themes/{ThemeName}.zip".Replace(" ", "").Replace("\"", "");
                CharUrl = FinalUrl.ToCharArray();
                Console.WriteLine(FinalUrl);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Scanning .......");
                var request = (HttpWebRequest)WebRequest.Create(FinalUrl);
                request.Method = "HEAD";
                try
                {
                    Response = (HttpWebResponse)request.GetResponse();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"Scanning for .zip Extension ..... ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Found .. Downloading Now");
                    if (File.Exists($"{DirName}/{ThemeName}.zip"))
                    {

                        File.WriteAllBytes($"{DirName}/{ThemeName +I}.zip", Microsoft(CharUrl).Result);
                        I++;
                    }
                    else File.WriteAllBytes($"{DirName}/{ThemeName}.zip", Microsoft(CharUrl).Result);
                    Console.WriteLine($"The File {ThemeName}.zip has been Downloaded To {AppDomain.CurrentDomain.BaseDirectory+ DirName}\\{ThemeName}.zip");
                    goodUrls.Append(new string(CharUrl)+Environment.NewLine);
                    File.AppendAllText($"{Urls}/GoodUrls.txt", goodUrls.ToString());

                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Theme With Zip Extension Found");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"Scanning for .rar Extension ..... ");
                   
                    try
                    {
                        if (File.Exists($"{DirName}/{ThemeName}.zip".Substring($"{ThemeName}.zip".Length - 3) + "rar"))
                        {

                            File.WriteAllBytes($"{DirName}/{ThemeName}.zip".Substring($"{ThemeName+I}.zip".Length - 3) + "rar", Microsoft(CharUrl).Result);
                            I++;
                        }
                        else File.WriteAllBytes($"{DirName}/{ThemeName}.zip".Substring($"{ThemeName}.zip".Length - 3) + "rar", Microsoft(CharUrl).Result);

                        Thread.Sleep(1000);
                        Console.WriteLine($"The File {ThemeName}.rar has been Downloaded To {AppDomain.CurrentDomain.BaseDirectory + DirName}\\{ThemeName}.rar ");
                        goodUrls.Append(new string(CharUrl)+Environment.NewLine);
                        File.AppendAllText($"{Urls}/GoodUrls.txt", goodUrls.ToString());


                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No Theme With Rar Extension Found");
                    }
                }
                finally
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Response?.Close();
                }
                Down: ;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            File.AppendAllText( $"{Urls}/GoodUrls.txt", goodUrls.ToString());
            Console.WriteLine("The Job is Done .. Check Themes and GoodUrls Folders");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Credits goes to : Mr.Ali ");
            Console.WriteLine("Author,Coded By => Bashar Bachir");

        }

        public static int I { get; set; }

        public static string Urls = "GoodUrls";

        public static string DirName = "Themes";
        public static HttpWebResponse Response { get; set; }
        public static char[] CharUrl { get; set; }
        public static string FinalUrl { get; set; }
        public static async Task<string> Microsoft(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync(url); return result;
            }
        }
        public static async Task<byte[]> Microsoft(char[] url)
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetByteArrayAsync(new string(url)); return result;
            }
        }
    }
}