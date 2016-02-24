using WpsLib.Sites;

namespace PttLib
{
    public class Etsy : Site
    {
        public override string Name
        {
            get
            {
                return "Etsy";
            }
        }

        public override string ItemsXPath
        {
            get
            {
                return "//div[@class='buyer-card card']/a";
            }
        }

        
    }
}