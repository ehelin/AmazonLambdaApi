namespace Shared.Interfaces
{
    public interface IService
    {
        bool TestAwsSignatureCode();
        string GetSatellite();
        bool InsertSatelliteUpdate(string update);
        bool DeleteSatelliteUpdate(int id);
        bool UpdateSatelliteUpdate(string update);
    }
}
