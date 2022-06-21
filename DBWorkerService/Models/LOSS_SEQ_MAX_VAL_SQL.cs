using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService.Models
{
    public class LOSS_SEQ_MAX_VAL_SQL
    {
        [Key]
        public int Id { get; set; }
        public int Loss_Seq { get; set; }
    }
}
