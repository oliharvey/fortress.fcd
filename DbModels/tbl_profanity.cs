namespace FortressCodesDomain.DbModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;   


    [Table("voucher_tbl_profanity")]
    public partial class tbl_Profanity
    {


        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string RegionCode { get; set; }

        [Required]
        [StringLength(500)]
        public string Profanity { get; set; }
    }
}
