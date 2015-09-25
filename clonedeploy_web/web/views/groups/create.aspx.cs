﻿using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Helpers;
using Models;
using Security;


namespace views.groups
{
    public partial class GroupCreate : BasePages.Groups
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (IsPostBack) return;
            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
            PopulateForm();
        }

        protected void PopulateForm()
        {
            ddlGroupImage.DataSource = new BLL.Image().SearchImages("").Select(i => new { i.Id, i.Name });
            ddlGroupImage.DataValueField = "Id";
            ddlGroupImage.DataTextField = "Name";
            ddlGroupImage.DataBind();
            ddlGroupImage.Items.Insert(0, new ListItem("Select Image", "0"));

        
         
        }

     

    

        protected void Submit_Click(object sender, EventArgs e)
        {
            var group = new Models.Group();

        
            group.Name = txtGroupName.Text;
            group.Description = txtGroupDesc.Text;
            group.Image = Convert.ToInt32(ddlGroupImage.SelectedValue);
            group.SenderArguments = txtGroupSenderArgs.Text;
            group.Type = ddlGroupType.Text;
            group.ImageProfile = group.Image == 0 ? 0 : Convert.ToInt32(ddlImageProfile.SelectedValue);
          
            var bllGroup = new BLL.Group();
            if (bllGroup.ValidateGroupData(group)) bllGroup.AddGroup(group);

            if (Message.Text.Contains("Successfully"))
                Response.Redirect("~/views/groups/edit.aspx?groupid=" + group.Id);

        }


        protected void ddlGroupImage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGroupImage.Text == "Select Image") return;
            ddlImageProfile.DataSource = new BLL.LinuxProfile().SearchProfiles(Convert.ToInt32(ddlGroupImage.SelectedValue)).Select(i => new { i.Id, i.Name });
            ddlImageProfile.DataValueField = "Id";
            ddlImageProfile.DataTextField = "Name";
            ddlImageProfile.DataBind();

        }
    }
}