using System.ComponentModel.DataAnnotations;

namespace HarryManual.DataAccess.HarryCarrier
{
    public class Quote
    {
        [Key]
        public int QuoteId {  get; set; }

        public string Content { get; set; }
    }
}
