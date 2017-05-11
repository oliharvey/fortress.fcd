namespace FortressCodesDomain.DbModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Tier")]
    public partial class Tier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tier()
        {
            PricingModels = new HashSet<PricingModel>();
            Vouchers = new HashSet<Voucher>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? PartnerId { get; set; }

        public int? TierLevel { get; set; }

        public bool? Protected { get; set; }

        public virtual Partner Partner { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PricingModel> PricingModels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
