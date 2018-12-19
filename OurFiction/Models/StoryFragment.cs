using Microsoft.AspNetCore.Identity;
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
        [StringLength(1000, ErrorMessage = "Keep {0} under {1} characters.")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [ForeignKey("StoryId")]
        public Story Story { get; set; }
        
        [ForeignKey("EntryId")]
        public Entry Entry { get; set; }

        [ForeignKey("OwnerId")]
        public IdentityUser Owner { get; set; }
    }
}
