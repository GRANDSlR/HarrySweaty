using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryManual.DataAccess.HarryCarrier
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public string Description { get; set; }

    }
}
