using System.ComponentModel.DataAnnotations;

namespace HarryManual.DataAccess.HarryCarrier
{
    public class CustomCategory
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
