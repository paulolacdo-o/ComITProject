using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OurFiction.Models
{
    public class Entry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryId { get; set; }
        [Required]
        public string Content { get; set; }
        public int Votes { get; set; }
        
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
