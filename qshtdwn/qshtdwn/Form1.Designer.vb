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
        Me.components = New System.ComponentModel.Container()
        Me.Nachricht = New System.Windows.Forms.Label()
        Me.txtNeueShutdownZeit = New System.Windows.Forms.TextBox()
        Me.tmrShutdown = New System.Windows.Forms.Timer(Me.components)
        Me.lblShutdownZeit = New System.Windows.Forms.Label()
        Me.btnVerlängern = New System.Windows.Forms.Button()
        Me.lblAnzMin = New System.Windows.Forms.Label()
        Me.txtKennwort = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnOK2 = New System.Windows.Forms.Button()
        Me.lblherunterfahren = New System.Windows.Forms.Label()
        Me.tmrBlinken = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Nachricht
        '
        Me.Nachricht.AutoSize = True
        Me.Nachricht.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nachricht.Location = New System.Drawing.Point(24, 22)
        Me.Nachricht.Name = "Nachricht"
        Me.Nachricht.Size = New System.Drawing.Size(211, 22)
        Me.Nachricht.TabIndex = 0
        Me.Nachricht.Text = "PC fährt herunter um:"
        '
        'txtNeueShutdownZeit
        '
        Me.txtNeueShutdownZeit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNeueShutdownZeit.Location = New System.Drawing.Point(172, 56)
        Me.txtNeueShutdownZeit.MaxLength = 5
        Me.txtNeueShutdownZeit.Name = "txtNeueShutdownZeit"
        Me.txtNeueShutdownZeit.Size = New System.Drawing.Size(54, 26)
        Me.txtNeueShutdownZeit.TabIndex = 1
        Me.txtNeueShutdownZeit.Text = "XX:XX"
        '
        'tmrShutdown
        '
        '
        'lblShutdownZeit
        '
        Me.lblShutdownZeit.AutoSize = True
        Me.lblShutdownZeit.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShutdownZeit.Location = New System.Drawing.Point(231, 13)
        Me.lblShutdownZeit.Name = "lblShutdownZeit"
        Me.lblShutdownZeit.Size = New System.Drawing.Size(99, 31)
        Me.lblShutdownZeit.TabIndex = 2
        Me.lblShutdownZeit.Text = "XX:XX"
        '
        'btnVerlängern
        '
        Me.btnVerlängern.Location = New System.Drawing.Point(409, 120)
        Me.btnVerlängern.Name = "btnVerlängern"
        Me.btnVerlängern.Size = New System.Drawing.Size(75, 23)
        Me.btnVerlängern.TabIndex = 3
        Me.btnVerlängern.Text = "Verlängern"
        Me.btnVerlängern.UseVisualStyleBackColor = True
        '
        'lblAnzMin
        '
        Me.lblAnzMin.AutoSize = True
        Me.lblAnzMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAnzMin.Location = New System.Drawing.Point(336, 24)
        Me.lblAnzMin.Name = "lblAnzMin"
        Me.lblAnzMin.Size = New System.Drawing.Size(18, 20)
        Me.lblAnzMin.TabIndex = 4
        Me.lblAnzMin.Text = "0"
        '
        'txtKennwort
        '
        Me.txtKennwort.Location = New System.Drawing.Point(28, 61)
        Me.txtKennwort.Name = "txtKennwort"
        Me.txtKennwort.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtKennwort.Size = New System.Drawing.Size(87, 20)
        Me.txtKennwort.TabIndex = 5
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(121, 58)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(30, 23)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnOK2
        '
        Me.btnOK2.Location = New System.Drawing.Point(232, 58)
        Me.btnOK2.Name = "btnOK2"
        Me.btnOK2.Size = New System.Drawing.Size(30, 23)
        Me.btnOK2.TabIndex = 7
        Me.btnOK2.Text = "OK"
        Me.btnOK2.UseVisualStyleBackColor = True
        '
        'lblherunterfahren
        '
        Me.lblherunterfahren.AutoSize = True
        Me.lblherunterfahren.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblherunterfahren.Location = New System.Drawing.Point(23, 121)
        Me.lblherunterfahren.Name = "lblherunterfahren"
        Me.lblherunterfahren.Size = New System.Drawing.Size(347, 25)
        Me.lblherunterfahren.TabIndex = 8
        Me.lblherunterfahren.Text = "Computer fährt gleich herunter!!"
        '
        'tmrBlinken
        '
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(496, 155)
        Me.Controls.Add(Me.lblherunterfahren)
        Me.Controls.Add(Me.btnOK2)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtKennwort)
        Me.Controls.Add(Me.lblAnzMin)
        Me.Controls.Add(Me.btnVerlängern)
        Me.Controls.Add(Me.lblShutdownZeit)
        Me.Controls.Add(Me.txtNeueShutdownZeit)
        Me.Controls.Add(Me.Nachricht)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Zeitplan"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Nachricht As System.Windows.Forms.Label
    Friend WithEvents txtNeueShutdownZeit As System.Windows.Forms.TextBox
    Friend WithEvents tmrShutdown As System.Windows.Forms.Timer
    Friend WithEvents lblShutdownZeit As System.Windows.Forms.Label
    Friend WithEvents btnVerlängern As System.Windows.Forms.Button
    Friend WithEvents lblAnzMin As System.Windows.Forms.Label
    Friend WithEvents txtKennwort As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnOK2 As System.Windows.Forms.Button
    Friend WithEvents lblherunterfahren As System.Windows.Forms.Label
    Friend WithEvents tmrBlinken As System.Windows.Forms.Timer

End Class
