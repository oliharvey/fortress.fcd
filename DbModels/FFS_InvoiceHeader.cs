namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FFS_InvoiceHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FFS_InvoiceHeader()
        {
            FFS_InvoiceLine = new HashSet<FFS_InvoiceLine>();
        }

        [Key]
        public int InvoiceId { get; set; }

        public int PartnerId { get; set; }

        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public bool AdvanceInvoice { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        public bool Active { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FFS_InvoiceLine> FFS_InvoiceLine { get; set; }
    }
}
