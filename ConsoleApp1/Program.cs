using System;
using System.Net;
using System.Threading;

class Program
{
    public class WebRes
    {
        public string SiteName { get; set; }
        public string SiteBody { get; set; }
    }

    static readonly HttpClient client = new HttpClient();

    static List<string> sites = new()
        {
            "https://www.google.com/",
            "https://www.microsoft.com/",
            "https://www.apple.com/",
            "https://www.samsung.com/",
            "https://www.youtube.com/",
            "https://www.cnn.com/",
            "https://www.yahoo.com/"
        };

    static async Task<WebRes> DownloadWebsite(string webAddr)
    {
        WebRes webRes = new WebRes();

        webRes.SiteName = webAddr;
        webRes.SiteBody = await client.GetStringAsync(webAddr);

        return webRes;
    }

    public static void GetSynch()
    {
        foreach (var site in sites)
        {
            WebRes res = DownloadWebsite(site).Result;

            Console.WriteLine($"{res.SiteName} - length: {res.SiteBody.Length}");
        }
    }

    public static async Task GetAsynch()
    {
        List<Task<WebRes>> listTasks = new List<Task<WebRes>>();

        foreach (var site in sites)
        {
            listTasks.Add(DownloadWebsite(site));
        }

        var results = await Task.WhenAll(listTasks);

        foreach (var result in results)
        {
            Console.WriteLine($"{result.SiteName} - length: {result.SiteBody.Length}");
        }
    }

    static async Task Main()
    {

        while (true)
        {
            Console.WriteLine("1 = Sync, 2 = Async, exit = quit");
            string input = Console.ReadLine();
            if (input == "1")
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                GetSynch();
                watch.Stop();
                Console.WriteLine($"Total time : {watch.ElapsedMilliseconds}");
            }
            else if (input == "2")
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                await GetAsynch();
                watch.Stop();
                Console.WriteLine($"Total time : {watch.ElapsedMilliseconds}");
            }
            else if (input == "exit")
            {
                break;
            }
        }
    }
}
