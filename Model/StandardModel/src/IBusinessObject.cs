using System.ComponentModel.DataAnnotations;

namespace Wells.StandardModel.Models
{
    public interface IBusinessObject
    {
        [Key]
        [Required]
        string Id { get; set; }

        void OnInitialize();
    }
}
