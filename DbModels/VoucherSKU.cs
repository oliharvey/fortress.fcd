

namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("voucher_VoucherSKU")]
    public partial class VoucherSKU
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string SKU { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public bool Disabled { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public int PartnerId { get; set; }

        public int VoucherTypeId { get; set; }

        public bool Numeric { get; set; }

        public int? NumericLength { get; set; }

        public int MembershipDays { get; set; }

        public int PricingModelId { get; set; }

        public bool MultiUse { get; set; }

        public int? MaxActivations { get; set; }

        public bool TestFlag { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public int? PaymentSourceId { get; set; }

        public bool? TransferToBasic { get; set; }

        public int? NumberFreeDays { get; set; }

        [StringLength(50)]
        public string OfferCode { get; set; }

        public DateTime? OfferExpiryDate { get; set; }

        public Decimal? SellPrice { get; set; }
        public Decimal? RetailPrice  { get; set; }

        public bool PPFlag { get; set; }
        public bool AdvanceInvoicing { get; set; }

        public virtual VoucherType VoucherType { get; set; }
        public virtual PricingModel PricingModel { get; set; }

    }

}
