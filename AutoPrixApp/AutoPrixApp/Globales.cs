
using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AutoPrixApp
{
    public class Globales
    {
        static string url = ConfigurationManager.AppSettings["Api"];
        //String conection = ConfigurationManager.AppSettings["config.text"];
        static HttpClient client = new HttpClient();

        public static StringBuilder getApiToJson(string ApiName, string parametro)
        {
            StringBuilder json = new StringBuilder("");
            if (parametro != string.Empty) ApiName += "?" + parametro;
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJhZTIwZTY2ZS03YjdhLTRlMTQtOWJlZS00YmEyYzk5YjdkN2QiLCJuYW1laWQiOiJleUpoYkdjaU9pSklVekkxTmlJc0luUjVjQ0k2SWtwWFZDSjkiLCJUSUVOREEiOiIuUm90X05NVWU4RGxZSnFnVkpkeThwaVlrZ3FjT2xyRF9JZkMxUlR3azdNayIsIkNMQVZFIjoiLmV5SnFkR2tpT2lJelptTTVNMkk1TUMweFlqTmlMVFF3TkdNdE9UYzVZaTFqT1ROaE1EbGhNV013TURZaUxDSnVZVzFsYVdRaU9pSk5WVVZDVEVWVElFVk1JRUpQVTFGVlJTSXNJbTV2YldKeVpTSTZJbEJQVWtOQlRVSkpRVklpTENKaGNHVnNiR2xrYjNNaU9pSk5SVUlpTENKbGJXRnBiQ0k2SWsxRlFrQklUMVJOUVVsTUxrTlBUU0lzSW01aVppSTZNVFU1T0RrM01UVTNOQ3dpWlhod0lqb3hOak13TlRBM05UYzBMQ0pwYzNNaU9pSjNkM2N1YlhWbFlteGxjMlZzWW05emNYVmxMbU52YlNJc0ltRjFaQ0k2SW5kM2R5NXRkV1ZpYkdWelpXeGliM054ZFdVdVkyOXRMMkZ3YVM5TWIyZHBiaUo5IiwibmJmIjoxNTk4OTgwMDA5LCJleHAiOjE2MzA1MTYwMDksImlzcyI6Ind3dy5tdWVibGVzZWxib3NxdWUuY29tIiwiYXVkIjoid3d3Lm11ZWJsZXNlbGJvc3F1ZS5jb20vYXBpL0xvZ2luIn0.EpZbJ3-IqNo2QqNGCtmZAXg-LkhzfMprA49D5hznx9s";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + ApiName);
            request.Headers.Add("Authorization", "Bearer " + token);
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                json.Append(reader.ReadToEnd());
                //Console.WriteLine("jsonm: "+json.ToString());
                //var agenda = JsonConvert.DeserializeObject<RespuestaDisponibilidad>(result);
                //CrearControles(agenda);

            }
            return json;
        }

        public static StringBuilder ConsumirApiGet(string ApiName, string parametro)
        {
            //Intenta enviar al servidor el jSon 
            //string url = Properties.Settings.Default.apiserver;
            StringBuilder json = new StringBuilder("");
            url = url + ApiName;
            if (parametro != string.Empty) url += "?" + parametro;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json;charset=utf-8'";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //jsonResult jsonres;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                json.Append(streamReader.ReadToEnd());
               // jsonres = JsonConvert.DeserializeObject<jsonResult>(result);
            }
            return json;
        }

        /*CONSUMO DE API VIA MOVIL*/
        /**
         * Autor: Janhundia
         * Fecha: 27/12/2020
         * consumo por Get **/
        public static async Task<StringBuilder> GetApiApp(string ApiName, string parametro)
        {
            HttpClient cliente = new HttpClient();
            var request = new HttpRequestMessage();
            StringBuilder json = new StringBuilder("");
            try
            {
                if (parametro != string.Empty) ApiName += "?" + parametro;
                request.RequestUri = new Uri(url + ApiName);
                request.Headers.Add("Accept", "aplication/json");
                request.Method = HttpMethod.Get;
                HttpResponseMessage response = await cliente.SendAsync(request);

                Console.WriteLine("status "+response.StatusCode);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ///jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(content.ToString());
                    json.Append(content.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return json;
        }

        public static async Task<StringBuilder> GetApiAppRoute(string ApiName, string parametro)
        {
            HttpClient cliente = new HttpClient();
            var request = new HttpRequestMessage();
            StringBuilder json = new StringBuilder("");
            try
            {
                if (parametro != string.Empty) ApiName += "/" + parametro;
                request.RequestUri = new Uri(url + ApiName);
                request.Headers.Add("Accept", "aplication/json");
                request.Method = HttpMethod.Get;
                HttpResponseMessage response = await cliente.SendAsync(request);

                Console.WriteLine("status " + response.StatusCode);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ///jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(content.ToString());
                    json.Append(content.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return json;
        }
        /**
         * Autor: Janhundia
         * Fecha: 27/12/2020
         * consumo por Post **/
        public static async Task<StringBuilder> PostApiApp(string ApiName, string parametro)
        {
            HttpClient cliente = new HttpClient();
            var request = new HttpRequestMessage();
            StringBuilder json = new StringBuilder("");
            try
            {
                //if (parametro != string.Empty) ApiName += "?" + parametro;
                request.RequestUri = new Uri(url+ApiName);
                //request.Headers.Add("Accept", "aplication/json");
                request.Method = HttpMethod.Post;
                var stringContent = new StringContent(parametro, Encoding.UTF8, "application/json");
                request.Content = stringContent;
                HttpResponseMessage response = await cliente.SendAsync(request);

                Console.WriteLine("status " + response.StatusCode);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ///jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(content.ToString());
                    json.Append(content.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return json;
        }

    }
          
}