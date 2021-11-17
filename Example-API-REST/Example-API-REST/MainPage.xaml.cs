using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace Example_API_REST
{
    public partial class MainPage : ContentPage
    {
        //API JTW EndPoint: http://platzi-markets.herokuapp.com/platzi-market/api/auth/authenticate
        /*{
            "username" : "diego",
            "password" : "platzi"
        }
         */
        //API URL
        private readonly string Url = "https://platzi-markets.herokuapp.com/platzi-market/api/products/all";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(Url);
            //GET Method
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            //Set JWT Token 
            var authHeader = new AuthenticationHeaderValue("Bearer",
                "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJEaWVnbyIsImlhdCI6MTYzNzE3MDQyMywiZXhwIjoxNjM3MjA2NDIzfQ.9ancBZJxGAckoi2VzKSw_UiNpOw0VcOPs-NEVzP8CLE");
            client.DefaultRequestHeaders.Authorization = authHeader;
            //Send petition
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK) //200 response 
            {
                await DisplayAlert("¡Success, Products Recovery!", response.StatusCode.ToString(), "Show Products");
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<Root>(content); //Root model content others class as headear,category and body
                //Set body in list view through model.body cast in list
                ListDemo.ItemsSource = model.body.ToList();
            }
            else
            {   
                await DisplayAlert("Error", response.StatusCode.ToString(), "OK");
            }
                
        }
    }

    public class Headers
    {
    }

    public class Category
    {
        public int categoryId { get; set; }
        public string category { get; set; }
        public bool active { get; set; }
    }

    public class Body
    {
        public int productId { get; set; }
        public string name { get; set; }
        public int categoryId { get; set; }
        public double price { get; set; }
        public int stock { get; set; }
        public bool active { get; set; }
        public Category category { get; set; }
    }

    public class Root
    {
        public Headers headers { get; set; }
        public List<Body> body { get; set; }
        public string statusCode { get; set; }
        public int statusCodeValue { get; set; }
    }

}
