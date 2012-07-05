using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Workflow.SeqWorkflow {
	public sealed partial class SeqWorkflow {
		#region Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent() {
			this.CanModifyActivities = true;
			System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
			System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
			this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
			this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
			// 
			// logToHistoryListActivity1
			// 
			this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
			this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
			this.logToHistoryListActivity1.HistoryDescription = "";
			this.logToHistoryListActivity1.HistoryOutcome = "";
			this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
			this.logToHistoryListActivity1.OtherData = "";
			this.logToHistoryListActivity1.UserId = -1;
			activitybind2.Name = "SeqWorkflow";
			activitybind2.Path = "workflowId";
			// 
			// onWorkflowActivated1
			// 
			correlationtoken1.Name = "workflowToken";
			correlationtoken1.OwnerActivityName = "SeqWorkflow";
			this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
			this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
			this.onWorkflowActivated1.Name = "onWorkflowActivated1";
			activitybind1.Name = "SeqWorkflow";
			activitybind1.Path = "workflowProperties";
			this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
			this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
			// 
			// SeqWorkflow
			// 
			this.Activities.Add(this.onWorkflowActivated1);
			this.Activities.Add(this.logToHistoryListActivity1);
			this.Name = "SeqWorkflow";
			this.CanModifyActivities = false;

		}

		#endregion

		private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

		private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;

	}
}
