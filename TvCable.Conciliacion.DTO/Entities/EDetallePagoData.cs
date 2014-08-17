using System;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class EDetallePagoData
    {
        [DataMember]
        public string ResultCode
        {
            get;
            set;
        }

        [DataMember]
        public DateTime FechaInicio
        {
            get;
            set;
        }

        [DataMember]
        public DateTime HoraInicio
        {
            get;
            set;
        }

        [DataMember]
        public string IdTransactionMdp
        {
            get;
            set;
        }

        [DataMember]
        public string IdTransactionTvCable
        {
            get;
            set;
        }

        [DataMember]
        public string Cedula
        {
            get;
            set;
        }

        [DataMember]
        public string IdMdp
        {
            get;
            set;
        }

        [DataMember]
        public string UsuarioVentas
        {
            get;
            set;
        }

        [DataMember]
        public string Producto
        {
            get;
            set;
        }

        [DataMember]
        public Decimal Monto
        {
            get;
            set;
        }

        [DataMember]
        public DateTime FechaFin
        {
            get;
            set;
        }

        [DataMember]
        public DateTime HoraFin
        {
            get;
            set;
        }
    }
}
