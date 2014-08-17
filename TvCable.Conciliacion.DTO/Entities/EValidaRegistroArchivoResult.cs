using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class EValidaRegistroArchivoResult
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
        public Boolean PermiteRegistroArchivo
        {
            get;
            set;
        }



    }
}
