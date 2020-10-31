using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebClient
{
    public partial class ObjectEditor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            FormViewItem.ChangeMode(FormViewMode.Insert);
        }

        protected void FormViewItem_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            DataBind();
        }

        protected void FormViewItem_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            DataBind();
        }

        protected void FormViewItem_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            DataBind();
        }
    }
}