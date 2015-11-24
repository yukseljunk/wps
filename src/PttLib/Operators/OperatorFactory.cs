using System.Collections.Generic;
using PttLib.Domain;
using PttLib.Operators.Operator;

namespace PttLib.Operators
{
    public class OperatorFactory : IOperatorFactory
    {
        private IList<IOperator> _operators;

        public OperatorFactory()
        {
            _operators = new List<IOperator>()
                             {   new Tez(),
                                 new Coral(),
                                 new Anex(),
                                  new Biblo(),
                                  new Polar(),
                                  new Troyka(),
                                  new Sunmar(),
                                  new Natalie(),
                                  new Versa(),
                                  new Space(),
                                  new Anex_(),
                                  new Labirinth(),
                                  new Tui(),
                                  new Intourist()
                             };
        }

        public IList<IOperator> Operators
        {
            get { return _operators; }
        }

        public IList<IOperator> OperatorsById(IList<string> ids)
        {
            if (ids==null) return null;
            var result = new List<IOperator>();
            foreach (var id in ids)
            {
                var op = GetOperatorById(id);
                if (op != null)
                {
                    result.Add(op);
                }
            }
            return result;
        }
        public IOperator GetOperatorById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            foreach (var op in _operators)
            {
                if(op.Name==id)
                {
                    return op;
                }
            }
            return null;
        }

    }
}