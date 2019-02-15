using BMC.Security.Web.Helpers;
using BMC.Security.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BMC.Security.Web
{
    public partial class Scan : System.Web.UI.Page
    {
        static AzureTableHelper DB;
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (DB == null)
            {
                DB = new AzureTableHelper("Absen");
            }
            try
            {
                string strID = Request.QueryString["IDS"];

                //deserialize the object
                //Absen objAbsen = Newtonsoft.Json.JsonConvert.DeserializeObject<Absen>(strJson);
                if (strID != null)
                {
                    var item = new AbsenData() { IDS = strID };
                    item.AssignKey();
                    await DB.InsertDataAbsen(item);
                  Lit1.Text =  (string.Format("Data={0}", strID));
                }
                else
                {
                    Lit1.Text = ("No Data");
                }
            }
            catch (Exception ex)
            {
                Lit1.Text = ("Error :" + ex.Message);
            }

        }
    }
    public class Absen
    {
        public string IDS { get; set; }
    }
}