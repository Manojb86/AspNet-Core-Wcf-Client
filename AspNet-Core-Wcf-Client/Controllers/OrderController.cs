using Microsoft.AspNetCore.Mvc;
using OrderService;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNet_Core_Wcf_Client.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderServiceClient orderServiceClient;

        public OrderController()
        {
            orderServiceClient = new OrderServiceClient();
        }
        public async Task<IActionResult> Index()
        {
            List<Order> orders = await orderServiceClient.GetOrdersAsync();

            return View(orders);
        }

        public async Task<IActionResult> ShowOrdersByType()
        {
            List<Order> orders = null;
            try
            {
                orders = await orderServiceClient.GetOrdersByTypeAsync("");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> ShowOrdersCountByRest()
        {
            ViewBag.EndPointUri = "http://localhost:64307/OrderService.svc/api/orders/count";
            ViewBag.RestMethod = "GET";
            
            HttpClient httpClient = new HttpClient();
            var uri = new Uri("http://localhost:64307/OrderService.svc/api/orders/count");
           
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(result))
                {
                    ViewBag.OrdersCount = result;
                }
            }
            else
            {
                ViewBag.ErrorMessage = response.StatusCode;
            }

            return View();
        }
    }
}