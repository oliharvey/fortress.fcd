namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FFS_InvoiceLineBatch
    {
        [Key]
        public int InvoiceBatchId { get; set; }

        public int InvoiceLineId { get; set; }

        public int BatchNumber { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual FFS_InvoiceLine FFS_InvoiceLine { get; set; }
    }
}
