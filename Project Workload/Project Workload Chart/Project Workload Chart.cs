using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Collections.Generic;

namespace Project_Workload.Project_Workload_Chart
{
	[ToolboxItemAttribute(false)]
	public class Project_Workload_Chart : WebPart
	{
		int _year_from = 2012;
		int _month_from = 1;
		protected override void CreateChildControls()
		{
			// Query and Order project workload list items
			SPList project_documents_list = SPContext.Current.Web.Lists["项目文档库"];
			SPList project_workload_list = SPContext.Current.Web.Lists["项目工作量"];
			

			// Loop in the list items to sort the data
			int year_to = DateTime.Now.Year;
			int month_to = DateTime.Now.Month;
			StringBuilder categories = new StringBuilder();
			for(int year = _year_from;year<=year_to;year++)
			{
				int current_month_to = year==year_to?month_to:12;
				for (int month = _month_from; month <= current_month_to; month++)
				{
					categories.Append("'");
					categories.Append(year);
					categories.Append("-");
					categories.Append(month);
					categories.Append("'");
					categories.Append(",");
				}
				_month_from = 1;
			}
			if (categories.Length > 0) { categories.Remove(categories.Length - 1, 1); }

			string div_id = "div-project-workloard";
			string component_lib = "/admin/DocLib";
			
			StringBuilder sb = new StringBuilder();
			sb.Append("<script type='text/javascript' src='" + component_lib + "/highcharts/highcharts.js'></script>");
			sb.Append("<div id='"); sb.Append(div_id); sb.Append("'></div>");
			sb.Append("<script>");
			sb.Append("var chart;");
			sb.Append("$(document).ready(function() {");
			sb.Append("		chart = new Highcharts.Chart({");
			sb.Append("		chart: {");
			sb.Append("			renderTo: '"); sb.Append(div_id); sb.Append("',");
			sb.Append("			type: 'column'");
			sb.Append("		},");
			sb.Append("		title: {");
			sb.Append("			text: 'Project Workload'");
			sb.Append("		},");
			sb.Append("		credits: {");
			sb.Append("			enabled: false");
			sb.Append("		},");
			sb.Append("		xAxis: {");
			sb.Append("			categories: ["); sb.Append(categories.ToString()); sb.Append("]");
			sb.Append("		},");
			sb.Append("		yAxis: {");
			sb.Append("			min: 0,");
			sb.Append("			title: {");
			sb.Append("				text: 'Number of docs'");
			sb.Append("			},");
			sb.Append("			stackLabels: {");
			sb.Append("				enabled: true,");
			sb.Append("				style: {");
			sb.Append("					fontWeight: 'bold',");
			sb.Append("					color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'");
			sb.Append("				}");
			sb.Append("			}");
			sb.Append("		},");
			sb.Append("		legend: {");
			sb.Append("			align: 'right',");
			sb.Append("			x: -100,");
			sb.Append("			verticalAlign: 'top',");
			sb.Append("			y: 20,");
			sb.Append("			floating: true,");
			sb.Append("			backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColorSolid) || 'white',");
			sb.Append("			borderColor: '#CCC',");
			sb.Append("			borderWidth: 1,");
			sb.Append("			shadow: false");
			sb.Append("		},");
			sb.Append("		tooltip: {");
			sb.Append("			formatter: function() {");
			sb.Append("				return '<b>'+ this.x +'</b><br/>'+");
			sb.Append("					this.series.name +': '+ this.y +'<br/>'+");
			sb.Append("					'Total: '+ this.point.stackTotal;");
			sb.Append("			}");
			sb.Append("		},");
			sb.Append("		plotOptions: {");
			sb.Append("			column: {");
			sb.Append("				stacking: 'normal',");
			sb.Append("				dataLabels: {");
			sb.Append("					enabled: true,");
			sb.Append("					color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'");
			sb.Append("				}");
			sb.Append("			}");
			sb.Append("		},");
			sb.Append("		series: [");
			SPFieldChoice document_type = (SPFieldChoice)project_documents_list.Fields["文档类型"];
			foreach (string choice in document_type.Choices)
			{
				sb.Append(			"{"); sb.Append("name: '"); sb.Append(choice);sb.Append("',"); sb.Append("data: [");
				SPQuery query = new SPQuery();
				query.Query = string.Concat(
					"<Where>",
						"<Eq><FieldRef Name='ProjectStatus'/>", "<Value Type='Text'>", choice, "</Value></Eq>",
					"</Where>",
					"<OrderBy>",
						"<FieldRef Name='Written Year'/>",
						"<FieldRef Name='Written Month'/>",
					"</OrderBy>"
					);
				SPListItemCollection workloads = project_workload_list.GetItems(query);
				StringBuilder documents_count = new StringBuilder();
				foreach (SPListItem workload in workloads)
				{
					documents_count.Append(workload["文档数量"]);
					documents_count.Append(",");
				}
				if (documents_count.Length > 0) { documents_count.Remove(documents_count.Length - 1, 1); }
				sb.Append(documents_count.ToString()); sb.Append("]");
				sb.Append("},");
			}
			if (sb[sb.Length - 1] == ',') sb.Remove(sb.Length - 1, 1);
			sb.Append("		]");
			sb.Append("	});");
			sb.Append("});");
			sb.Append("</script>");
			this.Controls.Add(new LiteralControl(sb.ToString()));

			Button b = new Button();
			b.Text = "刷新数据（全）";
			b.Click += new EventHandler(reload_data);
			this.Controls.Add(b);
		}

		void reload_data(object sender, EventArgs e)
		{
			try
			{
				SPWeb web = SPContext.Current.Web;
				SPList project_workload_list = web.Lists["项目工作量"];
				SPList project_documents_list = web.Lists["项目文档库"];
				for (int i = 0; i < project_workload_list.Items.Count; i++)
				{
					project_workload_list.Items.Delete(0);
				}
				SPFieldChoice document_type = (SPFieldChoice)project_documents_list.Fields["文档类型"];
				int year_to = DateTime.Now.Year;
				int month_to = DateTime.Now.Month;
				for (int year = _year_from; year <= year_to; year++)
				{
					int current_month_to = year == year_to ? month_to : 12;
					for (int month = _month_from; month <= current_month_to; month++)
					{
						foreach (string choice in document_type.Choices)
						{
							SPListItem workload = project_workload_list.Items.Add();
							workload["编写年份"] = year;
							workload["编写月份"] = month;
							workload["ProjectStatus"] = choice;
							workload["文档数量"] = 0;
							workload.Update();
						}
					}
					_month_from = 1;
				}
				//SPListItemCollection project_workload_items = project_workload_list.Items;
				SPListItemCollection project_documents_items = project_documents_list.Items;
				foreach (SPListItem document in project_documents_items)
				{
					DateTime wd = DateTime.Parse(document["编写日期"].ToString());
					SPQuery query = new SPQuery();
					query.Query = string.Concat(
						"<Where><And>",
							"<And>",
								"<Eq><FieldRef Name='Written Year'/>", "<Value Type='Integer'>", wd.Year, "</Value></Eq>",
								"<Eq><FieldRef Name='Written Month'/>", "<Value Type='Integer'>", wd.Month, "</Value></Eq>",
							"</And>",
							"<Eq><FieldRef Name='ProjectStatus'/>", "<Value Type='Text'>", document["文档类型"], "</Value></Eq>",
						"</And></Where>"
						);
					SPListItemCollection workloads = project_workload_list.GetItems(query);
					if (workloads.Count == 0)
					{
						throw new Exception(query.Query + ". Can not be found!");
					}
					else
					{
						SPListItem workload = workloads[0];
						workload["文档数量"] = int.Parse(workload["文档数量"].ToString()) + 1;
						workload.Update();
					}
				}
			}
			catch (Exception ex)
			{
				log(SPContext.Current.Site, "项目工作量刷新", "错误", ex.ToString());
			}
		}
		protected void log(SPSite site, string title, string type, string content)
		{
			using (SPWeb web = site.OpenWeb("/admin"))
			{
				SPList log_list = web.Lists["系统日志"];
				SPListItem log_item = log_list.AddItem();
				log_item["Title"] = title;
				log_item["日志类型"] = type;
				log_item["日志详情"] = content;
				log_item["相关系统"] = "请假管理";
				log_item.Update();
			}
		}

	}
}
