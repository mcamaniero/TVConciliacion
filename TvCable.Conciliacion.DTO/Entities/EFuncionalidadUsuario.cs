using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvCable.Conciliacion.DTO.Entities
{

    [DataContract]
    [Serializable]

    public class EFuncionalidadUsuario
    {
        [DataMember]
        public string Elemento { get; set; }
    }
}
