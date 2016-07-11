namespace Shared
{
    public interface IService
    {
        string GetSatellite(string satelliteId);
        bool InsertSatelliteUpdate(string update);
        bool DeleteSatelliteUpdate(int id);
        bool UpdateSatelliteUpdate(string update);
    }
}
