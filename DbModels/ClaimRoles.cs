
namespace FortressCodesDomain.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ClaimRoles")]
    public class ClaimRoles
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Permission { get; set; }
        public int BinaryValue { get; set; }
    }
}
