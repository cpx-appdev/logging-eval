using System;
using System.Collections.Generic;
using System.Text;

namespace Logging.ConsoleApp
{
    public class ProcessedInvoice
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public ICollection<Order> Orders { get; set; }
        public DateTimeOffset ProcessedDate { get; set; }
    }

    public class Order
    {
        public long Id { get; set; }
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTimeOffset InvoiceEndDate { get; set; }
        public string CurrencyCode { get; set; }
        public string AuthorizationId { get; set; }
        internal string ValidationFailuresJson { get; set; }

    }
}
