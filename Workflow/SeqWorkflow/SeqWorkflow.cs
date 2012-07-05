﻿using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace Workflow.SeqWorkflow {
	public sealed partial class SeqWorkflow : SequentialWorkflowActivity {
		public SeqWorkflow() {
			InitializeComponent();
		}

		public Guid workflowId = default(System.Guid);
		public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
	}
}
