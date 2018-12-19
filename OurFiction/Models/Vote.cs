using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OurFiction.Models
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteId { get; set; }

        [ForeignKey("FragmentId")]
        public StoryFragment Fragment { get; set; }

        [ForeignKey("EntryId")]
        public Entry Entry { get; set; }

        [ForeignKey("VoterId")]
        public IdentityUser Voter { get; set; }

        public int VotePoints { get; set; }
    }
}
