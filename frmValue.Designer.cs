namespace kCalc
{
    partial class frmValue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dsVariables1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsVariables1 = new kCalc.dsVariables();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valDoubleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valIntDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsVariables1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsVariables1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.valDoubleDataGridViewTextBoxColumn,
            this.valueTypeDataGridViewTextBoxColumn,
            this.valIntDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.dsVariables1BindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1104, 508);
            this.dataGridView1.TabIndex = 0;
            // 
            // dsVariables1BindingSource
            // 
            this.dsVariables1BindingSource.AllowNew = true;
            this.dsVariables1BindingSource.DataMember = "Variables";
            this.dsVariables1BindingSource.DataSource = this.dsVariables1;
            // 
            // dsVariables1
            // 
            this.dsVariables1.DataSetName = "dsVariables";
            this.dsVariables1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // valDoubleDataGridViewTextBoxColumn
            // 
            this.valDoubleDataGridViewTextBoxColumn.DataPropertyName = "ValDouble";
            this.valDoubleDataGridViewTextBoxColumn.HeaderText = "ValDouble";
            this.valDoubleDataGridViewTextBoxColumn.Name = "valDoubleDataGridViewTextBoxColumn";
            // 
            // valueTypeDataGridViewTextBoxColumn
            // 
            this.valueTypeDataGridViewTextBoxColumn.DataPropertyName = "ValueType";
            this.valueTypeDataGridViewTextBoxColumn.HeaderText = "ValueType";
            this.valueTypeDataGridViewTextBoxColumn.Name = "valueTypeDataGridViewTextBoxColumn";
            // 
            // valIntDataGridViewTextBoxColumn
            // 
            this.valIntDataGridViewTextBoxColumn.DataPropertyName = "ValInt";
            this.valIntDataGridViewTextBoxColumn.HeaderText = "ValInt";
            this.valIntDataGridViewTextBoxColumn.Name = "valIntDataGridViewTextBoxColumn";
            // 
            // frmValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 508);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmValue";
            this.Text = "Values list";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsVariables1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsVariables1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.BindingSource dsVariables1BindingSource;
        public dsVariables dsVariables1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valDoubleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valIntDataGridViewTextBoxColumn;
    }
}