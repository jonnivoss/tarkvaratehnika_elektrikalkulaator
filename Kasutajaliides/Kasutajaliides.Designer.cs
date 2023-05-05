
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kasutajaliides));
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
            this.dateStartTime = new System.Windows.Forms.DateTimePicker();
            this.dateStopTime = new System.Windows.Forms.DateTimePicker();
            this.cbShowPrice = new System.Windows.Forms.CheckBox();
            this.cbShowTabel = new System.Windows.Forms.CheckBox();
            this.rbStockPrice = new System.Windows.Forms.RadioButton();
            this.rbMonthlyCost = new System.Windows.Forms.RadioButton();
            this.txtMonthlyPrice = new System.Windows.Forms.TextBox();
            this.groupPriceType = new System.Windows.Forms.GroupBox();
            this.lblRate = new System.Windows.Forms.Label();
            this.tablePrice = new System.Windows.Forms.DataGridView();
            this.Aeg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hind = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.btnDarkMode = new System.Windows.Forms.Button();
            this.tablePackages = new System.Windows.Forms.DataGridView();
            this.Indeks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProviderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pakett = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonthlyPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellerMarginal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BasePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NightPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsStockPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsGreenPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsageCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplateCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOpenPackages = new System.Windows.Forms.Button();
            this.tmrCostNow = new System.Windows.Forms.Timer(this.components);
            this.groupExport = new System.Windows.Forms.GroupBox();
            this.btnExportSave = new System.Windows.Forms.Button();
            this.btnExportOpen = new System.Windows.Forms.Button();
            this.cbExportAppend = new System.Windows.Forms.CheckBox();
            this.txtExportPath = new System.Windows.Forms.TextBox();
            this.lblExportPath = new System.Windows.Forms.Label();
            this.txtExportQualifier = new System.Windows.Forms.TextBox();
            this.lblExportQualifier = new System.Windows.Forms.Label();
            this.txtExportDelimiter = new System.Windows.Forms.TextBox();
            this.lblExportDelimiter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartPrice)).BeginInit();
            this.groupPriceType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePackages)).BeginInit();
            this.groupExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartPrice
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea1.AxisX.Title = "Aeg";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX2.TitleFont = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.Title = "kWh";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY2.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated270;
            chartArea1.AxisY2.Title = "s/kWh";
            chartArea1.AxisY2.TitleFont = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.Name = "ChartArea1";
            this.chartPrice.ChartAreas.Add(chartArea1);
            legend1.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chartPrice.Legends.Add(legend1);
            this.chartPrice.Location = new System.Drawing.Point(289, 12);
            this.chartPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chartPrice.Name = "chartPrice";
            this.chartPrice.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series1.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.Name = "Tarbimine";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series2.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series2.Legend = "Legend1";
            series2.Name = "Elektrihind";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series2.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chartPrice.Series.Add(series1);
            this.chartPrice.Series.Add(series2);
            this.chartPrice.Size = new System.Drawing.Size(762, 485);
            this.chartPrice.TabIndex = 0;
            this.chartPrice.Text = "chartElektrihind";
            this.chartPrice.MouseDown += new System.Windows.Forms.MouseEventHandler(this.priceChart_MouseDown);
            this.chartPrice.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartPrice_MouseMove);
            this.chartPrice.MouseUp += new System.Windows.Forms.MouseEventHandler(this.priceChart_MouseUp);
            // 
            // cbKasutusmall
            // 
            this.cbKasutusmall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKasutusmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbKasutusmall.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbKasutusmall.FormattingEnabled = true;
            this.cbKasutusmall.Location = new System.Drawing.Point(12, 47);
            this.cbKasutusmall.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbKasutusmall.Name = "cbKasutusmall";
            this.cbKasutusmall.Size = new System.Drawing.Size(240, 33);
            this.cbKasutusmall.TabIndex = 2;
            this.cbKasutusmall.SelectedValueChanged += new System.EventHandler(this.cbKasutusmall_SelectedValueChanged);
            // 
            // lblKasutusmall
            // 
            this.lblKasutusmall.AutoSize = true;
            this.lblKasutusmall.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKasutusmall.Location = new System.Drawing.Point(11, 13);
            this.lblKasutusmall.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblKasutusmall.Name = "lblKasutusmall";
            this.lblKasutusmall.Size = new System.Drawing.Size(160, 25);
            this.lblKasutusmall.TabIndex = 3;
            this.lblKasutusmall.Text = "Kasutusmalli valik";
            // 
            // lblAeg
            // 
            this.lblAeg.AutoSize = true;
            this.lblAeg.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAeg.Location = new System.Drawing.Point(12, 87);
            this.lblAeg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAeg.Name = "lblAeg";
            this.lblAeg.Size = new System.Drawing.Size(76, 25);
            this.lblAeg.TabIndex = 4;
            this.lblAeg.Text = "Ajakulu:";
            // 
            // txtAjakulu
            // 
            this.txtAjakulu.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAjakulu.Location = new System.Drawing.Point(15, 119);
            this.txtAjakulu.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtAjakulu.MaxLength = 128;
            this.txtAjakulu.Name = "txtAjakulu";
            this.txtAjakulu.Size = new System.Drawing.Size(183, 32);
            this.txtAjakulu.TabIndex = 5;
            this.txtAjakulu.TextChanged += new System.EventHandler(this.txtAjakulu_TextChanged);
            this.txtAjakulu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAjakulu_KeyPress);
            // 
            // lblTund
            // 
            this.lblTund.AutoSize = true;
            this.lblTund.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTund.Location = new System.Drawing.Point(202, 122);
            this.lblTund.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTund.Name = "lblTund";
            this.lblTund.Size = new System.Drawing.Size(53, 25);
            this.lblTund.TabIndex = 6;
            this.lblTund.Text = "tundi";
            // 
            // lblVoimsus
            // 
            this.lblVoimsus.AutoSize = true;
            this.lblVoimsus.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVoimsus.Location = new System.Drawing.Point(12, 168);
            this.lblVoimsus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVoimsus.Name = "lblVoimsus";
            this.lblVoimsus.Size = new System.Drawing.Size(126, 25);
            this.lblVoimsus.TabIndex = 7;
            this.lblVoimsus.Text = "Võimsustarve:";
            // 
            // txtVoimsus
            // 
            this.txtVoimsus.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVoimsus.Location = new System.Drawing.Point(15, 200);
            this.txtVoimsus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtVoimsus.MaxLength = 128;
            this.txtVoimsus.Name = "txtVoimsus";
            this.txtVoimsus.Size = new System.Drawing.Size(183, 32);
            this.txtVoimsus.TabIndex = 8;
            this.txtVoimsus.TextChanged += new System.EventHandler(this.txtVoimsus_TextChanged);
            this.txtVoimsus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVoimsus_KeyPress);
            // 
            // lblkW
            // 
            this.lblkW.AutoSize = true;
            this.lblkW.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkW.Location = new System.Drawing.Point(202, 201);
            this.lblkW.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblkW.Name = "lblkW";
            this.lblkW.Size = new System.Drawing.Size(38, 25);
            this.lblkW.TabIndex = 9;
            this.lblkW.Text = "kW";
            // 
            // lblHind
            // 
            this.lblHind.AutoSize = true;
            this.lblHind.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHind.Location = new System.Drawing.Point(11, 247);
            this.lblHind.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHind.Name = "lblHind";
            this.lblHind.Size = new System.Drawing.Size(52, 25);
            this.lblHind.TabIndex = 10;
            this.lblHind.Text = "Hind:";
            // 
            // txtHind
            // 
            this.txtHind.Enabled = false;
            this.txtHind.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHind.Location = new System.Drawing.Point(15, 275);
            this.txtHind.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtHind.Name = "txtHind";
            this.txtHind.ReadOnly = true;
            this.txtHind.Size = new System.Drawing.Size(183, 32);
            this.txtHind.TabIndex = 11;
            // 
            // btnAvaCSV
            // 
            this.btnAvaCSV.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvaCSV.Location = new System.Drawing.Point(12, 455);
            this.btnAvaCSV.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAvaCSV.Name = "btnAvaCSV";
            this.btnAvaCSV.Size = new System.Drawing.Size(130, 41);
            this.btnAvaCSV.TabIndex = 12;
            this.btnAvaCSV.Text = "Ava CSV fail";
            this.btnAvaCSV.UseVisualStyleBackColor = true;
            this.btnAvaCSV.Click += new System.EventHandler(this.btnAvaCSV_Click);
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(12, 388);
            this.txtDebug.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDebug.Size = new System.Drawing.Size(254, 58);
            this.txtDebug.TabIndex = 13;
            // 
            // dateStartTime
            // 
            this.dateStartTime.CalendarFont = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.dateStartTime.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.dateStartTime.Location = new System.Drawing.Point(353, 506);
            this.dateStartTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dateStartTime.Name = "dateStartTime";
            this.dateStartTime.Size = new System.Drawing.Size(304, 32);
            this.dateStartTime.TabIndex = 15;
            this.dateStartTime.CloseUp += new System.EventHandler(this.dateStartTime_CloseUp);
            this.dateStartTime.DropDown += new System.EventHandler(this.dateStartTime_DropDown);
            // 
            // dateStopTime
            // 
            this.dateStopTime.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.dateStopTime.Location = new System.Drawing.Point(727, 506);
            this.dateStopTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dateStopTime.Name = "dateStopTime";
            this.dateStopTime.Size = new System.Drawing.Size(324, 32);
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
            this.cbShowPrice.Location = new System.Drawing.Point(11, 513);
            this.cbShowPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbShowPrice.Name = "cbShowPrice";
            this.cbShowPrice.Size = new System.Drawing.Size(154, 29);
            this.cbShowPrice.TabIndex = 18;
            this.cbShowPrice.Text = "Kuva börsihind";
            this.cbShowPrice.UseVisualStyleBackColor = true;
            this.cbShowPrice.CheckedChanged += new System.EventHandler(this.cbShowPrice_CheckedChanged);
            // 
            // cbShowTabel
            // 
            this.cbShowTabel.AutoSize = true;
            this.cbShowTabel.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowTabel.Location = new System.Drawing.Point(11, 583);
            this.cbShowTabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbShowTabel.Name = "cbShowTabel";
            this.cbShowTabel.Size = new System.Drawing.Size(119, 29);
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
            this.rbStockPrice.Location = new System.Drawing.Point(10, 33);
            this.rbStockPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbStockPrice.Name = "rbStockPrice";
            this.rbStockPrice.Size = new System.Drawing.Size(110, 29);
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
            this.rbMonthlyCost.Location = new System.Drawing.Point(10, 75);
            this.rbMonthlyCost.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbMonthlyCost.Name = "rbMonthlyCost";
            this.rbMonthlyCost.Size = new System.Drawing.Size(99, 29);
            this.rbMonthlyCost.TabIndex = 21;
            this.rbMonthlyCost.Text = "Kuutasu";
            this.rbMonthlyCost.UseVisualStyleBackColor = true;
            // 
            // txtMonthlyPrice
            // 
            this.txtMonthlyPrice.Enabled = false;
            this.txtMonthlyPrice.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.txtMonthlyPrice.Location = new System.Drawing.Point(126, 74);
            this.txtMonthlyPrice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtMonthlyPrice.MaxLength = 128;
            this.txtMonthlyPrice.Name = "txtMonthlyPrice";
            this.txtMonthlyPrice.Size = new System.Drawing.Size(169, 32);
            this.txtMonthlyPrice.TabIndex = 22;
            this.txtMonthlyPrice.TextChanged += new System.EventHandler(this.txtMonthlyPrice_TextChanged);
            this.txtMonthlyPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMonthlyPrice_KeyPress);
            // 
            // groupPriceType
            // 
            this.groupPriceType.Controls.Add(this.lblRate);
            this.groupPriceType.Controls.Add(this.txtMonthlyPrice);
            this.groupPriceType.Controls.Add(this.rbMonthlyCost);
            this.groupPriceType.Controls.Add(this.rbStockPrice);
            this.groupPriceType.Font = new System.Drawing.Font("Impact", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.groupPriceType.Location = new System.Drawing.Point(306, 548);
            this.groupPriceType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupPriceType.Name = "groupPriceType";
            this.groupPriceType.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupPriceType.Size = new System.Drawing.Size(382, 118);
            this.groupPriceType.TabIndex = 23;
            this.groupPriceType.TabStop = false;
            this.groupPriceType.Text = "Hinnatüüp";
            // 
            // lblRate
            // 
            this.lblRate.AutoSize = true;
            this.lblRate.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRate.Location = new System.Drawing.Point(299, 77);
            this.lblRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(65, 25);
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablePrice.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tablePrice.RowTemplate.Height = 24;
            this.tablePrice.Size = new System.Drawing.Size(762, 484);
            this.tablePrice.TabIndex = 24;
            // 
            // Aeg
            // 
            this.Aeg.HeaderText = "Aeg";
            this.Aeg.MinimumWidth = 6;
            this.Aeg.Name = "Aeg";
            this.Aeg.ReadOnly = true;
            this.Aeg.Width = 350;
            // 
            // Hind
            // 
            this.Hind.HeaderText = "Hind";
            this.Hind.MinimumWidth = 6;
            this.Hind.Name = "Hind";
            this.Hind.ReadOnly = true;
            this.Hind.Width = 350;
            // 
            // btnChangeSize
            // 
            this.btnChangeSize.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeSize.Location = new System.Drawing.Point(1000, 625);
            this.btnChangeSize.Name = "btnChangeSize";
            this.btnChangeSize.Size = new System.Drawing.Size(51, 41);
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
            this.lblBeginning.Size = new System.Drawing.Size(60, 25);
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
            this.lblEnd.Size = new System.Drawing.Size(54, 25);
            this.lblEnd.TabIndex = 16;
            this.lblEnd.Text = "Lõpp:";
            // 
            // lblAndresEek
            // 
            this.lblAndresEek.AutoSize = true;
            this.lblAndresEek.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAndresEek.Location = new System.Drawing.Point(202, 278);
            this.lblAndresEek.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAndresEek.Name = "lblAndresEek";
            this.lblAndresEek.Size = new System.Drawing.Size(23, 25);
            this.lblAndresEek.TabIndex = 27;
            this.lblAndresEek.Text = "€";
            // 
            // cbShowUsage
            // 
            this.cbShowUsage.AutoSize = true;
            this.cbShowUsage.Checked = true;
            this.cbShowUsage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowUsage.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowUsage.Location = new System.Drawing.Point(11, 548);
            this.cbShowUsage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbShowUsage.Name = "cbShowUsage";
            this.cbShowUsage.Size = new System.Drawing.Size(156, 29);
            this.cbShowUsage.TabIndex = 28;
            this.cbShowUsage.Text = "Kuva tarbimine";
            this.cbShowUsage.UseVisualStyleBackColor = true;
            this.cbShowUsage.CheckedChanged += new System.EventHandler(this.cbShowUsage_CheckedChanged);
            // 
            // lblCostNow
            // 
            this.lblCostNow.AutoSize = true;
            this.lblCostNow.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostNow.Location = new System.Drawing.Point(12, 633);
            this.lblCostNow.Name = "lblCostNow";
            this.lblCostNow.Size = new System.Drawing.Size(113, 25);
            this.lblCostNow.TabIndex = 29;
            this.lblCostNow.Text = "Hind praegu:";
            // 
            // txtCostNow
            // 
            this.txtCostNow.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCostNow.Location = new System.Drawing.Point(147, 630);
            this.txtCostNow.Name = "txtCostNow";
            this.txtCostNow.ReadOnly = true;
            this.txtCostNow.Size = new System.Drawing.Size(78, 32);
            this.txtCostNow.TabIndex = 30;
            // 
            // lblSKwh2
            // 
            this.lblSKwh2.AutoSize = true;
            this.lblSKwh2.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSKwh2.Location = new System.Drawing.Point(225, 634);
            this.lblSKwh2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSKwh2.Name = "lblSKwh2";
            this.lblSKwh2.Size = new System.Drawing.Size(65, 25);
            this.lblSKwh2.TabIndex = 27;
            this.lblSKwh2.Text = "s/kWh";
            // 
            // lblTarbimisAeg
            // 
            this.lblTarbimisAeg.AutoSize = true;
            this.lblTarbimisAeg.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTarbimisAeg.Location = new System.Drawing.Point(12, 318);
            this.lblTarbimisAeg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTarbimisAeg.Name = "lblTarbimisAeg";
            this.lblTarbimisAeg.Size = new System.Drawing.Size(187, 25);
            this.lblTarbimisAeg.TabIndex = 31;
            this.lblTarbimisAeg.Text = "Odavaim tarbimisaeg:";
            // 
            // txtTarbimisAeg
            // 
            this.txtTarbimisAeg.Enabled = false;
            this.txtTarbimisAeg.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTarbimisAeg.Location = new System.Drawing.Point(12, 346);
            this.txtTarbimisAeg.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtTarbimisAeg.Name = "txtTarbimisAeg";
            this.txtTarbimisAeg.ReadOnly = true;
            this.txtTarbimisAeg.Size = new System.Drawing.Size(186, 32);
            this.txtTarbimisAeg.TabIndex = 32;
            // 
            // btnDarkMode
            // 
            this.btnDarkMode.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDarkMode.Location = new System.Drawing.Point(1000, 575);
            this.btnDarkMode.Name = "btnDarkMode";
            this.btnDarkMode.Size = new System.Drawing.Size(51, 41);
            this.btnDarkMode.TabIndex = 33;
            this.btnDarkMode.Text = "D";
            this.btnDarkMode.UseVisualStyleBackColor = true;
            this.btnDarkMode.Click += new System.EventHandler(this.btnDarkMode_Click);
            // 
            // tablePackages
            // 
            this.tablePackages.AllowUserToAddRows = false;
            this.tablePackages.AllowUserToDeleteRows = false;
            this.tablePackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablePackages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Indeks,
            this.ProviderName,
            this.Pakett,
            this.MonthlyPrice,
            this.SellerMarginal,
            this.BasePrice,
            this.NightPrice,
            this.IsStockPackage,
            this.IsGreenPackage,
            this.EndPrice,
            this.UsageCost,
            this.TemplateCost});
            this.tablePackages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePackages.Location = new System.Drawing.Point(0, 744);
            this.tablePackages.Name = "tablePackages";
            this.tablePackages.ReadOnly = true;
            this.tablePackages.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablePackages.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.tablePackages.RowTemplate.Height = 24;
            this.tablePackages.Size = new System.Drawing.Size(1067, 191);
            this.tablePackages.TabIndex = 34;
            this.tablePackages.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tablePackages_RowHeaderMouseClick);
            this.tablePackages.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tablePackages_RowHeaderMouseDoubleClick);
            // 
            // Indeks
            // 
            this.Indeks.HeaderText = "#";
            this.Indeks.MinimumWidth = 6;
            this.Indeks.Name = "Indeks";
            this.Indeks.ReadOnly = true;
            this.Indeks.Width = 6;
            // 
            // ProviderName
            // 
            this.ProviderName.HeaderText = "Ülepakkuja";
            this.ProviderName.MinimumWidth = 6;
            this.ProviderName.Name = "ProviderName";
            this.ProviderName.ReadOnly = true;
            this.ProviderName.Width = 220;
            // 
            // Pakett
            // 
            this.Pakett.HeaderText = "Pakett";
            this.Pakett.MinimumWidth = 6;
            this.Pakett.Name = "Pakett";
            this.Pakett.ReadOnly = true;
            this.Pakett.Width = 125;
            // 
            // MonthlyPrice
            // 
            this.MonthlyPrice.HeaderText = "Kuutasu (€)";
            this.MonthlyPrice.MinimumWidth = 6;
            this.MonthlyPrice.Name = "MonthlyPrice";
            this.MonthlyPrice.ReadOnly = true;
            this.MonthlyPrice.Width = 125;
            // 
            // SellerMarginal
            // 
            this.SellerMarginal.HeaderText = "Marginaal (s/kWh)";
            this.SellerMarginal.MinimumWidth = 6;
            this.SellerMarginal.Name = "SellerMarginal";
            this.SellerMarginal.ReadOnly = true;
            this.SellerMarginal.Width = 125;
            // 
            // BasePrice
            // 
            this.BasePrice.HeaderText = "Baashind/päevane (s/kWh)";
            this.BasePrice.MinimumWidth = 6;
            this.BasePrice.Name = "BasePrice";
            this.BasePrice.ReadOnly = true;
            this.BasePrice.Width = 140;
            // 
            // NightPrice
            // 
            this.NightPrice.HeaderText = "Öine hind (s/kWh)";
            this.NightPrice.MinimumWidth = 6;
            this.NightPrice.Name = "NightPrice";
            this.NightPrice.ReadOnly = true;
            this.NightPrice.Width = 90;
            // 
            // IsStockPackage
            // 
            this.IsStockPackage.HeaderText = "Börsipakett?";
            this.IsStockPackage.MinimumWidth = 6;
            this.IsStockPackage.Name = "IsStockPackage";
            this.IsStockPackage.ReadOnly = true;
            this.IsStockPackage.Width = 150;
            // 
            // IsGreenPackage
            // 
            this.IsGreenPackage.HeaderText = "Roheline?";
            this.IsGreenPackage.MinimumWidth = 6;
            this.IsGreenPackage.Name = "IsGreenPackage";
            this.IsGreenPackage.ReadOnly = true;
            this.IsGreenPackage.Width = 80;
            // 
            // EndPrice
            // 
            this.EndPrice.HeaderText = "Lõpphind (s/kWh)";
            this.EndPrice.MinimumWidth = 6;
            this.EndPrice.Name = "EndPrice";
            this.EndPrice.ReadOnly = true;
            this.EndPrice.Width = 125;
            // 
            // UsageCost
            // 
            this.UsageCost.HeaderText = "Tarbimishind (€)";
            this.UsageCost.MinimumWidth = 6;
            this.UsageCost.Name = "UsageCost";
            this.UsageCost.ReadOnly = true;
            this.UsageCost.Width = 125;
            // 
            // TemplateCost
            // 
            this.TemplateCost.HeaderText = "Mallihind (€)";
            this.TemplateCost.MinimumWidth = 6;
            this.TemplateCost.Name = "TemplateCost";
            this.TemplateCost.ReadOnly = true;
            this.TemplateCost.Width = 125;
            // 
            // btnOpenPackages
            // 
            this.btnOpenPackages.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenPackages.Location = new System.Drawing.Point(727, 625);
            this.btnOpenPackages.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnOpenPackages.Name = "btnOpenPackages";
            this.btnOpenPackages.Size = new System.Drawing.Size(268, 41);
            this.btnOpenPackages.TabIndex = 35;
            this.btnOpenPackages.Text = "Ava pakettide CSV";
            this.btnOpenPackages.UseVisualStyleBackColor = true;
            this.btnOpenPackages.Click += new System.EventHandler(this.btnOpenPackages_Click);
            // 
            // tmrCostNow
            // 
            this.tmrCostNow.Enabled = true;
            this.tmrCostNow.Interval = 60000;
            this.tmrCostNow.Tick += new System.EventHandler(this.tmrCostNow_Tick);
            // 
            // groupExport
            // 
            this.groupExport.Controls.Add(this.btnExportSave);
            this.groupExport.Controls.Add(this.btnExportOpen);
            this.groupExport.Controls.Add(this.cbExportAppend);
            this.groupExport.Controls.Add(this.txtExportPath);
            this.groupExport.Controls.Add(this.lblExportPath);
            this.groupExport.Controls.Add(this.txtExportQualifier);
            this.groupExport.Controls.Add(this.lblExportQualifier);
            this.groupExport.Controls.Add(this.txtExportDelimiter);
            this.groupExport.Controls.Add(this.lblExportDelimiter);
            this.groupExport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupExport.Location = new System.Drawing.Point(0, 672);
            this.groupExport.Name = "groupExport";
            this.groupExport.Size = new System.Drawing.Size(1067, 72);
            this.groupExport.TabIndex = 36;
            this.groupExport.TabStop = false;
            this.groupExport.Text = "CSV eksport";
            // 
            // btnExportSave
            // 
            this.btnExportSave.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportSave.Location = new System.Drawing.Point(426, 19);
            this.btnExportSave.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnExportSave.Name = "btnExportSave";
            this.btnExportSave.Size = new System.Drawing.Size(51, 41);
            this.btnExportSave.TabIndex = 42;
            this.btnExportSave.Text = "💾";
            this.btnExportSave.UseVisualStyleBackColor = true;
            this.btnExportSave.Click += new System.EventHandler(this.btnExportSave_Click);
            // 
            // btnExportOpen
            // 
            this.btnExportOpen.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportOpen.Location = new System.Drawing.Point(481, 19);
            this.btnExportOpen.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnExportOpen.Name = "btnExportOpen";
            this.btnExportOpen.Size = new System.Drawing.Size(105, 41);
            this.btnExportOpen.TabIndex = 37;
            this.btnExportOpen.Text = "Vali fail";
            this.btnExportOpen.UseVisualStyleBackColor = true;
            this.btnExportOpen.Click += new System.EventHandler(this.btnExportOpen_Click);
            // 
            // cbExportAppend
            // 
            this.cbExportAppend.AutoSize = true;
            this.cbExportAppend.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbExportAppend.Location = new System.Drawing.Point(307, 27);
            this.cbExportAppend.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbExportAppend.Name = "cbExportAppend";
            this.cbExportAppend.Size = new System.Drawing.Size(115, 29);
            this.cbExportAppend.TabIndex = 37;
            this.cbExportAppend.Text = "Lisa lõppu";
            this.cbExportAppend.UseVisualStyleBackColor = true;
            // 
            // txtExportPath
            // 
            this.txtExportPath.Enabled = false;
            this.txtExportPath.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExportPath.Location = new System.Drawing.Point(695, 25);
            this.txtExportPath.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtExportPath.MaxLength = 1;
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.Size = new System.Drawing.Size(361, 32);
            this.txtExportPath.TabIndex = 41;
            // 
            // lblExportPath
            // 
            this.lblExportPath.AutoSize = true;
            this.lblExportPath.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportPath.Location = new System.Drawing.Point(590, 28);
            this.lblExportPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExportPath.Name = "lblExportPath";
            this.lblExportPath.Size = new System.Drawing.Size(98, 25);
            this.lblExportPath.TabIndex = 40;
            this.lblExportPath.Text = "Kaustatee:";
            // 
            // txtExportQualifier
            // 
            this.txtExportQualifier.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExportQualifier.Location = new System.Drawing.Point(264, 24);
            this.txtExportQualifier.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtExportQualifier.MaxLength = 1;
            this.txtExportQualifier.Name = "txtExportQualifier";
            this.txtExportQualifier.Size = new System.Drawing.Size(39, 32);
            this.txtExportQualifier.TabIndex = 39;
            this.txtExportQualifier.Text = "\"";
            // 
            // lblExportQualifier
            // 
            this.lblExportQualifier.AutoSize = true;
            this.lblExportQualifier.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportQualifier.Location = new System.Drawing.Point(139, 28);
            this.lblExportQualifier.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExportQualifier.Name = "lblExportQualifier";
            this.lblExportQualifier.Size = new System.Drawing.Size(120, 25);
            this.lblExportQualifier.TabIndex = 38;
            this.lblExportQualifier.Text = "Kvalifikaator:";
            // 
            // txtExportDelimiter
            // 
            this.txtExportDelimiter.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExportDelimiter.Location = new System.Drawing.Point(96, 24);
            this.txtExportDelimiter.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtExportDelimiter.MaxLength = 1;
            this.txtExportDelimiter.Name = "txtExportDelimiter";
            this.txtExportDelimiter.Size = new System.Drawing.Size(39, 32);
            this.txtExportDelimiter.TabIndex = 37;
            this.txtExportDelimiter.Text = ";";
            // 
            // lblExportDelimiter
            // 
            this.lblExportDelimiter.AutoSize = true;
            this.lblExportDelimiter.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportDelimiter.Location = new System.Drawing.Point(11, 27);
            this.lblExportDelimiter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExportDelimiter.Name = "lblExportDelimiter";
            this.lblExportDelimiter.Size = new System.Drawing.Size(81, 25);
            this.lblExportDelimiter.TabIndex = 37;
            this.lblExportDelimiter.Text = "Eraldaja:";
            // 
            // Kasutajaliides
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 935);
            this.Controls.Add(this.groupExport);
            this.Controls.Add(this.btnOpenPackages);
            this.Controls.Add(this.tablePackages);
            this.Controls.Add(this.btnDarkMode);
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
            this.Controls.Add(this.groupPriceType);
            this.Controls.Add(this.cbShowTabel);
            this.Controls.Add(this.cbShowPrice);
            this.Controls.Add(this.dateStopTime);
            this.Controls.Add(this.dateStartTime);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            ((System.ComponentModel.ISupportInitialize)(this.tablePackages)).EndInit();
            this.groupExport.ResumeLayout(false);
            this.groupExport.PerformLayout();
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
        private System.Windows.Forms.DateTimePicker dateStartTime;
        private System.Windows.Forms.DateTimePicker dateStopTime;
        private System.Windows.Forms.CheckBox cbShowPrice;
        private System.Windows.Forms.CheckBox cbShowTabel;
        private System.Windows.Forms.RadioButton rbStockPrice;
        private System.Windows.Forms.RadioButton rbMonthlyCost;
        private System.Windows.Forms.TextBox txtMonthlyPrice;
        private System.Windows.Forms.GroupBox groupPriceType;
        private System.Windows.Forms.DataGridView tablePrice;
        private System.Windows.Forms.Label lblRate;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Aeg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hind;
        private System.Windows.Forms.Button btnDarkMode;
        private System.Windows.Forms.DataGridView tablePackages;
        private System.Windows.Forms.Button btnOpenPackages;
        private System.Windows.Forms.Timer tmrCostNow;
        private System.Windows.Forms.GroupBox groupExport;
        private System.Windows.Forms.TextBox txtExportPath;
        private System.Windows.Forms.Label lblExportPath;
        private System.Windows.Forms.TextBox txtExportQualifier;
        private System.Windows.Forms.Label lblExportQualifier;
        private System.Windows.Forms.TextBox txtExportDelimiter;
        private System.Windows.Forms.Label lblExportDelimiter;
        private System.Windows.Forms.CheckBox cbExportAppend;
        private System.Windows.Forms.Button btnExportSave;
        private System.Windows.Forms.Button btnExportOpen;
        private System.Windows.Forms.DataGridViewTextBoxColumn Indeks;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProviderName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pakett;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonthlyPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellerMarginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn BasePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn NightPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsStockPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsGreenPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsageCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateCost;
    }
}
