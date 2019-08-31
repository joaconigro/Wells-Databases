using System.ComponentModel;
using Wells.StandardModel.Models;

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
        [Description("Ninguna")] None,
        [Description("Id duplicado")] DuplicatedId,
        [Description("Nombre duplicado")] DuplicatedName,
        [Description("Pozo sin nombre")] WellNameEmpty,
        [Description("Pozo no encontrado")] WellNotFound,
        [Description("Profundidad de FLNA mayor al del agua")] FLNADepthGreaterThanWaterDepth,
        [Description("Fecha duplicada")] DuplicatedDate,
        [Description("Desconocido")] Unknown
    }
}
