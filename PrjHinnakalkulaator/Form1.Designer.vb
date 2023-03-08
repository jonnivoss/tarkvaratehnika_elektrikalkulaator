<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtSisend = New System.Windows.Forms.TextBox()
        Me.cmbTeisendus = New System.Windows.Forms.ComboBox()
        Me.txtVastus = New System.Windows.Forms.TextBox()
        Me.lblSisendYhik = New System.Windows.Forms.Label()
        Me.lblVastusYhik = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtSisend
        '
        Me.txtSisend.Location = New System.Drawing.Point(63, 59)
        Me.txtSisend.Name = "txtSisend"
        Me.txtSisend.Size = New System.Drawing.Size(121, 20)
        Me.txtSisend.TabIndex = 0
        '
        'cmbTeisendus
        '
        Me.cmbTeisendus.FormattingEnabled = True
        Me.cmbTeisendus.Location = New System.Drawing.Point(63, 105)
        Me.cmbTeisendus.Name = "cmbTeisendus"
        Me.cmbTeisendus.Size = New System.Drawing.Size(121, 21)
        Me.cmbTeisendus.TabIndex = 1
        '
        'txtVastus
        '
        Me.txtVastus.Location = New System.Drawing.Point(63, 153)
        Me.txtVastus.Name = "txtVastus"
        Me.txtVastus.Size = New System.Drawing.Size(121, 20)
        Me.txtVastus.TabIndex = 2
        '
        'lblSisendYhik
        '
        Me.lblSisendYhik.AutoSize = True
        Me.lblSisendYhik.Location = New System.Drawing.Point(208, 65)
        Me.lblSisendYhik.Name = "lblSisendYhik"
        Me.lblSisendYhik.Size = New System.Drawing.Size(39, 13)
        Me.lblSisendYhik.TabIndex = 3
        Me.lblSisendYhik.Text = "Label1"
        '
        'lblVastusYhik
        '
        Me.lblVastusYhik.AutoSize = True
        Me.lblVastusYhik.Location = New System.Drawing.Point(211, 159)
        Me.lblVastusYhik.Name = "lblVastusYhik"
        Me.lblVastusYhik.Size = New System.Drawing.Size(39, 13)
        Me.lblVastusYhik.TabIndex = 4
        Me.lblVastusYhik.Text = "Label1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(355, 428)
        Me.Controls.Add(Me.lblVastusYhik)
        Me.Controls.Add(Me.lblSisendYhik)
        Me.Controls.Add(Me.txtVastus)
        Me.Controls.Add(Me.cmbTeisendus)
        Me.Controls.Add(Me.txtSisend)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtSisend As TextBox
    Friend WithEvents cmbTeisendus As ComboBox
    Friend WithEvents txtVastus As TextBox
    Friend WithEvents lblSisendYhik As Label
    Friend WithEvents lblVastusYhik As Label
End Class
