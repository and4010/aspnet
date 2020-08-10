namespace CHPOUTSRCMES.TASK.Forms
{
    partial class UomConverterForm
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
            this.btnCalc = new System.Windows.Forms.Button();
            this.lbl_Amount = new System.Windows.Forms.Label();
            this.txt_Uom = new System.Windows.Forms.TextBox();
            this.lbl_FromUom = new System.Windows.Forms.Label();
            this.txt_ToUom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_RestultAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_Result = new System.Windows.Forms.Label();
            this.lbl_Item = new System.Windows.Forms.Label();
            this.txt_Amount = new System.Windows.Forms.TextBox();
            this.txt_Item = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCalc
            // 
            this.btnCalc.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCalc.Location = new System.Drawing.Point(31, 234);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(384, 53);
            this.btnCalc.TabIndex = 3;
            this.btnCalc.Text = "換算";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // lbl_Amount
            // 
            this.lbl_Amount.AutoSize = true;
            this.lbl_Amount.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Amount.Location = new System.Drawing.Point(27, 87);
            this.lbl_Amount.Name = "lbl_Amount";
            this.lbl_Amount.Size = new System.Drawing.Size(58, 24);
            this.lbl_Amount.TabIndex = 1;
            this.lbl_Amount.Text = "數量";
            // 
            // txt_Uom
            // 
            this.txt_Uom.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_Uom.Location = new System.Drawing.Point(137, 134);
            this.txt_Uom.MaxLength = 3;
            this.txt_Uom.Name = "txt_Uom";
            this.txt_Uom.Size = new System.Drawing.Size(102, 36);
            this.txt_Uom.TabIndex = 1;
            // 
            // lbl_FromUom
            // 
            this.lbl_FromUom.AutoSize = true;
            this.lbl_FromUom.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_FromUom.Location = new System.Drawing.Point(27, 137);
            this.lbl_FromUom.Name = "lbl_FromUom";
            this.lbl_FromUom.Size = new System.Drawing.Size(58, 24);
            this.lbl_FromUom.TabIndex = 4;
            this.lbl_FromUom.Text = "單位";
            // 
            // txt_ToUom
            // 
            this.txt_ToUom.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_ToUom.Location = new System.Drawing.Point(137, 184);
            this.txt_ToUom.MaxLength = 3;
            this.txt_ToUom.Name = "txt_ToUom";
            this.txt_ToUom.Size = new System.Drawing.Size(100, 36);
            this.txt_ToUom.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(27, 187);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "換算單位";
            // 
            // lbl_RestultAmount
            // 
            this.lbl_RestultAmount.AutoSize = true;
            this.lbl_RestultAmount.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_RestultAmount.Location = new System.Drawing.Point(27, 349);
            this.lbl_RestultAmount.Name = "lbl_RestultAmount";
            this.lbl_RestultAmount.Size = new System.Drawing.Size(21, 24);
            this.lbl_RestultAmount.TabIndex = 7;
            this.lbl_RestultAmount.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(373, 349);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "KG";
            // 
            // lbl_Result
            // 
            this.lbl_Result.AutoSize = true;
            this.lbl_Result.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Result.Location = new System.Drawing.Point(27, 305);
            this.lbl_Result.Name = "lbl_Result";
            this.lbl_Result.Size = new System.Drawing.Size(106, 24);
            this.lbl_Result.TabIndex = 9;
            this.lbl_Result.Text = "換算結果";
            // 
            // lbl_Item
            // 
            this.lbl_Item.AutoSize = true;
            this.lbl_Item.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Item.Location = new System.Drawing.Point(27, 37);
            this.lbl_Item.Name = "lbl_Item";
            this.lbl_Item.Size = new System.Drawing.Size(58, 24);
            this.lbl_Item.TabIndex = 10;
            this.lbl_Item.Text = "料號";
            // 
            // txt_Amount
            // 
            this.txt_Amount.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_Amount.Location = new System.Drawing.Point(137, 84);
            this.txt_Amount.MaxLength = 35;
            this.txt_Amount.Name = "txt_Amount";
            this.txt_Amount.Size = new System.Drawing.Size(278, 36);
            this.txt_Amount.TabIndex = 0;
            // 
            // txt_Item
            // 
            this.txt_Item.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_Item.Location = new System.Drawing.Point(137, 34);
            this.txt_Item.MaxLength = 35;
            this.txt_Item.Name = "txt_Item";
            this.txt_Item.Size = new System.Drawing.Size(278, 36);
            this.txt_Item.TabIndex = 11;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // UomConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 406);
            this.Controls.Add(this.txt_Item);
            this.Controls.Add(this.lbl_Item);
            this.Controls.Add(this.lbl_Result);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_RestultAmount);
            this.Controls.Add(this.txt_ToUom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbl_FromUom);
            this.Controls.Add(this.txt_Uom);
            this.Controls.Add(this.txt_Amount);
            this.Controls.Add(this.lbl_Amount);
            this.Controls.Add(this.btnCalc);
            this.MaximizeBox = false;
            this.Name = "UomConverterForm";
            this.Text = "單位換算";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label lbl_Amount;
        private System.Windows.Forms.TextBox txt_Uom;
        private System.Windows.Forms.Label lbl_FromUom;
        private System.Windows.Forms.TextBox txt_ToUom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_RestultAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_Result;
        private System.Windows.Forms.Label lbl_Item;
        private System.Windows.Forms.TextBox txt_Amount;
        private System.Windows.Forms.TextBox txt_Item;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}