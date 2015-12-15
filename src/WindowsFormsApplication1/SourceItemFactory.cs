using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using PttLib;

namespace WindowsFormsApplication1
{
    public class SourceItemFactory
    {
        #region BackgroundWorker
        static BackgroundWorker _bw;


        #endregion
        #region Events
        
        public event EventHandler<IList<ListViewItem>> SourceItemsGot;
        public event EventHandler<ListViewItem> SourceItemGot;
        public event EventHandler GettingSourceItemsStopped;
        public event EventHandler ProcessFinished;
        public event EventHandler<string> NoSourceFound;


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
        #endregion

        public void GetSourceItems(IList<string> siteNames, string keyword, int pageStart, int pageEnd, int startingOrder)
        {
            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _bw.DoWork += (obj, e) => GetSourceItemsOnWorker(siteNames, keyword, pageStart, pageEnd, startingOrder, e);
            _bw.ProgressChanged += SingleSourceItemGot;
            _bw.RunWorkerCompleted += GettingSourceItemsFinished;
            _bw.RunWorkerAsync();



        }

        private void GettingSourceItemsFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            OnProcessFinished();
        }

        private void SingleSourceItemGot(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
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
                    OnNoSourceFound(e.UserState.ToString());
                }
                
            }
        }

        private void GetSourceItemsOnWorker(IList<string> siteNames, string keyword, int pageStart, int pageEnd, int startingOrder, DoWorkEventArgs e)
        {
            var siteFactory = new SiteFactory();
            foreach (var siteName in siteNames)
            {
                var allResults = new List<Tuple<string, string>>();
                var site = siteFactory.GetByName(siteName);

                for (var page = pageStart; page <= pageEnd; page++)
                {
                    int pageCount;
                    var results = site.GetItems(keyword, out pageCount, page);
                    if (page > pageCount)
                    {
                        break;
                    }
                    if (results == null)
                    {
                        continue;
                    }
                    if (_bw.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    allResults.AddRange(results);
                }


                if (!allResults.Any())
                {
                    _bw.ReportProgress(100, string.Format("No results found for keyword {0} for pages {1}-{2} for the site {3} ", keyword, pageStart, pageEnd, site.Name));
                    continue;
                }

                var itemIndex = startingOrder;
                var allResultsCount = allResults.Count;
                var blockIndex = 0;
                do
                {
                    var subResults = allResults.Skip(5 * blockIndex).Take(20);
                    if (!subResults.Any()) break;
                    blockIndex++;

                    var listViewItems = new List<ListViewItem>();

                    foreach (var etsyResult in subResults)
                    {

                        var item = site.GetItem(etsyResult.Item1, etsyResult.Item2);
                        if (item != null)
                        {
                            string[] row1 =
                            {
                                item.Id.ToString(), item.Url, item.Title, item.MetaDescription, item.Content,
                                item.Price.ToString(CultureInfo.GetCultureInfo("en-US")),
                                string.Join(",", item.Images), string.Join(",", item.Tags), site.Name, item.WordCount.ToString(),""
                            };

                            var listViewitem = new ListViewItem(itemIndex.ToString());
                            listViewitem.SubItems.AddRange(row1);
                            listViewItems.Add(listViewitem);

                            _bw.ReportProgress((itemIndex - startingOrder) / allResultsCount * 100, listViewitem);

                            itemIndex++;
                        }
                        if (_bw.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }

                    }

                    _bw.ReportProgress((itemIndex - startingOrder) / allResultsCount * 100, listViewItems);
                    
                    if (_bw.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                } while (true);


            }
        }


      
    }
}