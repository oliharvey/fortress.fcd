namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VoucherMetadata")]
    public partial class VoucherMetadata
    {
        public int Id { get; set; }

        public int? PricingModelID { get; set; }

        public int? VoucherID { get; set; }

        public int? VoucherTypeID { get; set; }
        public int? FreeDays { get; set; }
        public int? PaymentSource { get; set; }
        public int? TransferToBasic { get; set; }

        public virtual PricingModel PricingModel { get; set; }

        public virtual Voucher Voucher { get; set; }

        public virtual VoucherType VoucherType { get; set; }

        public Boolean? TestAccount { get; set; } 
    }
}
