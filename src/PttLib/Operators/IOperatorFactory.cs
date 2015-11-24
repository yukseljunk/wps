using System.Collections.Generic;
using PttLib.Domain;

namespace PttLib.Operators
{
    public interface IOperatorFactory
    {
        IList<IOperator> Operators{
            get;
        }

        IOperator GetOperatorById(string id);
        IList<IOperator> OperatorsById(IList<string> ids);


    }
}