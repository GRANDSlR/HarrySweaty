using System.ComponentModel.DataAnnotations;

namespace HarryManual.DataAccess.HarryCarrier
{
    public class CustomCategory_Note
    {
        [Key]
        public int CustomCategory_NoteId { get; set; }

        public int CustomCategoryId { get; set; }

        public int NoteId { get; set; }
    }
}
