

namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("tbl_PreloadedDevices")]
    public partial class tbl_PreloadedDevice
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string DeviceMake { get; set; }
        [Required]
        [StringLength(50)]
        public string DeviceModel { get; set; }
        [Required]
        [StringLength(50)]
        public string Imei { get; set; }
        [Required]
        public string VoucherID { get; set; }
        public bool Activated { get; set; }
        public DateTime? ActivatedDate { get; set; }

    }
}
