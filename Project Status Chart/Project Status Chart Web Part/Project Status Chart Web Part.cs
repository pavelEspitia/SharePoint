using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;

namespace Project_Status_Chart.Project_Status_Chart_Web_Part
{
	[ToolboxItemAttribute(false)]
	public class Project_Status_Chart_Web_Part : WebPart
	{
		protected override void CreateChildControls()
		{
			// Query and Order project workload list items
			SPList project_status_list = SPContext.Current.Web.Lists["Project Status"];

			// Loop in the list items to sort the data
			int year_from = 2012;	// TODO: You can query the min Year from the Project Status list, instead of this fix number
			int month_from = 1;		// TODO: You can query the min Month from the Project Status list, instead of this fix number
			int year_to = DateTime.Now.Year;
			int month_to = DateTime.Now.Month;
			StringBuilder categories = new StringBuilder();
			for (int year = year_from; year <= year_to; year++)
			{
				int current_month_to = year == year_to ? month_to : 12;
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

			string div_id = "div-project-status-chart";
			StringBuilder sb = new StringBuilder();
			sb.Append("<script src='/Component/jquery/jquery-1.6.2.min.js' type='text/javascript'></script>");
			sb.Append("<script src='/Component/highcharts/highcharts.js' type='text/javascript'></script>");
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
			sb.Append("			text: 'Project Status'");
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
			sb.Append("				text: 'Number of Porjects'");
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
			SPFieldChoice status = (SPFieldChoice)project_status_list.Fields["Status"];
			foreach (string state in status.Choices)
			{
				sb.Append("{"); sb.Append("name: '"); sb.Append(state); sb.Append("',"); sb.Append("data: [");
				StringBuilder status_count = new StringBuilder();
				for (int year = year_from; year <= year_to; year++)
				{
					int current_month_to = year == year_to ? month_to : 12;
					for (int month = month_from; month <= current_month_to; month++)
					{
						SPQuery query = new SPQuery();
						query.Query = string.Concat(
							"<Where><And>",
								"<Eq><FieldRef Name='ProjectStatus'/>", "<Value Type='Choice'>", state, "</Value></Eq>",
								"<And>",
								"<Geq><FieldRef Name='ProjectStatusDate'/>", "<Value Type='DateTime'>", year+"-"+month+"-1", "</Value></Geq>",
								"<Lt><FieldRef Name='ProjectStatusDate'/>", "<Value Type='DateTime'>", year+"-"+(month==12?1:month+1)+"-1", "</Value></Gt>",
								"</And>",
							"</And></Where>",
							"<OrderBy>",
								"<FieldRef Name='StatusRelatedProject'/>",
								"<FieldRef Name='ProjectStatusDate' Ascending='FALSE'/>",
							"</OrderBy>"
						);
						SPListItemCollection current_state_items = project_status_list.GetItems(query);
						status_count.Append(current_state_items.Count);
						status_count.Append(",");
					}
					month_from = 1;
				}
				
				if (status_count.Length > 0) { status_count.Remove(status_count.Length - 1, 1); }
				sb.Append(status_count.ToString()); sb.Append("]");
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
