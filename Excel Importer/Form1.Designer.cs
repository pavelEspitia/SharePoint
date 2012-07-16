namespace Excel_Importer {
	partial class Form1 {
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			this.txtExcel = new System.Windows.Forms.TextBox();
			this.cmdSelectFile = new System.Windows.Forms.Button();
			this.cmdImport = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.cmbSheets = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.chkIsDevelop = new System.Windows.Forms.CheckBox();
			this.chkUseClaims = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbLists = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtSiteUrl = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.dgMapping = new System.Windows.Forms.DataGridView();
			this.cmdMap = new System.Windows.Forms.Button();
			this.lbCounter = new System.Windows.Forms.Label();
			this.ExcelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ListColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.TableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FolderLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgMapping)).BeginInit();
			this.SuspendLayout();
			// 
			// txtExcel
			// 
			this.txtExcel.Location = new System.Drawing.Point(84, 17);
			this.txtExcel.Name = "txtExcel";
			this.txtExcel.Size = new System.Drawing.Size(375, 21);
			this.txtExcel.TabIndex = 0;
			this.txtExcel.Text = "c:\\test.xls";
			// 
			// cmdSelectFile
			// 
			this.cmdSelectFile.Location = new System.Drawing.Point(465, 15);
			this.cmdSelectFile.Name = "cmdSelectFile";
			this.cmdSelectFile.Size = new System.Drawing.Size(75, 23);
			this.cmdSelectFile.TabIndex = 1;
			this.cmdSelectFile.Text = "Select ...";
			this.cmdSelectFile.UseVisualStyleBackColor = true;
			// 
			// cmdImport
			// 
			this.cmdImport.BackColor = System.Drawing.Color.PeachPuff;
			this.cmdImport.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cmdImport.Location = new System.Drawing.Point(658, 469);
			this.cmdImport.Name = "cmdImport";
			this.cmdImport.Size = new System.Drawing.Size(75, 23);
			this.cmdImport.TabIndex = 2;
			this.cmdImport.Text = "Import";
			this.cmdImport.UseVisualStyleBackColor = false;
			this.cmdImport.Click += new System.EventHandler(this.cmdImport_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.cmbSheets);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtExcel);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cmdSelectFile);
			this.groupBox1.Location = new System.Drawing.Point(12, 10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(744, 86);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Excel";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.ForeColor = System.Drawing.Color.Red;
			this.label6.Location = new System.Drawing.Point(327, 57);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(11, 12);
			this.label6.TabIndex = 6;
			this.label6.Text = "*";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(642, 15);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(79, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Analyze";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.cmdCheckExcelFile_Click);
			// 
			// cmbSheets
			// 
			this.cmbSheets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSheets.FormattingEnabled = true;
			this.cmbSheets.Location = new System.Drawing.Point(84, 54);
			this.cmbSheets.Name = "cmbSheets";
			this.cmbSheets.Size = new System.Drawing.Size(236, 20);
			this.cmbSheets.TabIndex = 4;
			this.cmbSheets.SelectedIndexChanged += new System.EventHandler(this.cmbSheets_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 57);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 12);
			this.label4.TabIndex = 3;
			this.label4.Text = "Sheet name:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "Choose file:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.chkIsDevelop);
			this.groupBox2.Controls.Add(this.chkUseClaims);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.cmbLists);
			this.groupBox2.Controls.Add(this.button1);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.txtSiteUrl);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(12, 107);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(744, 82);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "SharePoint site";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.ForeColor = System.Drawing.Color.Red;
			this.label7.Location = new System.Drawing.Point(327, 56);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(11, 12);
			this.label7.TabIndex = 7;
			this.label7.Text = "*";
			// 
			// chkIsDevelop
			// 
			this.chkIsDevelop.AutoSize = true;
			this.chkIsDevelop.Location = new System.Drawing.Point(384, 22);
			this.chkIsDevelop.Name = "chkIsDevelop";
			this.chkIsDevelop.Size = new System.Drawing.Size(72, 16);
			this.chkIsDevelop.TabIndex = 7;
			this.chkIsDevelop.Text = "Develop?";
			this.chkIsDevelop.UseVisualStyleBackColor = true;
			// 
			// chkUseClaims
			// 
			this.chkUseClaims.AutoSize = true;
			this.chkUseClaims.Location = new System.Drawing.Point(472, 22);
			this.chkUseClaims.Name = "chkUseClaims";
			this.chkUseClaims.Size = new System.Drawing.Size(156, 16);
			this.chkUseClaims.TabIndex = 6;
			this.chkUseClaims.Text = "Use Claims Based Auth?";
			this.chkUseClaims.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(357, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(0, 12);
			this.label5.TabIndex = 5;
			// 
			// cmbLists
			// 
			this.cmbLists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLists.FormattingEnabled = true;
			this.cmbLists.Location = new System.Drawing.Point(84, 53);
			this.cmbLists.Name = "cmbLists";
			this.cmbLists.Size = new System.Drawing.Size(236, 20);
			this.cmbLists.TabIndex = 4;
			this.cmbLists.SelectedIndexChanged += new System.EventHandler(this.cmbLists_SelectedIndexChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(642, 18);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(79, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Validate";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.cmdValidateSharePointSite_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "List name:";
			// 
			// txtSiteUrl
			// 
			this.txtSiteUrl.Location = new System.Drawing.Point(84, 20);
			this.txtSiteUrl.Name = "txtSiteUrl";
			this.txtSiteUrl.Size = new System.Drawing.Size(273, 21);
			this.txtSiteUrl.TabIndex = 1;
			this.txtSiteUrl.Text = "https://share.gm.com/sites/sptraining";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "Site url:";
			// 
			// dgMapping
			// 
			this.dgMapping.AllowUserToResizeRows = false;
			this.dgMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgMapping.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExcelColumn,
            this.ListColumn,
            this.TableName,
            this.FolderLevel});
			this.dgMapping.Location = new System.Drawing.Point(12, 239);
			this.dgMapping.Name = "dgMapping";
			this.dgMapping.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.dgMapping.RowTemplate.Height = 23;
			this.dgMapping.Size = new System.Drawing.Size(744, 216);
			this.dgMapping.TabIndex = 6;
			// 
			// cmdMap
			// 
			this.cmdMap.Location = new System.Drawing.Point(654, 210);
			this.cmdMap.Name = "cmdMap";
			this.cmdMap.Size = new System.Drawing.Size(79, 23);
			this.cmdMap.TabIndex = 7;
			this.cmdMap.Text = "Re-Map";
			this.cmdMap.UseVisualStyleBackColor = true;
			this.cmdMap.Click += new System.EventHandler(this.cmdMap_Click);
			// 
			// lbCounter
			// 
			this.lbCounter.AutoSize = true;
			this.lbCounter.Location = new System.Drawing.Point(545, 474);
			this.lbCounter.Name = "lbCounter";
			this.lbCounter.Size = new System.Drawing.Size(35, 12);
			this.lbCounter.TabIndex = 8;
			this.lbCounter.Text = "0 / 0";
			this.lbCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ExcelColumn
			// 
			this.ExcelColumn.HeaderText = "Excel Column";
			this.ExcelColumn.Name = "ExcelColumn";
			this.ExcelColumn.Width = 200;
			// 
			// ListColumn
			// 
			this.ListColumn.HeaderText = "List Column";
			this.ListColumn.Name = "ListColumn";
			this.ListColumn.Sorted = true;
			this.ListColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.ListColumn.Width = 240;
			// 
			// TableName
			// 
			this.TableName.HeaderText = "Table Name";
			this.TableName.Name = "TableName";
			this.TableName.Width = 120;
			// 
			// FolderLevel
			// 
			this.FolderLevel.HeaderText = "Folder Level";
			this.FolderLevel.Name = "FolderLevel";
			this.FolderLevel.Width = 110;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(768, 504);
			this.Controls.Add(this.lbCounter);
			this.Controls.Add(this.cmdMap);
			this.Controls.Add(this.dgMapping);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmdImport);
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Excel Importer";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgMapping)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtExcel;
		private System.Windows.Forms.Button cmdSelectFile;
		private System.Windows.Forms.Button cmdImport;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbSheets;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox cmbLists;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtSiteUrl;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.DataGridView dgMapping;
		private System.Windows.Forms.CheckBox chkUseClaims;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkIsDevelop;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button cmdMap;
		private System.Windows.Forms.Label lbCounter;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExcelColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn ListColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn TableName;
		private System.Windows.Forms.DataGridViewTextBoxColumn FolderLevel;
	}
}

