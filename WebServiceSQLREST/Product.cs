using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;


namespace WebServiceSQLREST
{
    class Product
    {
        private int _id;
        private string _name;
        private double _price;
        static private string _url = "http://www.thomas-bayer.com/sqlrest/PRODUCT/";        

        public int ID
        {
            get { return _id; }
            private set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public double Price
        {
            get { return _price; }
            private set { _price = value; }
        }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }

        private static List<int> GetProductList()
        {
            List<int> productIDs = new List<int>();
            WebRequest request = WebRequest.Create(_url);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseFromServer);
            XmlNodeList products = xmlDoc.SelectNodes("//PRODUCT");
            foreach (XmlNode node in products)
                productIDs.Add(Int32.Parse(node.InnerText));
            dataStream.Close();
            reader.Close();
            response.Close();
            return productIDs;
        }

        public static void CreateProduct(Product product)
        {
            int newID = 0;
            Random rnd = new Random();
            foreach (int id in GetProductList())
            {
                newID = rnd.Next(int.MaxValue);
                if (newID != id)
                    break;
            }
            product.ID = newID;
            WebRequest request = WebRequest.Create(_url);
            request.Method = "POST";
            string postData = "<resource>" +
                "<ID>" + product.ID.ToString() + "</ID>" +
                "<NAME>" + product.Name + "</NAME>" +
                "<PRICE>" + product.Price.ToString().Replace(',', '.') + "</PRICE>" +
                "</resource>";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
           WebResponse response = request.GetResponse();
           dataStream = response.GetResponseStream();
           StreamReader reader = new StreamReader(dataStream);
           string responseFromServer = reader.ReadToEnd();            
        }

        public void DeleteProduct()
        {
            WebRequest request = WebRequest.Create(_url + ID);
            request.Method = "DELETE";
            WebResponse response = request.GetResponse();
        }

        public bool ProductExist()
        {
            foreach(int id in GetProductList())
            {
                if (id == ID)
                    return true;
            }
            return false;
        }
    }
}
