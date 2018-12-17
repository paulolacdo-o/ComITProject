using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OurFiction.Models
{
    public class StoryFragment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FragmentId { get; set; }
        [Required]
        public int StorySequenceNumber { get; set; }

        [ForeignKey("StoryId")]
        public Story Story { get; set; }
        [ForeignKey("EntryId")]
        public Entry Entry { get; set; }
    }
}
