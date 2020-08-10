namespace CHPOUTSRCMES.TASK.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.檔案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.功能表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.單位換算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.主檔ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檔案ToolStripMenuItem,
            this.功能表ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 檔案ToolStripMenuItem
            // 
            this.檔案ToolStripMenuItem.Name = "檔案ToolStripMenuItem";
            this.檔案ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.檔案ToolStripMenuItem.Text = "檔案";
            // 
            // 功能表ToolStripMenuItem
            // 
            this.功能表ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.單位換算ToolStripMenuItem,
            this.主檔ToolStripMenuItem});
            this.功能表ToolStripMenuItem.Name = "功能表ToolStripMenuItem";
            this.功能表ToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.功能表ToolStripMenuItem.Text = "功能表";
            // 
            // 單位換算ToolStripMenuItem
            // 
            this.單位換算ToolStripMenuItem.Name = "單位換算ToolStripMenuItem";
            this.單位換算ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.單位換算ToolStripMenuItem.Text = "單位換算";
            this.單位換算ToolStripMenuItem.Click += new System.EventHandler(this.單位換算ToolStripMenuItem_Click);
            // 
            // 主檔ToolStripMenuItem
            // 
            this.主檔ToolStripMenuItem.Name = "主檔ToolStripMenuItem";
            this.主檔ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.主檔ToolStripMenuItem.Text = "主檔";
            this.主檔ToolStripMenuItem.Click += new System.EventHandler(this.主檔ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MES 轉檔程式";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 檔案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 功能表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 單位換算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 主檔ToolStripMenuItem;
    }
}

