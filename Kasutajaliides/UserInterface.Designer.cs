
namespace Kasutajaliides
{
    partial class UserInterface
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartElektrihind = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbKasutusmall = new System.Windows.Forms.ComboBox();
            this.lblKasutusmall = new System.Windows.Forms.Label();
            this.lblAeg = new System.Windows.Forms.Label();
            this.txtAjakulu = new System.Windows.Forms.TextBox();
            this.lblTund = new System.Windows.Forms.Label();
            this.lblVoimsus = new System.Windows.Forms.Label();
            this.txtVoimsus = new System.Windows.Forms.TextBox();
            this.lblkW = new System.Windows.Forms.Label();
            this.lblHind = new System.Windows.Forms.Label();
            this.txtHind = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartElektrihind)).BeginInit();
            this.SuspendLayout();
            // 
            // chartElektrihind
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.chartElektrihind.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartElektrihind.Legends.Add(legend1);
            this.chartElektrihind.Location = new System.Drawing.Point(346, 12);
            this.chartElektrihind.Name = "chartElektrihind";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series1.Legend = "Legend1";
            series1.Name = "Elektrihind";
            this.chartElektrihind.Series.Add(series1);
            this.chartElektrihind.Size = new System.Drawing.Size(625, 420);
            this.chartElektrihind.TabIndex = 0;
            this.chartElektrihind.Text = "chartElektrihind";
            // 
            // cbKasutusmall
            // 
            this.cbKasutusmall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKasutusmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbKasutusmall.FormattingEnabled = true;
            this.cbKasutusmall.Items.AddRange(new object[] {
            "Auto"});
            this.cbKasutusmall.Location = new System.Drawing.Point(16, 57);
            this.cbKasutusmall.Name = "cbKasutusmall";
            this.cbKasutusmall.Size = new System.Drawing.Size(113, 28);
            this.cbKasutusmall.TabIndex = 2;
            // 
            // lblKasutusmall
            // 
            this.lblKasutusmall.AutoSize = true;
            this.lblKasutusmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKasutusmall.Location = new System.Drawing.Point(12, 34);
            this.lblKasutusmall.Name = "lblKasutusmall";
            this.lblKasutusmall.Size = new System.Drawing.Size(132, 20);
            this.lblKasutusmall.TabIndex = 3;
            this.lblKasutusmall.Text = "Kasutusmalli valik";
            this.lblKasutusmall.Click += new System.EventHandler(this.lblKasutusmall_Click);
            // 
            // lblAeg
            // 
            this.lblAeg.AutoSize = true;
            this.lblAeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAeg.Location = new System.Drawing.Point(12, 99);
            this.lblAeg.Name = "lblAeg";
            this.lblAeg.Size = new System.Drawing.Size(65, 20);
            this.lblAeg.TabIndex = 4;
            this.lblAeg.Text = "Ajakulu:";
            // 
            // txtAjakulu
            // 
            this.txtAjakulu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAjakulu.Location = new System.Drawing.Point(16, 122);
            this.txtAjakulu.Name = "txtAjakulu";
            this.txtAjakulu.Size = new System.Drawing.Size(110, 26);
            this.txtAjakulu.TabIndex = 5;
            // 
            // lblTund
            // 
            this.lblTund.AutoSize = true;
            this.lblTund.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTund.Location = new System.Drawing.Point(136, 125);
            this.lblTund.Name = "lblTund";
            this.lblTund.Size = new System.Drawing.Size(44, 20);
            this.lblTund.TabIndex = 6;
            this.lblTund.Text = "tundi";
            // 
            // lblVoimsus
            // 
            this.lblVoimsus.AutoSize = true;
            this.lblVoimsus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVoimsus.Location = new System.Drawing.Point(12, 171);
            this.lblVoimsus.Name = "lblVoimsus";
            this.lblVoimsus.Size = new System.Drawing.Size(109, 20);
            this.lblVoimsus.TabIndex = 7;
            this.lblVoimsus.Text = "Võimsustarve:";
            this.lblVoimsus.Click += new System.EventHandler(this.lblVoimsus_Click);
            // 
            // txtVoimsus
            // 
            this.txtVoimsus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVoimsus.Location = new System.Drawing.Point(16, 194);
            this.txtVoimsus.Name = "txtVoimsus";
            this.txtVoimsus.Size = new System.Drawing.Size(110, 26);
            this.txtVoimsus.TabIndex = 8;
            // 
            // lblkW
            // 
            this.lblkW.AutoSize = true;
            this.lblkW.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkW.Location = new System.Drawing.Point(136, 201);
            this.lblkW.Name = "lblkW";
            this.lblkW.Size = new System.Drawing.Size(32, 20);
            this.lblkW.TabIndex = 9;
            this.lblkW.Text = "kW";
            // 
            // lblHind
            // 
            this.lblHind.AutoSize = true;
            this.lblHind.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHind.Location = new System.Drawing.Point(16, 246);
            this.lblHind.Name = "lblHind";
            this.lblHind.Size = new System.Drawing.Size(46, 20);
            this.lblHind.TabIndex = 10;
            this.lblHind.Text = "Hind:";
            // 
            // txtHind
            // 
            this.txtHind.Enabled = false;
            this.txtHind.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHind.Location = new System.Drawing.Point(16, 269);
            this.txtHind.Name = "txtHind";
            this.txtHind.Size = new System.Drawing.Size(110, 26);
            this.txtHind.TabIndex = 11;
            // 
            // UserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 453);
            this.Controls.Add(this.txtHind);
            this.Controls.Add(this.lblHind);
            this.Controls.Add(this.lblkW);
            this.Controls.Add(this.txtVoimsus);
            this.Controls.Add(this.lblVoimsus);
            this.Controls.Add(this.lblTund);
            this.Controls.Add(this.txtAjakulu);
            this.Controls.Add(this.lblAeg);
            this.Controls.Add(this.lblKasutusmall);
            this.Controls.Add(this.cbKasutusmall);
            this.Controls.Add(this.chartElektrihind);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserInterface";
            this.Text = "Elektrihinna kalkulaator";
            this.Load += new System.EventHandler(this.UserInterface_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartElektrihind)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartElektrihind;
        private System.Windows.Forms.ComboBox cbKasutusmall;
        private System.Windows.Forms.Label lblKasutusmall;
        private System.Windows.Forms.Label lblAeg;
        private System.Windows.Forms.TextBox txtAjakulu;
        private System.Windows.Forms.Label lblTund;
        private System.Windows.Forms.Label lblVoimsus;
        private System.Windows.Forms.TextBox txtVoimsus;
        private System.Windows.Forms.Label lblkW;
        private System.Windows.Forms.Label lblHind;
        private System.Windows.Forms.TextBox txtHind;
    }
}

