using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.Models;

public partial class Product
{
    public string ProductCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int OnHandQuantity { get; set; }

    public virtual ICollection<Invoicelineitem> Invoicelineitems { get; set; } = new List<Invoicelineitem>();
}
