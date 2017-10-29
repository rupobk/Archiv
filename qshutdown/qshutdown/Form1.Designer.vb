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
        Me.sperren = New System.Windows.Forms.Button()
        Me.ändern = New System.Windows.Forms.Button()
        Me.lblKennwort = New System.Windows.Forms.Label()
        Me.txtKennwort = New System.Windows.Forms.TextBox()
        Me.btnKWok = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'sperren
        '
        Me.sperren.Location = New System.Drawing.Point(492, 244)
        Me.sperren.Name = "sperren"
        Me.sperren.Size = New System.Drawing.Size(75, 23)
        Me.sperren.TabIndex = 0
        Me.sperren.Text = "Sperren"
        Me.sperren.UseVisualStyleBackColor = True
        '
        'ändern
        '
        Me.ändern.Location = New System.Drawing.Point(411, 244)
        Me.ändern.Name = "ändern"
        Me.ändern.Size = New System.Drawing.Size(75, 23)
        Me.ändern.TabIndex = 1
        Me.ändern.Text = "Ändern"
        Me.ändern.UseVisualStyleBackColor = True
        '
        'lblKennwort
        '
        Me.lblKennwort.AutoSize = True
        Me.lblKennwort.Location = New System.Drawing.Point(16, 22)
        Me.lblKennwort.Name = "lblKennwort"
        Me.lblKennwort.Size = New System.Drawing.Size(55, 13)
        Me.lblKennwort.TabIndex = 2
        Me.lblKennwort.Text = "Kennwort:"
        Me.lblKennwort.Visible = False
        '
        'txtKennwort
        '
        Me.txtKennwort.Location = New System.Drawing.Point(77, 19)
        Me.txtKennwort.Name = "txtKennwort"
        Me.txtKennwort.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtKennwort.Size = New System.Drawing.Size(100, 20)
        Me.txtKennwort.TabIndex = 3
        Me.txtKennwort.Visible = False
        '
        'btnKWok
        '
        Me.btnKWok.Location = New System.Drawing.Point(183, 19)
        Me.btnKWok.Name = "btnKWok"
        Me.btnKWok.Size = New System.Drawing.Size(27, 23)
        Me.btnKWok.TabIndex = 4
        Me.btnKWok.Text = "ok"
        Me.btnKWok.UseVisualStyleBackColor = True
        Me.btnKWok.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnKWok)
        Me.GroupBox1.Controls.Add(Me.lblKennwort)
        Me.GroupBox1.Controls.Add(Me.txtKennwort)
        Me.GroupBox1.Location = New System.Drawing.Point(160, 227)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(230, 56)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(615, 295)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ändern)
        Me.Controls.Add(Me.sperren)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents sperren As System.Windows.Forms.Button
    Friend WithEvents ändern As System.Windows.Forms.Button
    Friend WithEvents lblKennwort As System.Windows.Forms.Label
    Friend WithEvents txtKennwort As System.Windows.Forms.TextBox
    Friend WithEvents btnKWok As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox

End Class
