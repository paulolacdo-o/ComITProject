using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OurFiction.Models
{
    public class Story
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoryId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} can only have up to {1} characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(300, ErrorMessage ="Keep {0} under {1} characters.")]
        [Display(Name = "Synopsis")]
        public string Synopsis { get; set; }

        [Display(Name = "Number of Fragments")]
        public int FragCount { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage ="Keep {0} under {1} characters.")]
        [Display(Name = "Start Fragment")]
        public string InitialContent { get; set; }

        [ForeignKey("OwnerId")]
        public IdentityUser Owner { get; set; }

        public bool IsActive { get; set; }
    }
}
