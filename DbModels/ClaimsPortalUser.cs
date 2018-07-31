
namespace FortressCodesDomain.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LoginUser")]
    public class ClaimsPortalUser
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public bool? Active { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string HandlerName { get; set; }

        [Obsolete]
        public string Password { get; set; }

        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }

        public int? Roles { get; set; }

        [StringLength(5)]
        public string Country { get; set; }

        [StringLength(5)]
        public string Language { get; set; }

    }
}
