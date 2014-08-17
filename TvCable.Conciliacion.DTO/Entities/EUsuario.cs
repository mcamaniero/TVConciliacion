using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]
    public class EUsuario
    {

        [DataMember]
        public int ResponseCode  {  get; set;}

        [DataMember]
        public string ResponseDescription {get; set;}
        [DataMember]
        public string Usuario { get; set; }
        [DataMember]
        public string Rol { get; set; }

        [DataMember]
        public EFuncionalidadUsuario[] ItemFuncionalidad
        {
            get;
            set;
        }

    }
}
