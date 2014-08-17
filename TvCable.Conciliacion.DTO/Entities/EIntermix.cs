using System;

namespace TvCable.Conciliacion.DTO.Entities
{
    public class EIntermix
    {
        public int IDCONCILREC { get; set; }
        public int COD_EMPRESA { get; set; }
                public string  COD_PUNTOEMISION { get; set; }
        public string RUC { get; set; }
        public int COD_CLIENTE { get; set; }
        public int TIPO_CLIENTE { get; set; }
        public string TELEFONO { get; set; }
        public int TIPO_PERSONA { get; set; }
        public int TIPO_CONTRIBUYENTE { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string EMAIL { get; set; }
          public string  COD_ITEM  { get; set; }
        public double VALOR_NETO { get; set; }
        public double BASE_ICE { get; set; }
        public double VALOR_ICE { get; set; }
        public double BASE_IVA { get; set; }
        public double VALOR_IVA { get; set; }
        public double VALOR_TOTAL { get; set; }
        public DateTime FECHA_PROCESO { get; set; }
        public DateTime FECHA_EMISION { get; set; }
        public DateTime FECHA_DESDE { get; set; }
        public DateTime FECHA_HASTA { get; set; }
        public string REFERENCIA { get; set; }
        public string COMENTARIO { get; set; }
        public int FACTURADO { get; set; }

    }
}
