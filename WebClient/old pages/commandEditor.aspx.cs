using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebClient
{
    public partial class commandEditor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void actionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            FormViewAction.ChangeMode(FormViewMode.Insert);
        }

        protected void FormViewAction_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
             
        }

        protected void FormViewAction_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            ListBox1.DataBind();
        }

        protected void FormViewAction_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            ListBox1.DataBind();
        }

        protected void FormViewAction_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            ListBox1.DataBind();
        }
    }
}