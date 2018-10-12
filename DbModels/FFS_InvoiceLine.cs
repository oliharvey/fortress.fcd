namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FFS_InvoiceLine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FFS_InvoiceLine()
        {
            FFS_InvoiceLineBatch = new HashSet<FFS_InvoiceLineBatch>();
        }

        [Key]
        public int InvoiceLineId { get; set; }

        public int InvoiceId { get; set; }

        [Required]
        [StringLength(20)]
        public string SKU { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal LineTotal { get; set; }

        public virtual FFS_InvoiceHeader FFS_InvoiceHeader { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FFS_InvoiceLineBatch> FFS_InvoiceLineBatch { get; set; }
    }
}
