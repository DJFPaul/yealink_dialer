Imports System.IO
Imports System.Management
Imports System.Security.Cryptography
Imports System.Text
Public Class Form1
    Inherits System.Windows.Forms.Form

    Dim CFGSecret As String

    Dim DIAL As String
    Dim PHONEIP As String
    Dim SIPACCOUNT As String
    Dim SIPUSERNAME As String
    Dim SIPPASSWORD As String
    Dim CTime As Integer = 8
    Dim DTime As Integer = 3
    Dim CRDTA As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Load phone configuration file and decrypt it.
        Try
            CFGSecret = My.Computer.FileSystem.ReadAllText(Application.StartupPath() & "\data\secret.cfg")
        Catch ex As Exception
            CFGSecret = "SJKGSLD3529ASDGksgjsdlkgjSFASF!%!"
        End Try
        Try
            Dim PhoneContent As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath() & "\data\phone.cfg")

            Dim rd As New RijndaelManaged
            Dim rijndaelIvLength As Integer = 16
            Dim md5 As New MD5CryptoServiceProvider
            Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(CFGSecret))
            md5.Clear()
            Dim encdata() As Byte = Convert.FromBase64String(PhoneContent)
            Dim ms As New MemoryStream(encdata)
            Dim iv(15) As Byte
            ms.Read(iv, 0, rijndaelIvLength)
            rd.IV = iv
            rd.Key = key
            Dim cs As New CryptoStream(ms, rd.CreateDecryptor, CryptoStreamMode.Read)
            Dim data(ms.Length - rijndaelIvLength) As Byte
            Dim i As Integer = cs.Read(data, 0, data.Length)
            CRDTA = System.Text.Encoding.UTF8.GetString(data, 0, i)
            cs.Close()
            rd.Clear()
            PhoneConfigBox.Text = CRDTA

            'Load application settings.
            CTime = PhoneConfigBox.Lines(5).Replace("closedelay=", "")
            DTime = PhoneConfigBox.Lines(7).Replace("dialdelay=", "")


            If PhoneConfigBox.Lines(4).Replace("autoclose=", "") = "True" Then
                AutoCloseCheckBox.Checked = True
            Else
                AutoCloseCheckBox.Checked = False
            End If
            If PhoneConfigBox.Lines(6).Replace("autodial=", "") = "True" Then
                AutoDialCheckBox.Checked = True
            Else
                AutoDialCheckBox.Checked = False
            End If
        Catch ex As Exception
            NormalMode()
            SettingsPanel.Show()
        Finally
        End Try

        'Configure parameters
        PHONEIP = PhoneConfigBox.Lines(0).Replace("PHONEIP=", "")
        SIPACCOUNT = PhoneConfigBox.Lines(1).Replace("SIPACCOUNT=", "")
        SIPUSERNAME = PhoneConfigBox.Lines(2).Replace("USERNAME=", "")
        SIPPASSWORD = PhoneConfigBox.Lines(3).Replace("PASSWORD=", "")

        PhoneIPBox.Text = PHONEIP
        SipAccountBox.Text = SIPACCOUNT
        SipUsernameBox.Text = SIPUSERNAME
        SipPasswordBox.Text = SIPPASSWORD

        AutoCloseDelayBox.Value = CTime
        AutoDialDelayBox.Value = DTime
        CallDelay.Interval = DTime * 1000

        'Grab command line args and sanitize them.
        Try
            DIAL = My.Application.CommandLineArgs(0).Replace("tel:", "").Replace("phone:", "").Replace("sip:", "").Replace("callto:", "").Replace("/", "").Replace("-", "")
            TelNumberBox.Text = DIAL

            'Start timers depending on config file.
            If AutoCloseCheckBox.Checked = True Then CloseTimer.Start()
            If AutoCloseCheckBox.Checked = True Then CallDelay.Start()
            SmallMode()
        Catch
            TelNumberBox.Text = ""
            Button2.Show()
            DIAL = "012345678910"
            NormalMode()
        Finally
            IPLabel.Text = "Phone IP: " & PHONEIP
        End Try
        SettingsPanel.Location = New Point(12, 12)
        SettingsPanel.Size = New Size(205, 364)
    End Sub
    Private Function SmallMode()
        BottomPanel.Location = New Point(0, 44)
        Dial1.Hide()
        Dial2.Hide()
        Dial3.Hide()
        Dial4.Hide()
        Dial5.Hide()
        Dial6.Hide()
        Dial7.Hide()
        Dial8.Hide()
        Dial9.Hide()
        Dial0.Hide()
        DialPlus.Hide()
        DialErase.Hide()
        Me.Size = New Size(245, 140)
    End Function
    Private Function NormalMode()
        BottomPanel.Location = New Point(0, 324)
        Dial1.Show()
        Dial2.Show()
        Dial3.Show()
        Dial4.Show()
        Dial5.Show()
        Dial6.Show()
        Dial7.Show()
        Dial8.Show()
        Dial9.Show()
        Dial0.Show()
        DialPlus.Show()
        DialErase.Show()
        Me.Size = New Size(245, 420)
    End Function

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles CallButton.Click

        If CallButton.Enabled Then
            'If launched with callto link, this button instead is the CALL button and initiates the call once pressed.
            DIAL = TelNumberBox.Text
            CloseTimer.Stop()
            WebBrowser1.Navigate("http://" & SIPUSERNAME & ":" & SIPPASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")
        End If
    End Sub

    'Autoclose after delay, if autoclose is enabled.
    Private Sub CloseTimer_Tick(sender As Object, e As EventArgs) Handles CloseTimer.Tick
        If CTime > 1 Then
            CTime = CTime - 1
        Else
            Close()
        End If
    End Sub

    'Execute call after delay, if autocall is enabled.
    Private Sub CallDelay_Tick(sender As Object, e As EventArgs) Handles CallDelay.Tick
        WebBrowser1.Navigate("http://" & SIPUSERNAME & ":" & SIPPASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")
        CallDelay.Stop()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'Assemble config file
        PhoneConfigBox.Text = "PHONEIP=" & PhoneIPBox.Text & vbNewLine & "SIPACCOUNT=" & SipAccountBox.Text & vbNewLine & "USERNAME=" & SipUsernameBox.Text & vbNewLine & "PASSWORD=" & SipPasswordBox.Text & vbNewLine & "autoclose=" & AutoCloseCheckBox.Checked & vbNewLine & "closedelay=" & AutoCloseDelayBox.Value & vbNewLine & "autodial=" & AutoDialCheckBox.Checked & vbNewLine & "dialdelay=" & AutoDialDelayBox.Value & vbNewLine

        'Test Call Button

        Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim Result As DialogResult
        Result = MessageBox.Show("Initiate test call?" & vbNewLine & "(Calls 01234567890)" & vbNewLine & vbNewLine & "Phone IP: " & PhoneIPBox.Text & vbNewLine & "Account: " & SipAccountBox.Text & vbNewLine & "Username: " & SipUsernameBox.Text & vbNewLine & "Password: " & SipPasswordBox.Text, "Test call?", Buttons, MessageBoxIcon.Information)

        If Result = DialogResult.Yes Then
            PHONEIP = PhoneConfigBox.Lines(0).Replace("PHONEIP=", "")
            SIPACCOUNT = PhoneConfigBox.Lines(1).Replace("SIPACCOUNT=", "")
            SIPUSERNAME = PhoneConfigBox.Lines(2).Replace("USERNAME=", "")
            SIPPASSWORD = PhoneConfigBox.Lines(3).Replace("PASSWORD=", "")

            WebBrowser1.Navigate("http://" & SIPUSERNAME & ":" & SIPPASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")

        End If
    End Sub

    'This function generated the random string to use for encrypting the config file.
    Function RandomString(minCharacters As Integer, maxCharacters As Integer)
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789=?!&+-#.$§"
        Static r As New Random
        Dim chactersInString As Integer = r.Next(minCharacters, maxCharacters)
        Dim sb As New StringBuilder
        For i As Integer = 1 To chactersInString
            Dim idx As Integer = r.Next(0, s.Length)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function

    'Generate uinque computer ID for encryption (currently unused)
    Public Shared Function GetComputerID() As Long
        Dim objMOS As New ManagementObjectSearcher("Select * From Win32_Processor")
        Dim computerinfo As String = My.Computer.Name
        For Each objMO As Management.ManagementObject In objMOS.Get
            computerinfo &= objMO("ProcessorID")
        Next
        Return computerinfo.GetHashCode
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Process.Start("explorer.exe", Application.StartupPath & "\data")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Assemble config file
        PhoneConfigBox.Text = "PHONEIP=" & PhoneIPBox.Text & vbNewLine & "SIPACCOUNT=" & SipAccountBox.Text & vbNewLine & "USERNAME=" & SipUsernameBox.Text & vbNewLine & "PASSWORD=" & SipPasswordBox.Text & vbNewLine & "autoclose=" & AutoCloseCheckBox.Checked & vbNewLine & "closedelay=" & AutoCloseDelayBox.Value & vbNewLine & "autodial=" & AutoDialCheckBox.Checked & vbNewLine & "dialdelay=" & AutoDialDelayBox.Value & vbNewLine

        'Generate new random crypto password and save it.

        CFGSecret = RandomString(250, 256)
        Using writer As New System.IO.StreamWriter(Application.StartupPath() & "\data\secret.cfg")
            For Each line In "A"
                writer.Write(CFGSecret)
            Next
        End Using


        'Encrypt config with generated string.

        Dim rd As New RijndaelManaged
        Dim md5 As New MD5CryptoServiceProvider
        Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(CFGSecret))
        md5.Clear()
        rd.Key = key
        rd.GenerateIV()
        Dim iv() As Byte = rd.IV
        Dim ms As New MemoryStream
        ms.Write(iv, 0, iv.Length)
        Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
        Dim data() As Byte = System.Text.Encoding.UTF8.GetBytes(PhoneConfigBox.Text)
        cs.Write(data, 0, data.Length)
        cs.FlushFinalBlock()
        Dim encdata() As Byte = ms.ToArray()
        CRDTA = Convert.ToBase64String(encdata)
        cs.Close()
        rd.Clear()

        'Save encrypted data.
        Using writer As New System.IO.StreamWriter(Application.StartupPath() & "\data\phone.cfg")
            For Each line In "A"
                writer.Write(CRDTA)
            Next
        End Using
        SettingsPanel.Hide()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TelNumberBox.TextChanged
        'If numberfield is not empty, enable call button.
        If TelNumberBox.Text = "" Then
            CallButton.Enabled = False
        Else
            CallButton.Enabled = True
        End If
    End Sub

    Private Sub Dial1_Click(sender As Object, e As EventArgs) Handles Dial1.Click
        TelNumberBox.Text = TelNumberBox.Text & "1"
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Dial2.Click
        TelNumberBox.Text = TelNumberBox.Text & "2"
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Dial3.Click
        TelNumberBox.Text = TelNumberBox.Text & "3"
    End Sub

    Private Sub Dial4_Click(sender As Object, e As EventArgs) Handles Dial4.Click
        TelNumberBox.Text = TelNumberBox.Text & "4"
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Dial5.Click
        TelNumberBox.Text = TelNumberBox.Text & "5"
    End Sub

    Private Sub Dial6_Click(sender As Object, e As EventArgs) Handles Dial6.Click
        TelNumberBox.Text = TelNumberBox.Text & "6"
    End Sub

    Private Sub Dial7_Click(sender As Object, e As EventArgs) Handles Dial7.Click
        TelNumberBox.Text = TelNumberBox.Text & "7"
    End Sub

    Private Sub Dial8_Click(sender As Object, e As EventArgs) Handles Dial8.Click
        TelNumberBox.Text = TelNumberBox.Text & "8"
    End Sub

    Private Sub Dial9_Click(sender As Object, e As EventArgs) Handles Dial9.Click
        TelNumberBox.Text = TelNumberBox.Text & "9"
    End Sub

    Private Sub Dial0_Click(sender As Object, e As EventArgs) Handles Dial0.Click
        TelNumberBox.Text = TelNumberBox.Text & "0"
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles DialErase.Click
        If TelNumberBox.Text.Length > 0 Then TelNumberBox.Text = TelNumberBox.Text.Substring(0, TelNumberBox.Text.Length - 1)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles DialPlus.Click
        TelNumberBox.Text = TelNumberBox.Text & "+"
    End Sub

    Private Sub SettingsLabel_Click(sender As Object, e As EventArgs) Handles SettingsLabel.Click
        NormalMode()
        SettingsPanel.Show()
    End Sub
End Class
