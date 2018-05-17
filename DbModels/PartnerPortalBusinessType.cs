namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartnerPortalBusinessType")]
    public partial class PartnerPortalBusinessType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartnerPortalBusinessType()
        {
            PartnerPortalBusinessEntities = new HashSet<PartnerPortalBusinessEntity>();
            PartnerPortalBusinessType1 = new HashSet<PartnerPortalBusinessType>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string BusinessType { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public string Notes { get; set; }

        public int? ParentBusinessTypeId { get; set; }

        public int? MaxVouchers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartnerPortalBusinessEntity> PartnerPortalBusinessEntities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartnerPortalBusinessType> PartnerPortalBusinessType1 { get; set; }

        public virtual PartnerPortalBusinessType PartnerPortalBusinessType2 { get; set; }
    }
}
