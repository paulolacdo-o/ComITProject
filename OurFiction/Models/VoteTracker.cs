using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OurFiction.Models
{
    public class VoteTracker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("VoterId")]
        public IdentityUser Voter { get; set; }

        [ForeignKey("VoteId")]
        public Vote Vote { get; set; }
    }
}
