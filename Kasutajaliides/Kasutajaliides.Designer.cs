﻿
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chartPrice = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
            this.groupPriceType = new System.Windows.Forms.GroupBox();
            this.lblRate = new System.Windows.Forms.Label();
            this.tablePrice = new System.Windows.Forms.DataGridView();
            this.Aeg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblEur = new System.Windows.Forms.Label();
            this.btnChangeSize = new System.Windows.Forms.Button();
            this.lblBeginning = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblAndresEek = new System.Windows.Forms.Label();
            this.cbShowUsage = new System.Windows.Forms.CheckBox();
            this.lblCostNow = new System.Windows.Forms.Label();
            this.txtCostNow = new System.Windows.Forms.TextBox();
            this.lblSKwh2 = new System.Windows.Forms.Label();
            this.lblTarbimisAeg = new System.Windows.Forms.Label();
            this.txtTarbimisAeg = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartPrice)).BeginInit();
            this.groupPriceType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePrice)).BeginInit();
            this.SuspendLayout();
            // 
            // chartPrice
            // 
            chartArea4.AxisX.MajorGrid.Enabled = false;
            chartArea4.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea4.AxisX.Title = "Aeg";
            chartArea4.AxisX.TitleFont = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.AxisX2.TitleFont = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.AxisY.MajorGrid.Enabled = false;
            chartArea4.AxisY.Title = "kWh";
            chartArea4.AxisY.TitleFont = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea4.AxisY2.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated270;
            chartArea4.AxisY2.Title = "s/kWh";
            chartArea4.AxisY2.TitleFont = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.Name = "ChartArea1";
            this.chartPrice.ChartAreas.Add(chartArea4);
            legend4.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend4.IsTextAutoFit = false;
            legend4.Name = "Legend1";
            this.chartPrice.Legends.Add(legend4);
            this.chartPrice.Location = new System.Drawing.Point(289, 13);
            this.chartPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chartPrice.Name = "chartPrice";
            this.chartPrice.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series7.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series7.Legend = "Legend1";
            series7.Name = "Elektrihind";
            series7.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series7.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series7.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series8.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series8.Legend = "Legend1";
            series8.Name = "Tarbimine";
            series8.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series8.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chartPrice.Series.Add(series7);
            this.chartPrice.Series.Add(series8);
            this.chartPrice.Size = new System.Drawing.Size(742, 485);
            this.chartPrice.TabIndex = 0;
            this.chartPrice.Text = "chartElektrihind";
            this.chartPrice.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartPrice_MouseMove);
            // 
            // cbKasutusmall
            // 
            this.cbKasutusmall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKasutusmall.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbKasutusmall.FormattingEnabled = true;
            this.cbKasutusmall.Location = new System.Drawing.Point(14, 66);
            this.cbKasutusmall.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbKasutusmall.Name = "cbKasutusmall";
            this.cbKasutusmall.Size = new System.Drawing.Size(240, 28);
            this.cbKasutusmall.TabIndex = 2;
            this.cbKasutusmall.SelectedValueChanged += new System.EventHandler(this.cbKasutusmall_SelectedValueChanged);
            // 
            // lblKasutusmall
            // 
            this.lblKasutusmall.AutoSize = true;
            this.lblKasutusmall.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKasutusmall.Location = new System.Drawing.Point(11, 38);
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
            this.lblAeg.Location = new System.Drawing.Point(11, 113);
            this.lblAeg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAeg.Name = "lblAeg";
            this.lblAeg.Size = new System.Drawing.Size(59, 20);
            this.lblAeg.TabIndex = 4;
            this.lblAeg.Text = "Ajakulu:";
            // 
            // txtAjakulu
            // 
            this.txtAjakulu.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAjakulu.Location = new System.Drawing.Point(14, 141);
            this.txtAjakulu.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtAjakulu.Name = "txtAjakulu";
            this.txtAjakulu.Size = new System.Drawing.Size(183, 27);
            this.txtAjakulu.TabIndex = 5;
            this.txtAjakulu.TextChanged += new System.EventHandler(this.txtAjakulu_TextChanged);
            this.txtAjakulu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAjakulu_KeyPress);
            // 
            // lblTund
            // 
            this.lblTund.AutoSize = true;
            this.lblTund.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTund.Location = new System.Drawing.Point(201, 143);
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
            this.lblVoimsus.Location = new System.Drawing.Point(10, 196);
            this.lblVoimsus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVoimsus.Name = "lblVoimsus";
            this.lblVoimsus.Size = new System.Drawing.Size(102, 20);
            this.lblVoimsus.TabIndex = 7;
            this.lblVoimsus.Text = "Võimsustarve:";
            // 
            // txtVoimsus
            // 
            this.txtVoimsus.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVoimsus.Location = new System.Drawing.Point(14, 224);
            this.txtVoimsus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtVoimsus.Name = "txtVoimsus";
            this.txtVoimsus.Size = new System.Drawing.Size(183, 27);
            this.txtVoimsus.TabIndex = 8;
            this.txtVoimsus.TextChanged += new System.EventHandler(this.txtVoimsus_TextChanged);
            this.txtVoimsus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVoimsus_KeyPress);
            // 
            // lblkW
            // 
            this.lblkW.AutoSize = true;
            this.lblkW.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkW.Location = new System.Drawing.Point(201, 226);
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
            this.lblHind.Location = new System.Drawing.Point(10, 273);
            this.lblHind.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHind.Name = "lblHind";
            this.lblHind.Size = new System.Drawing.Size(41, 20);
            this.lblHind.TabIndex = 10;
            this.lblHind.Text = "Hind:";
            // 
            // txtHind
            // 
            this.txtHind.Enabled = false;
            this.txtHind.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHind.Location = new System.Drawing.Point(14, 301);
            this.txtHind.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtHind.Name = "txtHind";
            this.txtHind.ReadOnly = true;
            this.txtHind.Size = new System.Drawing.Size(183, 27);
            this.txtHind.TabIndex = 11;
            // 
            // btnAvaCSV
            // 
            this.btnAvaCSV.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvaCSV.Location = new System.Drawing.Point(16, 466);
            this.btnAvaCSV.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAvaCSV.Name = "btnAvaCSV";
            this.btnAvaCSV.Size = new System.Drawing.Size(130, 31);
            this.btnAvaCSV.TabIndex = 12;
            this.btnAvaCSV.Text = "Ava CŠV fail";
            this.btnAvaCSV.UseVisualStyleBackColor = true;
            this.btnAvaCSV.Click += new System.EventHandler(this.btnAvaCSV_Click);
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(2, 402);
            this.txtDebug.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDebug.Size = new System.Drawing.Size(283, 58);
            this.txtDebug.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 25;
            // 
            // dateStartTime
            // 
            this.dateStartTime.CalendarFont = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.dateStartTime.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.dateStartTime.Location = new System.Drawing.Point(353, 506);
            this.dateStartTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dateStartTime.Name = "dateStartTime";
            this.dateStartTime.Size = new System.Drawing.Size(304, 27);
            this.dateStartTime.TabIndex = 15;
            this.dateStartTime.CloseUp += new System.EventHandler(this.dateStartTime_CloseUp);
            this.dateStartTime.DropDown += new System.EventHandler(this.dateStartTime_DropDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 24;
            // 
            // dateStopTime
            // 
            this.dateStopTime.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.dateStopTime.Location = new System.Drawing.Point(727, 506);
            this.dateStopTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dateStopTime.Name = "dateStopTime";
            this.dateStopTime.Size = new System.Drawing.Size(324, 27);
            this.dateStopTime.TabIndex = 17;
            this.dateStopTime.CloseUp += new System.EventHandler(this.dateStopTime_CloseUp);
            this.dateStopTime.DropDown += new System.EventHandler(this.dateStopTime_DropDown);
            // 
            // cbShowPrice
            // 
            this.cbShowPrice.AutoSize = true;
            this.cbShowPrice.Checked = true;
            this.cbShowPrice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowPrice.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowPrice.Location = new System.Drawing.Point(11, 530);
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
            this.cbShowTabel.Location = new System.Drawing.Point(11, 600);
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
            this.rbStockPrice.Location = new System.Drawing.Point(10, 25);
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
            this.rbMonthlyCost.Location = new System.Drawing.Point(10, 60);
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
            this.tbMonthlyPrice.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbMonthlyPrice.Location = new System.Drawing.Point(126, 59);
            this.tbMonthlyPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbMonthlyPrice.Name = "tbMonthlyPrice";
            this.tbMonthlyPrice.Size = new System.Drawing.Size(169, 27);
            this.tbMonthlyPrice.TabIndex = 22;
            this.tbMonthlyPrice.TextChanged += new System.EventHandler(this.tbMonthlyPrice_TextChanged);
            this.tbMonthlyPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMonthlyPrice_KeyPress);
            // 
            // groupPriceType
            // 
            this.groupPriceType.Controls.Add(this.lblRate);
            this.groupPriceType.Controls.Add(this.tbMonthlyPrice);
            this.groupPriceType.Controls.Add(this.rbMonthlyCost);
            this.groupPriceType.Controls.Add(this.rbStockPrice);
            this.groupPriceType.Font = new System.Drawing.Font("Impact", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.groupPriceType.Location = new System.Drawing.Point(289, 548);
            this.groupPriceType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupPriceType.Name = "groupPriceType";
            this.groupPriceType.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupPriceType.Size = new System.Drawing.Size(368, 105);
            this.groupPriceType.TabIndex = 23;
            this.groupPriceType.TabStop = false;
            this.groupPriceType.Text = "Hinnatüüp";
            // 
            // lblRate
            // 
            this.lblRate.AutoSize = true;
            this.lblRate.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRate.Location = new System.Drawing.Point(299, 62);
            this.lblRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(52, 20);
            this.lblRate.TabIndex = 26;
            this.lblRate.Text = "s/kWh";
            // 
            // tablePrice
            // 
            this.tablePrice.AllowUserToAddRows = false;
            this.tablePrice.AllowUserToDeleteRows = false;
            this.tablePrice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablePrice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Aeg,
            this.Hind});
            this.tablePrice.Location = new System.Drawing.Point(289, 13);
            this.tablePrice.Name = "tablePrice";
            this.tablePrice.ReadOnly = true;
            this.tablePrice.RowHeadersWidth = 51;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablePrice.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.tablePrice.RowTemplate.Height = 24;
            this.tablePrice.Size = new System.Drawing.Size(742, 484);
            this.tablePrice.TabIndex = 24;
            // 
            // Aeg
            // 
            this.Aeg.HeaderText = "Aeg";
            this.Aeg.MinimumWidth = 6;
            this.Aeg.Name = "Aeg";
            this.Aeg.ReadOnly = true;
            this.Aeg.Width = 125;
            // 
            // Hind
            // 
            this.Hind.HeaderText = "Hind";
            this.Hind.MinimumWidth = 6;
            this.Hind.Name = "Hind";
            this.Hind.ReadOnly = true;
            this.Hind.Width = 125;
            // 
            // lblEur
            // 
            this.lblEur.Location = new System.Drawing.Point(0, 0);
            this.lblEur.Name = "lblEur";
            this.lblEur.Size = new System.Drawing.Size(100, 23);
            this.lblEur.TabIndex = 0;
            // 
            // btnChangeSize
            // 
            this.btnChangeSize.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeSize.Location = new System.Drawing.Point(979, 608);
            this.btnChangeSize.Name = "btnChangeSize";
            this.btnChangeSize.Size = new System.Drawing.Size(51, 54);
            this.btnChangeSize.TabIndex = 26;
            this.btnChangeSize.Text = "+";
            this.btnChangeSize.UseVisualStyleBackColor = true;
            this.btnChangeSize.Click += new System.EventHandler(this.btnNormalSize_Click);
            // 
            // lblBeginning
            // 
            this.lblBeginning.AutoSize = true;
            this.lblBeginning.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeginning.Location = new System.Drawing.Point(273, 506);
            this.lblBeginning.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBeginning.Name = "lblBeginning";
            this.lblBeginning.Size = new System.Drawing.Size(47, 20);
            this.lblBeginning.TabIndex = 14;
            this.lblBeginning.Text = "Algus:";
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnd.Location = new System.Drawing.Point(661, 506);
            this.lblEnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(42, 20);
            this.lblEnd.TabIndex = 16;
            this.lblEnd.Text = "Lõpp:";
            // 
            // lblAndresEek
            // 
            this.lblAndresEek.AutoSize = true;
            this.lblAndresEek.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAndresEek.Location = new System.Drawing.Point(201, 303);
            this.lblAndresEek.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAndresEek.Name = "lblAndresEek";
            this.lblAndresEek.Size = new System.Drawing.Size(18, 20);
            this.lblAndresEek.TabIndex = 27;
            this.lblAndresEek.Text = "€";
            // 
            // cbShowUsage
            // 
            this.cbShowUsage.AutoSize = true;
            this.cbShowUsage.Checked = true;
            this.cbShowUsage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowUsage.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowUsage.Location = new System.Drawing.Point(11, 565);
            this.cbShowUsage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbShowUsage.Name = "cbShowUsage";
            this.cbShowUsage.Size = new System.Drawing.Size(127, 24);
            this.cbShowUsage.TabIndex = 28;
            this.cbShowUsage.Text = "Kuva tarbimine";
            this.cbShowUsage.UseVisualStyleBackColor = true;
            this.cbShowUsage.CheckedChanged += new System.EventHandler(this.cbShowUsage_CheckedChanged);
            // 
            // lblCostNow
            // 
            this.lblCostNow.AutoSize = true;
            this.lblCostNow.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostNow.Location = new System.Drawing.Point(11, 649);
            this.lblCostNow.Name = "lblCostNow";
            this.lblCostNow.Size = new System.Drawing.Size(90, 20);
            this.lblCostNow.TabIndex = 29;
            this.lblCostNow.Text = "Hind praegu:";
            // 
            // txtCostNow
            // 
            this.txtCostNow.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCostNow.Location = new System.Drawing.Point(132, 649);
            this.txtCostNow.Name = "txtCostNow";
            this.txtCostNow.ReadOnly = true;
            this.txtCostNow.Size = new System.Drawing.Size(78, 27);
            this.txtCostNow.TabIndex = 30;
            // 
            // lblSKwh2
            // 
            this.lblSKwh2.AutoSize = true;
            this.lblSKwh2.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSKwh2.Location = new System.Drawing.Point(215, 652);
            this.lblSKwh2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSKwh2.Name = "lblSKwh2";
            this.lblSKwh2.Size = new System.Drawing.Size(52, 20);
            this.lblSKwh2.TabIndex = 27;
            this.lblSKwh2.Text = "s/kWh";
            // 
            // lblTarbimisAeg
            // 
            this.lblTarbimisAeg.AutoSize = true;
            this.lblTarbimisAeg.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTarbimisAeg.Location = new System.Drawing.Point(11, 336);
            this.lblTarbimisAeg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTarbimisAeg.Name = "lblTarbimisAeg";
            this.lblTarbimisAeg.Size = new System.Drawing.Size(92, 20);
            this.lblTarbimisAeg.TabIndex = 31;
            this.lblTarbimisAeg.Text = "Tarbimisaeg:";
            // 
            // txtTarbimisAeg
            // 
            this.txtTarbimisAeg.Enabled = false;
            this.txtTarbimisAeg.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTarbimisAeg.Location = new System.Drawing.Point(11, 364);
            this.txtTarbimisAeg.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtTarbimisAeg.Name = "txtTarbimisAeg";
            this.txtTarbimisAeg.ReadOnly = true;
            this.txtTarbimisAeg.Size = new System.Drawing.Size(183, 27);
            this.txtTarbimisAeg.TabIndex = 32;
            // 
            // Kasutajaliides
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 696);
            this.Controls.Add(this.txtTarbimisAeg);
            this.Controls.Add(this.lblTarbimisAeg);
            this.Controls.Add(this.lblSKwh2);
            this.Controls.Add(this.txtCostNow);
            this.Controls.Add(this.lblCostNow);
            this.Controls.Add(this.cbShowUsage);
            this.Controls.Add(this.lblAndresEek);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.lblBeginning);
            this.Controls.Add(this.btnChangeSize);
            this.Controls.Add(this.lblEur);
            this.Controls.Add(this.groupPriceType);
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
            this.Controls.Add(this.chartPrice);
            this.Controls.Add(this.tablePrice);
            this.Font = new System.Drawing.Font("Impact", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "Kasutajaliides";
            this.Text = "Elektrihinna kalkulaator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Kasutajaliides_FormClosing);
            this.Load += new System.EventHandler(this.Kasutajaliides_Load);
            this.Resize += new System.EventHandler(this.Kasutajaliides_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.chartPrice)).EndInit();
            this.groupPriceType.ResumeLayout(false);
            this.groupPriceType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartPrice;
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
        private System.Windows.Forms.GroupBox groupPriceType;
        private System.Windows.Forms.DataGridView tablePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Aeg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hind;
        private System.Windows.Forms.Label lblRate;
        private System.Windows.Forms.Label lblEur;
        private System.Windows.Forms.Button btnChangeSize;
        private System.Windows.Forms.Label lblBeginning;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblAndresEek;
        private System.Windows.Forms.CheckBox cbShowUsage;
        private System.Windows.Forms.Label lblCostNow;
        private System.Windows.Forms.TextBox txtCostNow;
        private System.Windows.Forms.Label lblSKwh2;
        private System.Windows.Forms.Label lblTarbimisAeg;
        private System.Windows.Forms.TextBox txtTarbimisAeg;
    }
}
