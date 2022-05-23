using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    class Product
    {
        public int id { get; set; }
        public string name{ get; set; }
        public float cost { get; set; }

        public Product() { }
        public Product(int id, string name, float cost)
        {
            this.id = id;
            this.name = name;
            this.cost = cost;
        }
    }
}
