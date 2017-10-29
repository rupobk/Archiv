Imports System
Imports System.IO
Imports System.Text


Public Class Form1
    Public Uhrzeit As Date
    Public Kennwort As String
    Public BeimHerunterfahren As Boolean
    Public Datei = "C:\temp\shtdwn" + Now.ToString("yyyyMMdd") + ".txt"
    Public Ende As Boolean

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim args(10) As String
        Dim Minuten As Integer
        BeimHerunterfahren = False
        tmrBlinken.Interval = 500
        tmrShutdown.Interval = 2000     '20000
        Ende = False

        'Parameter Zeit u. Kennwort kann der Routine mitgegeben werden
        args = Environment.GetCommandLineArgs()
        'MessageBox.Show(args.Length)
        If args.Length >= 2 Then
            If args.Length = 3 Then
                Kennwort = args(2)
            Else
                Kennwort = "r.o,30132077"
            End If
            Try
                Minuten = CInt(args(1))
            Catch
            End Try
            If Err.Number = 0 Then
                If Minuten > 0 Then
                    'los gehts
                    txtNeueShutdownZeit.Visible = False
                    txtKennwort.Visible = False
                    btnOK.Visible = False
                    btnOK2.Visible = False
                    lblherunterfahren.Visible = False

                    Uhrzeit = Now
                    Uhrzeit = Uhrzeit.AddMinutes(Minuten)
                    lblShutdownZeit.Text = Uhrzeit.ToString("HH:mm")
                    lblAnzMin.Text = "(in " & CStr(DateDiff(DateInterval.Minute, Now, Uhrzeit)) & " Min.)"
                    tmrShutdown.Enabled = True
                    tmrShutdown.Start()
                Else
                    MessageBox.Show("nichts zu tun (Minuten=0)")
                    Application.Exit()
                    Application.ExitThread()
                End If
            Else
                MessageBox.Show("nichts zu tun (kein numerischer Parameter)")
                Application.Exit()
                Application.ExitThread()
            End If
        Else
            MessageBox.Show("Aufruf mit den Parametern 'Anzahl Minuten' u. eventuell 'Kennwort' (ansonsten Standardkennwort r.o,....)")
            Application.Exit()
            Application.ExitThread()
        End If

    End Sub

    Private Sub tmrShutdown_Tick(sender As Object, e As EventArgs) Handles tmrShutdown.Tick
        Dim x As String
        Dim y As Date

        If My.Computer.FileSystem.FileExists(Datei) Then
            x = DateiLesen()
            If x.Length > 0 Then
                y = Convert.ToDateTime(x)
                If DateDiff(DateInterval.Minute, Now, y) <= 0 Then
                    Herunterfahren()
                Else
                    Uhrzeit = y
                    lblShutdownZeit.Text = Uhrzeit.ToString("HH:mm")
                End If
            End If
        End If

        If GetCurrentTime() >= lblShutdownZeit.Text Then
            Herunterfahren()
        Else
            lblAnzMin.Text = "(in " & CStr(DateDiff(DateInterval.Minute, Now, Uhrzeit)) & " Min.)"
            'Daten in Datei wegschreiben, damit wenn zwischendurch gebootet wird, nicht wieder der Countdown bei der vollen Zeit beginnt
            InDateiSchreiben()
        End If

    End Sub

    Sub DateiLoeschen()
        Try
            My.Computer.FileSystem.DeleteFile(Datei)
        Catch
        End Try
    End Sub

    Sub InDateiSchreiben()
        Dim fstream As FileStream

        'Nur beim ersten Mal schreiben bzw. wenn sich Uhrzeit geändert hat
        If Not File.Exists(Datei) Then
            fstream = New FileStream(Datei, FileMode.Create, FileAccess.ReadWrite)
            Dim sWriter As New StreamWriter(fstream)
            sWriter.BaseStream.Seek(0, SeekOrigin.Begin)
            sWriter.Write(Uhrzeit.ToString())
            sWriter.Close()
        End If
    End Sub

    Function DateiLesen() As String
        Dim fileReader As String

        If My.Computer.FileSystem.FileExists(Datei) Then
            fileReader = My.Computer.FileSystem.ReadAllText(Datei)
            If fileReader <> "" Then
                Return fileReader
            End If
            Return ""
        Else
            Return ""
        End If
    End Function


    Sub Herunterfahren()
        lblherunterfahren.Visible = True
        tmrBlinken.Enabled = True
        tmrBlinken.Start()
        tmrShutdown.Enabled = False
        tmrShutdown.Stop()

        'Jetzt effektiv herunterfahren
        'Shell("Shutdown -s -t 0")
    End Sub

    Function GetCurrentTime()
        Dim Zeit As String = Now.ToString("HH:mm")
        Return Zeit
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnVerlängern.Click
        txtKennwort.Text = ""
        txtKennwort.Visible = True
        txtKennwort.Focus()
        btnOK.Visible = True
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If txtKennwort.Text = Kennwort Then
            txtNeueShutdownZeit.Text = lblShutdownZeit.Text
            txtNeueShutdownZeit.Visible = True
            txtNeueShutdownZeit.Focus()
            btnOK2.Visible = True
        End If
        txtKennwort.Visible = False
        btnOK.Visible = False
    End Sub

    Private Sub btnOK2_Click(sender As Object, e As EventArgs) Handles btnOK2.Click
        If Not txtNeueShutdownZeit.Text.Contains(":") Then
            txtNeueShutdownZeit.Text = txtNeueShutdownZeit.Text.Substring(0, 2) + ":" + txtNeueShutdownZeit.Text.Substring(2, 2)
        End If
        lblShutdownZeit.Text = txtNeueShutdownZeit.Text
        txtNeueShutdownZeit.Visible = False
        btnOK2.Visible = False
        Uhrzeit = Convert.ToDateTime(txtNeueShutdownZeit.Text)
        DateiLoeschen()
    End Sub

    Private Sub tmrBlinken_Tick(sender As Object, e As EventArgs) Handles tmrBlinken.Tick
        lblherunterfahren.Visible = Not lblherunterfahren.Visible
    End Sub

    Private Sub txtKennwort_KeyDown(sender As Object, e As KeyEventArgs) Handles txtKennwort.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtKennwort.Text = Kennwort Then
                txtNeueShutdownZeit.Text = lblShutdownZeit.Text
                txtNeueShutdownZeit.Visible = True
                txtNeueShutdownZeit.Focus()
                btnOK2.Visible = True
            End If
            txtKennwort.Visible = False
            btnOK.Visible = False
        End If
    End Sub

    Private Sub txtNeueShutdownZeit_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNeueShutdownZeit.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Not txtNeueShutdownZeit.Text.Contains(":") Then
                txtNeueShutdownZeit.Text = txtNeueShutdownZeit.Text.Substring(0, 2) + ":" + txtNeueShutdownZeit.Text.Substring(2, 2)
            End If
            lblShutdownZeit.Text = txtNeueShutdownZeit.Text
            txtNeueShutdownZeit.Visible = False
            btnOK2.Visible = False
            Uhrzeit = Convert.ToDateTime(txtNeueShutdownZeit.Text)
            DateiLoeschen()
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not Ende Then
            Dim a = MessageBox.Show("Bist du sicher? Mit 'Ja' schaltet sich der Computer nämlich aus!!", "Ausschaltdialog", MessageBoxButtons.YesNo)
            If a = DialogResult.Yes Then
                Herunterfahren()
            Else
                e.Cancel = True
            End If
        End If
    End Sub
End Class
