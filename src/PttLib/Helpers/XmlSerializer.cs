using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace PttLib.Helpers
{
    public class XmlSerializer
    {
        private readonly string _version;
        private readonly string _encoding;

        public XmlSerializer()
        {
            _version = "1.0";
            _encoding = "UTF-8";

        }

        public XmlSerializer(string version, string encoding)
        {
            _version = version;
            _encoding = encoding;
        }

        public void Serialize(string filePath, object obj)
        {
            var doc = GetXmlDoc(obj);
            if (doc != null) doc.Save(filePath);
        }

        public string Serialize(object obj)
        {
            var doc = GetXmlDoc(obj);
            return doc.InnerXml;
        }

        
        public XmlDocument GetXmlDoc(object obj)
        {
            var objectType = obj.GetType();
            //if(objectType.BaseType!=null && objectType.BaseType!=typeof(object))
            //{
            //    objectType = objectType.BaseType;
            //}

            var classField = new ClassField(objectType.Name, objectType, obj);

            FillClassFieldTree(classField);

            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration(_version, _encoding, null));

            TraverseClassFieldTree(doc, null, classField);
            return doc;
        }

        private void TraverseClassFieldTree(XmlDocument xmlDocument, XmlElement xmlElement, ClassField classField)
        {
            var fieldLabel = classField.FieldName;
            if (!string.IsNullOrEmpty(fieldLabel))
            {
                var splitted = fieldLabel.Split(new char[] { '.' });
                fieldLabel = splitted[splitted.Count() - 1];
            }

            var element = xmlDocument.CreateElement(fieldLabel);

            if (xmlElement == null)
            {
                xmlDocument.AppendChild(element);
            }
            else
            {
                xmlElement.AppendChild(element);
            }

            if (classField.IsBasicType() || classField.FieldType.IsEnum)
            {

                var htmlEncoding = new HtmlEncoding();
                element.InnerText = classField.FieldValue == null ? "" : htmlEncoding.HtmlEncode(classField.FieldValue.ToString());

                return;
            }

            var traverseFields = true;
            if ((classField.FieldType.IsArray || classField.FieldType.GetGenericArguments().Count() > 0) && classField.FieldValue != null)
            {
                traverseFields = false;
                var typeHelper = new TypeHelper();
                var childFieldValues = (IList)classField.FieldValue;
                foreach (var childFieldValue in childFieldValues)
                {
                    var isBasicType = typeHelper.IsBasicType(childFieldValue.GetType());
                    if (!isBasicType)
                    {
                        traverseFields = true;
                        break;
                    }
                    var subChildField = new ClassField(childFieldValue.GetType().Name, classField.Fields[0].FieldType, childFieldValue);
                    TraverseClassFieldTree(xmlDocument, element, subChildField);
                }
            }
            if (!traverseFields)
            {
                return;
            }
            foreach (var field in classField.Fields)
            {
                TraverseClassFieldTree(xmlDocument, element, field);
            }
        }

        private void FillClassFieldTree(ClassField classField)
        {
            var propertyInfos = classField.FieldType.GetProperties();

            if (classField.IsBasicType() || propertyInfos.Count() == 0)
            {
                return;
            }

            foreach (var propertyInfo in propertyInfos)
            {
                var xmlIgnoreAttr = propertyInfo.GetCustomAttributes(typeof(XmlIgnoreAttribute), true);
                if (xmlIgnoreAttr.Any()) continue;

                FillClassFieldTreeWithProperties(classField, propertyInfo);
            }
        }

        private void FillClassFieldTreeWithProperties(ClassField classField, PropertyInfo propertyInfo)
        {
            object childFieldValue = null;
            try
            {
                childFieldValue = propertyInfo.GetValue(classField.FieldValue, null);
            }
            catch (Exception)
            {

            }

            if (childFieldValue == null && propertyInfo.PropertyType != typeof(String))
                return;

            var childField = new ClassField(propertyInfo.Name, propertyInfo.PropertyType, childFieldValue);
            classField.Fields.Add(childField);

            if (propertyInfo.PropertyType.IsInterface)
            {
                var underLyingObject = propertyInfo.GetValue(classField.FieldValue, null);
                var classMethodNames = underLyingObject.GetType().GetInterfaceMap(propertyInfo.PropertyType).TargetMethods;
                foreach (var classMethodName in classMethodNames)
                {
                    FillInterfaceMethods(childField, underLyingObject, classMethodName);
                }
                return;
            }

            if (!propertyInfo.PropertyType.IsArray && propertyInfo.PropertyType.GetGenericArguments().Count() <= 0)
            {
                FillClassFieldTree(childField);
                return;
            }
            if (childFieldValue == null) return;
            var childFieldItems = (IList)childFieldValue;
            foreach (var childFieldItem in childFieldItems)
            {
                var subChildItem = new ClassField(childFieldItem.GetType().ToString(), childFieldItem.GetType(),
                                                  childFieldItem);
                childField.Fields.Add(subChildItem);
                FillClassFieldTree(subChildItem);
            }
        }

        private void FillInterfaceMethods(ClassField childField, object underLyingObject, MethodInfo classMethodName)
        {
            var result = underLyingObject.GetType().InvokeMember(classMethodName.Name, BindingFlags.InvokeMethod, null, underLyingObject, null);
            var subChildItem = new ClassField(classMethodName.Name, classMethodName.ReturnType, result);
            childField.Fields.Add(subChildItem);

            var typeHelper = new TypeHelper();
            if (typeHelper.IsArrayType(result))
            {
                FillClassFieldTree(subChildItem);
                return;
            }

            var resultlist = (IList)result;
            foreach (var resultItem in resultlist)
            {
                var subsubChildItem = new ClassField(resultItem.GetType().ToString(), resultItem.GetType(),
                                                     resultItem);
                subChildItem.Fields.Add(subsubChildItem);
                FillClassFieldTree(subsubChildItem);
            }
        }

        public object DeSerialize(string filePath)
        {
            throw new NotImplementedException();
        }

        public object DeSerialize(string xmlString, bool dummy)
        {
            throw new NotImplementedException();
        }

        public object DeSerialize(XmlDocument xmlDocument)
        {
            throw new NotImplementedException();
        }


    }
}
