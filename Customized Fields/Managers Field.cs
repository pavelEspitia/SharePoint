using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

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
				string defaultValue = string.Format("{0};#{1}", user.ID.ToString(), user.Name);

				if (this.SelectionGroup > 0)
				{
					SPGroup group = web.Groups[this.SelectionGroup];
					if ((group != null) && (group.ContainsCurrentUser))
						return defaultValue;
				}
				else
				{
					return defaultValue;
				}
				return string.Empty;
			}
		}
	}
}
