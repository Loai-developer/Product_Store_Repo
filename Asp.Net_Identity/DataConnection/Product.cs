namespace DataConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Photo { get; set; }
        
        public decimal Price { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
