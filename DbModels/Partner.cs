namespace FortressCodesDomain.DbModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Partner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Partner()
        {
            DeviceLevels = new HashSet<DeviceLevel>();
            PricingModels = new HashSet<PricingModel>();
            Tiers = new HashSet<Tier>();
        }

        [Key]
        public int userid { get; set; }

        [Required]
        [StringLength(50)]
        public string partnername { get; set; }

        public int? CountryID { get; set; }

        public bool? Protected { get; set; }

        public bool? BillingAllowed { get; set; }

        public bool? AnnualBillingAllowed { get; set; }

        public bool? MonthlyBillingAllowed { get; set; }

        public bool? AceAllowed { get; set; }

        public int? API_DefaultVoucherFamily { get; set; }

        public string API_DES_EncryptionKey { get; set; }

        public string API_DES_IV { get; set; }

        public string SecretKey { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeviceLevel> DeviceLevels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PricingModel> PricingModels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tier> Tiers { get; set; }
    }
}
