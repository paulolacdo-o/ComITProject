using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OurFiction.Models
{
    public class EntryCycle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryCycleId { get; set; }
        public DateTimeOffset StartTimeDate { get; set; }
        public DateTimeOffset EndTimeDate { get; set; }

        [ForeignKey("StoryId")]
        public Story Story { get; set; }
    }
}
