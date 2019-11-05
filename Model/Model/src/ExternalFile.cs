using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Diagnostics;
using Wells.BaseModel.Models;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Wells.Model
{
    public class ExternalFile : BusinessObject
    {
        public ExternalFile() { }

        public ExternalFile(string filename)
        {
            Name = Path.GetFileNameWithoutExtension(filename);
            FileExtension = Path.GetExtension(filename);
            Data = File.ReadAllBytes(filename);
        }

        [Browsable(true), DisplayName("Nombre")]
        public string Name { get; set; }

        #region Properties
        [Required]
        [Browsable(false)]
        public string WellId { get; set; }


        [Browsable(true), DisplayName("Pozo")]
        public string WellName => Well?.Name;

        [Browsable(true), DisplayName("Extensión"), Required]
        public string FileExtension { get; set; }

        [Browsable(true), DisplayName("Archivo")]
        public string CompleteFilename => Name + FileExtension;

        [Required, Browsable(false)]
        public byte[] Data { get; set; }
        #endregion


        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                return GetBrowsableProperties(typeof(ExternalFile));
            }
        }


        #region Lazy-Load Properties
        /// <summary>
        /// The parent well.
        /// </summary>
        [ForeignKey("WellId")]
        [Browsable(false)]
        public virtual Well Well { get; set; }
        #endregion


        public void Open()
        {
            var file = Path.Combine(Path.GetTempPath(), CompleteFilename);
            File.WriteAllBytes(file, Data);
            if (File.Exists(file))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(file) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al tratar de abrir el archivo {CompleteFilename}", ex);
                }
            }
        }

        public override int CompareTo(IBusinessObject other)
        {
            return Name.CompareTo((other as ExternalFile).Name);
        }

        public static string GetDisplayName(string propertyName)
        {
            return GetDisplayName(typeof(ExternalFile), propertyName);
        }

        public override string ToString()
        {
            return CompleteFilename;
        }
    }
}
