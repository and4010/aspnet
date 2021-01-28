namespace CHPOUTSRCMES.TASK.Forms
{
    partial class ScheduleForm
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
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ltv_Schedule = new System.Windows.Forms.ListView();
            this.col_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_TimeInterval = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_LastExecutedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ltv_Schedule
            // 
            this.ltv_Schedule.BackColor = System.Drawing.Color.Ivory;
            this.ltv_Schedule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ltv_Schedule.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_Name,
            this.col_TimeInterval,
            this.col_Status,
            this.col_LastExecutedTime});
            this.ltv_Schedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ltv_Schedule.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ltv_Schedule.GridLines = true;
            this.ltv_Schedule.HideSelection = false;
            this.ltv_Schedule.Location = new System.Drawing.Point(0, 0);
            this.ltv_Schedule.Name = "ltv_Schedule";
            this.ltv_Schedule.Size = new System.Drawing.Size(597, 406);
            this.ltv_Schedule.TabIndex = 0;
            this.ltv_Schedule.UseCompatibleStateImageBehavior = false;
            this.ltv_Schedule.View = System.Windows.Forms.View.Details;
            // 
            // col_Name
            // 
            this.col_Name.Text = "任務名稱";
            this.col_Name.Width = 173;
            // 
            // col_TimeInterval
            // 
            this.col_TimeInterval.Text = "間隔時間";
            this.col_TimeInterval.Width = 93;
            // 
            // col_Status
            // 
            this.col_Status.Text = "狀態";
            this.col_Status.Width = 72;
            // 
            // col_LastExecutedTime
            // 
            this.col_LastExecutedTime.Text = "上次執行時間";
            this.col_LastExecutedTime.Width = 155;
            // 
            // ScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 406);
            this.ControlBox = false;
            this.Controls.Add(this.ltv_Schedule);
            this.MaximizeBox = false;
            this.Name = "ScheduleForm";
            this.Text = "任務";
            this.Load += new System.EventHandler(this.ScheduleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ListView ltv_Schedule;
        private System.Windows.Forms.ColumnHeader col_Name;
        private System.Windows.Forms.ColumnHeader col_TimeInterval;
        private System.Windows.Forms.ColumnHeader col_Status;
        private System.Windows.Forms.ColumnHeader col_LastExecutedTime;
    }
}