using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryManual.DataAccess.HarryCarrier
{
    public class Article
    {
        [Key]

        public int ArticleId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateOfPublication { get; set; }

    }
}
