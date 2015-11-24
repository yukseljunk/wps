using System;
using System.Collections.Generic;

namespace PttLib.Helpers
{
    internal class ClassField
    {
        public ClassField()
        {
            Fields = new List<ClassField>();
        }

        public ClassField(string fieldName)
            : this()
        {
            FieldName = fieldName;
        }
        public ClassField(string fieldName, Type fieldType)
            : this(fieldName)
        {
            FieldType = fieldType;
        }
        public ClassField(string fieldName, Type fieldType, object fieldValue)
            : this(fieldName, fieldType)
        {
            FieldValue = fieldValue;
        }

        public Type FieldType { get; set; }
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public List<ClassField> Fields { get; set; }

        public bool IsBasicType()
        {
            var typeHelper = new TypeHelper();
            return typeHelper.IsBasicType(FieldType);
        }

    }
}