using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GoogleTasksService.Controllers
{
    public class GoogleController : Controller
    {
        //
        // GET: /Google/

        public ActionResult Tasks()
        {
            var url =
                string.Format(
                    "https://accounts.google.com/o/oauth2/auth?scope={0}&redirect_uri={1}&client_id={2}&response_type=code",
                    "https://www.googleapis.com/auth/tasks.readonly+https://www.googleapis.com/auth/tasks",
                    "http://localhost:5555/Google/TasksCode",
                    "78820519907.apps.googleusercontent.com");

            return Redirect(url);
        }
        
        public ActionResult TasksCode(string code)
        {
            var  client = new HttpClient();

            var key = "6Rk9J9IHANM8uLuz5fOCjtTC";

            var dic = new Dictionary<string, string>();
            dic.Add("code",code);
            dic.Add("client_id", "78820519907.apps.googleusercontent.com");
            dic.Add("client_secret", "6Rk9J9IHANM8uLuz5fOCjtTC");
            dic.Add("redirect_uri", "http://localhost:5555/Google/TasksCode");
            dic.Add("grant_type", "authorization_code");

            var content = new FormUrlEncodedContent(dic);
            var response = client.PostAsync("https://accounts.google.com/o/oauth2/token", content);

            var body = response.Result.Content.ReadAsStringAsync();

            var jsonMaster = new JavaScriptSerializer();
            var token = jsonMaster.Deserialize<Token>(body.Result);

            var googleReq = string.Format("https://www.googleapis.com/tasks/v1/users/@me/lists",
                                          token.access_token);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(token.token_type, token.access_token);

            var tasksJson = client.GetAsync(googleReq);

            var tasklist = JsonValue.Parse(tasksJson.Result.Content.ReadAsStringAsync().Result).AsDynamic();

            var id = (string) tasklist.items[0].id;

            googleReq = string.Format("https://www.googleapis.com/tasks/v1/lists/{0}/tasks", id);
            var list = JsonValue.Parse(client.GetAsync(googleReq).Result.Content.ReadAsStringAsync().Result);



            return View();
        }

        public string Catchtoken(string access_token)
        {
            var  client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", access_token);

            var googleReq = string.Format("https://www.googleapis.com/tasks/v1/users/@me/lists");

            var tasksJson = client.GetAsync(googleReq);

            var tasklist = JsonValue.Parse(tasksJson.Result.Content.ReadAsStringAsync().Result).AsDynamic();

            return tasklist.ToString();
        }

        public ActionResult TasksImplicitExample()
        {
            var url =
                string.Format(
                    "https://accounts.google.com/o/oauth2/auth?scope={0}&redirect_uri={1}&client_id={2}&response_type=token&approval_prompt=force",
                    "https://www.googleapis.com/auth/tasks.readonly+https://www.googleapis.com/auth/tasks",
                    "http://localhost:5555/Google/TasksImplicitResult",
                    "78820519907.apps.googleusercontent.com");

            return Redirect(url);
        }

        public ActionResult TasksImplicitResult()
        {
            return View();
        }


        private class Token
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
            
        }
    }
}
