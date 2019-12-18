using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Models.Products
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Maximum number of characters (255) exceeded")]
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        [Required]
        public bool Discontinued { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }
    }
}
