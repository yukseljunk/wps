using System.Collections.Generic;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;

namespace PttLib.Operators.Operator
{
    internal class Intourist : OperatorBase
    {
        const string OperatorName = "INTOURIST";
        private const string DATA_START = "<table class=\"gvResults\"";
        private const string DATA_END = "</table>";

        public override List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                {
                    new List<string>(){"BB"},
                    new List<string>(){"HB"},
                    new List<string>(){"FB"},
                    new List<string>(){"ALL"},
                    new List<string>(){"UALL"}
                };
            }
        }

        public override string Name
        {
            get { return OperatorName; }
        }

        public Intourist() 
        {
            AllHotelsXmlConverter = new JSONArrayToXmlConverterRecursively();
        }

        public override string Refine(IPttRequest pttRequest, string htmlSource)
        {
            return htmlSource.TrimMiddle(DATA_START, DATA_END,true);
        }

        public override int DatePartitioningDays
        {
            get
            {
                return 21;
            }
        }
    }
}