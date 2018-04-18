

namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbl_VoucherRegistration")]
    public partial class tbl_VoucherRegistration
    {
        public int Id { get; set; }

        [Required]
        public int VoucherID { get; set; }

        [StringLength(200)]
        //[Required]
        public string FirstName { get; set; }

        [StringLength(200)]
        //[Required]
        public string Surname { get; set; }

        [StringLength(200)]
        //[Required]
        public string ZipCode { get; set; }

        [StringLength(200)]
        //[Required]
        public string EmailAddress { get; set; }

        [StringLength(10)]
        [Required]
        public string CountryISO { get; set; }

        [StringLength(100)]
        public string DeviceMake { get; set; }

        [StringLength(100)]
        public string DeviceModel { get; set; }

        [StringLength(20)]
        public string DeviceCapacity { get; set; }

        [StringLength(50)]
        public string Imei { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
