using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;
using RestRequest = RestSharp.Serializers.Newtonsoft.Json.RestRequest;

namespace Northwind.Client.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = GetNorthwindApi();

            var categoriesRequest = new RestRequest("categories", RestSharp.Method.GET)
            {
                JsonSerializer = new NewtonsoftJsonSerializer(new Newtonsoft.Json.JsonSerializer()
                    {Formatting = Newtonsoft.Json.Formatting.Indented})
            };

            categoriesRequest.AddQueryParameter("count", "5");

            var productsRequest = new RestRequest("products", RestSharp.Method.GET)
            {
                JsonSerializer = new NewtonsoftJsonSerializer(new Newtonsoft.Json.JsonSerializer()
                    {Formatting = Newtonsoft.Json.Formatting.Indented})
            };

            productsRequest.AddQueryParameter("count", "10");

            api.ExecuteAsync(categoriesRequest, response =>
                {
                    Console.WriteLine(">>>>>>>>>>>>>>>CATEGORIES<<<<<<<<<<<<<<<");
                    Console.WriteLine(response.Content);
                });

            api.ExecuteAsync(productsRequest, response =>
                {
                    Console.WriteLine(">>>>>>>>>>>>>>>>PRODUCTS<<<<<<<<<<<<<<<");
                    Console.WriteLine(response.Content);
                });

            Console.ReadLine();
        }

        private static IRestClient GetNorthwindApi()
        {
            var productsApi = new RestClient("https://localhost:44355/api");

            return productsApi;
        }
    }
}
