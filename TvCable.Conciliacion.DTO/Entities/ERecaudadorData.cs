using System;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class ERecaudadorData
    {
        [DataMember]
        public int IdRec
        {
            get;
            set;
        }

        public string IdRecaudador
        {
            get;
            set;
        }

        public string Ruc
        {
            get;
            set;
        }

        public string IdTipoCliente
        {
            get;
            set;
        }

        public string Telefono
        {
            get;
            set;
        }

        public string IdPersona
        {
            get;
            set;
        }

        public string IdContribuyente
        {
            get;
            set;
        }

        public string RazonSocial
        {
            get;
            set;
        }

        public string Direccion
        {
            get;
            set;
        }

        public string Correo
        {
            get;
            set;
        }

        public string CodigoServicio
        {
            get;
            set;
        }

        public string Codigo
        {
            get;
            set;
        }

        public string CodigoTuves
        {
            get;
            set;
        }
    }
}
