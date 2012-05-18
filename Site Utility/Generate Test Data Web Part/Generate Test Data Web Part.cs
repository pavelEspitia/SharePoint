using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;

namespace Site_Utility.Generate_Test_Data_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Generate_Test_Data_Web_Part : WebPart
	{
		protected int _count = 10000;
		protected string _names = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		protected string[] _sex = {"-","男","女"};
		protected override void CreateChildControls()
		{
			SPWeb web = SPContext.Current.Web;
			SPList list = web.Lists["人物列表"];
			Label l = new Label();
			l.ID = "labelCount";
			//l.Text = "人物列表：" + list.Items.Count + " 条";
			this.Controls.Add(l);
			Button btGenerate = new Button();
			btGenerate.Text = "生成数据（" + _count + " 条）";
			btGenerate.Click+=new EventHandler(btGenerate_Click);
			this.Controls.Add(btGenerate);

			this.Controls.Add(new LiteralControl("<hr>"));
			Button btClear = new Button();
			btClear.Text = "清除所有数据";
			btClear.Click+=new EventHandler(btClear_Click);
			this.Controls.Add(btClear);

			Button btClearCount = new Button();
			btClearCount.Text = "清除 "+_count+" 条数据";
			btClearCount.Click+=new EventHandler(btClearCount_Click);
			this.Controls.Add(btClearCount);
		}
		private StringBuilder BuildBatchDeleteCommand(SPList spList)
		{
			StringBuilder sbDelete = new StringBuilder();
			sbDelete.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Batch>");
			string command = "<Method><SetList Scope=\"Request\">" + spList.ID +
				"</SetList><SetVar Name=\"ID\">{0}</SetVar><SetVar Name=\"Cmd\">Delete</SetVar></Method>";

			foreach (SPListItem item in spList.Items)
			{
				sbDelete.Append(string.Format(command, item.ID.ToString()));
			}
			sbDelete.Append("</Batch>");
			return sbDelete;
		}  
		void btClearCount_Click(object sender, EventArgs e)
		{
			SPWeb web = SPContext.Current.Web;
			SPList list = web.Lists["人物列表"];
			StringBuilder sbDelete = BuildBatchDeleteCommand(list);
			web.ProcessBatchData(sbDelete.ToString());  

			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i].ID == "labelCount")
				{
					Label l = this.Controls[i] as Label;
					l.Text = "人物列表：" + list.Items.Count + " 条";
				}
			}
		}
		void btClear_Click(object sender, EventArgs e)
		{
			SPWeb web = SPContext.Current.Web;
			SPList list = web.Lists["人物列表"];
			StringBuilder sbDelete = BuildBatchDeleteCommand(list);
			web.ProcessBatchData(sbDelete.ToString());  

			for (int i = 0; i < this.Controls.Count; i++)
			{
				if (this.Controls[i].ID == "labelCount")
				{
					Label l = this.Controls[i] as Label;
					l.Text = "人物列表：" + list.Items.Count + " 条";
				}
			}
		}
		void btGenerate_Click(object sender, EventArgs e)
		{
			((Button)sender).Enabled = false;
			SPWeb web = SPContext.Current.Web;
			SPList list = web.Lists["人物列表"];
			Random random = new Random();
			for (int i = 0; i < _count; i++)
			{
				SPListItem item = list.AddItem();
				item["Title"] = generate_name(random,i);
				item["性别"] = generate_sex(random);
				item["职位"] = generate_title(random);
				item.Update();
			}
			((Button)sender).Enabled = true;
			for (int i = 0; i < this.Controls.Count;i++ )
			{
				if (this.Controls[i].ID == "labelCount")
				{
					Label l = this.Controls[i] as Label;
					l.Text = "人物列表：" + list.Items.Count + " 条";
				}
			}
		}

		protected string generate_name(Random random, int i)
		{
			string n = _names[random.Next(_names.Length)].ToString();
			return n + n + n + " - " + i.ToString();
		}
		protected string generate_sex(Random random){
			return _sex[random.Next(_sex.Length)];
		}
		protected string generate_title(Random random)
		{
			return _names[random.Next(_names.Length)].ToString();
		}
	}
}
