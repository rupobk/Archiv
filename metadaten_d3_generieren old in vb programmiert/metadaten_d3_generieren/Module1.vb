Imports System.IO
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Net.Mail

Module Module1

    Public cond3 As New SqlConnection("Server=baan4\d3;Database=d3p;User Id=sa; Password=Baan123;")
    Public conBaan4db As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
    Public file As System.IO.StreamWriter
    Public dateipfad As String
    Public args(10) As String

    Sub Main()

        LogSchreiben("Programm metadaten_d3_generieren gestartet.")
        Console.WriteLine("Programm 'metadaten_d3_generieren gestartet'!")

        ProgrammParameterLesen()

        Select Case args(1).ToUpper
            Case "BUCHUNGSBELEGE"
                BuchungsBelege_Metadaten()
            Case "AUSGANGSRECHN_PER_EMAIL"
                Ausgangsrechn_Per_Email_Metadaten()
            Case "AUSGANGSRECHN_SONSTIGE"
                Ausgangsrechn_Sonstige()
            Case "EINGANGSRECHN_BARCODE_STUENDLICH"
                Eingangsrechn_Barcode("stuendlich")
            Case "EINGANGSRECHN_BARCODE_TAEGLICH"
                Eingangsrechn_Barcode("taeglich")
            Case "KUNDENALLE"
                Kunden_Metadaten("all")
            Case "KUNDENAENDERUNGEN"
                Kunden_Metadaten("change")
            Case "LIEFERANTENALLE"
                Lieferanten_Metadaten("all")

                'Case ...
                'Case else ...
        End Select


        Console.WriteLine("Programm ohne Fehler beendet!")
        LogSchreiben("Programm 'metadaten_d3_generieren' ohne Fehler beendet!")
        Threading.Thread.Sleep(30000)
    End Sub

    Sub ProgrammParameterLesen()
        'args(0) = Programmname
        'args(1) = Prozedurname
        'args(2) = Pfad (wenn notwendig)
        Try
            Dim x, z As Integer
            z = Environment.GetCommandLineArgs.Count - 1
            If z > 0 Then
                For x = 1 To z
                    args(x) = Environment.GetCommandLineArgs(x)
                Next
                If x = 2 Then
                    dateipfad = args(2)
                End If
            Else
                Console.WriteLine("")
                Console.WriteLine("")
                Console.WriteLine("Programm metadaten_d3_generieren wird mit 2 Argumenten aufgerufen:")
                Console.WriteLine("Name Prozedur: welche Metadaten sollen generiert werden (buchungsbelege, ausgangsrechn_per_email, ausgangsrechn_sonstige, eingangsrechn_barcode_stuendlich, eingangsrechn_barcode_taeglich, kundenalle, kundenaenderungen, lieferantenalle ...)")
                Console.WriteLine("Pfad: Pfad der Textdatei. Beispiel: c:\temp\")
                Console.WriteLine("Zweck des Programmes: Alle möglichen Metadaten-Dateien für d3-Archivierung von Dokumenten generieren.")
                Console.WriteLine("")
                Console.WriteLine("")
                Console.WriteLine("Programmm beendet.")
                LogSchreiben("Sub ProgrammParameterLesen: Programm ohne Parameter aufgerufen!")
                LogSchreiben("Programm metadaten_d3_generieren mit Warnung beendet.")
                Threading.Thread.Sleep(30000)
                Environment.Exit(-1)
            End If
        Catch ex As Exception
            LogSchreiben("Fehler in Sub ProgrammParameterLesen!")
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=ProgrammParameterLesen, Fehler=""" + ex.Message)
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try

    End Sub



    Sub Kunden_Metadaten(Typ As String)
        Console.WriteLine("Metadaten Kundenakte werden generiert ...")
        Try
            'im ersten Schritt für alle neu zu übertragenden Kunden .jpl-Dateien generieren, damit diese in d3 angelegt werden
            Using conBaan4db
                'Suche neue Kunden
                Dim command As SqlCommand = New SqlCommand("SELECT t_cuno, LTRIM(t_nama) AS t_nama, LTRIM(CASE when t_fovn='' THEN t_crbu ELSE t_fovn END) AS t_fovn, " +
                                                                "LTrim(t_ccty) As t_ccty, LTrim(t_name) As t_name, " +
                                "LTrim(CASE LTRIM(t_clan) WHEN 'I' THEN 'IT' WHEN 'D' THEN 'DE' WHEN 'EN' THEN 'GB' ELSE LTRIM(t_clan) END) AS t_clan, " +
                                "CASE when dbo.SearchText(t_txta, 'Email Rechnung:')='' THEN 'info@atzwanger.net' ELSE dbo.SearchText(t_txta, 'Email Rechnung:') END AS email, " +
                                "CASE when dbo.SearchText(t_txta, 'Email Kanal:')='' THEN 'NOPEC' ELSE dbo.SearchText(t_txta, 'Email Kanal:') END AS typ, LTRIM(t_telp) AS t_telp " +
                                "FROM ttccom010100 WHERE NOT([t_cuno] IN (SELECT dok_dat_feld_21 COLLATE database_default FROM [BAAN4\D3].d3p.dbo.firmen_spezifisch " +
                                "WHERE kue_dokuart='AKUND'))", conBaan4db)
                conBaan4db.Open()
                command.Connection = conBaan4db
                Dim reader As SqlDataReader = command.ExecuteReader()
                If reader.HasRows Then
                    Do While reader.Read()
                        dateipfad = "\\appsrv01\c$\d3\import\D3P\Hostimp\Stammdaten\KUND" + reader.GetString(0) + ".jpl"
                        If (My.Computer.FileSystem.FileExists(dateipfad)) Then
                            My.Computer.FileSystem.DeleteFile(dateipfad)
                        End If
                        file = My.Computer.FileSystem.OpenTextFileWriter(dateipfad, True)
                        file.WriteLine("dok_dat_feld[21] = """ + ZeichenPruefung(reader.GetString(0)) + """")
                        file.WriteLine("dok_dat_feld[22] = """ + ZeichenPruefung(reader.GetString(1)) + """")
                        file.WriteLine("dok_dat_feld[3] = """ + ZeichenPruefung(reader.GetString(2)) + """")
                        file.WriteLine("dok_dat_feld[9] = """ + ZeichenPruefung(reader.GetString(3)) + """")
                        file.WriteLine("dok_dat_feld[47] = """ + ZeichenPruefung(reader.GetString(4)) + """")
                        file.WriteLine("dok_dat_feld[46] = """ + ZeichenPruefung(reader.GetString(5)) + """")
                        file.WriteLine("dok_dat_feld[12] = """ + ZeichenPruefung(reader.GetString(6).Replace(";", ",") + ", accounting@atzwanger.net") + """")
                        file.WriteLine("dok_dat_feld[49] = """ + ZeichenPruefung(reader.GetString(7)) + """")
                        file.WriteLine("dok_dat_feld[48] = """ + ZeichenPruefung(reader.GetString(8)) + """")
                        file.WriteLine("as400_erlaube_ueberspielen = """ + "1" + """")
                        file.WriteLine("dokuart = """ + "AKUND" + """")
                        file.WriteLine("bearbeiter = """ + "admin01" + """")
                        file.WriteLine("logi_verzeichnis = """ + "Be" + """")
                        file.WriteLine("zeich_nr = """ + "AKUND" + ZeichenPruefung(reader.GetString(0)) + """")
                        file.Close()
                    Loop
                End If
                reader.Close()
                conBaan4db.Close()

                If Typ = "all" Then
                    'im zweiten Schritt die Attribute aller bestehenden Kunden mit jenen von Baan überschreiben, und zwar mittels UPD-Dateien
                    Dim command2 As SqlCommand = New SqlCommand("SELECT distinct t_cuno, LTRIM(t_nama) AS t_nama, LTRIM(CASE when t_fovn='' THEN t_crbu ELSE t_fovn END) AS t_fovn, " +
                                                                "LTrim(t_ccty) As t_ccty, LTrim(t_name) As t_name, " +
                                "LTrim(CASE LTRIM(t_clan) WHEN 'I' THEN 'IT' WHEN 'D' THEN 'DE' WHEN 'EN' THEN 'GB' ELSE LTRIM(t_clan) END) AS t_clan, " +
                                "CASE when dbo.SearchText(t_txta, 'Email Rechnung:')='' THEN 'info@atzwanger.net' ELSE dbo.SearchText(t_txta, 'Email Rechnung:') END AS email, " +
                                "CASE when dbo.SearchText(t_txta, 'Email Kanal:')='' THEN 'NOPEC' ELSE dbo.SearchText(t_txta, 'Email Kanal:') END AS typ, LTRIM(t_telp) AS t_telp " +
                                "FROM ttccom010100 a LEFT JOIN [BAAN4\D3].d3p.dbo.firmen_spezifisch b " +
                                "ON ltrim(a.t_cuno) COLLATE DATABASE_DEFAULT = b.dok_dat_feld_21 COLLATE DATABASE_DEFAULT where b.kue_dokuart='AKUND'", conBaan4db)
                    conBaan4db.Open()
                    command2.Connection = conBaan4db
                    Dim reader2 As SqlDataReader = command2.ExecuteReader()
                    If reader2.HasRows Then
                        Do While reader2.Read()
                            dateipfad = "\\appsrv01\c$\d3\import\D3P\Async\Stammdaten\KUND" + reader2.GetString(0) + ".upd"
                            If (My.Computer.FileSystem.FileExists(dateipfad)) Then
                                My.Computer.FileSystem.DeleteFile(dateipfad)
                            End If
                            file = My.Computer.FileSystem.OpenTextFileWriter(dateipfad, True)
                            file.WriteLine("o_dokuart = """ + "AKUND" + """")
                            file.WriteLine("o_dok_dat_feld_21 = """ + ZeichenPruefung(reader2.GetString(0)) + """")
                            file.WriteLine("n_dok_dat_feld_22 = """ + ZeichenPruefung(reader2.GetString(1)) + """")
                            file.WriteLine("n_dok_dat_feld_3 = """ + ZeichenPruefung(reader2.GetString(2)) + """")
                            file.WriteLine("n_dok_dat_feld_9 = """ + ZeichenPruefung(reader2.GetString(3)) + """")
                            file.WriteLine("n_dok_dat_feld_47 = """ + ZeichenPruefung(reader2.GetString(4)) + """")
                            file.WriteLine("n_dok_dat_feld_46 = """ + ZeichenPruefung(reader2.GetString(5)) + """")
                            file.WriteLine("n_dok_dat_feld_12 = """ + ZeichenPruefung(reader2.GetString(6).Replace(";", ",") + ", accounting@atzwanger.net") + """")
                            file.WriteLine("n_dok_dat_feld_49 = """ + ZeichenPruefung(reader2.GetString(7)) + """")
                            file.WriteLine("n_dok_dat_feld_48 = """ + ZeichenPruefung(reader2.GetString(8)) + """")
                            file.Close()
                        Loop
                    End If
                    reader2.Close()
                    conBaan4db.Close()

                    'hier die schlanke Version über einen SQL-Update
                    'funktioniert leider nicht, denn man müsste nachher den JSTORE-Task beenden u. ihm seine Dump-Datei löschen, damit er beim Neustart den Cache für die Verschlagwortung neu aufbaut,
                    'was mir aber nicht gelungen ist. Siehe im Onenote unter d3-Administration, Problemlösungen
                    'Dim command2 As SqlCommand = New SqlCommand("UPDATE [BAAN4\D3].d3p.dbo.firmen_spezifisch " +
                    '                    "SET dok_dat_feld_22 = LTRIM(b.t_nama), " +
                    '                    "dok_dat_feld_3 = LTRIM(CASE when b.t_fovn='' THEN b.t_crbu ELSE b.t_fovn END), " +
                    '                    "dok_dat_feld_9 = LTRIM(t_ccty), " +
                    '                    "dok_dat_feld_47 = LTRIM(t_name), " +
                    '                    "dok_dat_feld_46 = LTrim(Case LTRIM(t_clan) When 'I' THEN 'IT' WHEN 'D' THEN 'DE' WHEN 'EN' THEN 'GB' ELSE LTRIM(t_clan) END), " +
                    '                    "dok_dat_feld_12 = Case When dbo.SearchText(t_txta, 'Email Rechnung:')='' THEN 'info@atzwanger.net' ELSE dbo.SearchText(t_txta, 'Email Rechnung:') END, " +
                    '                    "dok_dat_feld_49 = Case When dbo.SearchText(t_txta, 'Email Kanal:')='' THEN 'PEC' ELSE 'NOPEC' END, " +
                    '                    "dok_dat_feld_48 = LTrim(t_telp) " +
                    '                    "FROM [BAAN4\D3].d3p.dbo.firmen_spezifisch a LEFT JOIN ttccom010100 b " +
                    '                    "On ltrim(b.t_cuno) COLLATE DATABASE_DEFAULT = a.dok_dat_feld_21 COLLATE DATABASE_DEFAULT " +
                    '                    "WHERE a.kue_dokuart='AKUND' ", conBaan4db)
                    'conBaan4db.Open()
                    'command2.Connection = conBaan4db
                    'command2.ExecuteNonQuery()
                    'jetzt nicht vergessen den jstore-Prozess zu beenden, damit der Cache auf dem Server neu aufgebaut wird (der Job startet sich dann von alleine neu)
                    'Dim p As New Process()
                    'p.StartInfo = New ProcessStartInfo("cmd.exe", "/c taskkill /IM jstore.exe /F")
                    'p.StartInfo.CreateNoWindow = True
                    'p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    'p.Start()
                Else
                    'Überschreibe bei allen bestehenden Kunden, wo sich die Email-Adresse geändert hat, die Attribute mit den Baan-Daten, und zwar mittels UPD-Dateien
                    Dim command2 As SqlCommand = New SqlCommand("SELECT distinct t_cuno, LTRIM(t_nama) AS t_nama, LTRIM(CASE when t_fovn='' THEN t_crbu ELSE t_fovn END) AS t_fovn, " +
                                                                    "LTrim(t_ccty) As t_ccty, LTrim(t_name) As t_name, " +
                                    "LTrim(CASE LTRIM(t_clan) WHEN 'I' THEN 'IT' WHEN 'D' THEN 'DE' WHEN 'EN' THEN 'GB' ELSE LTRIM(t_clan) END) AS t_clan, " +
                                    "CASE when dbo.SearchText(t_txta, 'Email Rechnung:')='' THEN 'info@atzwanger.net' ELSE dbo.SearchText(t_txta, 'Email Rechnung:') END AS email, " +
                                    "CASE when dbo.SearchText(t_txta, 'Email Kanal:')='' THEN 'NOPEC' ELSE dbo.SearchText(t_txta, 'Email Kanal:') END AS typ, LTRIM(t_telp) AS t_telp " +
                                    "FROM ttccom010100 a LEFT JOIN [BAAN4\D3].d3p.dbo.firmen_spezifisch b " +
                                    "ON ltrim(a.t_cuno) COLLATE DATABASE_DEFAULT = b.dok_dat_feld_21 COLLATE DATABASE_DEFAULT " +
                                    "WHERE b.kue_dokuart='AKUND' and dbo.SearchText(t_txta, 'Email Rechnung:')<>'' " +
                                    "And dbo.SearchText(t_txta, 'Email Rechnung:')<>b.dok_dat_feld_12", conBaan4db)
                    conBaan4db.Open()
                    command2.Connection = conBaan4db
                    Dim reader2 As SqlDataReader = command2.ExecuteReader()
                    If reader2.HasRows Then
                        Do While reader2.Read()
                            dateipfad = "\\appsrv01\c$\d3\import\D3P\Async\Stammdaten\KUND" + reader2.GetString(0) + ".upd"
                            If (My.Computer.FileSystem.FileExists(dateipfad)) Then
                                My.Computer.FileSystem.DeleteFile(dateipfad)
                            End If
                            file = My.Computer.FileSystem.OpenTextFileWriter(dateipfad, True)
                            file.WriteLine("o_dokuart = """ + "AKUND" + """")
                            file.WriteLine("o_dok_dat_feld_21 = """ + ZeichenPruefung(reader2.GetString(0)) + """")
                            file.WriteLine("n_dok_dat_feld_22 = """ + ZeichenPruefung(reader2.GetString(1)) + """")
                            file.WriteLine("n_dok_dat_feld_3 = """ + ZeichenPruefung(reader2.GetString(2)) + """")
                            file.WriteLine("n_dok_dat_feld_9 = """ + ZeichenPruefung(reader2.GetString(3)) + """")
                            file.WriteLine("n_dok_dat_feld_47 = """ + ZeichenPruefung(reader2.GetString(4)) + """")
                            file.WriteLine("n_dok_dat_feld_46 = """ + ZeichenPruefung(reader2.GetString(5)) + """")
                            file.WriteLine("n_dok_dat_feld_12 = """ + ZeichenPruefung(reader2.GetString(6).Replace(";", ",") + ", accounting@atzwanger.net") + """")
                            file.WriteLine("n_dok_dat_feld_49 = """ + ZeichenPruefung(reader2.GetString(7)) + """")
                            file.WriteLine("n_dok_dat_feld_48 = """ + ZeichenPruefung(reader2.GetString(8)) + """")
                            file.Close()
                        Loop
                    End If
                    reader2.Close()
                    conBaan4db.Close()
                End If
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Sub Kunden_Metadaten!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=Kunden_Metadaten, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
        Console.WriteLine("Metadaten Kundenakte fertig generiert!")
    End Sub

    Sub Lieferanten_Metadaten(Typ As String)
        Console.WriteLine("Metadaten Lieferantenakte werden generiert ...")

        Try
            If Typ = "all" Then
                'im ersten Schritt für alle neu zu übertragenden Kunden .jpl-Dateien generieren, damit diese in d3 angelegt werden
                Using conBaan4db
                    'Suche neue Kunden
                    Dim command As SqlCommand = New SqlCommand("SELECT distinct LTRIM(t_suno), LTRIM(t_nama), LTRIM(CASE when t_fovn='' THEN t_refa ELSE t_fovn END), LTRIM(t_ccty), LTRIM(t_name), " +
                        "ltrim(CASE LTRIM(t_clan) WHEN 'I' THEN 'IT' WHEN 'D' THEN 'DE' WHEN 'EN' THEN 'GB' WHEN 'F' THEN 'FR' ELSE LTRIM(t_clan) END), t_namd, LTRIM(t_telp) " +
                        "From ttccom020100 WHERE NOT([t_suno] IN (SELECT dok_dat_feld_1 COLLATE database_default FROM [BAAN4\D3].d3p.dbo.firmen_spezifisch " +
                        "WHERE kue_dokuart='ALIEF'))", conBaan4db)
                    conBaan4db.Open()
                    command.Connection = conBaan4db
                    Dim reader As SqlDataReader = command.ExecuteReader()
                    If reader.HasRows Then
                        Do While reader.Read()
                            dateipfad = "\\appsrv01\c$\d3\import\D3P\Hostimp\Stammdaten\LIEF" + reader.GetString(0) + ".jpl"
                            If (My.Computer.FileSystem.FileExists(dateipfad)) Then
                                My.Computer.FileSystem.DeleteFile(dateipfad)
                            End If
                            file = My.Computer.FileSystem.OpenTextFileWriter(dateipfad, True)
                            file.WriteLine("dok_dat_feld[1] = """ + ZeichenPruefung(reader.GetString(0)) + """")
                            file.WriteLine("dok_dat_feld[2] = """ + ZeichenPruefung(reader.GetString(1)) + """")
                            file.WriteLine("dok_dat_feld[3] = """ + ZeichenPruefung(reader.GetString(2)) + """")
                            file.WriteLine("dok_dat_feld[9] = """ + ZeichenPruefung(reader.GetString(3)) + """")
                            file.WriteLine("dok_dat_feld[47] = """ + ZeichenPruefung(reader.GetString(4)) + """")
                            file.WriteLine("dok_dat_feld[46] = """ + ZeichenPruefung(reader.GetString(5)) + """")
                            file.WriteLine("dok_dat_feld[11] = """ + ZeichenPruefung(reader.GetString(6)) + """")
                            file.WriteLine("dok_dat_feld[48] = """ + ZeichenPruefung(reader.GetString(7)) + """")
                            file.WriteLine("as400_erlaube_ueberspielen = """ + "1" + """")
                            file.WriteLine("dokuart = """ + "ALIEF" + """")
                            file.WriteLine("bearbeiter = """ + "admin01" + """")
                            file.WriteLine("logi_verzeichnis = """ + "Be" + """")
                            file.WriteLine("zeich_nr = """ + "ALIEF" + ZeichenPruefung(reader.GetString(0)) + """")
                            file.Close()
                        Loop
                    End If
                    reader.Close()
                    conBaan4db.Close()

                    'im zweiten Schritt die Attribute der bestehenden Kunden mit jenen von Baan überschreiben, und zwar mittels UPD-Dateien
                    Dim command2 As SqlCommand = New SqlCommand("SELECT distinct LTRIM(t_suno), LTRIM(t_nama), LTRIM(CASE when t_fovn='' THEN t_refa ELSE t_fovn END), LTRIM(t_ccty), " +
                        "LTRIM(t_name), ltrim(CASE LTRIM(t_clan) WHEN 'I' THEN 'IT' WHEN 'D' THEN 'DE' WHEN 'EN' THEN 'GB' WHEN 'F' THEN 'FR' ELSE LTRIM(t_clan) END), " +
                        "t_namd, LTrim(t_telp) FROM ttccom020100 a LEFT JOIN [BAAN4\D3].d3p.dbo.firmen_spezifisch b " +
                        "ON a.t_suno COLLATE DATABASE_DEFAULT = b.dok_dat_feld_1 COLLATE DATABASE_DEFAULT WHERE b.kue_dokuart='ALIEF'", conBaan4db)
                    conBaan4db.Open()
                    command2.Connection = conBaan4db
                    Dim reader2 As SqlDataReader = command2.ExecuteReader()
                    If reader2.HasRows Then
                        Do While reader2.Read()
                            dateipfad = "\\appsrv01\c$\d3\import\D3P\Async\Stammdaten\LIEF" + reader2.GetString(0) + ".upd"
                            If (My.Computer.FileSystem.FileExists(dateipfad)) Then
                                My.Computer.FileSystem.DeleteFile(dateipfad)
                            End If
                            file = My.Computer.FileSystem.OpenTextFileWriter(dateipfad, True)
                            file.WriteLine("o_dokuart = """ + "ALIEF" + """")
                            file.WriteLine("o_dok_dat_feld_1 = """ + ZeichenPruefung(reader2.GetString(0)) + """")
                            file.WriteLine("n_dok_dat_feld_2 = """ + ZeichenPruefung(reader2.GetString(1)) + """")
                            file.WriteLine("n_dok_dat_feld_3 = """ + ZeichenPruefung(reader2.GetString(2)) + """")
                            file.WriteLine("n_dok_dat_feld_9 = """ + ZeichenPruefung(reader2.GetString(3)) + """")
                            file.WriteLine("n_dok_dat_feld_47 = """ + ZeichenPruefung(reader2.GetString(4)) + """")
                            file.WriteLine("n_dok_dat_feld_46 = """ + ZeichenPruefung(reader2.GetString(5)) + """")
                            file.WriteLine("n_dok_dat_feld_11 = """ + ZeichenPruefung(reader2.GetString(6)) + """")
                            file.WriteLine("n_dok_dat_feld_48 = """ + ZeichenPruefung(reader2.GetString(7)) + """")
                            file.Close()
                        Loop
                    End If
                    reader2.Close()
                    conBaan4db.Close()
                End Using
            End If
        Catch ex As Exception
            LogSchreiben("Fehler In Sub Lieferanten_Metadaten!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=Lieferanten_Metadaten, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
        Console.WriteLine("Metadaten Lieferantenakte fertig generiert!")
    End Sub



    Sub Ausgangsrechn_Per_Email_Metadaten()
        'Diese Routine verschlagwortet alle Rechnungen, die per E-Mail an Kunden versendet werden
        Console.WriteLine("Metadaten Ausgangsrechnungen per E-Mail versendet werden generiert ...")
        Try
            Using cond3
                'Suche alle Rechnungen, die in d3 archiviert wurden, aber noch keine Verschlagwortung haben bzw. jene, die noch keine vollständige Verschlagwortung 
                'haben (weil z. B. die Rechnungen noch nicht in die Fibu übergeleitet waren)
                Dim command As SqlCommand = New SqlCommand("select * from v_AR_Email_ohne_Verschlagwortung", cond3)
                cond3.Open()
                command.Connection = cond3
                Dim reader As SqlDataReader = command.ExecuteReader()
                If reader.HasRows Then
                    Dim sql2 As String = "INSERT INTO tmpRechDetail Select * from v_metadaten_ar_email_fehlende_d3"
                    'aus Geschwindigkeitsgründen wird eine temporäre Tabelle generiert u. die Detaildaten dort hineingeschrieben
                    RechnRestDatenTempTableGen("anlegen", sql2)
                    Do While reader.Read()
                        UPDDateiRechnEmailGenerieren(reader.GetString(0), reader.GetString(1), "rechnung")
                    Loop
                    RechnRestDatenTempTableGen("löschen", "")
                End If
                reader.Close()

                'Jetzt die Dateianhänge
                command = New SqlCommand("select dok_dat_feld_10, doku_id from [BAAN4\D3].d3p.dbo.firmen_spezifisch where kue_dokuart='DARAN' " +
                                                           "And (dok_dat_feld_21 Is null And dok_dat_feld_44 Is null And dok_dat_feld_10 Is Not null And dok_dat_feld_36='JA')", cond3)
                command.Connection = cond3
                reader = command.ExecuteReader()
                If reader.HasRows Then
                    Do While reader.Read()
                        UPDDateiRechnEmailGenerieren(reader.GetString(0), reader.GetString(1), "rechnungsanhang")
                    Loop
                End If
                reader.Close()

            End Using
        Catch ex As Exception
            LogSchreiben("Fehler in Sub Ausgangsrechn_Per_Email_Metadaten!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=Ausgangsrechn_Per_Email_Metadaten, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
        Console.WriteLine("Metadaten Ausgangsrechnungen per E-Mail versendet fertig generiert!")
    End Sub

    Sub UPDDateiRechnEmailGenerieren(rechnr As String, dokuid As String, rechntyp As String)
        Dim datei As String
        Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")

        Try
            Using conBaan
                'datei = "\\appsrv01\c$\d3\import\D3P\Async\Ausgangsrechnungen\" + rechnr + ".upd"
                If rechntyp = "rechnung" Then
                    datei = "\\appsrv01\c$\d3\import\D3P\Async\Ausgangsrechnungen\rechnung_" + dokuid + "_" + rechnr + ".upd"
                Else
                    datei = "\\appsrv01\c$\d3\import\D3P\Async\Ausgangsrechnungen\rechnungsanhang_" + dokuid + "_" + rechnr + ".upd"
                End If
                If (My.Computer.FileSystem.FileExists(datei)) Then
                    My.Computer.FileSystem.DeleteFile(datei)
                End If
                file = My.Computer.FileSystem.OpenTextFileWriter(datei, True)
                'Zuerst die Indexdaten, damit das entsprechende Dokument gefunden werden kann

                'Durch die doku_id ist das Dokument eindeutig identifiziert
                'If rechntyp = "rechnung" Then
                ' file.WriteLine("o_dokuart=""DAREC""")
                'Else
                ' file.WriteLine("o_dokuart=""DARAN""")
                'End If
                'file.WriteLine("o_dok_dat_feld_10=""" + rechnr + """")
                file.WriteLine("o_doku_id=""" + dokuid + """")
                'Jetzt die Verschlagwortungsattribute aus Baan auslesen
                'n_dok_dat_feld_21="kundnr"
                'n_dok_dat_feld_22="name"
                'n_dok_dat_feld_50="rechdatum"
                'n_dok_dat_feld_51="buchungsdatum"
                'n_dok_dat_feld_80="betrag"
                'n_dok_dat_feld_60[1]="Kostenstelle - Beschreibung"
                'n_dok_dat_feld_61[1]="G02323 - Baustellenname"
                'n_dok_dat_feld_62[1]="Konto - Beschreibung"
                'n_dok_dat_feld_63[1]="F.ECO BZ - OFFERTA 105-2017" (kspr)
                'n_dok_dat_feld_64[1]="Lieferscheinnr"
                'n_dok_dat_feld_65[1]="Bestellnr"


                Dim cmd As SqlCommand = New SqlCommand("SELECT t_cuno, t_nama, CONVERT(varchar(10), t_buchdt, 104) as t_buchdt, CONVERT(varchar(10), t_rgdt, 104) as t_rgdt, " +
                                                            "replace(betrag,',','.') from v_metadaten_ar_email where rechnr='" + rechnr + "'", conBaan)
                conBaan.Open()
                cmd.Connection = conBaan
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    file.WriteLine("n_dok_dat_feld_21=""" + ZeichenPruefung(reader.GetString(0)) + """")
                    file.WriteLine("n_dok_dat_feld_22=""" + ZeichenPruefung(reader.GetString(1)) + """")
                    file.WriteLine("n_dok_dat_feld_50=""" + ZeichenPruefung(reader.GetString(2)) + """")
                    file.WriteLine("n_dok_dat_feld_51=""" + ZeichenPruefung(reader.GetString(3)) + """")
                    file.WriteLine("n_dok_dat_feld_80=""" + ZeichenPruefung(reader.GetString(4)) + """")
                End If
                reader.Close()
                conBaan.Close()

                If rechntyp = "rechnung" Then
                    RechnRestDatenLesen("kostenstelle", rechnr)
                    RechnRestDatenLesen("projekt", rechnr)
                    RechnRestDatenLesen("konto", rechnr)
                    RechnRestDatenLesen("kspr", rechnr)
                    RechnRestDatenLesen("lieferschein", rechnr)
                    RechnRestDatenLesen("bestellung", rechnr)
                End If
                file.Close()

            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Sub UPDDateiRechnEmailGenerieren!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=UPDDateiRechnEmailGenerieren, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            file.Close()
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
    End Sub

    Sub RechnRestDatenLesen(typ As String, rechnr As String)
        Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
        Dim cmd = New SqlCommand("select feld, kodex, beschreib from tmpRechDetail where  typ='" + typ + "' and rechnr='" + rechnr + "'", conBaan)
        Dim pos As Integer = 1
        conBaan.Open()
        cmd.Connection = conBaan
        Dim reader = cmd.ExecuteReader()
        If reader.HasRows Then
            Do While reader.Read()
                file.WriteLine(reader.GetString(0) + "[" + pos.ToString + "]=""" + reader.GetString(1) + " - " + reader.GetString(2) + """")
                pos = pos + 1
            Loop
        End If
        reader.Close()
        conBaan.Close()
    End Sub



    Sub Ausgangsrechn_Sonstige()
        'Diese Routine generiert die jpl-Dateien für jene Rechnungen, die noch nicht von Baan nach d3 übergeleitet wurden
        Console.WriteLine("Metadaten sonstige Ausgangsrechnungen werden generiert ...")
        Try
            Using conBaan4db
                'Suche alle Rechnungen in Baan, die noch nicht in d3 archiviert wurden
                Dim command As SqlCommand = New SqlCommand("select * from v_ausgangsrech_noch_nicht_in_d3", conBaan4db)
                conBaan4db.Open()
                command.Connection = conBaan4db
                Dim reader As SqlDataReader = command.ExecuteReader()
                If reader.HasRows Then
                    Dim sql2 As String = "INSERT INTO tmpRechDetail Select b.rechnr, kodex, beschreib, typ, feld FROM v_ausgangsrech_noch_nicht_in_d3 a LEFT join " +
                                  "v_metadaten_ar_email_rest b On a.rechnr=b.rechnr"
                    'aus Geschwindigkeitsgründen wird eine temporäre Tabelle generiert u. die Detaildaten dort hineingeschrieben
                    RechnRestDatenTempTableGen("anlegen", sql2)
                    Do While reader.Read()
                        JPLGenerieren(reader.GetString(1), reader.GetString(2))
                    Loop
                    RechnRestDatenTempTableGen("löschen", "")
                End If
                reader.Close()
            End Using
            Console.WriteLine("Metadaten sonstige Ausgangsrechnungen fertig generiert!")
        Catch ex As Exception
            LogSchreiben("Fehler In Sub Ausgangsrechn_Sonstige!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=Ausgangsrechn_Sonstige, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try

    End Sub

    Sub JPLGenerieren(rechnr As String, schluessel As String)
        Dim datei As String
        Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")

        Try
            Using conBaan
                datei = "\\appsrv01\c$\d3\import\D3P\Hostimp\Ausgangsrechnungen\" + rechnr + ".jpl"
                If (My.Computer.FileSystem.FileExists(datei)) Then
                    My.Computer.FileSystem.DeleteFile(datei)
                End If
                file = My.Computer.FileSystem.OpenTextFileWriter(datei, True)
                file.WriteLine("as400_erlaube_ueberspielen = " + """" + "1" + """")
                file.WriteLine("zeich_nr = " + """" + schluessel + """")
                file.WriteLine("dokuart = " + """" + "DAREC" + """")
                file.WriteLine("var_nr = " + """" + "1" + """")
                file.WriteLine("logi_verzeichnis = " + """" + "Be" + """")
                file.WriteLine("bearbeiter = " + """" + "admin01" + """")
                file.WriteLine("dok_dat_feld[10]= " + """" + rechnr + """")

                'Jetzt die Verschlagwortungsattribute aus Baan auslesen
                'n_dok_dat_feld_21="kundnr"
                'n_dok_dat_feld_22="name"
                'n_dok_dat_feld_50="rechdatum"
                'n_dok_dat_feld_51="buchungsdatum"
                'n_dok_dat_feld_80="betrag"
                'n_dok_dat_feld_60[1]="Kostenstelle - Beschreibung"
                'n_dok_dat_feld_61[1]="G02323 - Baustellenname"
                'n_dok_dat_feld_62[1]="Konto - Beschreibung"
                'n_dok_dat_feld_63[1]="F.ECO BZ - OFFERTA 105-2017" (kspr)
                'n_dok_dat_feld_64[1]="Lieferscheinnr"
                'n_dok_dat_feld_65[1]="Bestellnr"
                Dim cmd As SqlCommand = New SqlCommand("Select t_cuno, t_nama, CONVERT(varchar(10), t_buchdt, 104) As t_buchdt, CONVERT(varchar(10), t_rgdt, 104) As t_rgdt, " +
                                                            "replace(betrag,',','.') from v_metadaten_ar_email where rechnr='" + rechnr + "'", conBaan)
                conBaan.Open()
                cmd.Connection = conBaan
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    file.WriteLine("dok_dat_feld[21]= """ + ZeichenPruefung(reader.GetString(0)) + """")
                    file.WriteLine("dok_dat_feld[22]= """ + ZeichenPruefung(reader.GetString(1)) + """")
                    file.WriteLine("dok_dat_feld[50]= """ + ZeichenPruefung(reader.GetString(2)) + """")
                    file.WriteLine("dok_dat_feld[51]= """ + ZeichenPruefung(reader.GetString(3)) + """")
                    file.WriteLine("dok_dat_feld[80]= """ + ZeichenPruefung(reader.GetString(4)) + """")
                End If
                reader.Close()
                conBaan.Close()

                RechnRestDatenLesen2("kostenstelle", rechnr)
                RechnRestDatenLesen2("projekt", rechnr)
                RechnRestDatenLesen2("konto", rechnr)
                RechnRestDatenLesen2("kspr", rechnr)
                RechnRestDatenLesen2("lieferschein", rechnr)
                RechnRestDatenLesen2("bestellung", rechnr)

                file.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Sub JPLGeneriern!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=JPLGenerieren, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            file.Close()
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
    End Sub

    Sub RechnRestDatenLesen2(typ As String, rechnr As String)
        Try
            Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
            Using conBaan
                Dim cmd = New SqlCommand("select feld, kodex, beschreib from tmpRechDetail where  typ='" + typ + "' and rechnr='" + rechnr + "'", conBaan)
                Dim pos As Integer = 1
                conBaan.Open()
                cmd.Connection = conBaan
                Dim reader = cmd.ExecuteReader()
                If reader.HasRows Then
                    Do While reader.Read()
                        file.Write(reader.GetString(0).Substring(2, 15) + "[" + pos.ToString + "] = """ + reader.GetString(1))
                        If reader.GetString(2) <> "" Then
                            file.WriteLine(" - " + reader.GetString(2) + """")
                        Else
                            file.WriteLine("""")
                        End If
                        pos = pos + 1
                    Loop
                End If
                reader.Close()
                conBaan.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Sub RechnRestDatenLesen2!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=RechnRestDatenLesen2, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            file.Close()
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
    End Sub

    Sub RechnRestDatenTempTableGen(aktion As String, sql As String)
        Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
        Dim conBaan2 As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
        Try
            Using conBaan
                If aktion = "anlegen" Then
                    'Zuerst temporäre Tabelle löschen, falls noch als Leiche vorhanden
                    Dim cmd As SqlCommand = New SqlCommand("DROP TABLE tmpRechDetail", conBaan)
                    Try
                        Using conBaan
                            conBaan.Open()
                            cmd.Connection = conBaan
                            cmd.ExecuteNonQuery()
                        End Using
                    Catch ex As Exception
                    End Try

                    'temporäre Tabelle erstellen
                    Dim cmd2 As SqlCommand = New SqlCommand("CREATE TABLE tmpRechDetail (rechnr varchar(20), kodex varchar(20), beschreib varchar(255), typ varchar(20), " +
                                                               " feld varchar(20));", conBaan2)
                    conBaan2.Open()
                    cmd2.Connection = conBaan2
                    cmd2.ExecuteNonQuery()

                    'Jetzt Daten schreiben
                    cmd2.CommandText = sql
                    cmd2.CommandTimeout = 1000 'Sekunden
                    cmd2.ExecuteNonQuery()
                Else
                    Dim cmd2 As SqlCommand = New SqlCommand("drop table tmpRechDetail", conBaan2)
                    conBaan2.Open()
                    cmd2.Connection = conBaan2
                    cmd2.ExecuteNonQuery()
                End If
                conBaan.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Sub RechnRestDatenTempTableGen!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=RechnRestDatenTempTableGen, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
    End Sub


    Sub Eingangsrechn_Barcode(wieoft As String)
        'Diese Routine generiert die UPD-Dateien für die Eingangsrechnungen (Die Rechnungen sind bereits gescannt u. in d3 archiviert u. warten nur mehr auf die Verschlagwortung.
        'Diese erfolgt aufgrund des Barcodes
        Console.WriteLine("Metadaten Eingangsrechn_Barcode werden generiert ...")
        Try
            Using conBaan4db
                Dim sql, sql2 As String
                If wieoft = "stuendlich" Then
                    'Suche alle Rechnungen in d3, die noch nicht verschlagwortet sind, und zwar aufgrund eines leeren Lieferantenkodexes u. wo es 
                    'in Baan einen Buchungssatz gibt
                    sql = "SELECT barcode FROM v_metadaten_er_fehlende_d3_0"
                    sql2 = "INSERT INTO tmpRechDetail SELECT * FROM v_metadaten_er_fehlende_d3_1"
                Else
                    'Suche alle Rechnungen der letzten 4 Monate in d3 (weil Lieferscheine kommen oft erst viel später)
                    sql = "SELECT barcode FROM v_eingangsrechnungen_d3 WHERE dok_dat_feld_51>DATEADD(MONTH, -4, GETDATE())"
                    sql2 = "INSERT INTO tmpRechDetail SELECT * FROM v_metadaten_er_fehlende_d3_2"
                End If
                Dim command As SqlCommand = New SqlCommand(sql, conBaan4db)
                conBaan4db.Open()
                command.Connection = conBaan4db
                Dim reader As SqlDataReader = command.ExecuteReader()
                If reader.HasRows Then
                    'aus Geschwindigkeitsgründen wird eine temporäre Tabelle generiert u. die Detaildaten dort hineingeschrieben
                    RechnRestDatenTempTableGen("anlegen", sql2)
                    Do While reader.Read()
                        'XMLGenerieren(reader.GetString(0)) -> alte Prozedur wie Access, jedoch zu umständlich wegen zuerst Cold-Prozess u. dann erst Update
                        ER_UPDGenerieren(reader.GetString(0))
                    Loop
                    RechnRestDatenTempTableGen("loeschen", "")
                End If
                reader.Close()
            End Using
            Console.WriteLine("Metadaten Eingangsrechn_Barcode fertig generiert!")

            'Prüfen, ob es alte Barcodes gibt, wo die Buchung fehlt
            ERAlteBarcodesPruefen()
        Catch ex As Exception
            LogSchreiben("Fehler in Sub Eingangsrechn_Barcode!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=Eingangsrechn_Barcode, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try

    End Sub

    'Sub XMLGenerieren(barcode As String)
    '    Dim datei, rechnr As String
    '    Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")

    '    Try
    '        Using conBaan
    '            datei = "\\appsrv01\c$\d3\import\D3P\Baan\Eingangsrechnungen\" + barcode + ".xml"

    '            Dim cmd As SqlCommand = New SqlCommand("SELECT * from v_eingangsrechnungen_baan where barcode='" + barcode + "'", conBaan)
    '            conBaan.Open()
    '            cmd.Connection = conBaan
    '            Dim reader As SqlDataReader = cmd.ExecuteReader()
    '            If reader.HasRows Then
    '                reader.Read()
    '                rechnr = reader.GetString(11)

    '                If (My.Computer.FileSystem.FileExists(datei)) Then
    '                    My.Computer.FileSystem.DeleteFile(datei)
    '                End If
    '                file = My.Computer.FileSystem.OpenTextFileWriter(datei, True)

    '                'Kopfdaten
    '                file.WriteLine("<?xml version=""1.0"" encoding=""iso-8859-1""?>")
    '                file.WriteLine("<Daten>")
    '                file.WriteLine("<Operation>INSERT</Operation>")
    '                file.WriteLine("<Dokumenttyp>")
    '                file.WriteLine("<Dokumentkodex>Eingangsrechnung</Dokumentkodex>")
    '                file.WriteLine("</Dokumenttyp>")
    '                file.WriteLine("<Werte>")
    '                file.WriteLine("<Barcode>" + barcode + "</Barcode>")
    '                file.WriteLine("<Lieferantennummer>" + ReplaceXMLCharacters(reader.GetString(1)) + "</Lieferantennummer>")
    '                file.WriteLine("<Lieferantenname>" + ReplaceXMLCharacters(reader.GetString(2)) + "</Lieferantenname>")
    '                file.WriteLine("<MwSt-Nummer>" + reader.GetString(3) + "</MwSt-Nummer>")
    '                file.WriteLine("<Steuernummer>" + reader.GetString(4).Trim() + "</Steuernummer>")
    '                file.WriteLine("<Mwst-Registernummer>" + reader.GetInt32(5).ToString() + "</Mwst-Registernummer>")
    '                file.WriteLine("<Lieferantenrechnungsnummer>" + ReplaceXMLCharacters(reader.GetString(6)) + "</Lieferantenrechnungsnummer>")
    '                file.WriteLine("<Rechnungsdatum>" + reader.GetDateTime(7).ToShortDateString() + "</Rechnungsdatum>")
    '                file.WriteLine("<Buchungsdatum>" + reader.GetDateTime(8).ToShortDateString() + "</Buchungsdatum>")
    '                file.WriteLine("<Transaktionsnummer-BaaN>" + rechnr + "</Transaktionsnummer-BaaN>")
    '                file.WriteLine("<Dokumentennummer-BaaN>" + reader.GetInt32(9).ToString() + "</Dokumentennummer-BaaN>")
    '                file.WriteLine("<Gesamtbetrag>" + reader.GetDouble(12).ToString().Replace(",", ".") + "</Gesamtbetrag>")
    '                file.WriteLine("<Saldo>" + reader.GetDouble(18).ToString().Replace(",", ".") + "</Saldo>")
    '                file.WriteLine("<Beschreibung>" + ReplaceXMLCharacters(reader.GetString(13)) + "</Beschreibung>")

    '                'Schleife für Baustellen
    '                file.WriteLine("<Baustellen>")
    '                ERRestDatenLesen("projekt", rechnr)
    '                file.WriteLine("</Baustellen>")

    '                'Schleife für Bestellnummern
    '                file.WriteLine("<Bestellnummern>")
    '                ERRestDatenLesen("bestellung", rechnr)
    '                file.WriteLine("</Bestellnummern>")

    '                'Schleife für Lieferscheinnummern
    '                file.WriteLine("<Lieferscheine>")
    '                ERRestDatenLesen("lieferschein", rechnr)
    '                file.WriteLine("</Lieferscheine>")

    '                'Schleife für Kostenobjekt
    '                file.WriteLine("<Kostenobjekte-PRJ>")
    '                ERRestDatenLesen("kspr", rechnr)
    '                file.WriteLine("</Kostenobjekte-PRJ>")

    '                'Schleife für Konten
    '                file.WriteLine("<Konten>")
    '                ERRestDatenLesen("konten", rechnr)
    '                file.WriteLine("</Konten>")

    '                'Schleife für Kostenstellen
    '                file.WriteLine("<Kostenstellen>")
    '                ERRestDatenLesen("kostenstelle", rechnr)
    '                file.WriteLine("</Kostenstellen>")

    '                file.WriteLine("</Werte>")
    '                file.WriteLine("</Daten>")
    '                file.Close()
    '            End If
    '            reader.Close()
    '            conBaan.Close()
    '        End Using
    '    Catch ex As Exception
    '        LogSchreiben("Fehler In Sub JPLGeneriern!")
    '        LogSchreiben(ex.Message)
    '        Console.WriteLine(ex.Message)
    '        LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
    '        file.Close()
    '        Threading.Thread.Sleep(30000)
    '        Environment.Exit(-1)
    '    End Try
    'End Sub

    'Sub ERRestDatenLesen(typ As String, rechnr As String)
    '    Try
    '        Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
    '        Using conBaan
    '            Dim cmd = New SqlCommand("select * from v_metadaten_er_rest where  typ='" + typ + "' and rechnr='" + rechnr + "'", conBaan)
    '            conBaan.Open()
    '            cmd.Connection = conBaan
    '            Dim reader = cmd.ExecuteReader()
    '            If reader.HasRows Then
    '                Do While reader.Read()
    '                    Select Case typ
    '                        Case "projekt"
    '                            file.WriteLine("<Baustelle Baustellennummer=""" + reader.GetString(3) + """ Baustellenname=""" + ReplaceXMLCharacters(reader.GetString(4)) + """ />")
    '                        Case "bestellung"
    '                            file.WriteLine("<Bestellnummer>" + reader.GetString(3) + "</Bestellnummer>")
    '                        Case "lieferschein"
    '                            file.WriteLine("<Lieferschein>" + ReplaceXMLCharacters(reader.GetString(3)) + "</Lieferschein>")
    '                        Case "kspr"
    '                            file.WriteLine("<Kostenobjekt-PRJ Kostenobjekt-PRJ-Nummer=""" + reader.GetString(3) + """ Kostenobjekt-PRJ-Name=""" + ReplaceXMLCharacters(reader.GetString(4)) + """ />")
    '                        Case "konten"
    '                            file.WriteLine("<Konto Kontennummer=""" + reader.GetString(3) + """ Kontenname=""" + reader.GetString(4) + """ />")
    '                        Case "kostenstelle"
    '                            file.WriteLine("<Kostenstelle Kostenstellennummer=""" + reader.GetString(3) + """ Kostenstellenname=""" + ReplaceXMLCharacters(reader.GetString(4)) + """/>")
    '                    End Select
    '                Loop
    '            End If
    '            reader.Close()
    '            conBaan.Close()
    '        End Using
    '    Catch ex As Exception
    '        LogSchreiben("Fehler in Sub ERRestDatenLesen!")
    '        LogSchreiben(ex.Message)
    '        Console.WriteLine(ex.Message)
    '        LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
    '        Threading.Thread.Sleep(30000)
    '        Environment.Exit(-1)
    '    End Try
    'End Sub

    Sub ER_UPDGenerieren(barcode As String)
        Dim datei, rechnr As String
        Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
        Dim conBaan2 As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")

        Try
            Using conBaan
                datei = "\\appsrv01\c$\d3\import\D3P\Async\Eingangsrechnungen\" + barcode + ".upd"
                Dim cmd As SqlCommand = New SqlCommand("SELECT * FROM v_eingangsrechnungen_baan WHERE barcode='" + barcode + "'", conBaan)
                conBaan.Open()
                cmd.Connection = conBaan
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    reader.Read()
                    rechnr = reader.GetString(11)

                    If (My.Computer.FileSystem.FileExists(datei)) Then
                        My.Computer.FileSystem.DeleteFile(datei)
                    End If
                    file = My.Computer.FileSystem.OpenTextFileWriter(datei, True)

                    'Kopfdaten
                    file.WriteLine("o_dokuart = ""DEREC""")
                    file.WriteLine("o_dok_dat_feld_7 = """ + barcode + """")
                    file.WriteLine("o_zeich_nr = """ + barcode + """")
                    file.WriteLine("n_dok_dat_feld_1 = """ + ZeichenPruefung(reader.GetString(1)) + """")
                    file.WriteLine("n_dok_dat_feld_2 = """ + ZeichenPruefung(reader.GetString(2)) + """")
                    file.WriteLine("n_dok_dat_feld_3 = """ + ZeichenPruefung(reader.GetString(3)) + """")
                    file.WriteLine("n_dok_dat_feld_4 = """ + ZeichenPruefung(reader.GetString(4)) + """")
                    file.WriteLine("n_dok_dat_feld_6 = """ + ZeichenPruefung(reader.GetInt32(5).ToString()) + """")
                    file.WriteLine("n_dok_dat_feld_5 = """ + ZeichenPruefung(reader.GetString(6)) + """")
                    file.WriteLine("n_dok_dat_feld_8 = """ + ZeichenPruefung(reader.GetString(13)) + """")
                    file.WriteLine("n_dok_dat_feld_9 = """ + ZeichenPruefung(reader.GetInt32(9).ToString()) + """")
                    file.WriteLine("n_dok_dat_feld_10 = """ + rechnr + """")

                    file.WriteLine("n_dok_dat_feld_50 = """ + ZeichenPruefung(reader.GetString(19)) + """")
                    file.WriteLine("n_dok_dat_feld_51 = """ + ZeichenPruefung(reader.GetString(20)) + """")
                    file.WriteLine("n_dok_dat_feld_80 = """ + ZeichenPruefung(reader.GetDouble(12).ToString().Replace(",", ".")) + """")

                    'Schleife für die verschiedenen Unterattribute
                    ER_UPDRestDatenLesen("projekt", rechnr)
                    ER_UPDRestDatenLesen("bestellung", rechnr)
                    ER_UPDRestDatenLesen("lieferschein", rechnr)
                    ER_UPDRestDatenLesen("kspr", rechnr)
                    ER_UPDRestDatenLesen("konto", rechnr)
                    ER_UPDRestDatenLesen("kostenstelle", rechnr)

                    Using conBaan2
                        'Zahlunsinformation suchen
                        Dim cmd2 As SqlCommand = New SqlCommand("SELECT * FROM  v_zahlungen_eingangsrechnungen WHERE barcode='" + barcode + "'", conBaan2)
                        conBaan2.Open()
                        cmd2.Connection = conBaan2
                        Dim reader2 As SqlDataReader = cmd2.ExecuteReader()
                        If reader2.HasRows Then
                            reader2.Read()
                            file.WriteLine("n_dok_dat_feld_11 = """ + ZeichenPruefung(reader2.GetString(2)) + "-" +
                                   ZeichenPruefung(reader2.GetDouble(3).ToString()) + "-" + ZeichenPruefung(reader2.GetString(4)) + """")
                            file.WriteLine("n_dok_dat_feld_81 = """ + "0" + """")
                        Else
                            file.WriteLine("n_dok_dat_feld_81 = """ + ZeichenPruefung(reader.GetDouble(18).ToString().Replace(",", ".")) + """")
                        End If
                        reader2.Close()
                        conBaan2.Close()
                    End Using
                    file.Close()
                End If
                reader.Close()
                conBaan.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Sub ER_UMLGenerieren!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=ER_UMLGenerieren, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            file.Close()
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
    End Sub

    Sub ERAlteBarcodesPruefen()
        Dim mailtext As String
        Dim cond3 As New SqlConnection("Server=baan4\d3;Database=d3p;User Id=sa; Password=Baan123;")
        Try
            Using cond3
                Dim cmd As SqlCommand = New SqlCommand("Select barcode FROM v_eingangsrechnungen_d3 WHERE (liefnr='' OR liefnr IS NULL) " +
                                                           "And DATEDIFF(DAY, last_update_attr, GETDATE())>21", cond3)
                mailtext = "Hallo," + vbCrLf + "Bei der Verschlagwortung der Eingangsrechnungen mit Barcode in d3 habe ich Belege gefunden, die vor über 3 Wochen " +
                           "gescannt und in d3 archiviert wurden. Es gibt aber keine dazugehörigen Buchungssätze in Baan. Es handelt sich um die folgenden Barcodes:" + vbCrLf
                cond3.Open()
                cmd.Connection = cond3
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    Do While reader.Read()
                        mailtext += "- " + reader.GetString(0) + vbCrLf
                    Loop
                    mailtext += vbCrLf + "Grüße" + vbCrLf + "Rupert Obkircher"
                    MailVersenden("accounting@atzwanger.net", "Fehlende Buchungen", mailtext)
                End If
                reader.Close()
                cond3.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Function ERBuchungVorhandenPruefen!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=ERBuchungVorhandenPruefen, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            file.Close()
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
    End Sub

    Sub ER_UPDRestDatenLesen(typ As String, rechnr As String)
        Try
            Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
            Using conBaan
                Dim cmd = New SqlCommand("select feld, kodex, beschreib from tmpRechDetail where  typ='" + typ + "' and rechnr='" + rechnr + "'", conBaan)
                Dim pos As Integer = 1
                conBaan.Open()
                cmd.Connection = conBaan
                Dim reader = cmd.ExecuteReader()
                If reader.HasRows Then
                    Do While reader.Read()
                        file.Write(reader.GetString(0) + "[" + pos.ToString + "]=""" + reader.GetString(1))
                        If reader.GetString(2) <> "" Then
                            file.WriteLine(" - " + reader.GetString(2) + """")
                        Else
                            file.WriteLine("""")
                        End If
                        pos = pos + 1
                    Loop
                End If
                reader.Close()
                conBaan.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler in Sub ERUMLRestDatenLesen!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=ERUMLRestDatenLesen, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
    End Sub



    Sub BuchungsBelege_Metadaten()
        Console.WriteLine("Buchungsbelege werden verschlagwortet ...")
        Try
            Using cond3
                'Suche alle Datensätze, die noch keine Zahlungsdaten hinterlegt haben. Die wurden noch nicht verschlagwortet
                Dim command As SqlCommand = New SqlCommand("Select doku_id, kue_dokuart, LTrim(Str(dok_dat_feld_82)) As batchnr, dok_dat_feld_19 As bank
                    From firmen_spezifisch WHERE kue_dokuart IN ('DZAGR', 'DBAZW', 'DBUCH') and dok_dat_feld_9 IS NULL order by doku_id", cond3)
                cond3.Open()
                command.Connection = cond3
                Dim reader As SqlDataReader = command.ExecuteReader()
                If reader.HasRows Then
                    Do While reader.Read()
                        UPDDateiGenerieren(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3))
                    Loop
                End If
                reader.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler in Sub BuchungsBelege_Metadaten!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=BuchungsBelege_Metadaten, Fehler=""" + ex.Message)
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try
        Console.WriteLine("Buchungsbelege fertig verschlagwortet!")
    End Sub

    Sub UPDDateiGenerieren(dokid As String, dokuart As String, batchnr As String, bank As String)
        Dim datei As String = "\\appsrv01\c$\d3\import\D3P\Async\Buchungsbelege\" + dokid + ".upd"
        Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
        Try
            Using conBaan
                'Metadaten aus Baan lesen
                'Wichtig: die OR-Bedingung in der Where-Anweisung ist notwendig, weil die gesuchte Batchnr. aus einem Avviso stammen
                '         könnte oder aus einer sonstigen Zahlungsbuchung. Leider gibt es da 2 unterschiedliche Batchnr.-Felder
                Dim command2 As SqlCommand = New SqlCommand("SELECT lief, liefname, belegnr_er, CONVERT(varchar(10), belegdat_er, 104), 
                            CAST(betrag AS varchar(10)), beschreib, LTRIM(STR(batchnr_jahr)), LTRIM(STR(batchnr_bu)), belegnr_bu, 
                            CONVERT(varchar(10), datum_bu, 104) FROM v_metadaten_buchungsbelege WHERE belegnr_bu LIKE '" + bank +
                            "%' and (batchnr_avv=" + batchnr.Replace(".00", "") + " or batchnr_bu=" + batchnr + ")", conBaan)
                conBaan.Open()
                command2.Connection = conBaan
                Dim reader2 As SqlDataReader = command2.ExecuteReader()
                If reader2.HasRows Then
                    Dim pos As Integer = 1
                    Dim nureinmalschreiben As Boolean = True
                    'UPD-Datei generieren u. schreiben
                    If (My.Computer.FileSystem.FileExists(datei)) Then
                        My.Computer.FileSystem.DeleteFile(datei)
                    End If
                    file = My.Computer.FileSystem.OpenTextFileWriter(datei, True)
                    file.WriteLine("o_dokuart=""" + dokuart + """")
                    file.WriteLine("o_doku_id=""" + dokid + """")
                    Do While reader2.Read()
                        If nureinmalschreiben Then
                            file.WriteLine("n_dok_dat_feld_83=""" + ZeichenPruefung(reader2.GetString(7)) + """")                                  'Batchnr Buchung
                            file.WriteLine("n_dok_dat_feld_84=""" + ZeichenPruefung(reader2.GetString(6)) + """")                                  'Jahr Batchnr Buchung
                            file.WriteLine("n_dok_dat_feld_53=""" + ZeichenPruefung(reader2.GetString(9)) + """")                                  'Datum Buchung
                            file.WriteLine("n_dok_dat_feld_9=""" + ZeichenPruefung(reader2.GetString(8)) + """")                                   'Transaktionsnummer Buchung
                            nureinmalschreiben = False
                        End If
                        file.WriteLine("n_dok_dat_feld_68[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(5)) + """")                  'Beschreibung
                        file.WriteLine("n_dok_dat_feld_67[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(4)).Replace(",", ".") + """") 'Betrag
                        file.WriteLine("n_dok_dat_feld_66[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(3)) + """")                  'Transaktionsdatum Beleg
                        file.WriteLine("n_dok_dat_feld_65[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(2)) + """")                  'Transaktionsnummer Beleg
                        file.WriteLine("n_dok_dat_feld_63[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(0)) + """")                  'Lieferantennummer
                        file.WriteLine("n_dok_dat_feld_64[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(1)) + """")                  'Lieferantenname
                        pos = pos + 1
                    Loop
                    file.Close()
                Else
                    'Bei den Umbuchungen ändert sich das SQL, weil keine Bank hinterlegt ist
                    UPDDateiUmbuchungGen(dokid, dokuart, batchnr, bank)
                End If
                reader2.Close()
                conBaan.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Sub UPDDateiGenerieren!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=UPDDateiGenerieren, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            file.Close()
            My.Computer.FileSystem.DeleteFile(datei)
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try

    End Sub

    Sub UPDDateiUmbuchungGen(dokid As String, dokuart As String, batchnr As String, bank As String)
        Dim datei As String = dateipfad + dokid + ".upd"
        Dim conBaan As New SqlConnection("Server=baan4;Database=baan4db;User Id=sa; Password=Baan123;")
        Try
            Using conBaan
                'Metadaten aus Baan lesen
                'Wichtig: die OR-Bedingung in der Where-Anweisung ist notwendig, weil die gesuchte Batchnr. aus einem Avviso stammen
                '         könnte oder aus einer sonstigen Zahlungsbuchung. Leider gibt es da 2 unterschiedliche Batchnr.-Felder
                Dim command2 As SqlCommand = New SqlCommand("SELECT lief, liefname, belegnr_er, CONVERT(varchar(10), belegdat_er, 104), 
                            CAST(betrag AS varchar(10)), beschreib, LTRIM(STR(batchnr_jahr)), LTRIM(STR(batchnr_bu)), belegnr_bu, 
                            CONVERT(varchar(10), datum_bu, 104) FROM v_metadaten_buchungsbelege WHERE batchnr_bu=" + batchnr, conBaan)
                conBaan.Open()
                command2.Connection = conBaan
                Dim reader2 As SqlDataReader = command2.ExecuteReader()
                If reader2.HasRows Then
                    Dim pos As Integer = 1
                    Dim nureinmalschreiben As Boolean = True
                    'UPD-Datei generieren u. schreiben
                    If (My.Computer.FileSystem.FileExists(datei)) Then
                        My.Computer.FileSystem.DeleteFile(datei)
                    End If
                    file = My.Computer.FileSystem.OpenTextFileWriter(datei, True)
                    file.WriteLine("o_dokuart=""" + dokuart + """")
                    file.WriteLine("o_doku_id=""" + dokid + """")
                    Do While reader2.Read()
                        If nureinmalschreiben Then
                            file.WriteLine("n_dok_dat_feld_83=""" + ZeichenPruefung(reader2.GetString(7)) + """")                                  'Batchnr Buchung
                            file.WriteLine("n_dok_dat_feld_84=""" + ZeichenPruefung(reader2.GetString(6)) + """")                                  'Jahr Batchnr Buchung
                            file.WriteLine("n_dok_dat_feld_53=""" + ZeichenPruefung(reader2.GetString(9)) + """")                                  'Datum Buchung
                            file.WriteLine("n_dok_dat_feld_9=""" + ZeichenPruefung(reader2.GetString(8)) + """")                                   'Transaktionsnummer Buchung
                            nureinmalschreiben = False
                        End If
                        file.WriteLine("n_dok_dat_feld_68[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(5)) + """")                  'Beschreibung
                        file.WriteLine("n_dok_dat_feld_67[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(4)).Replace(",", ".") + """") 'Betrag
                        file.WriteLine("n_dok_dat_feld_66[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(3)) + """")                  'Transaktionsdatum Beleg
                        file.WriteLine("n_dok_dat_feld_65[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(2)) + """")                  'Transaktionsnummer Beleg
                        file.WriteLine("n_dok_dat_feld_63[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(0)) + """")                  'Lieferantennummer
                        file.WriteLine("n_dok_dat_feld_64[" + pos.ToString + "]=""" + ZeichenPruefung(reader2.GetString(1)) + """")                  'Lieferantenname
                        pos = pos + 1
                    Loop
                    file.Close()
                Else
                    UPDDateiUmbuchungGen(dokid, dokuart, batchnr, bank)
                End If
                reader2.Close()
                conBaan.Close()
            End Using
        Catch ex As Exception
            LogSchreiben("Fehler In Sub UPDDateiGenerieren!")
            LogSchreiben(ex.Message)
            Console.WriteLine(ex.Message)
            Process.Start("c:\baanapps\fehlermailsenden.exe", """Fehler im Programm metadaten_d3_generieren"" ""Prozedur=UPDDateiGenerieren, Fehler=""" + ex.Message)
            LogSchreiben("Programm metadaten_d3_generieren mit Fehler beendet.")
            file.Close()
            My.Computer.FileSystem.DeleteFile(datei)
            Threading.Thread.Sleep(30000)
            Environment.Exit(-1)
        End Try

    End Sub

    Sub LogSchreiben(ByVal sEvent As String)
        Dim sSource As String
        Dim sLog As String
        Try
            sSource = "metadaten_d3_generieren"
            sLog = "Application"
            If Not EventLog.SourceExists(sSource) Then
                EventLog.CreateEventSource(sSource, sLog)
            End If
            EventLog.WriteEntry(sSource, sEvent)
            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234)
        Catch ex As Exception
            'Bei Fehler nichts tun, denn das Log schreiben funktioniert auf einem lokal PC nicht, nur auf einem Server
        End Try
    End Sub



    Function ZeichenPruefung(inhalt As String) As String
        inhalt = inhalt.Replace(":", " ")
        inhalt = inhalt.Replace("""", "")
        inhalt = inhalt.Replace("'", "")
        inhalt = inhalt.Replace("°", ".")
        inhalt = inhalt.Replace("ß", "ss")
        inhalt = inhalt.Replace("à", "a")
        inhalt = inhalt.Replace("è", "e")
        inhalt = inhalt.Replace("ì", "i")
        inhalt = inhalt.Replace("ò", "o")
        inhalt = inhalt.Replace("ù", "u")
        inhalt = inhalt.Replace("á", "a")
        inhalt = inhalt.Replace("é", "e")
        inhalt = inhalt.Replace("í", "i")
        inhalt = inhalt.Replace("ó", "o")
        inhalt = inhalt.Replace("ú", "u")
        inhalt = inhalt.Replace("À", "A")
        inhalt = inhalt.Replace("È", "E")
        inhalt = inhalt.Replace("Ì", "I")
        inhalt = inhalt.Replace("Ò", "O")
        inhalt = inhalt.Replace("Ù", "U")
        inhalt = inhalt.Replace("Á", "A")
        inhalt = inhalt.Replace("É", "E")
        inhalt = inhalt.Replace("Í", "I")
        inhalt = inhalt.Replace("Ó", "O")
        inhalt = inhalt.Replace("Ú", "U")
        inhalt = inhalt.Replace("&", " ")
        inhalt = inhalt.Replace("<", "")
        inhalt = inhalt.Replace(">", "")
        inhalt = inhalt.Trim()
        ZeichenPruefung = inhalt
    End Function

    Function ReplaceXMLCharacters(aValue As String) As String

        Dim aNewValue As String
        aNewValue = aValue
        aNewValue = UCase(aNewValue)
        aNewValue = Replace(aNewValue, "&", "&amp;")
        aNewValue = Replace(aNewValue, "'", "&apos;")
        aNewValue = Replace(aNewValue, "<", "&lt;")
        aNewValue = Replace(aNewValue, ">", "&gt;")
        aNewValue = Replace(aNewValue, Chr(34), "&quot;")
        aNewValue = Trim(aNewValue)

        ReplaceXMLCharacters = aNewValue
    End Function

    Sub MailVersenden(emailempfaenger As String, betreff As String, mailbody As String)
        Dim MyEmail As New MailMessage

        Try
            MyEmail.From = New MailAddress("it@atzwanger.net")
            MyEmail.To.Add(emailempfaenger)
            MyEmail.CC.Add("rupert@atzwanger.net")
            MyEmail.Subject = betreff
            MyEmail.Body = mailbody
            Dim smtp As New SmtpClient("10.0.0.50")
            smtp.Port = 25
            smtp.Credentials = New System.Net.NetworkCredential("admin@atzwanger.net", "")
            smtp.Send(MyEmail)
        Catch ex As Exception
            MsgBox("Fehler: " & ex.Message.ToString)
        End Try
    End Sub

    Sub FehlerMailSenden(proz As String, message As String)
        Dim smtp As New SmtpClient("10.0.0.50")
        Dim MyEmail As New MailMessage

        MyEmail.From = New MailAddress("admin@atzwanger.net")
        MyEmail.To.Add("rupert@atzwanger.net")
        MyEmail.Subject = "Fehler im Programm LieferantenMahnung.exe"
        MyEmail.Body = "Fehler in Sub " + proz + vbCrLf + message
        smtp.Port = 25
        smtp.Credentials = New System.Net.NetworkCredential("admin@atzwanger.net", "")
        smtp.Send(MyEmail)
    End Sub

End Module
