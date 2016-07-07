using Shared.Dto;

namespace Shared.Interfaces
{
    public interface IService
    {
        bool TestAwsSignatureCode();
        SatelliteUpdate GetSatellite();
        bool InsertSatelliteUpdate(SatelliteUpdate update);
        bool DeleteSatelliteUpdate(int id);
        bool UpdateSatelliteUpdate(SatelliteUpdate update);
    }
}
