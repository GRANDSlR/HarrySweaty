using System.ComponentModel.DataAnnotations;

namespace HarryManual.DataAccess.HarryCarrier
{
    public class Notes
    {
        [Key]

        public int NoteId { get; set; }

        public string NoteTitle { get; set; }

        public string NoteContent { get; set; }
    }
}
