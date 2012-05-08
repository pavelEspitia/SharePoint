using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;

namespace Customized_Fields
{
	public class Managers_Field:SPFieldUser
	{
		public Managers_Field(SPFieldCollection fields, string fName) : base(fields, fName) { this.AllowMultipleValues = true; }
		public Managers_Field(SPFieldCollection fields, string tName, string dName) : base(fields, tName, dName) {  this.AllowMultipleValues = true; }
		public override string DefaultValue
		{
			get {
				SPWeb web = SPContext.Current.Web;
				SPUser user = web.CurrentUser;


				// 先从当前用户所参与的项目中找项目经理作为默认的审核人。
				SPWeb project_web = web.Site.OpenWeb("/projects");
				SPList project_list = project_web.Lists["项目列表"];

				SPQuery query = new SPQuery();
				query.ViewFields="<FieldRef Name='ProjectManager' />";
				StringBuilder sb = new StringBuilder();
				sb.Append("<Where>");
				if (user.Groups.Count > 0) {build_group_where(user, sb, 0);}
				else { build_user_where(user, sb); }
				sb.Append("</Where>");
				query.Query = sb.ToString();

				string defaultValue = "";
				SPListItemCollection projects = project_list.GetItems(query);
				foreach (SPListItem project in projects)
				{
					//SPFieldUserValue field_user_value = (SPFieldUserValue)project.Fields["项目经理"].GetFieldValue(project["ProjectManager"].ToString());
					//defaultValue += string.Format("{0};#{1}", field_user_value.User.ID.ToString(), field_user_value.User.Name);
					defaultValue += project["ProjectManager"].ToString()+";#";
				}

				// 如果当前用户没有参加任何项目，则找其直接的主管经理。
				if (defaultValue == ""||defaultValue==";#")
				{
					// Initialize user profile config manager object 
					UserProfileManager upm = new UserProfileManager(SPServiceContext.Current);
					if (upm.UserExists(user.LoginName))
					{
						UserProfile u = upm.GetUserProfile(user.LoginName);
						UserProfile[] managers = u.GetManagers();
						if (managers != null)
						{
							foreach(UserProfile manager in managers){
								SPUser u_manager = web.SiteUsers[manager[PropertyConstants.AccountName].Value.ToString()];
								defaultValue += string.Format("{0};#{1}", u_manager.ID.ToString(), u_manager.Name)+";#";
							}
						}
					}
				}
				if (defaultValue == ";#" || defaultValue == "")
				{
					defaultValue = "";
				}
				else
				{
					defaultValue = defaultValue.Substring(0, defaultValue.Length - 2);
				}
				return defaultValue;
			}
		}

		/// <summary>
		/// 在当前用户属于某个用户组的时候，构造 Where 查询 CAML 字符串。
		/// </summary>
		/// <param name="user">用户</param>
		/// <param name="sb">字符串</param>
		/// <param name="begin_index">起始用户组的索引下标</param>
		protected void build_group_where(SPUser user, StringBuilder sb,int begin_index)
		{
			if (begin_index >= user.Groups.Count) return;
			sb.Append("<Or>");
			int i = 0;
			for(i=begin_index;i<Math.Min(1+begin_index,user.Groups.Count);i++){
				sb.Append("<Contains>");
				sb.Append("<FieldRef Name='ProjectMembers' LookupId='TRUE' />");
				sb.Append("<Value Type='User'>"); sb.Append(user.Groups[i].ID); sb.Append("</Value>");
				sb.Append("</Contains>");
				if (begin_index == (user.Groups.Count - 1))
				{
					build_user_where(user, sb);
				}
			}
			build_group_where(user, sb, i);
			sb.Append("</Or>");
		}

		/// <summary>
		/// 为当前用户构造 Where 查询 CAML 字符串。
		/// </summary>
		/// <param name="user">用户</param>
		/// <param name="sb">字符串</param>
		protected void build_user_where(SPUser user, StringBuilder sb)
		{
			sb.Append("<Contains>");
			sb.Append("<FieldRef Name='ProjectMembers' LookupId='TRUE' />");
			sb.Append("<Value Type='User'>"); sb.Append(user.ID); sb.Append("</Value>");
			sb.Append("</Contains>");
		}
	}
}