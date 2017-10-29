Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles sperren.Click
        For Each ctrl As Control In Me.Controls
            If ctrl.Text <> "Ändern" Then
                ctrl.Enabled = False
            End If
        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ändern.Click
        ' Passwortfeld anzeigen
        lblKennwort.Visible = True
        lblKennwort.Enabled = True
        txtKennwort.Visible = True
        txtKennwort.Enabled = True


        For Each ctrl As Control In Me.Controls
            ctrl.Enabled = True
        Next
    End Sub
End Class
