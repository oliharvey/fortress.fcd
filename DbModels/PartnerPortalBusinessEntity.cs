namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartnerPortalBusinessEntity")]
    public partial class PartnerPortalBusinessEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartnerPortalBusinessEntity()
        {
            PartnerPortalBusinessEntityChildren = new HashSet<PartnerPortalBusinessEntity>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string BusinessName { get; set; }

        public int BusinessTypeId { get; set; }

        [StringLength(5)]
        public string Country { get; set; }

        public int? ParentBusinessId { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Logo { get; set; }

        [StringLength(250)]
        public string AddressLine1 { get; set; }

        [StringLength(250)]
        public string AddressLine2 { get; set; }

        [StringLength(250)]
        public string AddressLine3 { get; set; }

        [StringLength(250)]
        public string AddressLine4 { get; set; }

        [StringLength(50)]
        public string Postcode { get; set; }

        public int? PrimaryContactId { get; set; }

        public bool PartnerFlag { get; set; }

        public bool HasStripeAccount { get; set; }

        [StringLength(2000)]
        public string StripeDetails { get; set; }

        public bool HasWallet { get; set; }

        public int PartnerId { get; set; }

        public bool Disabled { get; set; }

        public int? MaxVouchers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartnerPortalBusinessEntity> PartnerPortalBusinessEntityChildren { get; set; }

        public virtual PartnerPortalBusinessEntity PartnerPortalBusinessEntityParent { get; set; }

        public virtual PartnerPortalBusinessType PartnerPortalBusinessType { get; set; }
    }
}
