using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryManual.DataAccess.HarryCarrier
{
    public class Person_Film
    {
        [Key]
        public int Person_FilmId { get; set; }

        public int PersonId { get; set; }

        public int FilmId { get; set; }
    }
}
