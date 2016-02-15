using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PttLib;
using PttLib.Helpers;
using WordpressScraper;

namespace WindowsFormsApplication1
{
    [Serializable]
    public sealed class TotalResultsFoundEventArgs : EventArgs
    {
        private readonly string _site;
        private readonly int _totalInPageRange;
        private readonly int _totalForKeyword;


        public TotalResultsFoundEventArgs(string site, int totalInPageRange, int totalForKeyword)
        {
            _site = site;
            _totalInPageRange = totalInPageRange;
            _totalForKeyword = totalForKeyword;
        }

        public string Site { get { return _site; } }
        public int TotalPageInRange { get { return _totalInPageRange; } }
        public int TotalForKeyword { get { return _totalForKeyword; } }
    }

    public class SourceItemFactory
    {
        #region BackgroundWorker
        static List<BackgroundWorker> _bws;
        private static object _lock;
        private int _workersCount;

        #endregion
        #region Events

        public event EventHandler<IList<ListViewItem>> SourceItemsGot;
        public event EventHandler<ListViewItem> SourceItemGot;
        public event EventHandler GettingSourceItemsStopped;
        public event EventHandler ProcessFinished;
        public event EventHandler<string> NoSourceFound;
        public event EventHandler<int> PageParsed;
        public event EventHandler<Exception> ExceptionOccured;
        public event EventHandler<TotalResultsFoundEventArgs> TotalResultsFound;

        public void OnTotalResultsFound(TotalResultsFoundEventArgs e)
        {
            EventHandler<TotalResultsFoundEventArgs> handler = TotalResultsFound;
            if (handler != null) handler(this, e);
        }


        protected virtual void OnProcessFinished()
        {
            var handler = ProcessFinished;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnSourceItemsGot(IList<ListViewItem> e)
        {
            var handler = SourceItemsGot;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnSourceItemGot(ListViewItem e)
        {
            var handler = SourceItemGot;
            if (handler != null) handler(this, e);
        }
        protected virtual void OnExceptionOccured(Exception e)
        {
            var handler = ExceptionOccured;
            if (handler != null) handler(this, e);
        }
        protected virtual void OnGettingSourceItemsStopped()
        {
            var handler = GettingSourceItemsStopped;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        protected virtual void OnNoSourceFound(string e)
        {
            var handler = NoSourceFound;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnPageParsed(int e)
        {
            var handler = PageParsed;
            if (handler != null) handler(this, e);
        }
        #endregion

        public void GetSourceItems(IList<string> siteNames, string keyword, int pageStart, int pageEnd, int startingOrder, HashSet<string> existingIds)
        {
            _lock = new Object();
            _bws = new List<BackgroundWorker>();
            _workersCount = siteNames.Count;
            _itemIndex = startingOrder;

            foreach (var siteName in siteNames)
            {
                var bw = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                string name = siteName;

                bw.DoWork += (obj, e) => GetSourceItemsOnWorker(bw, name, keyword, pageStart, pageEnd, e, existingIds);
                bw.ProgressChanged += SingleSourceItemGot;
                bw.RunWorkerCompleted += GettingSourceItemsFinished;
                _bws.Add(bw);
                bw.RunWorkerAsync();

            }

        }

        private void GettingSourceItemsFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error!=null)
            {
                OnExceptionOccured(e.Error);
            }

            if (e.Cancelled)
            {
                OnGettingSourceItemsStopped();
            }
            Interlocked.Decrement(ref _workersCount);
            if (_workersCount == 0)
            {
                OnProcessFinished();
            }

        }

        private void SingleSourceItemGot(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                var totalResultsFoundEventArgs = e.UserState as TotalResultsFoundEventArgs;
                if (totalResultsFoundEventArgs != null)
                {
                    OnTotalResultsFound(totalResultsFoundEventArgs);
                }
                var lvItem = e.UserState as ListViewItem;
                if (lvItem != null)
                {
                    OnSourceItemGot(lvItem);
                    return;
                }
                var lvItemList = e.UserState as List<ListViewItem>;
                if (lvItemList != null)
                {
                    OnSourceItemsGot(lvItemList);
                    return;
                }

                if (!string.IsNullOrEmpty(e.UserState.ToString()))
                {
                    int count;
                    if (Int32.TryParse(e.UserState.ToString(), out count))
                    {
                        OnPageParsed(count);
                        return;
                    }
                    OnNoSourceFound(e.UserState.ToString());
                }

            }
        }

        public void CancelGettingSource()
        {
            if (_bws == null) return;
            foreach (var bw in _bws)
            {
                if (bw.IsBusy)
                {
                    bw.CancelAsync();
                }

            }
        }

        private int _itemIndex = 0;
        private void GetSourceItemsOnWorker(BackgroundWorker bw, string siteName, string keyword, int pageStart, int pageEnd, DoWorkEventArgs e, HashSet<string> existingIds)
        {
            var siteFactory = new SiteFactory();
            var relevanceCalculater = new RelevanceCalculator();

            var allResults = new List<Tuple<string, string, string>>();
            var site = siteFactory.GetByName(siteName);
            int totalItemCount = 0;
            for (var page = pageStart; page <= pageEnd; page++)
            {
                int pageCount;

                var results = site.GetItems(keyword, out pageCount, out totalItemCount, page);
                if (page > pageCount)
                {
                    break;
                }
                if (results == null)
                {
                    continue;
                }
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                allResults.AddRange(results);
            }

            if (!allResults.Any())
            {
                if (!e.Cancel)
                {
                    bw.ReportProgress(100, string.Format("No results found for keyword {0} for pages {1}-{2} for the site {3} ", keyword, pageStart, pageEnd, site.Name));
                }
                return;
            }
            bw.ReportProgress(100, allResults.Count);
            bw.ReportProgress(100, new TotalResultsFoundEventArgs(siteName, allResults.Count, totalItemCount));

            var allResultsCount = allResults.Count;
            var blockIndex = 0;
            do
            {
                var subResults = allResults.Skip(site.BlockSize * blockIndex).Take(site.BlockSize);
                if (!subResults.Any()) break;
                blockIndex++;

                var listViewItems = new List<ListViewItem>();

                var tasks = new List<Task<int>>();

                foreach (var result in subResults)
                {
                    var fk = site.Name +"_"+site.GetId(result.Item2);
                    if (existingIds != null)
                    {
                        if (existingIds.Contains(fk))
                        {
                            continue;
                        }
                    }

                    tasks.Add(new Task<int>(() =>
                                                {

                                                    var item = site.GetItem(result.Item1, result.Item2, result.Item3);
                                                    if (item != null)
                                                    {
                                                        var relevance = relevanceCalculater.GetRelevance(item, keyword);
                                                        string[] row1 =
                                                            {
                                                                item.Id.ToString(), item.Url, item.Title,
                                                                item.MetaDescription, item.Content,
                                                                item.Price.ToString(CultureInfo.GetCultureInfo("en-US"))
                                                                ,
                                                                string.Join(",",
                                                                            item.ItemImages.Select(
                                                                                ii => ii.OriginalSource)),
                                                                string.Join(",", item.Tags), site.Name,
                                                                item.WordCount.ToString(new CultureInfo("en-US")),
                                                                item.Created.ToString("dd-MMM-yyyy",
                                                                                      new CultureInfo("en-US")),
                                                                relevance.ToString(),
                                                                ""
                                                            };
                                                        lock (_lock)
                                                        {

                                                            var listViewitem = new ListViewItem("");
                                                            listViewitem.SubItems.AddRange(row1);
                                                            listViewItems.Add(listViewitem);

                                                            bw.ReportProgress(_itemIndex / allResultsCount * 100,
                                                                              listViewitem);

                                                            _itemIndex++;
                                                        }
                                                    }
                                                    return 1;

                                                }));


                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                }
                foreach (var task in tasks)
                {
                    task.Start();
                }
                Task.WaitAll(tasks.ToArray());

                lock (_lock)
                {
                    bw.ReportProgress(_itemIndex / allResultsCount * 100, listViewItems);
                }

                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

            } while (true);

        }


       
    }
}