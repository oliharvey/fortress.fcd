namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("voucher_Status")]
    public partial class Status
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
