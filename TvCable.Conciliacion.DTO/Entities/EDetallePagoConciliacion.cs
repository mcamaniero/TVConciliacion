using System;


namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class EDetallePagoConciliacion
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string MontoTUV { get; set; }
        [DataMember]
        public string MontoMdp { get; set; }
        [DataMember]
        public string FechaTuv { get; set; }
        [DataMember]
        public string FechainiMdp { get; set; }
        [DataMember]
        public string UsuarioVentaTuv { get; set; }
        [DataMember]
        public string UsuarioVentaMpd { get; set; }
        [DataMember]
        public string IdTuv { get; set; }
        [DataMember]
        public string IdMdp { get; set; }
       
    }
}
