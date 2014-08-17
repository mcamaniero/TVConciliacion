using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class EConciliacionResult
    {
        [DataMember]
        public int ResultCode
        {
            get;
            set;
        }

        [DataMember]
        public string ResultDescription
        {
            get;
            set;
        }

        [DataMember]
        public string TiempoEjecucion
        {
            get;
            set;
        }

        [DataMember]
        public int TransaccionesConciliadas
        {
            get;
            set;
        }

        [DataMember]
        public int TransaccionesErrorMonto
        {
            get;
            set;
        }

        [DataMember]
        public int TransaccionesErrorCliente
        {
            get;
            set;
        }

        [DataMember]
        public int TransaccionesSobrantesTuves
        {
            get;
            set;
        }

        [DataMember]
        public int TransaccionesFaltantesTuves
        {
            get;
            set;
        }
    }
}
