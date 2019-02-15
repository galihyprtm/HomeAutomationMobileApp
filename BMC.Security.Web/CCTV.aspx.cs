using BMC.Security.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BMC.Security.Web
{
    public partial class CCTV : System.Web.UI.Page
    {
        static AzureTableHelper tablehelper;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (tablehelper == null) tablehelper = new AzureTableHelper("cctv");
            if (!IsPostBack) RefreshGrid();
        }
        async void RefreshGrid()
        {
            var datas = await tablehelper.GetCCTVData();
            GvData.DataSource = datas;
            GvData.DataBind();

            if (GvData.Rows.Count > 0)
            {
                //This replaces <td> with <th>    
                GvData.UseAccessibleHeader = true;
                //This will add the <thead> and <tbody> elements    
                GvData.HeaderRow.TableSection = TableRowSection.TableHeader;
                //This adds the <tfoot> element. Remove if you don't have a footer row    
                GvData.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}