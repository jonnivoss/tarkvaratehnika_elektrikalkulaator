
namespace Kasutajaliides
{
    partial class Kasutajaliides
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
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(44956D, 250D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(44959D, 420D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(44960D, 50D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(44963D, 56D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(45028D, 421D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(45029D, 69D);
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.btnAvaCSV = new System.Windows.Forms.Button();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateStartTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateStopTime = new System.Windows.Forms.DateTimePicker();
            this.cbShowPrice = new System.Windows.Forms.CheckBox();
            this.cbShowTabel = new System.Windows.Forms.CheckBox();
            this.rbStockPrice = new System.Windows.Forms.RadioButton();
            this.rbMonthlyCost = new System.Windows.Forms.RadioButton();
            this.tbMonthlyPrice = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartElektrihind)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartElektrihind
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.chartElektrihind.ChartAreas.Add(chartArea1);
            legend1.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chartElektrihind.Legends.Add(legend1);
            this.chartElektrihind.Location = new System.Drawing.Point(289, 13);
            this.chartElektrihind.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chartElektrihind.Name = "chartElektrihind";
            this.chartElektrihind.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series1.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.Name = "Elektrihind";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            series1.Points.Add(dataPoint6);
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series2.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series2.Legend = "Legend1";
            series2.Name = "Tarbimine";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chartElektrihind.Series.Add(series1);
            this.chartElektrihind.Series.Add(series2);
            this.chartElektrihind.Size = new System.Drawing.Size(742, 485);
            this.chartElektrihind.TabIndex = 0;
            this.chartElektrihind.Text = "chartElektrihind";
            // 
            // cbKasutusmall
            // 
            this.cbKasutusmall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKasutusmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbKasutusmall.FormattingEnabled = true;
            this.cbKasutusmall.Items.AddRange(new object[] {
            "",
            "Automaat",
            "Manuaal",
            "Pesumasin",
            "Veekeetja",
            "Elektripliit",
            "Kohvimasin",
            "Where\'s my bible?",
            "Librarian",
            "HÖÖÖ",
            "Bwoah",
            "KÄH"});
            this.cbKasutusmall.Location = new System.Drawing.Point(14, 66);
            this.cbKasutusmall.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbKasutusmall.Name = "cbKasutusmall";
            this.cbKasutusmall.Size = new System.Drawing.Size(128, 28);
            this.cbKasutusmall.TabIndex = 2;
            // 
            // lblKasutusmall
            // 
            this.lblKasutusmall.AutoSize = true;
            this.lblKasutusmall.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKasutusmall.Location = new System.Drawing.Point(10, 39);
            this.lblKasutusmall.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblKasutusmall.Name = "lblKasutusmall";
            this.lblKasutusmall.Size = new System.Drawing.Size(130, 20);
            this.lblKasutusmall.TabIndex = 3;
            this.lblKasutusmall.Text = "Kasutusmalli valik";
            // 
            // lblAeg
            // 
            this.lblAeg.AutoSize = true;
            this.lblAeg.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAeg.Location = new System.Drawing.Point(10, 114);
            this.lblAeg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAeg.Name = "lblAeg";
            this.lblAeg.Size = new System.Drawing.Size(59, 20);
            this.lblAeg.TabIndex = 4;
            this.lblAeg.Text = "Ajakulu:";
            // 
            // txtAjakulu
            // 
            this.txtAjakulu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAjakulu.Location = new System.Drawing.Point(14, 141);
            this.txtAjakulu.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtAjakulu.Name = "txtAjakulu";
            this.txtAjakulu.Size = new System.Drawing.Size(128, 26);
            this.txtAjakulu.TabIndex = 5;
            this.txtAjakulu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAjakulu_KeyPress);
            // 
            // lblTund
            // 
            this.lblTund.AutoSize = true;
            this.lblTund.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTund.Location = new System.Drawing.Point(152, 144);
            this.lblTund.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTund.Name = "lblTund";
            this.lblTund.Size = new System.Drawing.Size(42, 20);
            this.lblTund.TabIndex = 6;
            this.lblTund.Text = "tundi";
            // 
            // lblVoimsus
            // 
            this.lblVoimsus.AutoSize = true;
            this.lblVoimsus.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVoimsus.Location = new System.Drawing.Point(10, 197);
            this.lblVoimsus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVoimsus.Name = "lblVoimsus";
            this.lblVoimsus.Size = new System.Drawing.Size(102, 20);
            this.lblVoimsus.TabIndex = 7;
            this.lblVoimsus.Text = "Võimsustarve:";
            // 
            // txtVoimsus
            // 
            this.txtVoimsus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVoimsus.Location = new System.Drawing.Point(14, 224);
            this.txtVoimsus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtVoimsus.Name = "txtVoimsus";
            this.txtVoimsus.Size = new System.Drawing.Size(128, 26);
            this.txtVoimsus.TabIndex = 8;
            this.txtVoimsus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVoimsus_KeyPress);
            // 
            // lblkW
            // 
            this.lblkW.AutoSize = true;
            this.lblkW.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkW.Location = new System.Drawing.Point(152, 227);
            this.lblkW.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblkW.Name = "lblkW";
            this.lblkW.Size = new System.Drawing.Size(30, 20);
            this.lblkW.TabIndex = 9;
            this.lblkW.Text = "kW";
            // 
            // lblHind
            // 
            this.lblHind.AutoSize = true;
            this.lblHind.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHind.Location = new System.Drawing.Point(10, 278);
            this.lblHind.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHind.Name = "lblHind";
            this.lblHind.Size = new System.Drawing.Size(41, 20);
            this.lblHind.TabIndex = 10;
            this.lblHind.Text = "Hind:";
            // 
            // txtHind
            // 
            this.txtHind.Enabled = false;
            this.txtHind.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHind.Location = new System.Drawing.Point(14, 301);
            this.txtHind.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtHind.Name = "txtHind";
            this.txtHind.Size = new System.Drawing.Size(128, 26);
            this.txtHind.TabIndex = 11;
            // 
            // btnAvaCSV
            // 
            this.btnAvaCSV.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvaCSV.Location = new System.Drawing.Point(16, 466);
            this.btnAvaCSV.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAvaCSV.Name = "btnAvaCSV";
            this.btnAvaCSV.Size = new System.Drawing.Size(104, 31);
            this.btnAvaCSV.TabIndex = 12;
            this.btnAvaCSV.Text = "Ava CŠV fail";
            this.btnAvaCSV.UseVisualStyleBackColor = true;
            this.btnAvaCSV.Click += new System.EventHandler(this.btnAvaCSV_Click);
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(2, 346);
            this.txtDebug.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDebug.Size = new System.Drawing.Size(283, 114);
            this.txtDebug.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(289, 505);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Algus:";
            // 
            // dateStartTime
            // 
            this.dateStartTime.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateStartTime.Location = new System.Drawing.Point(337, 504);
            this.dateStartTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dateStartTime.Name = "dateStartTime";
            this.dateStartTime.Size = new System.Drawing.Size(173, 21);
            this.dateStartTime.TabIndex = 15;
            this.dateStartTime.ValueChanged += new System.EventHandler(this.dateStartTime_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(547, 506);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Lõpp:";
            // 
            // dateStopTime
            // 
            this.dateStopTime.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateStopTime.Location = new System.Drawing.Point(593, 505);
            this.dateStopTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dateStopTime.Name = "dateStopTime";
            this.dateStopTime.Size = new System.Drawing.Size(169, 21);
            this.dateStopTime.TabIndex = 17;
            this.dateStopTime.ValueChanged += new System.EventHandler(this.dateStopTime_ValueChanged);
            // 
            // cbShowPrice
            // 
            this.cbShowPrice.AutoSize = true;
            this.cbShowPrice.Checked = true;
            this.cbShowPrice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowPrice.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowPrice.Location = new System.Drawing.Point(9, 534);
            this.cbShowPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbShowPrice.Name = "cbShowPrice";
            this.cbShowPrice.Size = new System.Drawing.Size(135, 24);
            this.cbShowPrice.TabIndex = 18;
            this.cbShowPrice.Text = "Kuva elektrihind";
            this.cbShowPrice.UseVisualStyleBackColor = true;
            this.cbShowPrice.CheckedChanged += new System.EventHandler(this.cbShowPrice_CheckedChanged);
            // 
            // cbShowTabel
            // 
            this.cbShowTabel.AutoSize = true;
            this.cbShowTabel.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowTabel.Location = new System.Drawing.Point(9, 565);
            this.cbShowTabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbShowTabel.Name = "cbShowTabel";
            this.cbShowTabel.Size = new System.Drawing.Size(97, 24);
            this.cbShowTabel.TabIndex = 19;
            this.cbShowTabel.Text = "Kuva tabel";
            this.cbShowTabel.UseVisualStyleBackColor = true;
            this.cbShowTabel.CheckedChanged += new System.EventHandler(this.cbShowTabel_CheckedChanged);
            // 
            // rbStockPrice
            // 
            this.rbStockPrice.AutoSize = true;
            this.rbStockPrice.Checked = true;
            this.rbStockPrice.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbStockPrice.Location = new System.Drawing.Point(10, 18);
            this.rbStockPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbStockPrice.Name = "rbStockPrice";
            this.rbStockPrice.Size = new System.Drawing.Size(90, 24);
            this.rbStockPrice.TabIndex = 20;
            this.rbStockPrice.TabStop = true;
            this.rbStockPrice.Text = "Börsihind";
            this.rbStockPrice.UseVisualStyleBackColor = true;
            this.rbStockPrice.CheckedChanged += new System.EventHandler(this.rbStockPrice_CheckedChanged);
            // 
            // rbMonthlyCost
            // 
            this.rbMonthlyCost.AutoSize = true;
            this.rbMonthlyCost.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMonthlyCost.Location = new System.Drawing.Point(10, 43);
            this.rbMonthlyCost.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbMonthlyCost.Name = "rbMonthlyCost";
            this.rbMonthlyCost.Size = new System.Drawing.Size(81, 24);
            this.rbMonthlyCost.TabIndex = 21;
            this.rbMonthlyCost.Text = "Kuutasu";
            this.rbMonthlyCost.UseVisualStyleBackColor = true;
            // 
            // tbMonthlyPrice
            // 
            this.tbMonthlyPrice.Enabled = false;
            this.tbMonthlyPrice.Location = new System.Drawing.Point(95, 46);
            this.tbMonthlyPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbMonthlyPrice.Name = "tbMonthlyPrice";
            this.tbMonthlyPrice.Size = new System.Drawing.Size(73, 21);
            this.tbMonthlyPrice.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbMonthlyPrice);
            this.groupBox1.Controls.Add(this.rbMonthlyCost);
            this.groupBox1.Controls.Add(this.rbStockPrice);
            this.groupBox1.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(279, 546);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(175, 79);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hinnatüüp";
            // 
            // Kasutajaliides
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 639);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbShowTabel);
            this.Controls.Add(this.cbShowPrice);
            this.Controls.Add(this.dateStopTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateStartTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.btnAvaCSV);
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
            this.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "Kasutajaliides";
            this.Text = "Elektrihinna kalkulaator";
            this.Load += new System.EventHandler(this.Kasutajaliides_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartElektrihind)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Button btnAvaCSV;
        private System.Windows.Forms.TextBox txtDebug;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateStartTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateStopTime;
        private System.Windows.Forms.CheckBox cbShowPrice;
        private System.Windows.Forms.CheckBox cbShowTabel;
        private System.Windows.Forms.RadioButton rbStockPrice;
        private System.Windows.Forms.RadioButton rbMonthlyCost;
        private System.Windows.Forms.TextBox tbMonthlyPrice;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

