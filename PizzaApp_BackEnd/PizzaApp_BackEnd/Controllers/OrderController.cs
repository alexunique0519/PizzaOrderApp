using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PizzaApp_BackEnd.Models;
using System.IO;

namespace PizzaApp_BackEnd.Controllers
{
    //string static sPath = "~/Exported_Orders";

    [RoutePrefix("api/pizzaOrder")]
    public class OrderController : ApiController
    {
        Dictionary<string, decimal> taxDictionary = null;

        private OrderController() 
        {
            InitTaxDictionary();
        }

        private void InitTaxDictionary()
        {
            taxDictionary = new Dictionary<string, decimal>();
            taxDictionary.Add("Ontario", 0.13M);
            taxDictionary.Add("Manitoba", 0.08M);
            taxDictionary.Add("Quebec", 0.05M);
            taxDictionary.Add("Saskatchewan", 0.05M);
        }
        
        private decimal CalculateTotalPrice(OrderModel order, out decimal subTotal)
        {
            decimal price = 0M;

            //depand on the size
            price += GetPizzaPrice(order.pizzaSize);

            //add the cost of toppings
            price += GetToppingsPrice(order.toppings.Count());

            //add the cost of crust type
            price += 2;
            subTotal = price;

            //add tax
            price = price * (1 + taxDictionary[order.province]);

           
            return price;
        }
        
        
        [AllowAnonymous]
        [Route("order")]
        public HttpResponseMessage Order(PizzaApp_BackEnd.Models.OrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, you submitted an invalid order.");
            }

            OrderedResult or = new OrderedResult();
            try
            {
                decimal subTotal;
                decimal totalPrice = CalculateTotalPrice(order, out subTotal);

                OrderItem[] items = CollectOrderItems(order);

               
                or.message = "We received your order. Thank your for choosing us.";
                or.orderedItems = items;
                or.subTotal = subTotal.ToString("C2");
                or.totalCost = totalPrice.ToString("C2");

                string[] stringToWrite = GenerateOrderStringArray(order);

                WriteOrderInfoToFile(stringToWrite);
            
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, or);    
            }
           

            return Request.CreateResponse(HttpStatusCode.OK, or);
        }

        private string[] GenerateOrderStringArray(OrderModel order)
        {
            List<string> list = new List<string>();

            string sDateTime = DateTime.Now.ToString();
            list.Add(sDateTime);

            string sCustomerName = "Customer Name : " + order.customerName;
            list.Add(sCustomerName);

            string sPostalCode = "Postal Code: " + order.postalCode;
            list.Add(sPostalCode);

            string sProvince = "Province :" + order.province;
            list.Add(sProvince);

            string sCity = "City: " + order.city;
            list.Add(sCity);

            string sPhoneNumber = "Phone Number :" + order.telephoneNumber;
            list.Add(sPhoneNumber);

            string sEmail = "Email :" + order.email;
            list.Add(sEmail);

            string sPizzaSize = "Pizza Siza: " + order.pizzaSize;
            list.Add(sPizzaSize);

            string sToppings = "Toppings: ";
            foreach(var t in order.toppings)
            {
                sToppings += t;
                sToppings += ",";
            }

            list.Add(sToppings);

            string sCrustType = "Crust Type: " + order.crustType;
            list.Add(sCrustType);


            return list.ToArray();
        }
  
        private OrderItem[] CollectOrderItems(OrderModel order)
        {
            OrderItem[] orderItems = new OrderItem[3];

            try
            {
                OrderItem item1 = new OrderItem();
                item1.itemName = order.pizzaSize + " pizza";
                item1.quantity = 1;
                item1.cost = GetPizzaPrice(order.pizzaSize).ToString("C2");
                orderItems[0] = item1;

                OrderItem item2 = new OrderItem();
                item2.itemName = "Toppings";
                item2.quantity =  order.toppings.Count();
                item2.cost =  GetToppingsPrice(order.toppings.Count()).ToString("C2");
                orderItems[1] = item2;

                OrderItem item3 = new OrderItem();
                item3.itemName = "Stuffed Crust";
                item3.quantity = 1;
                item3.cost = "$2.00";
                orderItems[2] = item3;
            }
            catch (Exception ex)
            {
                string s = ex.InnerException.Message;
            }
           
            return orderItems;
        }

        private decimal GetPizzaPrice(string size)
        {
            decimal price = 0M;

            switch (size)
            {
                case "small":
                    price += 5M;
                    break;
                case "medium":
                    price += 10M;
                    break;
                case "large":
                    price += 15M;
                    break;
                case "X-large":
                    price += 20M;
                    break;
                default:
                    break;
            }

            return price;
        }

        private decimal GetToppingsPrice(int toppingsCount)
        {
            decimal price = 0M;

            if (toppingsCount > 1)
            {
                price += (toppingsCount - 1) * 0.5M;
            }

            return price;
        }

        private void WriteOrderInfoToFile(string[] orderInfo)
        {
            string path = Path.GetDirectoryName(
                     System.Reflection.Assembly.GetAssembly(typeof(OrderController)).CodeBase);

            int nlen = path.Length;

            int leftPos = path.IndexOf("\\");
            path = path.Remove(0, leftPos + 1);

            int nPos = path.LastIndexOf("\\");
            path = path.Remove(nPos + 1);

            string sOrderFolder = "Exported_Orders";
            path += sOrderFolder;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string FileName = path + "\\order.txt";

            if (!File.Exists(FileName))
            {
                File.WriteAllLines(FileName, orderInfo);
            }
            else
            {
                foreach (var text in orderInfo)
                {
                    File.AppendAllText(FileName, text + Environment.NewLine);
                }
               
            }
        }
    }
}
