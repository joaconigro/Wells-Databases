using System.ComponentModel;
using Wells.BaseModel.Models;

namespace Wells.CorePersistence
{
    public class RejectedEntity
    {
        public IBusinessObject Entity { get; }
        public int OriginalRow { get; }
        public RejectedReasons Reason { get; }

        public RejectedEntity(IBusinessObject entity, int originalRow, RejectedReasons reason)
        {
            Entity = entity;
            OriginalRow = originalRow;
            Reason = reason;
        }
    }

    public enum RejectedReasons
    {
        [DisplayName("Ninguna")] None,
        [DisplayName("Id duplicado")] DuplicatedId,
        [DisplayName("Nombre duplicado")] DuplicatedName,
        [DisplayName("Pozo sin nombre")] WellNameEmpty,
        [DisplayName("Pozo no encontrado")] WellNotFound,
        [DisplayName("Profundidad de FLNA mayor al del agua")] FLNADepthGreaterThanWaterDepth,
        [DisplayName("Fecha duplicada")] DuplicatedDate,
        [DisplayName("Desconocido")] Unknown
    }
}
