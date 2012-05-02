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
		protected override void CreateChildControls()
		{
			// Query and Order project workload list items
			SPList project_documents_list = SPContext.Current.Web.Lists["Project Documents"];
			SPList project_workload_list = SPContext.Current.Web.Lists["Project Workload"];
			

			// Loop in the list items to sort the data
			int year_from = 2010;
			int month_from = 12;
			int year_to = DateTime.Now.Year;
			int month_to = DateTime.Now.Month;
			StringBuilder categories = new StringBuilder();
			for(int year = year_from;year<=year_to;year++)
			{
				int current_month_to = year==year_to?month_to:12;
				for (int month = month_from; month <= current_month_to; month++)
				{
					categories.Append("'");
					categories.Append(year);
					categories.Append("-");
					categories.Append(month);
					categories.Append("'");
					categories.Append(",");
				}
				month_from = 1;
			}
			if (categories.Length > 0) { categories.Remove(categories.Length - 1, 1); }

			string div_id = "div-project-workloard";
			StringBuilder sb = new StringBuilder();
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
			SPFieldChoice document_type = (SPFieldChoice)project_documents_list.Fields["Document Type"];
			foreach (string choice in document_type.Choices)
			{
				sb.Append(			"{"); sb.Append("name: '"); sb.Append(choice);sb.Append("',"); sb.Append("data: [");
				SPQuery query = new SPQuery();
				query.Query = string.Concat(
					"<Where>",
						"<Eq><FieldRef Name='Document Type'/>", "<Value Type='Text'>", choice, "</Value></Eq>",
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
					documents_count.Append(workload["Document Count"]);
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
		}
	}
}
