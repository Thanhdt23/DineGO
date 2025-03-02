using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using DineGO_Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Client.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly HttpClient client = null;
        private string RestaurantApiUrl = "";


        public RestaurantController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            RestaurantApiUrl = "https://localhost:5001/api/Restaurant";

        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(RestaurantApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API request failed: {response.StatusCode}");
            }

            string strData = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw API Response: " + strData);

            if (string.IsNullOrWhiteSpace(strData) || (!strData.TrimStart().StartsWith("[") && !strData.TrimStart().StartsWith("{")))
            {
                throw new Exception("Invalid JSON format: " + strData);
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Restaurant> listRestaurants = JsonSerializer.Deserialize<List<Restaurant>>(strData, options);

            return View(listRestaurants);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            HttpResponseMessage res = await client.GetAsync($"{RestaurantApiUrl}/{id}");

            if (!res.IsSuccessStatusCode) return NotFound();

            string strData = await res.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Restaurant restaurant = JsonSerializer.Deserialize<Restaurant>(strData, options);

            return View(restaurant);
        }


        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Restaurant p)
        {
            if (ModelState.IsValid)
            {
                string data = JsonSerializer.Serialize(p);
                var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage reponse = client.PostAsync(RestaurantApiUrl, content).Result;
                if (reponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(p);

        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            HttpResponseMessage res = client.GetAsync($"{RestaurantApiUrl}/id?ID={id}").Result;
            string strData = res.Content.ReadAsStringAsync().Result;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Restaurant p = JsonSerializer.Deserialize<Restaurant>(strData, options);


            return View(p);

        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Restaurant p)
        {
            try
            {
                string data = JsonSerializer.Serialize(p);
                var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync(RestaurantApiUrl, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction("Edit", p.res_id);
            }
            catch (Exception e)
            {
                return View(p);
            }

        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            HttpResponseMessage res = client.GetAsync($"{RestaurantApiUrl}/id?ID={id}").Result;
            string strData = res.Content.ReadAsStringAsync().Result;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Restaurant p = JsonSerializer.Deserialize<Restaurant>(strData, options);


            return View(p);

        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync($"{RestaurantApiUrl}/?Id={id}").Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

    }
}