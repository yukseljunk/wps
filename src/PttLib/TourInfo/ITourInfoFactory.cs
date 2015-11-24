namespace PttLib.TourInfo
{
    public interface ITourInfoFactory
    {
        ITourInfo Deserialize(string serialized);
    }
}