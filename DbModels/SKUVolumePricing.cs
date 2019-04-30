

namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("voucher_SKUVolumePricing")]
    public partial class SKUVolumePricing
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string RawSKU { get; set; }

        public int FromQty { get; set; }
        public int ToQty { get; set; }

        public string ReportSKU { get; set; }

        public Decimal? SellPrice { get; set; }
    }

}
