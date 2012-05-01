using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Collections.Generic;
using System.Xml;

namespace News_Events_Receiver.SitePage_Events_Receiver
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class SitePage_Events_Receiver : SPItemEventReceiver
    {
       /// <summary>
       /// 已添加项.
       /// </summary>
       public override void ItemAdded(SPItemEventProperties properties)
       {
           base.ItemAdded(properties);
           update_summary(properties);
       }

       /// <summary>
       /// 已更新项.
       /// </summary>
       public override void ItemUpdated(SPItemEventProperties properties)
       {
               base.ItemUpdated(properties);
               update_summary(properties);
           //bool is_tag_begin = false;
           //string tag_type = "";
           //Dictionary<string,string> tag_properties = new Dictionary<string, string>();
           //foreach (char c in content)
           //{
           //    if (c == '>') { is_tag_begin = false; }
           //    if (is_tag_begin)
           //    {
           //        if (c == ' ')
           //        {
           //        }
           //        else
           //        {
           //            tag_type += c;
           //        }
           //    }
           //    if (c == '<') { is_tag_begin = true; }
           //}
       }

       /// <summary>
       /// 已删除项.
       /// </summary>
       public override void ItemDeleted(SPItemEventProperties properties)
       {
           base.ItemDeleted(properties);
		   delete_summary(properties);
       }

       protected void update_summary(SPItemEventProperties properties)
       {

           SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               using (SPSite site = new SPSite(properties.SiteId))
               {
                   using (SPWeb web = site.OpenWeb())
                   {
                       string summary = null, image_url = null, title = null, type = null;
                       try
                       {
                           SPListItem page = properties.ListItem;
						   if (page.GetFormattedValue("_ModerationStatus") != "已批准")
						   {
							   delete_summary(properties);
							   return;
						   }
                           string content = page["Wiki 内容"].ToString();

                           XmlDocument xml_doc = new XmlDocument();
                           xml_doc.LoadXml(content);
                           XmlNode node = xml_doc.SelectSingleNode("descendant::div[contains(@class,'ms-rteElement-Callout2')]");
                           if (node != null) summary = node.InnerText;
                           node = xml_doc.SelectSingleNode("descendant::img[contains(@class,'ms-rteImage-2')]");
                           if (node != null) image_url = node.Attributes["src"].Value;
                           node = xml_doc.SelectSingleNode("descendant::h3[contains(@class,'ms-rteElement-H3B')]");
                           if (node != null) title = node.InnerText;
                           node = xml_doc.SelectSingleNode("descendant::div[contains(@class,'ms-rteElement-Callout1')]");
                           if (node != null) type = node.InnerText;
                       }
                       catch (Exception ex)
                       {
                           log(site, "Wiki Page ID:" + properties.ListItemId + " 解析内容时发生错误。" , "错误", ex.ToString());
                       }
                       if (summary != null && image_url != null && title != null && type != null)
                       {
                           try
                           {
                               SPList summary_list = web.Lists["新闻摘要"];
                               SPQuery query = new SPQuery();
                               query.Query = "<Where><Eq><FieldRef Name='_x65b0__x95fb_ID' /><Value Type='Text'>" + properties.ListItemId + "</Value></Eq></Where>";
                               SPListItem summary_item = null;
                               SPListItemCollection summary_items = summary_list.GetItems(query);
                               if (summary_items != null && summary_items.Count > 0) { summary_item = summary_items[0]; }
                               else { summary_item = summary_list.AddItem(); }
                               summary_item["Title"] = title;
                               summary_item["图片"] = image_url;
                               summary_item["摘要"] = summary;
                               summary_item["分类"] = type;
							   summary_item["新闻链接"] = properties.ListItem.Url;
                               summary_item["新闻ID"] = properties.ListItemId;
                               summary_item.Update();
                               log(site, "Wiki Page ID:" + properties.ListItemId + " 【"+title+"】摘要已经更新。", "消息", "摘要更新完成。");
                           }
                           catch (Exception ex)
                           {
                               if (site != null)
                               {
                                   log(site, "Wiki Page ID:" + properties.ListItemId + " 写入新闻摘要时出错。", "警告", ex.ToString());
                               }
                           }
                       }
                       else
                       {
						   log(site, "Wiki Page ID:" + properties.ListItemId + " 未能包含足够的新闻要素。", "警告", "未能包含足够的新闻要素");
                       }
                   }
               }
           });
       }
		protected void delete_summary(SPItemEventProperties properties){
			
           SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               using (SPSite site = new SPSite(properties.SiteId))
               {
                   using (SPWeb web = site.OpenWeb())
                   {
                       SPList summary_list = web.Lists["新闻摘要"];
                       SPQuery query = new SPQuery();
                       query.Query = "<Where><Eq><FieldRef Name='_x65b0__x95fb_ID' /><Value Type='Text'>" + properties.ListItemId + "</Value></Eq></Where>";
                       SPListItemCollection summary_items = summary_list.GetItems(query);
                       if (summary_items != null && summary_items.Count > 0) { 
						   summary_list.Items.DeleteItemById(summary_items[0].ID); 
							log(site, "Wiki Page ID:" + properties.ListItemId + "摘要已经删除。", "消息", "摘要已经删除，可能是原文被删除，或者，未能通过审批。");
					   }
                   }
               }
           });
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
               log_item["相关系统"] = "新闻中心";
               log_item.Update();
           }
       }
    }
}
