using System.ComponentModel.DataAnnotations;

namespace Wells.BaseModel.Models
{
    public interface IBusinessObject
    {
        [Key]
        [Required]
        string Id { get; set; }

        void OnInitialize();
    }
}
