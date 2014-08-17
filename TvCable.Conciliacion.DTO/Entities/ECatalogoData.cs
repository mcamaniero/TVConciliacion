using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class ECatalogoData
    {
        [DataMember]
        public int ResponseCode
        {
            get;
            set;
        }

        [DataMember]
        public string ResponseDescription
        {
            get;
            set;
        }

        [DataMember]
        public int IdCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public string CodigoCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public string NombreCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public string DescripcionCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public int EstadoCatalogoId
        {
            get;
            set;
        }

        [DataMember]
        public EItemCatalogoData[] ItemCatalogo
        {
            get;
            set;
        }

    }
}
