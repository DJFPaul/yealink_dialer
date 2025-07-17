Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Public Class Form1
    Inherits System.Windows.Forms.Form

    Dim APPVERSION = New Integer() {3, 0}
    Dim MINIMUMVERSION = New Integer() {3, 0}

    Dim CFGSecret As String

    Dim DIAL As String
    Dim PHONEIP As String
    Dim SIPACCOUNT As String
    Dim APIUSERNAME As String
    Dim APIPASSWORD As String
    Dim CTime As Integer = 8
    Dim DTime As Integer = 3
    Dim CRDTA As String
    Dim PWRevealLock As Boolean
    Dim USESSL As Boolean
    Dim SKIPCERTCHECK As Boolean

    Structure ErrorLogStructure
        Private Index As Integer
        Private Description As String
        Private Notify As Boolean
    End Structure
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Read config secret key.
        Try
            CFGSecret = My.Computer.FileSystem.ReadAllText(Application.StartupPath() & "\data\secret.cfg")
        Catch ex As Exception
            CFGSecret = "SJKGSLD3529ASDGksgjsdlkgjSFASF!%!"
        End Try

        'Load phone configuration file and decrypt it.
        LoadConfigFile(Application.StartupPath() & "\data\phone.cfg", CFGSecret)

        'Grab command line args and sanitize them.
        Try
            DIAL = My.Application.CommandLineArgs(0).Replace("tel:", "").Replace("phone:", "").Replace("sip:", "").Replace("callto:", "").Replace("/", "").Replace("-", "")
            TelNumberBox.Text = DIAL

            'Start timers depending on config file.
            If AutoDialCheckBox.Checked = True Then CallDelay.Start()
            If AutoCloseCheckBox.Checked = True Then CloseTimer.Start()
            SmallMode()
        Catch
            TelNumberBox.Text = ""
            Button2.Show()
            DIAL = "012345678910"
            NormalMode()
        Finally
            IPLabel.Text = "IP: " & PHONEIP
        End Try
        SettingsPanel.Location = New Point(0, 0)
        SettingsPanel.Size = New Size(230, 382)
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
        CallDelay.Stop()
        CloseTimer.Stop()
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

    Private Sub CloseTimer_Tick(sender As Object, e As EventArgs) Handles CloseTimer.Tick
        'Autoclose after delay, if autoclose is enabled.
        If CTime > 1 Then
            CTime = CTime - 1
        Else
            Close()
        End If
    End Sub
    Private Sub CallDelay_Tick(sender As Object, e As EventArgs) Handles CallDelay.Tick
        'Execute call after delay, if autocall is enabled.
        SendRequestToPhone(APIUSERNAME, APIPASSWORD, PHONEIP, DIAL, SIPACCOUNT, USESSL, SKIPCERTCHECK)
        CallDelay.Stop()
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
    Private Function LoadConfigFile(ByVal FileToLoad As String, ByVal SecretToUse As String)
        'Load config and configure app with their data.

        'Create temporary variables used for clearer code structuring.
        Dim AutoCloseTemp As String = "True"
        Dim AutoDialTemp As String = "False"
        Dim PWRevealLockTemp As String = "True"
        Dim UseSSLTemp As String = "True"
        Dim SkipCertCheckTemp As String = "False"
        Dim TryErrorCount As Integer = 0
        Dim TryErrorLog As Array = {TryErrorCount, "", True}
        Try
            'Load application settings.
            Dim PhoneContent As String = My.Computer.FileSystem.ReadAllText(FileToLoad)
            TryErrorLog = {TryErrorCount, "Past file load", True} : TryErrorCount += 1

            'Decrypt configuration
            Dim rd As New RijndaelManaged
            Dim rijndaelIvLength As Integer = 16
            Dim md5 As New MD5CryptoServiceProvider
            Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(SecretToUse))
            md5.Clear()
            TryErrorLog = {TryErrorCount, "Past decryption prep", False} : TryErrorCount += 1
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
            TryErrorLog = {TryErrorCount, "Past decryption", True} : TryErrorCount += 1

            'Write data to configbox
            PhoneConfigBox.Text = CRDTA
            TryErrorLog = {TryErrorCount, "Past configbox setting", True} : TryErrorCount += 1

            'Append 8 empty lines to prevent possible old / bad config exceptions.
            PhoneConfigBox.Text = PhoneConfigBox.Text & vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine

            'Define temporary variables for version check.
            Dim MinV0Temp As Integer = PhoneConfigBox.Lines(0).Replace("MINVERSION=", "").Split(".")(0)
            Dim MinV1Temp As Integer = PhoneConfigBox.Lines(0).Replace("MINVERSION=", "").Split(".")(1)
            'Check if config file is older than the min version verification and migrate it.
            If PhoneConfigBox.Lines(0).Contains("PHONEIP") Then
                PhoneConfigBox.Text = "MINVERSION=" & MINIMUMVERSION(0) & "." & MINIMUMVERSION(1) & vbNewLine & PhoneConfigBox.Text
                MessageBox.Show("Settings have been imported from older version." & vbNewLine & "Please check and save your settings with the current version.", "Migration notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            'Check if current version is at least min versions, else abort load and show settings page.
            If MinV0Temp > APPVERSION(0) Then
                Throw New Exception("This config file is incompatible with this version" & vbNewLine & "Rreconfigure setting or update to the newest version.")
            Else
                If MinV0Temp = APPVERSION(0) Then
                    If MinV1Temp > APPVERSION(1) Then
                        Throw New Exception("This config file is incompatible with this version" & vbNewLine & "Rreconfigure setting or update to the newest version.")
                    End If
                End If
            End If

            'Setup Variables from config file.
            PHONEIP = PhoneConfigBox.Lines(1).Replace("PHONEIP=", "")
            SIPACCOUNT = PhoneConfigBox.Lines(2).Replace("SIPACCOUNT=", "")
            APIUSERNAME = PhoneConfigBox.Lines(3).Replace("WEBUSERNAME=", "")
            APIPASSWORD = PhoneConfigBox.Lines(4).Replace("WEBPASSWORD=", "")
            AutoCloseTemp = PhoneConfigBox.Lines(5).Replace("autoclose=", "")
            CTime = PhoneConfigBox.Lines(6).Replace("closedelay=", "")
            AutoDialTemp = PhoneConfigBox.Lines(7).Replace("autodial=", "")
            DTime = PhoneConfigBox.Lines(8).Replace("dialdelay=", "")
            PWRevealLockTemp = PhoneConfigBox.Lines(9).Replace("pwreveallock=", "")
            UseSSLTemp = PhoneConfigBox.Lines(10).Replace("usessl=", "")
            SkipCertCheckTemp = PhoneConfigBox.Lines(11).Replace("ignorecert=", "")

            TryErrorLog = {TryErrorCount, "Past variable parsing", True} : TryErrorCount += 1

            'Do some checks and adjustments if nesseary to correct loading or prevent errors when migrating from older configs.
            APIUSERNAME = APIUSERNAME.Replace("USERNAME=", "")
            APIPASSWORD = APIPASSWORD.Replace("PASSWORD=", "")
            If AutoCloseTemp = "" Then AutoCloseTemp = "True"
            If AutoDialTemp = "" Then AutoDialTemp = "False"
            If PWRevealLockTemp = "" Then PWRevealLockTemp = "True"
            If UseSSLTemp = "" Then UseSSLTemp = "False"
            If SkipCertCheckTemp = "" Then SkipCertCheckTemp = "True"

            TryErrorLog = {TryErrorCount, "Past migration mitigations", True} : TryErrorCount += 1

        Catch ex As Exception
            If TryErrorLog(2) = True Then MsgBox("Loading state: " & TryErrorLog(0) & vbNewLine & ex.Message)
            NormalMode()
            SettingsPanel.Show()
        Finally
        End Try

        'Configure app UI states.
        PhoneIPBox.Text = PHONEIP
        SipAccountBox.Text = SIPACCOUNT
        WebUsernameBox.Text = APIUSERNAME
        WebPasswordBox.Text = APIPASSWORD

        AutoCloseDelayBox.Value = CTime
        AutoDialDelayBox.Value = DTime
        CallDelay.Interval = DTime * 1000

        If Convert.ToBoolean(AutoCloseTemp) = True Then
            AutoCloseCheckBox.Checked = True
        Else
            AutoCloseCheckBox.Checked = False
        End If

        If Convert.ToBoolean(AutoDialTemp) = True Then
            AutoDialCheckBox.Checked = True
        Else
            AutoDialCheckBox.Checked = False
        End If

        If Convert.ToBoolean(PWRevealLockTemp) = True Then
            PWShowLockCheckbox.Checked = True
            PWRevealLock = True
        Else
            PWShowLockCheckbox.Checked = False
            PWRevealLock = False
        End If

        If Convert.ToBoolean(UseSSLTemp) = True Then
            UseSSLCheckbox.Checked = True
            USESSL = True
        Else
            UseSSLCheckbox.Checked = False
            USESSL = False
        End If

        If Convert.ToBoolean(SkipCertCheckTemp) = True Then
            SkipCertCheckCheckbox.Checked = True
            SKIPCERTCHECK = True
        Else
            SkipCertCheckCheckbox.Checked = False
            SKIPCERTCHECK = False
        End If
    End Function

    Private Function SaveConfigFile(ByVal FileToSave As String, ByVal SecretToUse As String)
        AssembleConfigFile()

        'Encrypt config with secret.
        Dim rd As New RijndaelManaged
        Dim md5 As New MD5CryptoServiceProvider
        Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(SecretToUse))
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
        Using writer As New System.IO.StreamWriter(FileToSave)
            For Each line In "A"
                writer.Write(CRDTA)
            Next
        End Using
    End Function
    Private Function AssembleConfigFile()
        'Combine all config fields into the invisible config textbox.
        PhoneConfigBox.Text = "MINVERSION=" & MINIMUMVERSION(0) & "." & MINIMUMVERSION(1) & vbNewLine & "PHONEIP=" & PhoneIPBox.Text & vbNewLine & "SIPACCOUNT=" & SipAccountBox.Text & vbNewLine & "WEBUSERNAME=" & WebUsernameBox.Text & vbNewLine & "WEBPASSWORD=" & WebPasswordBox.Text & vbNewLine & "autoclose=" & AutoCloseCheckBox.Checked & vbNewLine & "closedelay=" & AutoCloseDelayBox.Value & vbNewLine & "autodial=" & AutoDialCheckBox.Checked & vbNewLine & "dialdelay=" & AutoDialDelayBox.Value & vbNewLine & "pwreveallock=" & PWShowLockCheckbox.Checked & vbNewLine & "usessl=" & UseSSLCheckbox.Checked & vbNewLine & "ignorecert=" & SkipCertCheckCheckbox.Checked & vbNewLine
    End Function
    Public Shared Function GetComputerID() As Long
        'Generate uinque computer ID for encryption (currently unused)
        Dim objMOS As New ManagementObjectSearcher("Select * From Win32_Processor")
        Dim computerinfo As String = My.Computer.Name
        For Each objMO As Management.ManagementObject In objMOS.Get
            computerinfo &= objMO("ProcessorID")
        Next
        Return computerinfo.GetHashCode
    End Function
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TelNumberBox.TextChanged
        'If numberfield is not empty, enable call button.
        If TelNumberBox.Text = "" Then
            CallButton.Enabled = False
        Else
            CallButton.Enabled = True
        End If
    End Sub
    Private Function SendRequestToPhone(ByVal SENDWEBUSER As String, ByVal SENDWEBPASSWORD As String, ByVal SENDPHONEIP As String, ByVal SENDDIAL As String, ByVal SENDSIPACCOUNT As String, ByVal SENDUSEHTTPS As Boolean, ByVal SENDSKIPSSLCHECK As Boolean)
        'Make a Webrequest to the phones API URL

        Try
            Dim encoding As New UTF8Encoding
            Dim USEHTTPS As String = ""
            If SENDUSEHTTPS = True Then USEHTTPS = "s"
            Dim makeapirequest As HttpWebRequest = DirectCast(HttpWebRequest.Create("http" & USEHTTPS & "://" & SENDPHONEIP & "/servlet?key=number=" & SENDDIAL & "&outgoing_uri=" & SENDSIPACCOUNT), HttpWebRequest)
            makeapirequest.Method = "GET"
            makeapirequest.Timeout = 5000
            makeapirequest.UserAgent = "YealinkDialer"
            makeapirequest.ContentType = "application/json"

            'Ignore SSL Certificate Verification. Nesseary for self signed/offline devices.
            If SENDSKIPSSLCHECK = True Then
                makeapirequest.ServerCertificateValidationCallback = Function(s, c, h, e) True
            End If


            'Create header with the crendetials to use for the requests authorization.
            makeapirequest.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(encoding.GetEncoding("ISO-8859-1").GetBytes(SENDWEBUSER + ":" + SENDWEBPASSWORD)))

            'Read response to properly finish the request, but discard result as we do not need it.
            Dim postresponse As HttpWebResponse
            postresponse = DirectCast(makeapirequest.GetResponse, HttpWebResponse)
            Dim postreqreader As New StreamReader(postresponse.GetResponseStream())
            Dim respondetstring As String = postreqreader.ReadToEnd
            respondetstring = ""
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
    Public Function ProxySendRequestToPhone(ByVal SENDWEBUSER As String, ByVal SENDWEBPASSWORD As String, ByVal SENDPHONEIP As String, ByVal SENDDIAL As String, ByVal SENDSIPACCOUNT As String, ByVal SENDUSEHTTPS As Boolean, ByVal SENDSKIPSSLCHECK As Boolean)
        'Used by the Setup Wizard to initiate the test call.
        SendRequestToPhone(SENDWEBUSER, SENDWEBPASSWORD, SENDPHONEIP, SENDDIAL, SENDSIPACCOUNT, SENDUSEHTTPS, SENDSKIPSSLCHECK)
    End Function

    Private Function PWRevealStateConfigure()
        'When this function is called, the current state of the password lockout feature is evaluated and UI prepared accordingly.
        If PWShowLockCheckbox.Checked = True And WebPasswordBox.Text <> "" Then
            PWRevealLock = True
            WebPasswordBox.UseSystemPasswordChar = True
            'WebPasswordBox.Enabled = False
            PictureBox1.BackgroundImage = My.Resources.EyeGrayedOutCrossedOut
        Else
            PWRevealLock = False
            'WebPasswordBox.Enabled = True
            WebPasswordBox.UseSystemPasswordChar = True
            PictureBox1.BackgroundImage = My.Resources.EyeGrayedOut
        End If
    End Function
#Region "UI Buttons and other clickable things"
    Private Sub CallButton_1(sender As Object, e As EventArgs) Handles CallButton.Click
        'Call Button

        If CallButton.Enabled = True Then
            DIAL = TelNumberBox.Text
            CloseTimer.Stop()
            SendRequestToPhone(APIUSERNAME, APIPASSWORD, PHONEIP, DIAL, SIPACCOUNT, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked)
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Test Call Button

        Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim Result As DialogResult
        Dim PasswordToShow As String = WebPasswordBox.Text

        'Check if the password field is hidden, in that case convert the password to be shown to placeholder symbols of the same amount.
        If WebPasswordBox.UseSystemPasswordChar = True Then
            PasswordToShow = ""
            For Each letter In WebPasswordBox.Text
                PasswordToShow = PasswordToShow & "*"
            Next
        End If

        'Show user a messagebox with all info to be send to the phone for the test call.
        Result = MessageBox.Show("Initiate test call?" & vbNewLine & "(Calls 012345678910)" & vbNewLine & vbNewLine & "Phone IP: " & PhoneIPBox.Text & vbNewLine & "Account: " & SipAccountBox.Text & vbNewLine & "Username: " & WebUsernameBox.Text & vbNewLine & "Password: " & PasswordToShow & vbNewLine & vbNewLine & "Use SSL: " & UseSSLCheckbox.Checked.ToString & vbNewLine & "Ignore check: " & SkipCertCheckCheckbox.Checked.ToString, "Test call?", Buttons, MessageBoxIcon.Information)
        If Result = DialogResult.Yes Then
            SendRequestToPhone(WebUsernameBox.Text, WebPasswordBox.Text, PhoneIPBox.Text, "012345678910", SipAccountBox.Text, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked)
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Settings save button.

        'Generate new random crypto secret and save it.
        CFGSecret = RandomString(250, 256)
        Using writer As New System.IO.StreamWriter(Application.StartupPath() & "\data\secret.cfg")
            For Each line In "A"
                writer.Write(CFGSecret)
            Next
        End Using

        'Save config with new secret
        SaveConfigFile(Application.StartupPath() & "\data\phone.cfg", CFGSecret)
        SettingsPanel.Hide()

        'Set new vairables
        PHONEIP = PhoneConfigBox.Lines(1).Replace("PHONEIP=", "")
        SIPACCOUNT = PhoneConfigBox.Lines(2).Replace("SIPACCOUNT=", "")
        APIUSERNAME = PhoneConfigBox.Lines(3).Replace("WEBUSERNAME=", "")
        APIPASSWORD = PhoneConfigBox.Lines(4).Replace("WEBPASSWORD=", "")
        USESSL = UseSSLCheckbox.Checked
        SKIPCERTCHECK = SkipCertCheckCheckbox.Checked
    End Sub
    Private Sub SettingsLabel_Click(sender As Object, e As EventArgs) Handles SettingsLabel.Click
        'Configure UI and show the settings page.
        PWRevealStateConfigure()
        NormalMode()
        SettingsPanel.Show()
    End Sub
    Private Sub SetupWizardLabel_Click(sender As Object, e As EventArgs) Handles SetupWizardLabel.Click
        Me.Hide()
        SetupWizard.ShowDialog()
        Me.Show()
    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        'Show/reveal icon on the web password text field.

        If WebPasswordBox.UseSystemPasswordChar = True Then
            If PWRevealLock = False Then
                WebPasswordBox.UseSystemPasswordChar = False
                PictureBox1.BackgroundImage = My.Resources.EyeOn
            End If
        Else
            WebPasswordBox.UseSystemPasswordChar = True
            PictureBox1.BackgroundImage = My.Resources.EyeGrayedOut
        End If
    End Sub
    Private Sub PWShowLockCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles PWShowLockCheckbox.CheckedChanged
        'Enforce a password wipe if config was saved with lockout flag on to prevent leaking.

        If PWRevealLock = True Then
            WebPasswordBox.Text = ""
            WebPasswordBox.Enabled = True
            PWRevealLock = False
            PictureBox1.BackgroundImage = My.Resources.EyeGrayedOut
        End If
    End Sub

#Region "Dial Number Buttons"
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

    Private Sub SettingsPanel_Paint(sender As Object, e As PaintEventArgs) Handles SettingsPanel.Paint

    End Sub
#End Region
#End Region
End Class