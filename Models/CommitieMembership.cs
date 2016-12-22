using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models
{
    public class CommitieMembership
    {
        [Key]
        public int CommitieMembershipID { get; set; }
        public int PrID { get; set; }
        public int CommitteeID { get; set; }


        [ForeignKey("PrID")]
        public Professor Professor { get; set; }
        [ForeignKey("CommitteeID")]
        public Committee Committee { get; set; }
    }
}
