namespace FuncMaker
{
    partial class FuncForm
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
            this.textBoxFunc = new System.Windows.Forms.TextBox();
            this.buttonValidate = new System.Windows.Forms.Button();
            this.pictureBoxOutcome = new System.Windows.Forms.PictureBox();
            this.Range = new System.Windows.Forms.Label();
            this.textBoxRange = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.checkBoxRadX = new System.Windows.Forms.CheckBox();
            this.checkBoxRadY = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelOOB = new System.Windows.Forms.Label();
            this.labelBlue = new System.Windows.Forms.Label();
            this.labelGreen = new System.Windows.Forms.Label();
            this.labelRed = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelRange = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMin = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonView = new System.Windows.Forms.Button();
            this.listBoxOOB = new System.Windows.Forms.ListBox();
            this.radioStandard = new System.Windows.Forms.RadioButton();
            this.radioButtonFlatten = new System.Windows.Forms.RadioButton();
            this.radioButtonStretch = new System.Windows.Forms.RadioButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutcome)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxFunc
            // 
            this.textBoxFunc.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFunc.Location = new System.Drawing.Point(30, 31);
            this.textBoxFunc.Multiline = true;
            this.textBoxFunc.Name = "textBoxFunc";
            this.textBoxFunc.Size = new System.Drawing.Size(351, 184);
            this.textBoxFunc.TabIndex = 0;
            // 
            // buttonValidate
            // 
            this.buttonValidate.Location = new System.Drawing.Point(387, 31);
            this.buttonValidate.Name = "buttonValidate";
            this.buttonValidate.Size = new System.Drawing.Size(120, 35);
            this.buttonValidate.TabIndex = 1;
            this.buttonValidate.Text = "validate";
            this.buttonValidate.UseVisualStyleBackColor = true;
            this.buttonValidate.Click += new System.EventHandler(this.buttonValidate_Click);
            // 
            // pictureBoxOutcome
            // 
            this.pictureBoxOutcome.Location = new System.Drawing.Point(528, 31);
            this.pictureBoxOutcome.Name = "pictureBoxOutcome";
            this.pictureBoxOutcome.Size = new System.Drawing.Size(60, 60);
            this.pictureBoxOutcome.TabIndex = 2;
            this.pictureBoxOutcome.TabStop = false;
            // 
            // Range
            // 
            this.Range.AutoSize = true;
            this.Range.Location = new System.Drawing.Point(387, 100);
            this.Range.Name = "Range";
            this.Range.Size = new System.Drawing.Size(51, 13);
            this.Range.TabIndex = 3;
            this.Range.Text = "Range +-";
            // 
            // textBoxRange
            // 
            this.textBoxRange.Location = new System.Drawing.Point(454, 97);
            this.textBoxRange.Name = "textBoxRange";
            this.textBoxRange.Size = new System.Drawing.Size(89, 20);
            this.textBoxRange.TabIndex = 4;
            this.textBoxRange.TextChanged += new System.EventHandler(this.textBoxRange_TextChanged);
            this.textBoxRange.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxRange_KeyPress);
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(454, 123);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(53, 31);
            this.buttonGo.TabIndex = 5;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // checkBoxRadX
            // 
            this.checkBoxRadX.AutoSize = true;
            this.checkBoxRadX.Location = new System.Drawing.Point(530, 125);
            this.checkBoxRadX.Name = "checkBoxRadX";
            this.checkBoxRadX.Size = new System.Drawing.Size(53, 17);
            this.checkBoxRadX.TabIndex = 6;
            this.checkBoxRadX.Text = "RadX";
            this.checkBoxRadX.UseVisualStyleBackColor = true;
            // 
            // checkBoxRadY
            // 
            this.checkBoxRadY.AutoSize = true;
            this.checkBoxRadY.Location = new System.Drawing.Point(530, 148);
            this.checkBoxRadY.Name = "checkBoxRadY";
            this.checkBoxRadY.Size = new System.Drawing.Size(53, 17);
            this.checkBoxRadY.TabIndex = 7;
            this.checkBoxRadY.Text = "RadY";
            this.checkBoxRadY.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelOOB);
            this.groupBox1.Controls.Add(this.labelBlue);
            this.groupBox1.Controls.Add(this.labelGreen);
            this.groupBox1.Controls.Add(this.labelRed);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.labelRange);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelMax);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelMin);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(613, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 240);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "metrics";
            // 
            // labelOOB
            // 
            this.labelOOB.AutoSize = true;
            this.labelOOB.Location = new System.Drawing.Point(70, 181);
            this.labelOOB.Name = "labelOOB";
            this.labelOOB.Size = new System.Drawing.Size(41, 13);
            this.labelOOB.TabIndex = 13;
            this.labelOOB.Text = "label14";
            // 
            // labelBlue
            // 
            this.labelBlue.AutoSize = true;
            this.labelBlue.Location = new System.Drawing.Point(70, 157);
            this.labelBlue.Name = "labelBlue";
            this.labelBlue.Size = new System.Drawing.Size(41, 13);
            this.labelBlue.TabIndex = 12;
            this.labelBlue.Text = "label13";
            // 
            // labelGreen
            // 
            this.labelGreen.AutoSize = true;
            this.labelGreen.Location = new System.Drawing.Point(63, 130);
            this.labelGreen.Name = "labelGreen";
            this.labelGreen.Size = new System.Drawing.Size(41, 13);
            this.labelGreen.TabIndex = 11;
            this.labelGreen.Text = "label12";
            // 
            // labelRed
            // 
            this.labelRed.AutoSize = true;
            this.labelRed.Location = new System.Drawing.Point(69, 106);
            this.labelRed.Name = "labelRed";
            this.labelRed.Size = new System.Drawing.Size(41, 13);
            this.labelRed.TabIndex = 10;
            this.labelRed.Text = "label11";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "OOB";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "blue";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "green";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "red";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // labelRange
            // 
            this.labelRange.AutoSize = true;
            this.labelRange.Location = new System.Drawing.Point(66, 82);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(35, 13);
            this.labelRange.TabIndex = 5;
            this.labelRange.Text = "label6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "range%";
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Location = new System.Drawing.Point(66, 56);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(35, 13);
            this.labelMax.TabIndex = 3;
            this.labelMax.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "max";
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.Location = new System.Drawing.Point(66, 31);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(35, 13);
            this.labelMin.TabIndex = 1;
            this.labelMin.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "min";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(30, 233);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // buttonView
            // 
            this.buttonView.Location = new System.Drawing.Point(436, 233);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(75, 23);
            this.buttonView.TabIndex = 10;
            this.buttonView.Text = "View";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click += new System.EventHandler(this.buttonView_Click);
            // 
            // listBoxOOB
            // 
            this.listBoxOOB.FormattingEnabled = true;
            this.listBoxOOB.Items.AddRange(new object[] {
            "reject",
            "rollover",
            "stop",
            "bounce"});
            this.listBoxOOB.Location = new System.Drawing.Point(436, 278);
            this.listBoxOOB.Name = "listBoxOOB";
            this.listBoxOOB.Size = new System.Drawing.Size(75, 56);
            this.listBoxOOB.TabIndex = 11;
            // 
            // radioStandard
            // 
            this.radioStandard.AutoSize = true;
            this.radioStandard.Location = new System.Drawing.Point(525, 278);
            this.radioStandard.Name = "radioStandard";
            this.radioStandard.Size = new System.Drawing.Size(66, 17);
            this.radioStandard.TabIndex = 12;
            this.radioStandard.TabStop = true;
            this.radioStandard.Text = "standard";
            this.radioStandard.UseVisualStyleBackColor = true;
            this.radioStandard.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioButtonFlatten
            // 
            this.radioButtonFlatten.AutoSize = true;
            this.radioButtonFlatten.Location = new System.Drawing.Point(525, 301);
            this.radioButtonFlatten.Name = "radioButtonFlatten";
            this.radioButtonFlatten.Size = new System.Drawing.Size(54, 17);
            this.radioButtonFlatten.TabIndex = 13;
            this.radioButtonFlatten.TabStop = true;
            this.radioButtonFlatten.Text = "flatten";
            this.radioButtonFlatten.UseVisualStyleBackColor = true;
            this.radioButtonFlatten.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioButtonStretch
            // 
            this.radioButtonStretch.AutoSize = true;
            this.radioButtonStretch.Location = new System.Drawing.Point(525, 324);
            this.radioButtonStretch.Name = "radioButtonStretch";
            this.radioButtonStretch.Size = new System.Drawing.Size(57, 17);
            this.radioButtonStretch.TabIndex = 14;
            this.radioButtonStretch.TabStop = true;
            this.radioButtonStretch.Text = "stretch";
            this.radioButtonStretch.UseVisualStyleBackColor = true;
            this.radioButtonStretch.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(445, 364);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(379, 37);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 15;
            // 
            // FuncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 645);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.radioButtonStretch);
            this.Controls.Add(this.radioButtonFlatten);
            this.Controls.Add(this.radioStandard);
            this.Controls.Add(this.listBoxOOB);
            this.Controls.Add(this.buttonView);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxRadY);
            this.Controls.Add(this.checkBoxRadX);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.textBoxRange);
            this.Controls.Add(this.Range);
            this.Controls.Add(this.pictureBoxOutcome);
            this.Controls.Add(this.buttonValidate);
            this.Controls.Add(this.textBoxFunc);
            this.Name = "FuncForm";
            this.Text = "FuncForm";
            this.Load += new System.EventHandler(this.FuncForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutcome)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFunc;
        private System.Windows.Forms.Button buttonValidate;
        private System.Windows.Forms.PictureBox pictureBoxOutcome;
        private System.Windows.Forms.Label Range;
        private System.Windows.Forms.TextBox textBoxRange;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.CheckBox checkBoxRadX;
        private System.Windows.Forms.CheckBox checkBoxRadY;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelRange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelOOB;
        private System.Windows.Forms.Label labelBlue;
        private System.Windows.Forms.Label labelGreen;
        private System.Windows.Forms.Label labelRed;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonView;
        private System.Windows.Forms.ListBox listBoxOOB;
        private System.Windows.Forms.RadioButton radioStandard;
        private System.Windows.Forms.RadioButton radioButtonFlatten;
        private System.Windows.Forms.RadioButton radioButtonStretch;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

