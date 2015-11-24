using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;
using PttLib.Domain;
using PttLib.Helpers;
using PttLib.Helpers.XmlConverters;
using PttLib.PttRequestResponse;
using PttLib.TourInfo;
using PttLib.Tours;

namespace PttLib.Operators.Operator
{
    /// <summary>
    /// for data harvesting
    /// </summary>
    public abstract partial class OperatorBase : IOperator
    {
        private int _pageIndex;
        protected string _lastHotelId;
        private const int MaxChildSupported = 2;
        private IPttRequest _lastRequest = null;
        protected double _lastItemPrice;
        protected string _nextPageUrl;
        private bool _extensiveLoggingNeeded;

        public virtual List<List<string>> MealTypes
        {
            get
            {
                return new List<List<string>>()
                                                {
                                                  new List<string>{ "ALL"},
                                                   new List<string>{ "RO"},
                                                   new List<string>{ "BB"},
                                                   new List<string>{ "HB"},
                                                    new List<string>{"FB"},
                                                    new List<string>{"AI"},
                                                    new List<string>{"UA"},
                                                    new List<string>{"OTHER"}
                                                };

            }
        }
        
        public bool ExtensiveLoggingNeeded
        {
            get { return _extensiveLoggingNeeded; }
            set { _extensiveLoggingNeeded = value; }
        }

        private bool KeepAliveDefaultValue
        {
            get
            {
                var keepAliveDefaultFalseOperators = ConfigurationManager.AppSettings["keepAliveDefaultFalseOperators"];
                if (string.IsNullOrEmpty(keepAliveDefaultFalseOperators)) return true;
                if ((keepAliveDefaultFalseOperators.Surround()).ToLower().Contains(Name.Surround().ToLower())) return false;
                return true;
            }
        }

        private bool IsExtensiveLoggingNeeded
        {
            get
            {
                var extensiveLogOperators = ConfigurationManager.AppSettings["extensiveLogOperators"];
                if (string.IsNullOrEmpty(extensiveLogOperators)) return false;
                if ((extensiveLogOperators.Surround()).ToLower().Contains(Name.Surround().ToLower())) return true;
                return false;
            }

        }

        protected OperatorBase()
        {
            _extensiveLoggingNeeded = IsExtensiveLoggingNeeded;
            _lastHotelId = "";
            _pageIndex = 1;
        }

        public virtual IRequestResponse RequestResponseBehavior
        {
            get
            {
                return new RequestResponseWithoutCaptcha(RetryNeededForRequest, TryAgainInException);
            }
        }

        #region IOperator
        public string QueryTemplateFile
        {
            get
            {
                return Helper.AssemblyDirectory + @"\Operators\Query\Templates\" + Name + ".xslt";
            }
        }

        public string TourInfoFile
        {
            get
            {
                return Helper.AssemblyDirectory + @"\Operators\Query\Templates\TourInfo\" + Name + ".xml";
            }
        }


        public abstract string Name
        {
            get;
        }
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }


        public IEnumerable<IPttRequest> QueryRequest(string serializedRequests)
        {
            IPttRequestFactory requestFactory = new PttRequestFactory(Name, KeepAliveDefaultValue);
            return requestFactory.DeserializeList(serializedRequests, _lastRequest, FillSessionJar);
        }

        //todo: merge this method with GetAllHotelNamesAndIds, and merging two xslt files into single one, to share the variables
        public IList<Tour> GetTours(string hotelId, int pageNo)
        {
            //hotel change
            if (_lastHotelId != hotelId)
            {
                _lastHotelId = hotelId;
                _nextPageUrl = "";
                _lastItemPrice = QueryObject == null ? 0 : QueryObject.MinPrice;
            }


            var result = new List<Tour>();
            var queryParametersFilled = FillParameters(SerializedTourDataRequests, hotelId, pageNo);
            foreach (var pttRequest in QueryRequest(queryParametersFilled))
            {
                _lastRequest = pttRequest;
                if (!pttRequest.ConditionSatisfied) continue;
                if (pttRequest.RequestType == PttRequestType.Init)
                {
                    var htmlSource = RequestResponseBehavior.GetHtmlMaster(pttRequest, false, _extensiveLoggingNeeded, Name);
                    if (_extensiveLoggingNeeded)
                    {
                        Logger.LogProcess("Init request coming html for " + pttRequest.Url);
                        Logger.LogProcess(htmlSource);
                    }
                    if (!string.IsNullOrEmpty(htmlSource))
                    {
                        if (string.IsNullOrEmpty(pttRequest.ViewStateValue))
                        {
                            pttRequest.ViewStateValue = Helper.Encode(WebHelper.ExtractViewState(htmlSource));
                        }
                        pttRequest.EventValidationValue = Helper.Encode(WebHelper.ExtractEventValidation(htmlSource));
                    }
                }

                if (pttRequest.RequestType == PttRequestType.TourInfo)
                {
                    TourFactoryStatus status;
                    var list = TourFactory.Create(pttRequest, this, out status);
                    if (!status.Success)
                    {
                        Logger.LogProcess(string.Format("GetTours({0},{1}) null htmlsource:", hotelId, pageNo));
                        continue;
                    }
                    if (status.MoveToNextHotel)
                    {
                        _pageIndex = -1;
                    }
                    _nextPageUrl = status.NextPageUrl;

                    if (list != null && list.Any())
                    {
                        result.AddRange(list);
                        _lastItemPrice = list.Last().Price;
                    }
                    _pageIndex++;
                }


            }
            return result;
        }

        protected virtual void FillSessionJar(IPttRequest request, int requestCounter)
        {
        }

        public virtual int DatePartitioningDays
        {
            get { return 0; }
        }

        private string FillParameters(string query, string hotelId, int pageNo)
        {
            int hotelIdMinus1 = 1;
            Int32.TryParse(hotelId, out hotelIdMinus1);
            hotelIdMinus1--;

            var hotelCommonName = "%EB%FE%E1%EE%E9";
            if (HotelIds != null)
            {
                var hotelCommonNameIndex = HotelIds.IndexOf(hotelId);
                if (hotelCommonNameIndex > -1)
                {
                    hotelCommonName = Uri.EscapeDataString(HotelNames[hotelCommonNameIndex]);
                }
            }

            var templateHelper = new TemplateHelper();
            return templateHelper.ReplaceTokens(query, new Dictionary<TemplateTokenType, string>()
                                                           {
                                                               {TemplateTokenType.HOTELID, hotelId},
                                                               {TemplateTokenType.PAGENO, pageNo.ToString()},
                                                               {TemplateTokenType.PAGENO5, (5 * pageNo - 4).ToString()},
                                                               {TemplateTokenType.PAGENO25, ((pageNo - 1)*25).ToString()},
                                                               {TemplateTokenType.PAGENO100, ((pageNo - 1)*100).ToString()},
                                                               {TemplateTokenType.LASTPRICE, _lastItemPrice.ToString()},
                                                               {TemplateTokenType.STARTDATESHORT,QueryObject.StartDateShort}, 
                                                               {TemplateTokenType.ENDDATESHORT, QueryObject.EndDateShort},
                                                               {TemplateTokenType.NEXTPAGEURL, _nextPageUrl},
                                                               {TemplateTokenType.HOTELIDMINUS1,hotelIdMinus1.ToString()},
                                                               {TemplateTokenType.HOTELCOMMONNAME,hotelCommonName},

                                                           }
                                                );
        }

        public virtual string Refine(IPttRequest pttRequest, string htmlSource)
        {
            return htmlSource;
        }

        public IList<string> HotelIds { get; set; }
        public IList<string> HotelCommonNames { get; set; }
        public IList<string> HotelNames { get; set; }


        public string SerializedTourDataRequests { get; set; }
        public ITourInfo TourInfo { get; set; }

        public Query.Query QueryObject { get; set; }

        public CultureInfo CultureInfo
        {
            get
            {
                return new CultureInfo("ru-RU");
            }
        }

        public virtual ITourFactory TourFactory
        {
            get
            {
                return new TourFactory(null, null, null);
            }
        }

        public IOperator Clone()
        {
            var operatorCloned = (OperatorBase)Activator.CreateInstance(this.GetType());
            operatorCloned.SerializedTourDataRequests = this.SerializedTourDataRequests;
            operatorCloned.QueryObject = this.QueryObject.Clone();
            operatorCloned.SerializedHotelListRequests = this.SerializedHotelListRequests;
            operatorCloned.AllHotelsXmlConverter = this.AllHotelsXmlConverter;
            operatorCloned.TourInfo = this.TourInfo.Clone();
            operatorCloned.ExtensiveLoggingNeeded = this.ExtensiveLoggingNeeded;
            //operatorCloned.CultureInfo = this.CultureInfo;
            return operatorCloned;

        }

        public virtual bool HarvestSingleThread { get { return false; } }
        public virtual Tuple<int, int> TryAgainInException
        {
            get { return new Tuple<int, int>(0, 0); }
        }

        protected virtual bool RetryNeededForRequest(string result)
        {
            return false;
        }


        public virtual string QueryNotValid
        {
            get
            {
                if (QueryObject == null) return null;

                if (QueryObject.ChildrenAges != null && QueryObject.ChildrenAges.Count > MaxChildSupported)
                {
                    return string.Format("{0}, en fazla  {1} cocuk icin veri cekebiliyor!", Name, MaxChildSupported);
                }

                return null;
            }
        }
        #endregion

    }

    /// <summary> for hotel list
    /// </summary>
    public abstract partial class OperatorBase
    {
        private const string HotelListXpath = "d/hs/h";
        public IXmlConverter AllHotelsXmlConverter { get; set; }
        public string SerializedHotelListRequests { get; set; }
        public string AllHotelsTemplateFile
        {
            get
            {
                return Helper.AssemblyDirectory + @"\Operators\Query\Templates\HotelList\" + Name + ".xslt";
            }
        }



        public Dictionary<string, string> GetAllHotelNamesAndIds()
        {
            try
            {
                var allHotels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var queryParametersFilled = FillParameters(SerializedHotelListRequests, "0", 0);

                foreach (var pttRequest in QueryRequest(queryParametersFilled))
                {
                    if (_lastRequest != null)
                    {
                        //for successive requests, don't spoof
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    _lastRequest = pttRequest;

                    if (!pttRequest.ConditionSatisfied) continue;
                    if (pttRequest.RequestType == PttRequestType.Init)
                    {
                        var htmlSource = RequestResponseBehavior.GetHtmlMaster(pttRequest, true, _extensiveLoggingNeeded, Name);
                        if (!string.IsNullOrEmpty(htmlSource))
                        {
                            pttRequest.ViewStateValue = Helper.Encode(WebHelper.ExtractViewState(htmlSource));
                            pttRequest.EventValidationValue = Helper.Encode(WebHelper.ExtractEventValidation(htmlSource));
                        }
                    }
                    if (pttRequest.RequestType == PttRequestType.HotelInfo)
                    {
                        //read
                        var hotelsHtml = RequestResponseBehavior.GetHtmlMaster(pttRequest, true, _extensiveLoggingNeeded, Name);
                        if (_extensiveLoggingNeeded)
                        {
                            Logger.LogProcess("GetAllHotelNamesAndIds hotelHtml");
                            Logger.LogProcess(hotelsHtml);
                        }
                        if (string.IsNullOrEmpty(hotelsHtml)) continue;

                        //refine
                        hotelsHtml = RefineHotelNames(hotelsHtml);
                        if (_extensiveLoggingNeeded)
                        {
                            Logger.LogProcess("GetAllHotelNamesAndIds hotelHtml after refine");
                            Logger.LogProcess(hotelsHtml);
                        }

                        //convert
                        var convertedHotelsHtml = hotelsHtml;
                        if (AllHotelsXmlConverter != null)
                        {
                            convertedHotelsHtml = AllHotelsXmlConverter.ToXml(hotelsHtml);
                            if (_extensiveLoggingNeeded)
                            {
                                Logger.LogProcess("GetAllHotelNamesAndIds hotelHtml after conversion");
                                Logger.LogProcess(convertedHotelsHtml);
                            }
                        }
                        if (string.IsNullOrEmpty(convertedHotelsHtml)) return null;
                        //load
                        var html = new HtmlDocument();
                        html.LoadHtml(convertedHotelsHtml);

                        if (html.DocumentNode.ChildNodes == null || html.DocumentNode.ChildNodes.Count == 0)
                        {
                            if (_extensiveLoggingNeeded)
                            {
                                Logger.LogProcess("GetAllHotelNamesAndIds convertedHotelsHtml document childnode count 0");
                            }
                            continue;
                        }
                        if (html.DocumentNode.SelectNodes(HotelListXpath) == null)
                        {
                            if (_extensiveLoggingNeeded)
                            {
                                Logger.LogProcess("GetAllHotelNamesAndIds convertedHotelsHtml selectnodes hotellistxpath none:" + HotelListXpath);
                            }
                            continue;
                        }

                        //iterate
                        foreach (var hotelNode in html.DocumentNode.SelectNodes(HotelListXpath))
                        {
                            string hotelName, hotelId;
                            ParseHotelNameAndIdFromNode(hotelNode, out hotelName, out hotelId);
                            hotelName = Helper.RefineHotelName(hotelName).Trim();
                            if (!allHotels.ContainsKey(hotelName))
                            {
                                allHotels.Add(hotelName, hotelId);
                            }
                        }
                    }
                }
                return allHotels;
            }
            catch (Exception exception)
            {
                Logger.LogExceptions(exception);
            }
            return null;
        }

        protected virtual void ParseHotelNameAndIdFromNode(HtmlNode htmlNode, out string hotelName, out string hotelId)
        {
            hotelName = htmlNode.SelectNodes(".//i[@key='hotelName']")[0].InnerText;
            hotelId = htmlNode.SelectNodes(".//i[@key='hotelId']")[0].InnerText;
        }

        protected virtual string RefineHotelNames(string hotelsHtml)
        {
            return hotelsHtml;
        }

    }
}