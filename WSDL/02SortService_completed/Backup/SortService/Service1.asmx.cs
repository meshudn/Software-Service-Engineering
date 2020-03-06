using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace SortService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public int[] Sort(int[] arr)
        {
            Array.Sort(arr);
            return arr;
        }
    }
}