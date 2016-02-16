using System.Collections.Generic;
using System.Linq;

namespace PttLib
{
    public class SiteFactory
    {
        private List<Site> _sites = new List<Site>()
                                       {
                                           new Etsy(),
                                           new Dawanda(),
                                           new DeDawanda(),
                                           new Artfire(),
                                           new OverStock(),
                                           new Bonanza()
                                       };
        public List<Site> GetAll
        {
            get { return _sites; }
        }
        public Site GetByName(string name)
        {
             return _sites.FirstOrDefault(s => s.Name == name); 
        }

        public List<string> GetNames
        {
            get { return _sites.Select(s => s.Name).ToList(); }
        }
    }
}