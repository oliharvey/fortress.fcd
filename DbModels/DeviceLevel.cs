namespace FortressCodesDomain.DbModels
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DeviceLevel")]
    public partial class DeviceLevel
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public int LevelId { get; set; }

        public int? PartnerId { get; set; }

        public bool? Protected { get; set; }

        public decimal? DeviceValueUSD { get; set; }

        public decimal? DeviceValueGBP { get; set; }

        public decimal? DeviceValueEUR { get; set; }

        public virtual Device Device { get; set; }

        public virtual Partner Partner { get; set; }

        public virtual Level Level { get; set; }
    }
}
