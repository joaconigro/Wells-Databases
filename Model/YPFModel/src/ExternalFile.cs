using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wells.StandardModel.Models;

namespace Wells.YPFModel
{
    public class ExternalFile : BusinessObject
    {


        #region Properties
        [Required]
        [Browsable(false)]
        public string WellId { get; set; }


        [Browsable(true), DisplayName("Pozo")]
        public string WellName => Well?.Name;

        [Browsable(true), DisplayName("Extensión"), Required]
        public string FileExtension { get; set; }

        [Browsable(true), DisplayName("Archivo")]
        public string CompleteFilename => Name + "." + FileExtension;

        [Required, Browsable(false)]
        public byte[] Data { get; set; }
        #endregion


        #region Lazy-Load Properties
        /// <summary>
        /// The parent well.
        /// </summary>
        [ForeignKey("WellId")]
        [Browsable(false)]
        public virtual Well Well { get; set; }

        #endregion
    }
}
