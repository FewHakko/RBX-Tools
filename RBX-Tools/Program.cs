using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RBX_Tools
{
    //Fewhakko No1
    class Program
    {

        private static string api_url = "https://groups.roblox.com/v1/groups/";

        private static string proxyfile = "proxy.txt";

        private static int found = 0;

        private static int fail = 0;

        private static int ranfew = 0;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("TYPE SELECT");
            Console.WriteLine("============");
            Console.WriteLine("[1] FindGroup");
            Console.WriteLine("[2] Clothing-Copy");
            Console.WriteLine("[3] Random-Clothing-Copy");
            Console.WriteLine("============");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Choice (1,2,3) => ");
            string choice = Console.ReadLine();
            Console.Clear();
            if (choice == "1")
            {
                Console.Title = $"FindGroup-Roblox | Found : {found.ToString()} | Fail : {fail.ToString()}";
                Console.Write("THREAD => ");
                int thread = int.Parse(Console.ReadLine());
                Console.WriteLine("Please Wait...");
                try
                {
                    for (int i = 1; i <= thread; i++)
                    {
                        Thread threadstart = new Thread(new ThreadStart(FindGroup));
                        threadstart.Start();
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error");
                    Console.ReadKey();
                }
            }
            else if (choice == "2")
            {
                Console.Write("Clothing-Id => ");
                string clothingid = Console.ReadLine();
                ClothingShirt(clothingid);
            }
            else if (choice == "3")
            {
                Console.Title = $"GenClothing | Page: 0 | Success: {found.ToString()}";
                Console.Write("PAGELIMIT => ");
                int pagelimit = int.Parse(Console.ReadLine());
                Console.Clear();
                Console.WriteLine("TYPE SELECT");
                Console.WriteLine("============");
                Console.WriteLine("[1] Shirts");
                Console.WriteLine("[2] T-Shirts");
                Console.WriteLine("[3] Pants");
                Console.WriteLine("============");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Choice (1,2,3) => ");
                string choice3 = Console.ReadLine();
                Console.Clear();
                if (choice3 == "1")
                {
                    ClothingGen(12, pagelimit);
                }
                else if (choice3 == "2")
                {
                    ClothingGen(13, pagelimit);

                }
                else if (choice3 == "3")
                {
                    ClothingGen(14, pagelimit);

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fail - Choice");
                }
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fail - Choice");
                Console.ReadKey();
            }
        }

        private static string RandomProxy()
        {
            var lines = File.ReadAllLines(Program.proxyfile);
            var r = new Random();
            var randomLineNumber = r.Next(0, lines.Length - 1);
            var line = lines[randomLineNumber];
            return line;
        }

        private static void ClothingGen(int type, int page)
        {
            for (int i = 1; i <= page; i++)
            {
                Console.Title = $"GenClothing | Page: {i.ToString()} | Success: {found.ToString()}";
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                Console.WriteLine("----------------------");
                Console.WriteLine("SYSTEM | GEN CLOTHING PAGE: " + i.ToString());
                Console.WriteLine("----------------------");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                WebClient wc = new WebClient();
                string data = wc.DownloadString("https://search.roblox.com/catalog/json?Category=3&SortType=2&Subcategory=" + type + "&AggregationFrequency=3&PageNumber=" + i + "&CatalogContext=1");
                dynamic dynJson = JsonConvert.DeserializeObject(data);
                foreach (var item in dynJson)
                {
                    try
                    {
                        wc.DownloadFile("http://assetgame.roblox.com/Asset/?id=" + item.AssetId, "dl");

                        string fileContents = File.ReadAllText("dl");

                        Regex regex = new Regex(@"<url>(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})</url>");
                        Match match = regex.Match(fileContents);
                        string urldata = match.Value.ToString().Replace("<url>", null).Replace("</url>", null);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        try
                        {
                            if (fileContents.Contains("ShirtGraphic"))
                            {
                                Console.WriteLine("Detected Asset Type: T-Shirt");
                                wc.DownloadFile(urldata, "Clothing/T-Shirt/" + item.Name + ".png");
                            }
                            else if (fileContents.Contains("Pants"))
                            {
                                Console.WriteLine("Detected Asset Type: Pants");
                                wc.DownloadFile(urldata, "Clothing/Pants/" + item.Name + ".png");
                            }
                            else if (fileContents.Contains("Shirt"))
                            {
                                Console.WriteLine("Detected Asset Type: Shirt");
                                wc.DownloadFile(urldata, "Clothing/Shirt/" + item.Name + ".png");
                            }
                            else
                            {
                                Console.WriteLine("Detected Asset Type: Unknown");
                            }
                            Console.WriteLine("=====================");
                        }
                        catch
                        {
                            if (fileContents.Contains("ShirtGraphic"))
                            {
                                wc.DownloadFile(urldata, "Clothing/T-Shirt/" + item.AssetId + ".png");
                            }
                            else if (fileContents.Contains("Pants"))
                            {
                                wc.DownloadFile(urldata, "Clothing/Pants/" + item.AssetId + ".png");
                            }
                            else if (fileContents.Contains("Shirt"))
                            {
                                wc.DownloadFile(urldata, "Clothing/Shirt/" + item.AssetId + ".png");
                            }
                            else
                            {
                                Console.WriteLine("Detected Asset Type: Unknown");
                            }
                            Console.WriteLine("=====================");
                        }
                        found++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} | IDGROUP => {item.AssetId} - Name => {item.Name}");
                        Console.Title = $"GenClothing | Page: {i.ToString()} | Success: {found.ToString()}";
                    }
                    catch
                    {

                    }
                }
            }
        }

        private static void ClothingShirt(string id)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadFile("http://assetgame.roblox.com/Asset/?id=" + id, "dl");

                string fileContents = File.ReadAllText("dl");

                Regex regex = new Regex(@"<url>(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})</url>");
                Match match = regex.Match(fileContents);
                string urldata = match.Value.ToString().Replace("<url>", null).Replace("</url>", null);

                if (fileContents.Contains("ShirtGraphic"))
                {
                    Console.WriteLine("Detected Asset Type: T-Shirt");
                    wc.DownloadFile(urldata, "Clothing/T-Shirt/" + DateTime.Now.ToString().Replace(":", "-") + ".png");
                }
                else if (fileContents.Contains("Pants"))
                {
                    Console.WriteLine("Detected Asset Type: Pants");
                    wc.DownloadFile(urldata, "Clothing/Pants/" + DateTime.Now.ToString().Replace(":", "-") + ".png");
                }
                else if (fileContents.Contains("Shirt"))
                {
                    Console.WriteLine("Detected Asset Type: Shirt");
                    wc.DownloadFile(urldata, "Clothing/Shirt/" + DateTime.Now.ToString().Replace(":", "-") + ".png");
                }
                else
                {
                    Console.WriteLine("Detected Asset Type: Unknown");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Copy Clothing Success");
                Console.ReadKey();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR ID Clothing");
                Console.ReadKey();
            }
        }

        private static void FindGroup()
        {
            while (true)
            {
                try
                {
                    ranfew++;
                    WebClient wc = new WebClient();
                    Random random = new Random();
                    WebProxy proxy = new WebProxy(RandomProxy());
                    wc.Proxy = proxy;
                    string web = wc.DownloadString($"{api_url}{random.Next(100000, 1999999) + ranfew + random.Next(1, 9)}");
                    Example result = JsonConvert.DeserializeObject<Example>(web);

                    if (!web.Contains("buildersClubMembershipType"))
                    {
                        if (result.publicEntryAllowed == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            string date = DateTime.Now.ToLongTimeString();

                            Console.WriteLine($"{date} | IDGROUP => {result.id} - NAMEGROUP => {result.name}");
                            using (StreamWriter streamWriter = File.AppendText("hit.txt"))
                            {
                                streamWriter.WriteLine("https://www.roblox.com/groups/" + result.id);
                            }

                            found++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            string date = DateTime.Now.ToLongTimeString();
                            Console.WriteLine($"{date} | IDGROUP => {result.id} - NAMEGROUP => {result.name}");
                            fail++;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        string date = DateTime.Now.ToLongTimeString();
                        Console.WriteLine($"{date} | IDGROUP => {result.id} - NAMEGROUP => {result.name}");
                        fail++;
                    }
                }
                catch
                {
                }
                Console.Title = $"FindGroup | Found : {found.ToString()} | Fail : {fail.ToString()}";
            }
        }
    }
}
