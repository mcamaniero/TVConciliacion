using System;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class EValidaArchivoData
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
        public string TiempoValidacion
        {
            get;
            set;
        }

        [DataMember]
        public Boolean StatusFile
        {
            get;
            set;
        }

    }
}
