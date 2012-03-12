using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace ConsoleGoogleApplication
{
    class Program
    {
        private class Token
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }

        }

        static void Main(string[] args)
        {

            


            var url =
                string.Format(
                    "https://accounts.google.com/o/oauth2/auth?scope={0}&redirect_uri={1}&client_id={2}&response_type=code",
                    "https://www.googleapis.com/auth/tasks.readonly+https://www.googleapis.com/auth/tasks",
                    "urn:ietf:wg:oauth:2.0:oob",
                    "86232087946.apps.googleusercontent.com");

            var process = Process.Start(url);
            
            Thread.Sleep(5000);

            string code = null;

            try
            {
                code = process.MainWindowTitle.Substring(13, 30);
            }
            catch{}
            
            if (code == null)
                code = Process.GetProcesses()
                    .Where(p => p.MainWindowTitle.Contains("Success code="))
                    .Select(p =>
                            p.MainWindowTitle.Substring(13, 30))
                    .FirstOrDefault();

            
            if (code == null)
                code = Console.ReadLine();

            var client = new HttpClient();

            var key = "_ijwpkmtqGC6f_4p1MC4Pu3d";

            var dic = new Dictionary<string, string>();
            dic.Add("code", code);
            dic.Add("client_id", "86232087946.apps.googleusercontent.com");
            dic.Add("client_secret", key);
            dic.Add("redirect_uri", "urn:ietf:wg:oauth:2.0:oob");
            dic.Add("grant_type", "authorization_code");

            var content = new FormUrlEncodedContent(dic);
            var response = client.PostAsync("https://accounts.google.com/o/oauth2/token", content);

            var body = response.Result.Content.ReadAsStringAsync();

            var jsonMaster = new JavaScriptSerializer();
            var token = jsonMaster.Deserialize<Token>(body.Result);

            Console.WriteLine("access_token={0}",token.access_token);
            Console.ReadLine();
        }


    }
}
