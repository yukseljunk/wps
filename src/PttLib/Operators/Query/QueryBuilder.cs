using System.IO;
using PttLib.Helpers;
using PttLib.TourInfo;

namespace PttLib.Operators.Query
{
    public class QueryBuilder
    {
        private readonly IOperatorFactory _operatorFactory;

        public QueryBuilder(IOperatorFactory operatorFactory )
        {
            _operatorFactory = operatorFactory;
        }

        public void BuildQueries(Query query)
        {
            //serialize query object
            var xmlSerializer= new XmlSerializer();
            var xsltTransformer = new DefaultXslTransformer();
            var fileReaderWriter = new FileReaderWriter();
            var tourInfoFactory = new TourInfoFactory();
            //transform serialized object with accompanying xsl template and set to operator.query
            foreach (var op in _operatorFactory.Operators)
            {
                query.SetCultureInfo(op.CultureInfo);
                var serializedQuery = xmlSerializer.Serialize(query);
                op.SerializedTourDataRequests = xsltTransformer.Transform(serializedQuery, op.QueryTemplateFile, false);
                op.SerializedHotelListRequests = xsltTransformer.Transform(serializedQuery, op.AllHotelsTemplateFile, false);
                if (!string.IsNullOrEmpty(op.TourInfoFile) && File.Exists(op.TourInfoFile))
                {
                    op.TourInfo = tourInfoFactory.Deserialize(fileReaderWriter.ReadFile(op.TourInfoFile, true));
                }
                op.QueryObject = query.Clone();
            }

        }


      
    }
}
