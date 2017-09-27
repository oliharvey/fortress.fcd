namespace FortressCodesDomain.DbModels
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    [Table("tbl_DeviceValueAudit")]
    public partial class tbl_DeviceValueAudit
    {
        public int ID { get; set; }

        public int DeviceID { get; set; }

        public int? LevelID { get; set; }

        public int? PartnerId { get; set; }

        public decimal? DeviceValueUSDOld { get; set; }

        public decimal? DeviceValueUSDNew { get; set; }

        public decimal? DeviceValueGBPOld { get; set; }

        public decimal? DeviceValueGBPNew { get; set; }

        public decimal? DeviceValueEUROld { get; set; }

        public decimal? DeviceValueEURNew { get; set; }

        public DateTime DateModified { get; set; }

        public int? UserID { get; set; }

    }
}
