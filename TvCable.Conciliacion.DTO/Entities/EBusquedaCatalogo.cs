using System;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class EBusquedaCatalogo
    {
        [DataMember]
        public int Recaudador { get; set; }
        [DataMember]
        public DateTime TipoConciliacion { get; set; }
        [DataMember]
        public DateTime FechaInicio { get; set; }
        [DataMember]
        public int FechaFin { get; set; }
        

    }
}