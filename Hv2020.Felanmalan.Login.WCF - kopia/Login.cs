namespace Hv2020.Felanmalan.Login.WCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Login")]
    public partial class Login
    {
        public int LoginID { get; set; }

        [Required]
        [StringLength(50)]
        public string Anvandarnamn { get; set; }

        [Required]
        [StringLength(50)]
        public string Salt { get; set; }

        [Required]
        [StringLength(50)]
        public string Losenord { get; set; }
    }
}
