using System;

namespace TvCable.Conciliacion.DTO.Entities
{
    [DataContract]
    [Serializable]

    public class EItemCatalogoData
    {
        
        [DataMember]
        public int IdItemCatalogo
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
        public string CodigoItemCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public string NombreItemCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public string ValorItemCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public int IdPadreItemCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public int IdEstadoItemCatalogo
        {
            get;
            set;
        }

        [DataMember]
        public string DescripcionItemCatalogo
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
    }
}
