using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

//https://dashboard.elering.ee/assets/api-doc.html#/balance-controller/getAllUsingGET

namespace InternetiAndmepyydja
{
    public class CIAP:IIAP
    {
        class Program
        {
            static async Task Main(string[] args)
            {
                Console.WriteLine("mine putsi \n");
                using (var httpClient = new HttpClient())
                {
                    var url = "https://dashboard.elering.ee/api/balance?start=2020-06-30T10%3A59%3A59.999Z&end=2020-06-30T20%3A00%3A00.999Z";

                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    var responseString = await httpClient.GetStringAsync(url);



                    Console.WriteLine(responseString);
                }
                Console.ReadKey();

            }
        }
    }
}
