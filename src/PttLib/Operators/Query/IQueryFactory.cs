namespace PttLib.Operators.Query
{
    public interface IQueryFactory
    {
        Query Create(string queryOuterXml);
    }
}