using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace PttLib.Helpers
{
    public class DefaultXslTransformer
    {
        public string Transform(string xmlDocumentContent, string xsltFilePath, bool dummy)
        {

            var fileContent = GetFileContent(xsltFilePath);

            if (string.IsNullOrEmpty(fileContent))
            {
                return null;
            }

            return Transform(xmlDocumentContent, fileContent);
        }

        public string Transform(string xmlDocumentContent, string xsltFileContent)
        {
            string result = null;
            //guard

            if (xsltFileContent == "")
            {
                return null;
            }

            try
            {
                //read XML
                TextReader xmlFileTextReader = new StringReader(xmlDocumentContent);
                XmlTextReader tr11 = new XmlTextReader(xmlFileTextReader);
                XPathDocument xPathDocument = new XPathDocument(tr11);

                //read XSLT
                TextReader tr2 = new StringReader(xsltFileContent);
                XmlTextReader tr22 = new XmlTextReader(tr2);
                XslTransform xslt = new XslTransform();
                xslt.Load(tr22);

                //create the output stream
                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);

                xslt.Transform(xPathDocument, null, tw);

                //get result
                result = sb.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        public string Transform(XmlDocument xmlDocument, string xsltFileContent)
        {

            return Transform(xmlDocument.InnerXml, xsltFileContent);
        }

        public string Transform(XmlDocument xmlDocument, string xsltFilePath, bool dummy)
        {
            //guard
            var  fileContent = GetFileContent(xsltFilePath);
            
            if (string.IsNullOrEmpty(fileContent))
            {
                return null;
            }

            return Transform(xmlDocument, fileContent);
        }

        public string Transform(string xmlFilePath, string xsltFilePath, bool dummy, bool dummy2)
        {
            var xmlFileContent = GetFileContent(xmlFilePath);
            var fileContent = GetFileContent(xsltFilePath);
            if (string.IsNullOrEmpty(fileContent) || string.IsNullOrEmpty(xmlFileContent))
            {
                return null;
            }
            return Transform(xmlFileContent, fileContent);
        }
        public string Transform(string xmlFilePath, string xsltFileContent, bool dummy, bool dummy2, bool dummy3)
        {
            var xmlFileContent = GetFileContent(xmlFilePath);
            if (string.IsNullOrEmpty(xmlFileContent))
            {
                return null;
            }
            return Transform(xmlFileContent, xsltFileContent);
        }

        private string GetFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found: " + filePath);
            }

            var _fileReaderWriter = new FileReaderWriter();
            string fileContent = "";

            try
            {
                fileContent = _fileReaderWriter.ReadFile(filePath, true);

            }
            catch (Exception e)
            {

                throw e;
            }
            return fileContent;
        }
    }

   
}
