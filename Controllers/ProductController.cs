using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models;
using Newtonsoft.Json.Linq;

namespace webapi.Controllers
{
    public class ProductController : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();
        [ActionName("aaa")]
        [HttpPost]
        public IEnumerable<Product> GetProducts()
        {
            return repository.GetAll();
        }
        [HttpPost]
        public Product GetProduct(JObject postData)
        {
            
            Product item = repository.Get(int.Parse(postData["id"].ToString()));
            if(item==null)
            {
                throw new HttpRequestException(HttpStatusCode.NotFound.ToString());

            }
            return item;
        }
        public IEnumerable<Product> GetProductByCatergory(string category)
        {
            return repository.GetAll().Where(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }
        public HttpResponseMessage PostProduct(string name,string category,decimal price)
        {
            Product item = new Product();
            item.Name = name;
            item.Category = category;
            item.Price = price;
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);
            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        public void PutProduct(int id,Product item)
        {
            item.Id = id;
            if(!repository.Update(item))
            {
                throw new HttpRequestException(HttpStatusCode.NotFound.ToString());
            }
        }
        public HttpResponseMessage DeleteProduct(int id)
        {
            repository.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
