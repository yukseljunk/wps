using System;
using System.IO;
using System.Net;
using System.Text;
using PttLib.Helpers;

namespace PttLib.PttRequestResponse
{
    class PttResponse : IPttResponse
    {
        public string GetResponse(IPttRequest request)
        {
            PrepareForPost(request);

            try
            {
                string result = "";
                using (var response = (HttpWebResponse)request.WrappedRequest.GetResponse())
                {
                    if ((int)response.StatusCode >= 300 && (int)response.StatusCode <= 399)
                    {
                        //redirection varsa request url sini degistirt
                        var uriString = response.Headers["Location"];
                        var pttRequestFactory = new PttRequestFactory(request);
                        var newRequest = pttRequestFactory.Deserialize(string.Format("<Request><Url>{0}</Url><Method>GET</Method><Host>{1}</Host><Referer><![CDATA[{2}]]></Referer></Request>", uriString, request.WrappedRequest.Host, request.WrappedRequest.Referer));
                        request.Response = "Redirect";
                        response.Close();
                        Logger.LogProcess("PttResponse GetResponse url redirect newRequest:" + newRequest);
                        return GetResponse(newRequest);
                    }

                    //http://stackoverflow.com/questions/227575/encoding-problem-with-httpwebresponse
                    var charset = response.CharacterSet;
                    var encoding = Encoding.Default;
                    if (!string.IsNullOrEmpty(charset)) encoding = Encoding.GetEncoding(charset);

                    if (!request.Chunked)
                    {
                        //request.WrappedRequest.CookieContainer.Add(response.Cookies);//buna gerek yok gibi, request.Cookies ile debug edince GetResponse da cookiler otomatik olarak doluyor zaten     
                        using (var respStream = response.GetResponseStream())
                        {
                            respStream.ReadTimeout = 1000;
                            using (var sr = new StreamReader(respStream, encoding))
                            {
                                result = sr.ReadToEnd();
                                sr.Close();
                            }
                            response.Close();
                            request.Response = result;
                            return result;
                        }
                    }
                    else
                    {
                        var sb = new StringBuilder();
                        var buf = new byte[8192];

                        using (var resStream = response.GetResponseStream())
                        {
                            int count;
                            do
                            {
                                count = resStream.Read(buf, 0, buf.Length);
                                if (count == 0) continue;

                                var tmpString = encoding.GetString(buf, 0, count);
                                sb.Append(tmpString);
                            } while (count > 0);

                            resStream.Flush();
                            resStream.Close();
                        }

                        response.Close();
                        request.Response = sb.ToString();

                        return sb.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

                request.WrappedRequest.Abort();
                request.WrappedRequest = null;
                GC.Collect();
            }
        }

        public void PrepareForPost(IPttRequest request)
        {
            if (string.IsNullOrEmpty(request.PostValue)) return;
            var send = Encoding.Default.GetBytes(request.PostValue);
            request.WrappedRequest.ContentLength = send.Length;

            using (var sout = request.WrappedRequest.GetRequestStream())
            {
                sout.Write(send, 0, send.Length);
                sout.Flush();
                sout.Close();
            }
        }

        public byte[] GetResponseBytes(IPttRequest request)
        {
            byte[] content;
            using (var response = (HttpWebResponse)request.WrappedRequest.GetResponse())
            {
                using (var data = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(data))
                    {
                        using (var br = new BinaryReader(data))
                        {
                            content = br.ReadBytes(500000);
                            br.Close();
                        }
                        //request.WrappedRequest.CookieContainer.Add(response.Cookies);
                        reader.Close();
                    }
                    data.Flush();
                    data.Close();
                }
                response.Close();
                request.WrappedRequest.Abort();
                request.WrappedRequest = null;
                GC.Collect();
            }
            return content;
        }
    }
}