using System;
using System.ServiceModel;

namespace TvCable.Conciliacion.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITvCableConciliacionServices" in both code and config file together.
    [ServiceContract]
    public interface ITvCableConciliacionServices
    {
        [OperationContract]
        string RegistrarArchivoConciliacionMdp(string nombreArchivo, string fechaTransacciones, string codigoMdp, int numTransacciones, decimal montoTotal, string mailNotificacion, string observaciones);
    }
}
