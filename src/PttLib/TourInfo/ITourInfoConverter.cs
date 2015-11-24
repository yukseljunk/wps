namespace PttLib.TourInfo
{
    public interface ITourInfoConverter
    {
        string MethodFullName { get; }
        object Convert(object input);
        ITourInfoConverter Clone();
    }
}