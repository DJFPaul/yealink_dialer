Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading
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
    Dim ContactNameLabel As Label
    Dim CallStatusLabel As Label
    Dim CallPulse As System.Windows.Forms.Timer
    Dim HangupButton As Button
    Dim CallingPhrase As String
    Dim IncomingRinging As Boolean
    Dim SteadyStatus As Boolean
    Dim Held As Boolean
    Dim Muted As Boolean
    Dim DndEnabled As Boolean
    Dim StatusDots As Integer
    Dim HoldButton As Button
    Dim MuteButton As Button
    Dim TransferButton As Button
    Dim ConfButton As Button
    Dim SpeakerButton As Button
    Dim HeadsetButton As Button
    Dim DndButton As Button
    Dim RedialButton As Button
    Dim HistoryButton As Button
    Dim HistoryPanel As Panel
    Dim HistoryList As ListView
    Dim HistoryLines As New List(Of String)
    Dim AnsweredFromRing As Boolean

    Structure ErrorLogStructure
        Private Index As Integer
        Private Description As String
        Private Notify As Boolean
    End Structure
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Load phone configuration file and decrypt it.
        LoadConfigFile(Application.StartupPath() & "\data\phone.cfg")

        EnsureCallLabels()
        LoadCallHistory()

        'Grab command line args and sanitize them.
        Dim launch = ParseLaunchArgs(My.Application.CommandLineArgs)
        If launch.EventName <> "" Then
            ApplyPhoneEvent(launch.EventName, launch.Number, launch.Name)
        ElseIf launch.Number <> "" Then
            ShowIncomingNumber(launch.Number, launch.Name, launch.Incoming)
        Else
            TelNumberBox.Text = ""
            Button2.Show()
            DIAL = "012345678910"
            NormalMode()
        End If
        IPLabel.Text = "IP: " & PHONEIP
        SettingsPanel.Location = New Point(0, 0)
        SettingsPanel.Size = New Size(230, 382)
    End Sub
    Public Class LaunchArgs
        Public Number As String = ""
        Public Name As String = ""
        Public Incoming As Boolean = False
        Public EventName As String = ""
    End Class
    Public Shared Function ParseLaunchArgs(args As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As LaunchArgs
        Dim result As New LaunchArgs
        If args Is Nothing OrElse args.Count = 0 Then Return result
        Dim index As Integer = 0
        If index < args.Count AndAlso args(index) = "--incoming" Then
            result.Incoming = True
            index += 1
        End If
        If index < args.Count AndAlso args(index) = "--event" Then
            index += 1
            If index < args.Count Then
                result.EventName = args(index)
                index += 1
            End If
        End If
        If index >= args.Count Then Return result
        result.Number = args(index)
        Dim name As String = ""
        For i As Integer = index + 1 To args.Count - 1
            If name <> "" Then name &= " "
            name &= args(i)
        Next
        result.Name = name.Trim()
        If result.Number = "-" Then result.Number = ""
        Return result
    End Function
    Public Sub ShowIncomingNumber(raw As String, contactName As String, incoming As Boolean)
        DIAL = raw.Replace("tel:", "").Replace("phone:", "").Replace("sip:", "").Replace("callto:", "").Replace("/", "").Replace("-", "").Trim()
        If DIAL = "" AndAlso contactName = "" Then Return
        TelNumberBox.Text = DIAL
        If incoming AndAlso contactName = "" Then
            SetContactName("تماس ورودی")
        Else
            SetContactName(contactName)
        End If
        ClearCallingState()
        SettingsPanel.Hide()
        CallDelay.Stop()
        CloseTimer.Stop()
        If incoming Then
            ShowIncomingRing()
        Else
            If AutoDialCheckBox.Checked = True Then CallDelay.Start()
            If AutoCloseCheckBox.Checked = True Then
                CTime = AutoCloseDelayBox.Value
                CloseTimer.Start()
            End If
            SmallMode()
        End If
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        Me.Activate()
        Me.BringToFront()
    End Sub
    Private Sub ShowIncomingRing()
        IncomingRinging = True
        EnsureCallLabels()
        CallStatusLabel.Visible = True
        CallStatusLabel.BackColor = Color.FromArgb(138, 90, 24)
        CallingPhrase = "تماس ورودی"
        CallStatusLabel.Text = CallingPhrase
        CallButton.Enabled = True
        CallButton.UseVisualStyleBackColor = False
        CallButton.FlatStyle = FlatStyle.Flat
        CallButton.BackColor = Color.FromArgb(27, 95, 88)
        CallButton.ForeColor = Color.White
        CallButton.Font = New Font("Tahoma", 9, FontStyle.Bold)
        CallButton.Text = "پاسخ"
        HangupButton.Visible = True
        HangupButton.Enabled = True
        HangupButton.Text = "رد کردن"
        StatusDots = 0
        If CallPulse Is Nothing Then
            CallPulse = New System.Windows.Forms.Timer()
            CallPulse.Interval = 450
            AddHandler CallPulse.Tick, AddressOf PulseCalling
        End If
        CallPulse.Start()
        If contactNameTitle() <> "" Then
            Me.Text = "تماس ورودی - " & ContactNameLabel.Text
        Else
            Me.Text = "تماس ورودی"
        End If
        Me.TopMost = True
        CloseHistory()
        RememberCall("ورودی", TelNumberBox.Text, HistoryName(), "زنگ")
        SmallMode()
    End Sub
    Private Function contactNameTitle() As String
        If ContactNameLabel Is Nothing Then Return ""
        If ContactNameLabel.Text = "تماس ورودی" Then Return ""
        Return ContactNameLabel.Text
    End Function
    Private Sub EnsureCallLabels()
        If ContactNameLabel IsNot Nothing Then Return
        ContactNameLabel = New Label With {
            .AutoSize = False,
            .Size = New Size(204, 36),
            .Font = New Font("Tahoma", 13, FontStyle.Bold),
            .ForeColor = Color.FromArgb(23, 35, 44),
            .BackColor = Color.FromArgb(214, 236, 232),
            .TextAlign = ContentAlignment.MiddleCenter,
            .AutoEllipsis = True,
            .Visible = False
        }
        HangupButton = New Button With {
            .Size = New Size(204, 32),
            .Font = New Font("Tahoma", 9, FontStyle.Bold),
            .Text = "قطع تماس",
            .BackColor = Color.FromArgb(154, 60, 72),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Visible = False
        }
        HangupButton.FlatAppearance.BorderSize = 0
        AddHandler HangupButton.Click, AddressOf HangupButton_Click
        CallStatusLabel = New Label With {
            .AutoSize = False,
            .Size = New Size(204, 28),
            .Font = New Font("Tahoma", 9, FontStyle.Bold),
            .ForeColor = Color.White,
            .BackColor = Color.FromArgb(27, 95, 88),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Visible = False
        }
        HoldButton = MakeToolButton("نگه", AddressOf HoldButton_Click)
        MuteButton = MakeToolButton("بی‌صدا", AddressOf MuteButton_Click)
        TransferButton = MakeToolButton("انتقال", AddressOf TransferButton_Click)
        ConfButton = MakeToolButton("کنفرانس", AddressOf ConfButton_Click)
        SpeakerButton = MakeToolButton("اسپیکر", AddressOf SpeakerButton_Click)
        HeadsetButton = MakeToolButton("هدست", AddressOf HeadsetButton_Click)
        DndButton = MakeToolButton("DND", AddressOf DndButton_Click)
        RedialButton = MakeToolButton("تکرار", AddressOf RedialButton_Click)
        HistoryButton = MakeToolButton("سابقه", AddressOf HistoryButton_Click)
        HistoryButton.Size = New Size(204, 28)
        Controls.Add(ContactNameLabel)
        Controls.Add(CallStatusLabel)
        Controls.Add(HangupButton)
        ContactNameLabel.BringToFront()
        CallStatusLabel.BringToFront()
        HangupButton.BringToFront()
    End Sub
    Private Function MakeToolButton(caption As String, handler As EventHandler) As Button
        Dim button As New Button With {
            .Size = New Size(100, 28),
            .Font = New Font("Tahoma", 8, FontStyle.Bold),
            .Text = caption,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(23, 48, 56),
            .ForeColor = Color.White,
            .Visible = False
        }
        button.FlatAppearance.BorderSize = 0
        AddHandler button.Click, handler
        Controls.Add(button)
        button.BringToFront()
        Return button
    End Function
    Private Sub PlaceToolButtons(ByRef y As Integer, Optional alwaysShow As Boolean = False)
        If DndButton Is Nothing Then Return
        Dim tools() As Button = {HoldButton, MuteButton, TransferButton, ConfButton, SpeakerButton, HeadsetButton}
        Dim showCall As Boolean = alwaysShow OrElse (HangupButton IsNot Nothing AndAlso HangupButton.Visible AndAlso Not IncomingRinging)
        If showCall Then
            For i As Integer = 0 To tools.Length - 1
                tools(i).Visible = True
                tools(i).Location = New Point(12 + (i Mod 2) * 104, y + (i \ 2) * 32)
                tools(i).Size = New Size(100, 28)
            Next
            y += 96
        Else
            For Each button In tools
                button.Visible = False
            Next
        End If
        DndButton.Visible = True
        RedialButton.Visible = True
        DndButton.Location = New Point(12, y)
        DndButton.Size = New Size(100, 28)
        RedialButton.Location = New Point(116, y)
        RedialButton.Size = New Size(100, 28)
        y += 34
        HistoryButton.Visible = True
        HistoryButton.Location = New Point(12, y)
        HistoryButton.Size = New Size(204, 28)
        y += 34
    End Sub
    Private Sub HideToolButtons()
        For Each button In New Button() {HoldButton, MuteButton, TransferButton, ConfButton, SpeakerButton, HeadsetButton, DndButton, RedialButton, HistoryButton}
            If button IsNot Nothing Then button.Visible = False
        Next
    End Sub
    Private Sub SetContactName(contactName As String)
        EnsureCallLabels()
        Dim clean As String = contactName.Replace(vbCr, " ").Replace(vbLf, " ").Trim()
        ContactNameLabel.Text = clean
        ContactNameLabel.Visible = clean <> ""
        If clean <> "" Then
            Me.Text = clean
        Else
            Me.Text = " Yealink Dialer"
        End If
    End Sub
    Private Sub BeginCallingState()
        If Not Visible Then Return
        Me.TopMost = False
        CloseHistory()
        If AnsweredFromRing Then
            AnsweredFromRing = False
            FinishLastCall("پاسخ داده")
        Else
            RememberCall("خروجی", TelNumberBox.Text, HistoryName(), "در حال تماس")
        End If
        EnsureCallLabels()
        CloseTimer.Stop()
        CallButton.Enabled = False
        CallButton.UseVisualStyleBackColor = False
        CallButton.FlatStyle = FlatStyle.Flat
        CallButton.BackColor = Color.FromArgb(27, 95, 88)
        CallButton.ForeColor = Color.White
        CallButton.Font = New Font("Tahoma", 9, FontStyle.Bold)
        CallStatusLabel.Visible = True
        CallStatusLabel.BackColor = Color.FromArgb(27, 95, 88)
        HangupButton.Visible = True
        HangupButton.Enabled = True
        HangupButton.Text = "قطع تماس"
        IncomingRinging = False
        SteadyStatus = False
        Held = False
        Muted = False
        If HoldButton IsNot Nothing Then HoldButton.Text = "نگه"
        If MuteButton IsNot Nothing Then MuteButton.Text = "بی‌صدا"
        StatusDots = 0
        If ContactNameLabel.Text <> "" AndAlso ContactNameLabel.Text <> "تماس ورودی" Then
            CallingPhrase = "در حال تماس با " & ContactNameLabel.Text
        Else
            CallingPhrase = "در حال تماس گرفتن"
        End If
        CallStatusLabel.Text = CallingPhrase
        CallButton.Text = "در حال تماس"
        If CallPulse Is Nothing Then
            CallPulse = New System.Windows.Forms.Timer()
            CallPulse.Interval = 450
            AddHandler CallPulse.Tick, AddressOf PulseCalling
        End If
        CallPulse.Start()
        SmallMode()
    End Sub
    Private Sub PulseCalling(sender As Object, e As EventArgs)
        If SteadyStatus Then Return
        StatusDots = (StatusDots + 1) Mod 4
        Dim dots As String = New String("."c, StatusDots)
        CallStatusLabel.Text = CallingPhrase & dots
        If IncomingRinging Then
            CallButton.Text = "پاسخ"
        Else
            CallButton.Text = "در حال تماس" & dots
        End If
    End Sub
    Private Sub ClearCallingState()
        IncomingRinging = False
        If CallPulse IsNot Nothing Then CallPulse.Stop()
        If CallStatusLabel IsNot Nothing Then CallStatusLabel.Visible = False
        If HangupButton IsNot Nothing Then
            HangupButton.Visible = False
            HangupButton.Text = "قطع تماس"
        End If
        CallButton.UseVisualStyleBackColor = True
        CallButton.FlatStyle = FlatStyle.Standard
        CallButton.ForeColor = SystemColors.ControlText
        CallButton.Font = DefaultFont
        CallButton.Text = "Call"
        CallButton.Enabled = TelNumberBox.Text <> ""
    End Sub
    Private Function SmallMode()
        EnsureCallLabels()
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
        Dim y As Integer = 8
        If ContactNameLabel.Visible Then
            ContactNameLabel.Location = New Point(12, y)
            y += 42
        End If
        TelNumberBox.Location = New Point(12, y)
        y += 34
        If CallStatusLabel.Visible Then
            CallStatusLabel.Location = New Point(12, y)
            y += 34
        End If
        If HangupButton.Visible Then
            HangupButton.Location = New Point(12, y)
            y += 38
        End If
        PlaceToolButtons(y)
        BottomPanel.Location = New Point(0, y)
        Me.ClientSize = New Size(229, y + BottomPanel.Height + 8)
    End Function
    Private Function NormalMode()
        Me.TopMost = False
        CloseHistory()
        CallDelay.Stop()
        CloseTimer.Stop()
        TelNumberBox.Location = New Point(12, 12)
        If ContactNameLabel IsNot Nothing Then ContactNameLabel.Visible = False
        If CallStatusLabel IsNot Nothing Then CallStatusLabel.Visible = False
        If HangupButton IsNot Nothing Then HangupButton.Visible = False
        EnsureCallLabels()
        Dim y As Integer = 324
        PlaceToolButtons(y, True)
        BottomPanel.Location = New Point(0, y)
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
        Me.ClientSize = New Size(229, y + BottomPanel.Height + 8)
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
    Private Function LoadConfigFile(ByVal FileToLoad As String)
        'Load config and configure app with their data.

        'Create temporary variables used for clearer code structuring.
        Dim AutoCloseTemp As String = "True"
        Dim AutoDialTemp As String = "False"
        Dim PWRevealLockTemp As String = "True"
        Dim UseSSLTemp As String = "True"
        Dim SkipCertCheckTemp As String = "False"
        Dim TryErrorCount As Integer = 0
        Dim TryErrorLog As Array = {TryErrorCount, "", True}
        Dim HasMigrated As Boolean = False
        Try
            'Load application settings.
            Dim PhoneContent As String = My.Computer.FileSystem.ReadAllText(FileToLoad)
            TryErrorLog = {TryErrorCount, "Past file load", True} : TryErrorCount += 1

            'Decrypt configuration
            Dim rd As New RijndaelManaged
            Dim rijndaelIvLength As Integer = 16
            Dim md5 As New MD5CryptoServiceProvider
            Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(My.Computer.FileSystem.ReadAllText(Application.StartupPath() & "\data\secret.cfg")))
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

            'Check if config file is older than the min version verification and migrate it.
            If PhoneConfigBox.Lines(0).Contains("PHONEIP") Then
                PhoneConfigBox.Text = "MINVERSION=" & MINIMUMVERSION(0) & "." & MINIMUMVERSION(1) & vbNewLine & PhoneConfigBox.Text
                MessageBox.Show("Settings have been imported from older version." & vbNewLine & "Please check and save your settings with the current version.", "Migration notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                HasMigrated = True
            End If

            'Define temporary variables for version check.
            Dim MinV0Temp As Integer = PhoneConfigBox.Lines(0).Replace("MINVERSION=", "").Split(".")(0)
            Dim MinV1Temp As Integer = PhoneConfigBox.Lines(0).Replace("MINVERSION=", "").Split(".")(1)

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

            TryErrorLog = {TryErrorCount, "Past min version check.", True} : TryErrorCount += 1

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
            'Check if config file exists, if not then show first launch Setup Wizard else handle load error normally.
            If My.Computer.FileSystem.FileExists(Application.StartupPath() & "\data\phone.cfg") Then
                If TryErrorLog(2) = True Then MsgBox("Loading state: " & TryErrorLog(0) & " - " & TryErrorLog(1) & vbNewLine & ex.Message)
                OpenSettings()
                Exit Function
            Else
                OpenSettings()
                TriggerSetupWizard()
                Exit Function
            End If
        End Try
        TryErrorLog = {TryErrorCount, "Past main Try/Catch block", True} : TryErrorCount += 1

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
        TryErrorLog = {TryErrorCount, "Past ui state config.", True} : TryErrorCount += 1

        If HasMigrated = True Then
            OpenSettings()
        End If
    End Function

    Private Function SaveConfigFile()
        AssembleConfigFile()

        Dim dataDir As String = Application.StartupPath() & "\data"
        If Not My.Computer.FileSystem.DirectoryExists(dataDir) Then
            My.Computer.FileSystem.CreateDirectory(dataDir)
        End If

        'Generate new random crypto secret and save it.
        CFGSecret = RandomString(250, 256)
        Using writer As New System.IO.StreamWriter(dataDir & "\secret.cfg")
            For Each line In "A"
                writer.Write(CFGSecret)
            Next
        End Using

        'Encrypt config with secret.
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
        Using writer As New System.IO.StreamWriter(dataDir & "\phone.cfg")
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
    Private Sub SendKeyQuiet(keyQuery As String)
        SendPhoneKey(keyQuery, False, APIUSERNAME, APIPASSWORD, PHONEIP, USESSL, SKIPCERTCHECK, False)
    End Sub
    Private Sub SetSteadyStatus(text As String, color As Color)
        SteadyStatus = True
        If CallPulse IsNot Nothing Then CallPulse.Stop()
        EnsureCallLabels()
        CallStatusLabel.Visible = True
        CallStatusLabel.BackColor = color
        CallStatusLabel.Text = text
        CallingPhrase = text
        If Not Dial1.Visible Then SmallMode()
    End Sub
    Private Sub ShowActiveCall(statusText As String, statusColor As Color)
        IncomingRinging = False
        CloseTimer.Stop()
        CallDelay.Stop()
        SettingsPanel.Hide()
        SetSteadyStatus(statusText, statusColor)
        HangupButton.Visible = True
        HangupButton.Enabled = True
        HangupButton.Text = "قطع تماس"
        CallButton.Enabled = False
        CallButton.UseVisualStyleBackColor = False
        CallButton.FlatStyle = FlatStyle.Flat
        CallButton.BackColor = statusColor
        CallButton.ForeColor = Color.White
        CallButton.Font = New Font("Tahoma", 9, FontStyle.Bold)
        CallButton.Text = statusText
        SmallMode()
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        Me.Activate()
        Me.BringToFront()
    End Sub
    Public Sub ApplyPhoneEvent(eventName As String, number As String, contactName As String)
        If number <> "" Then
            DIAL = number
            TelNumberBox.Text = number
        End If
        If contactName <> "" Then SetContactName(contactName)
        Dim activeColor = Color.FromArgb(27, 95, 88)
        Dim waitColor = Color.FromArgb(138, 90, 24)
        Dim stopColor = Color.FromArgb(154, 60, 72)
        Select Case eventName
            Case "established"
                Held = False
                If HoldButton IsNot Nothing Then HoldButton.Text = "نگه"
                If Not FinishLastCall("برقرار") Then RememberCall("خروجی", number, contactName, "برقرار")
                ShowActiveCall("تماس برقرار شد", activeColor)
            Case "outgoing"
                If Not FinishLastCall("در حال تماس") Then RememberCall("خروجی", number, contactName, "در حال تماس")
                ShowActiveCall("تماس خروجی", activeColor)
            Case "terminated"
                FinishLastCall("قطع شد")
                MarkCallEnded()
            Case "missed"
                If Not FinishLastCall("بی‌پاسخ") Then RememberCall("ورودی", number, contactName, "بی‌پاسخ")
                If contactName = "" AndAlso (ContactNameLabel Is Nothing OrElse ContactNameLabel.Text = "") Then SetContactName("تماس بی‌پاسخ")
                HangupButton.Visible = False
                IncomingRinging = False
                SetSteadyStatus("تماس بی‌پاسخ", stopColor)
                CallButton.Text = "Call"
                CallButton.UseVisualStyleBackColor = True
                CallButton.Enabled = TelNumberBox.Text <> ""
                SettingsPanel.Hide()
                SmallMode()
                Me.Show()
                Me.Activate()
                Me.BringToFront()
            Case "hold"
                Held = True
                If HoldButton IsNot Nothing Then HoldButton.Text = "ادامه"
                ShowActiveCall("تماس در انتظار", waitColor)
            Case "unhold"
                Held = False
                If HoldButton IsNot Nothing Then HoldButton.Text = "نگه"
                ShowActiveCall("تماس برقرار شد", activeColor)
            Case "mute"
                Muted = True
                If MuteButton IsNot Nothing Then MuteButton.Text = "صدا"
                ShowActiveCall("بی‌صدا", waitColor)
            Case "unmute"
                Muted = False
                If MuteButton IsNot Nothing Then MuteButton.Text = "بی‌صدا"
                ShowActiveCall("تماس برقرار شد", activeColor)
            Case "dnd-on"
                DndEnabled = True
                If DndButton IsNot Nothing Then
                    DndButton.Text = "DND روشن"
                    DndButton.BackColor = stopColor
                End If
                SetSteadyStatus("مزاحم نشوید روشن شد", stopColor)
                SmallMode()
                Me.Show()
                Me.Activate()
            Case "dnd-off"
                DndEnabled = False
                If DndButton IsNot Nothing Then
                    DndButton.Text = "DND"
                    DndButton.BackColor = Color.FromArgb(23, 48, 56)
                End If
                SetSteadyStatus("مزاحم نشوید خاموش شد", activeColor)
                SmallMode()
                Me.Show()
                Me.Activate()
        End Select
    End Sub
    Private Sub HoldButton_Click(sender As Object, e As EventArgs)
        If Held Then
            SendKeyQuiet("UNHOLD")
            Held = False
            HoldButton.Text = "نگه"
            ShowActiveCall("تماس برقرار شد", Color.FromArgb(27, 95, 88))
        Else
            SendKeyQuiet("HOLD")
            Held = True
            HoldButton.Text = "ادامه"
            ShowActiveCall("تماس در انتظار", Color.FromArgb(138, 90, 24))
        End If
    End Sub
    Private Sub MuteButton_Click(sender As Object, e As EventArgs)
        SendKeyQuiet("MUTE")
        Muted = Not Muted
        If Muted Then
            MuteButton.Text = "صدا"
            SetSteadyStatus("بی‌صدا", Color.FromArgb(138, 90, 24))
        Else
            MuteButton.Text = "بی‌صدا"
            SetSteadyStatus("صدا وصل شد", Color.FromArgb(27, 95, 88))
        End If
    End Sub
    Private Sub TransferButton_Click(sender As Object, e As EventArgs)
        Dim target As String = DigitsOnly(InputBox("شماره مقصد انتقال را وارد کنید", "انتقال تماس", ""))
        If target = "" Then Return
        SendKeyQuiet("BTrans=" & target)
        SetSteadyStatus("در حال انتقال به " & target, Color.FromArgb(27, 95, 88))
    End Sub
    Private Sub ConfButton_Click(sender As Object, e As EventArgs)
        SendKeyQuiet("F_CONFERENCE")
        SetSteadyStatus("کنفرانس", Color.FromArgb(27, 95, 88))
    End Sub
    Private Sub SpeakerButton_Click(sender As Object, e As EventArgs)
        SendKeyQuiet("SPEAKER")
        SetSteadyStatus("اسپیکر", Color.FromArgb(27, 95, 88))
    End Sub
    Private Sub HeadsetButton_Click(sender As Object, e As EventArgs)
        SendKeyQuiet("HEADSET")
        SetSteadyStatus("هدست", Color.FromArgb(27, 95, 88))
    End Sub
    Private Sub DndButton_Click(sender As Object, e As EventArgs)
        If DndEnabled Then
            SendKeyQuiet("DNDOff")
            DndEnabled = False
            DndButton.Text = "DND"
            DndButton.BackColor = Color.FromArgb(23, 48, 56)
            SetSteadyStatus("مزاحم نشوید خاموش شد", Color.FromArgb(27, 95, 88))
        Else
            SendKeyQuiet("DNDOn")
            DndEnabled = True
            DndButton.Text = "DND روشن"
            DndButton.BackColor = Color.FromArgb(154, 60, 72)
            SetSteadyStatus("مزاحم نشوید روشن شد", Color.FromArgb(154, 60, 72))
        End If
    End Sub
    Private Sub RedialButton_Click(sender As Object, e As EventArgs)
        SendPhoneKey("RD", False, APIUSERNAME, APIPASSWORD, PHONEIP, USESSL, SKIPCERTCHECK, True)
    End Sub
    Private Function DigitsOnly(raw As String) As String
        Dim cleaned As String = ""
        For Each ch As Char In raw
            If Char.IsDigit(ch) OrElse ch = "+"c OrElse ch = "*"c OrElse ch = "#"c Then cleaned &= ch
        Next
        Return cleaned
    End Function
    Private Function HistoryName() As String
        If ContactNameLabel Is Nothing Then Return ""
        Dim text As String = ContactNameLabel.Text.Trim()
        If text = "تماس ورودی" OrElse text = "تماس بی‌پاسخ" Then Return ""
        Return text
    End Function
    Private Function HistoryFile() As String
        Return Application.StartupPath() & "\data\call-history.txt"
    End Function
    Private Sub LoadCallHistory()
        HistoryLines.Clear()
        Try
            Dim path As String = HistoryFile()
            If Not My.Computer.FileSystem.FileExists(path) Then Return
            For Each line In My.Computer.FileSystem.ReadAllText(path, Encoding.UTF8).Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)
                HistoryLines.Add(line)
            Next
        Catch
        End Try
    End Sub
    Private Sub SaveCallHistory()
        Try
            Dim dataDir As String = Application.StartupPath() & "\data"
            If Not My.Computer.FileSystem.DirectoryExists(dataDir) Then My.Computer.FileSystem.CreateDirectory(dataDir)
            Dim kept As Integer = Math.Min(200, HistoryLines.Count)
            Dim body As String = String.Join(vbCrLf, HistoryLines.GetRange(0, kept).ToArray())
            My.Computer.FileSystem.WriteAllText(HistoryFile(), body, False, Encoding.UTF8)
        Catch
        End Try
    End Sub
    Private Sub RememberCall(direction As String, number As String, contactName As String, result As String)
        Dim cleanName As String = If(contactName, "").Replace("|", " ").Replace(vbCr, " ").Replace(vbLf, " ").Trim()
        Dim cleanNumber As String = If(number, "").Replace("|", "")
        HistoryLines.Insert(0, DateTime.Now.ToString("yyyy/MM/dd HH:mm") & "|" & direction & "|" & cleanNumber & "|" & cleanName & "|" & result)
        If HistoryLines.Count > 200 Then HistoryLines.RemoveAt(HistoryLines.Count - 1)
        SaveCallHistory()
        RefreshHistoryList()
    End Sub
    Private Function FinishLastCall(result As String) As Boolean
        If HistoryLines.Count = 0 Then Return False
        Dim parts() As String = HistoryLines(0).Split("|"c)
        If parts.Length < 5 Then Return False
        Dim stillOpen As Boolean = parts(4) = "زنگ" OrElse parts(4) = "در حال تماس" OrElse parts(4) = "پاسخ داده" OrElse parts(4) = "برقرار"
        If Not stillOpen Then Return False
        parts(4) = result
        HistoryLines(0) = String.Join("|", parts)
        SaveCallHistory()
        RefreshHistoryList()
        Return True
    End Function
    Private Sub EnsureHistoryPanel()
        If HistoryPanel IsNot Nothing Then Return
        HistoryList = New ListView With {
            .Dock = DockStyle.Fill,
            .View = View.Details,
            .FullRowSelect = True,
            .GridLines = True,
            .Font = New Font("Tahoma", 9),
            .RightToLeft = RightToLeft.Yes,
            .RightToLeftLayout = True,
            .HeaderStyle = ColumnHeaderStyle.Nonclickable
        }
        HistoryList.Columns.Add("زمان", 130)
        HistoryList.Columns.Add("نوع", 70)
        HistoryList.Columns.Add("شماره", 120)
        HistoryList.Columns.Add("نام", 120)
        HistoryList.Columns.Add("وضعیت", 90)
        AddHandler HistoryList.DoubleClick, AddressOf HistoryList_DoubleClick
        Dim back As New Button With {
            .Dock = DockStyle.Top,
            .Height = 32,
            .Text = "بازگشت به شماره‌گیر",
            .Font = New Font("Tahoma", 9, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(23, 48, 56),
            .ForeColor = Color.White
        }
        back.FlatAppearance.BorderSize = 0
        AddHandler back.Click, AddressOf HistoryBack_Click
        HistoryPanel = New Panel With {
            .Visible = False,
            .BackColor = Color.White
        }
        HistoryPanel.Controls.Add(HistoryList)
        HistoryPanel.Controls.Add(back)
        Controls.Add(HistoryPanel)
    End Sub
    Private Sub RefreshHistoryList()
        If HistoryList Is Nothing Then Return
        HistoryList.Items.Clear()
        For Each line In HistoryLines
            Dim parts() As String = line.Split("|"c)
            If parts.Length < 5 Then Continue For
            Dim item As New ListViewItem(parts(0))
            item.SubItems.Add(parts(1))
            item.SubItems.Add(parts(2))
            item.SubItems.Add(parts(3))
            item.SubItems.Add(parts(4))
            HistoryList.Items.Add(item)
        Next
    End Sub
    Private Sub HistoryButton_Click(sender As Object, e As EventArgs)
        ShowCallHistory()
    End Sub
    Private Sub ShowCallHistory()
        EnsureHistoryPanel()
        For Each pad In New Button() {Dial1, Dial2, Dial3, Dial4, Dial5, Dial6, Dial7, Dial8, Dial9, Dial0, DialPlus, DialErase}
            pad.Hide()
        Next
        HideToolButtons()
        Dim top As Integer = 46
        TelNumberBox.Location = New Point(12, 12)
        If HangupButton IsNot Nothing AndAlso HangupButton.Visible Then
            HangupButton.Location = New Point(12, 46)
            top = 84
        End If
        HistoryPanel.Location = New Point(0, top)
        HistoryPanel.Size = New Size(540, 300)
        HistoryPanel.Visible = True
        HistoryPanel.BringToFront()
        BottomPanel.Location = New Point(0, top + 300)
        Me.ClientSize = New Size(540, top + 300 + BottomPanel.Height + 8)
        RefreshHistoryList()
    End Sub
    Private Sub CloseHistory()
        If HistoryPanel IsNot Nothing Then HistoryPanel.Visible = False
    End Sub
    Private Sub HistoryBack_Click(sender As Object, e As EventArgs)
        Dim restoreCall As Boolean = HangupButton IsNot Nothing AndAlso HangupButton.Visible
        CloseHistory()
        If restoreCall Then
            SmallMode()
        Else
            NormalMode()
        End If
    End Sub
    Private Sub HistoryList_DoubleClick(sender As Object, e As EventArgs)
        If HistoryList.SelectedItems.Count = 0 Then Return
        TelNumberBox.Text = HistoryList.SelectedItems(0).SubItems(2).Text
        HistoryBack_Click(sender, e)
    End Sub
    Private Sub HangupButton_Click(sender As Object, e As EventArgs)
        HangupButton.Enabled = False
        Dim rejectIncoming As Boolean = IncomingRinging
        IncomingRinging = False
        If CallPulse IsNot Nothing Then CallPulse.Stop()
        If rejectIncoming Then
            FinishLastCall("رد شده")
            'T31P rejects an incoming call with X. CANCEL is the backup.
            SendPhoneKey("X", False, APIUSERNAME, APIPASSWORD, PHONEIP, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked, False, True)
            SendPhoneKey("CANCEL", False, APIUSERNAME, APIPASSWORD, PHONEIP, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked, False, True)
        Else
            FinishLastCall("قطع شد")
            SendPhoneKey("CALLEND", False, APIUSERNAME, APIPASSWORD, PHONEIP, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked, False, True)
            SendPhoneKey("ONHOOK", False, APIUSERNAME, APIPASSWORD, PHONEIP, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked, False, True)
        End If
        MarkCallEnded()
    End Sub
    Private Sub MarkCallEnded()
        If CallPulse IsNot Nothing Then CallPulse.Stop()
        SteadyStatus = False
        Held = False
        Muted = False
        IncomingRinging = False
        If HoldButton IsNot Nothing Then HoldButton.Text = "نگه"
        If MuteButton IsNot Nothing Then MuteButton.Text = "بی‌صدا"
        If Not Visible Then Return
        CallButton.UseVisualStyleBackColor = True
        CallButton.FlatStyle = FlatStyle.Standard
        CallButton.ForeColor = SystemColors.ControlText
        CallButton.Font = DefaultFont
        CallButton.Text = "Call"
        CallButton.Enabled = TelNumberBox.Text <> ""
        Me.Text = " Yealink Dialer"
        NormalMode()
    End Sub
    Private Sub RegisterPhoneActionUrls()
        Dim worker As New Thread(AddressOf RegisterPhoneActionUrlsWorker)
        worker.IsBackground = True
        worker.Start()
    End Sub
    Private Sub RegisterPhoneActionUrlsWorker()
        Try
            Dim phoneAddress As String = Me.PHONEIP
            If phoneAddress Is Nothing Then phoneAddress = ""
            Dim cfg As String = DownloadText("http://127.0.0.1:8080/api/yealink-action.cfg")
            If cfg = "" OrElse phoneAddress = "" Then
                WriteActionLog("skipped cfgLen=" & cfg.Length.ToString() & " ipLen=" & phoneAddress.Length.ToString())
                Return
            End If
            Dim loginNote As String = ""
            Dim cookie As String = PhoneLoginCookie(phoneAddress, Me.APIUSERNAME, Me.APIPASSWORD, loginNote)
            Dim status As String = "login=" & loginNote & " " & UploadPhoneCfg("http", phoneAddress, Me.APIUSERNAME, Me.APIPASSWORD, cfg, True, cookie)
            WriteActionLog(status)
        Catch ex As Exception
            WriteActionLog(ex.GetType().Name & " " & ex.Message)
        End Try
    End Sub
    Private Function DownloadText(url As String) As String
        Dim request As HttpWebRequest = DirectCast(HttpWebRequest.Create(url), HttpWebRequest)
        request.Method = "GET"
        request.Timeout = 3000
        request.ReadWriteTimeout = 3000
        Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            Using reader As New StreamReader(response.GetResponseStream(), Encoding.UTF8)
                Return reader.ReadToEnd()
            End Using
        End Using
    End Function
    Private Function PhoneLoginCookie(phoneIp As String, user As String, password As String, ByRef note As String) As String
        Dim form As String = "username=" & Uri.EscapeDataString(user) & "&pwd=" & Uri.EscapeDataString(password)
        Dim cookie As String = ""
        note = RawPhoneExchange(phoneIp, "POST", "/servlet?m=mod_listener&p=login&q=login", "application/x-www-form-urlencoded", Encoding.ASCII.GetBytes(form), "", cookie)
        If password <> "" Then note = note.Replace(password, "***")
        Return cookie
    End Function
    Private Function UploadPhoneCfg(scheme As String, phoneIp As String, user As String, password As String, cfg As String, skipCert As Boolean, Optional cookie As String = "") As String
        Dim plain As String = RawPhonePost(phoneIp, "/servlet?m=mod_data&p=settings-upload", "application/octet-stream", Encoding.UTF8.GetBytes(cfg), user, password, cookie)
        If plain.StartsWith("200") OrElse plain.StartsWith("201") Then Return "ok plain " & plain
        Dim boundary As String = "----yealinkcfg"
        Dim body As String = "--" & boundary & vbCrLf &
            "Content-Disposition: form-data; name=""file""; filename=""action.cfg""" & vbCrLf &
            "Content-Type: text/plain" & vbCrLf & vbCrLf &
            cfg & vbCrLf &
            "--" & boundary & "--" & vbCrLf
        Dim multi As String = RawPhonePost(phoneIp, "/servlet?m=mod_data&p=settings-upload", "multipart/form-data; boundary=" & boundary, Encoding.UTF8.GetBytes(body), user, password, cookie)
        If multi.StartsWith("200") OrElse multi.StartsWith("201") Then Return "ok multipart " & multi
        Return "fail plain=" & plain & " multipart=" & multi
    End Function
    Private Function RawPhoneGet(phoneIp As String, path As String, user As String, password As String) As String
        Try
            Dim auth As String = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(user & ":" & password))
            Dim header As String = "GET " & path & " HTTP/1.0" & vbCrLf &
                "Host: " & phoneIp & vbCrLf &
                "Authorization: Basic " & auth & vbCrLf &
                "Connection: close" & vbCrLf & vbCrLf
            Using client As New System.Net.Sockets.TcpClient()
                client.SendTimeout = 4000
                client.ReceiveTimeout = 4000
                client.Connect(phoneIp, 80)
                Using stream = client.GetStream()
                    Dim headBytes() As Byte = Encoding.ASCII.GetBytes(header)
                    stream.Write(headBytes, 0, headBytes.Length)
                    stream.Flush()
                    Using reader As New StreamReader(stream, Encoding.UTF8)
                        Return reader.ReadToEnd()
                    End Using
                End Using
            End Using
        Catch
            Return ""
        End Try
    End Function
    Private Function RawPhonePost(phoneIp As String, path As String, contentType As String, payload() As Byte, user As String, password As String, Optional cookie As String = "") As String
        Dim ignored As String = ""
        Return RawPhoneExchange(phoneIp, "POST", path, contentType, payload, cookie, ignored, True, user, password)
    End Function
    Private Function RawPhoneExchange(phoneIp As String, method As String, path As String, contentType As String, payload() As Byte, cookie As String, ByRef responseCookie As String, Optional useBasic As Boolean = False, Optional user As String = "", Optional password As String = "") As String
        Try
            Dim header As String = method & " " & path & " HTTP/1.0" & vbCrLf &
                "Host: " & phoneIp & vbCrLf
            If useBasic AndAlso user <> "" Then
                Dim auth As String = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(user & ":" & password))
                header &= "Authorization: Basic " & auth & vbCrLf
            End If
            If cookie <> "" Then header &= "Cookie: " & cookie & vbCrLf
            header &= "Content-Type: " & contentType & vbCrLf &
                "Content-Length: " & payload.Length.ToString() & vbCrLf &
                "Connection: close" & vbCrLf & vbCrLf
            Using client As New System.Net.Sockets.TcpClient()
                client.SendTimeout = 4000
                client.ReceiveTimeout = 4000
                client.Connect(phoneIp, 80)
                Using stream = client.GetStream()
                    Dim headBytes() As Byte = Encoding.ASCII.GetBytes(header)
                    stream.Write(headBytes, 0, headBytes.Length)
                    stream.Write(payload, 0, payload.Length)
                    stream.Flush()
                    Using reader As New StreamReader(stream, Encoding.ASCII)
                        Dim status As String = reader.ReadLine()
                        If status Is Nothing Then status = "empty"
                        Dim code As String = "0"
                        Dim parts() As String = status.Split(" "c)
                        If parts.Length > 1 Then code = parts(1)
                        responseCookie = ""
                        Dim location As String = ""
                        Dim line As String = reader.ReadLine()
                        Do While line IsNot Nothing AndAlso line <> ""
                            If line.StartsWith("Location:", StringComparison.OrdinalIgnoreCase) Then location = " " & line
                            If line.StartsWith("Set-Cookie:", StringComparison.OrdinalIgnoreCase) Then
                                Dim value As String = line.Substring("Set-Cookie:".Length).Trim()
                                Dim semi As Integer = value.IndexOf(";"c)
                                If semi > 0 Then value = value.Substring(0, semi)
                                If responseCookie <> "" Then responseCookie &= "; "
                                responseCookie &= value
                            End If
                            line = reader.ReadLine()
                        Loop
                        Dim rawRest As String = reader.ReadToEnd()
                        Dim rest As String = rawRest.ToLower()
                        Dim mark As String = ""
                        If rest.Contains("invalid") Then mark &= " invalid"
                        If rest.Contains("error") Then mark &= " error"
                        If rest.Contains("success") Then mark &= " success"
                        If responseCookie <> "" Then mark &= " cookie=yes"
                        If rawRest.Length < 240 Then mark &= " body=" & rawRest.Replace(vbCr, " ").Replace(vbLf, " ")
                        Return code & " " & status & location & mark & " blen=" & rest.Length.ToString()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return ex.GetType().Name
        End Try
    End Function
    Private Sub WriteActionLog(message As String)
        Try
            Dim dataDir As String = Application.StartupPath() & "\data"
            If Not My.Computer.FileSystem.DirectoryExists(dataDir) Then My.Computer.FileSystem.CreateDirectory(dataDir)
            My.Computer.FileSystem.WriteAllText(dataDir & "\action-url.log", DateTime.Now.ToString("HH:mm:ss") & " " & message & vbCrLf, True)
        Catch
        End Try
    End Sub
    Private Function SendRequestToPhone(ByVal SENDWEBUSER As String, ByVal SENDWEBPASSWORD As String, ByVal SENDPHONEIP As String, ByVal SENDDIAL As String, ByVal SENDSIPACCOUNT As String, ByVal SENDUSEHTTPS As Boolean, ByVal SENDSKIPSSLCHECK As Boolean)
        SendPhoneKey("number=" & SENDDIAL & "&outgoing_uri=" & SENDSIPACCOUNT, False, SENDWEBUSER, SENDWEBPASSWORD, SENDPHONEIP, SENDUSEHTTPS, SENDSKIPSSLCHECK)
    End Function
    Private Sub SendPhoneKey(keyQuery As String, hangup As Boolean, SENDWEBUSER As String, SENDWEBPASSWORD As String, SENDPHONEIP As String, SENDUSEHTTPS As Boolean, SENDSKIPSSLCHECK As Boolean, Optional refreshCallUi As Boolean = True, Optional quiet As Boolean = False)
        'The phone usually acts before it answers this request, so do not block the window on it.
        If refreshCallUi AndAlso (Not hangup) AndAlso Visible Then BeginInvoke(New Action(AddressOf BeginCallingState))
        Dim worker As New Thread(
            Sub()
                Try
                    Dim encoding As New UTF8Encoding
                    Dim USEHTTPS As String = ""
                    If SENDUSEHTTPS = True Then USEHTTPS = "s"
                    Dim makeapirequest As HttpWebRequest = DirectCast(HttpWebRequest.Create("http" & USEHTTPS & "://" & SENDPHONEIP & "/servlet?key=" & keyQuery), HttpWebRequest)
                    makeapirequest.Method = "GET"
                    makeapirequest.Timeout = 4000
                    makeapirequest.ReadWriteTimeout = 4000
                    makeapirequest.KeepAlive = False
                    makeapirequest.UserAgent = "YealinkDialer"
                    makeapirequest.ContentType = "application/json"

                    If SENDSKIPSSLCHECK = True Then
                        makeapirequest.ServerCertificateValidationCallback = Function(s, c, h, e) True
                    End If

                    makeapirequest.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(encoding.GetEncoding("ISO-8859-1").GetBytes(SENDWEBUSER + ":" + SENDWEBPASSWORD)))

                    Using postresponse As HttpWebResponse = DirectCast(makeapirequest.GetResponse(), HttpWebResponse)
                        Using postreqreader As New StreamReader(postresponse.GetResponseStream())
                            postreqreader.ReadToEnd()
                        End Using
                    End Using
                    If hangup Then
                        Try
                            BeginInvoke(New Action(AddressOf MarkCallEnded))
                        Catch
                        End Try
                    End If
                Catch ex As WebException
                    'Yealink often acts and then never returns an HTTP body in time.
                    If ex.Status = WebExceptionStatus.Timeout OrElse ex.Status = WebExceptionStatus.RequestCanceled Then
                        If hangup Then
                            Try
                                BeginInvoke(New Action(AddressOf MarkCallEnded))
                            Catch
                            End Try
                        End If
                        Return
                    End If
                    If Not quiet Then ReportDialError(ex.Message)
                Catch ex As Exception
                    If Not quiet Then ReportDialError(ex.Message)
                End Try
            End Sub)
        worker.IsBackground = True
        worker.Start()
    End Sub
    Private Sub ReportDialError(message As String)
        Dim showError As Action = Sub()
                                      ClearCallingState()
                                      MsgBox(message)
                                  End Sub
        Try
            If IsHandleCreated AndAlso Not IsDisposed Then
                BeginInvoke(showError)
                Return
            End If
        Catch
        End Try
        showError()
    End Sub
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
            CloseTimer.Stop()
            If IncomingRinging Then
                IncomingRinging = False
                AnsweredFromRing = True
                SendPhoneKey("ANSWER", False, APIUSERNAME, APIPASSWORD, PHONEIP, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked)
            Else
                DIAL = TelNumberBox.Text
                SendRequestToPhone(APIUSERNAME, APIPASSWORD, PHONEIP, DIAL, SIPACCOUNT, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked)
            End If
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
        SaveSettings()
    End Sub
    Private Function SaveSettings()
        'Called to save new settings and updated varibales to use them in the current app session.
        SaveConfigFile()
        SettingsPanel.Hide()
        WebPasswordBox.UseSystemPasswordChar = True
        PictureBox1.BackgroundImage = My.Resources.EyeGrayedOut

        'Set new vairables
        PHONEIP = PhoneConfigBox.Lines(1).Replace("PHONEIP=", "")
        SIPACCOUNT = PhoneConfigBox.Lines(2).Replace("SIPACCOUNT=", "")
        APIUSERNAME = PhoneConfigBox.Lines(3).Replace("WEBUSERNAME=", "")
        APIPASSWORD = PhoneConfigBox.Lines(4).Replace("WEBPASSWORD=", "")
        USESSL = UseSSLCheckbox.Checked
        SKIPCERTCHECK = SkipCertCheckCheckbox.Checked
        IPLabel.Text = "IP: " & PHONEIP
        NormalMode()
    End Function
    Private Sub SettingsLabel_Click(sender As Object, e As EventArgs) Handles SettingsLabel.Click
        OpenSettings()
    End Sub
    Private Function OpenSettings()
        'Configure UI and show the settings page.
        PWRevealStateConfigure()
        NormalMode()
        HideToolButtons()
        BottomPanel.Location = New Point(0, 324)
        Me.ClientSize = New Size(229, 381)
        SettingsPanel.Size = New Size(230, 381)
        SettingsPanel.Show()
        SettingsPanel.BringToFront()
    End Function
    Private Sub SetupWizardLabel_Click(sender As Object, e As EventArgs) Handles SetupWizardLabel.Click
        TriggerSetupWizard()
    End Sub
    Private Function TriggerSetupWizard()
        Me.Hide()
        Dim result = SetupWizard.ShowDialog
        If result = DialogResult.OK Then
            'SetupWizard was completed successfully.
            PWShowLockCheckbox.Checked = SetupWizard.PWShowLockCheckbox.Checked
            PWRevealLock = PWShowLockCheckbox.Checked
            PhoneIPBox.Text = SetupWizard.PhoneIPBox.Text
            SipAccountBox.Text = SetupWizard.SipAccountBox.Text
            WebUsernameBox.Text = SetupWizard.WebUsernameBox.Text
            WebPasswordBox.Text = SetupWizard.WebPasswordBox.Text
            UseSSLCheckbox.Checked = SetupWizard.UseSSLCheckbox.Checked
            SkipCertCheckCheckbox.Checked = SetupWizard.SkipCertCheckCheckbox.Checked
            AutoCloseCheckBox.Checked = SetupWizard.AutoCloseCheckBox.Checked
            AutoCloseDelayBox.Value = SetupWizard.AutoCloseDelayBox.Value
            AutoDialCheckBox.Checked = SetupWizard.AutoDialCheckBox.Checked
            AutoDialDelayBox.Value = SetupWizard.AutoDialDelayBox.Value
            'SaveSettings()
        Else
            'SetupWizard was aborted.
            MsgBox("Wizard was aborted." & vbNewLine & "Nothing was saved.", MsgBoxStyle.Exclamation)
        End If
        SetupWizard.ResetWizard()
        OpenSettings()
        Me.Show()
    End Function
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
#End Region
#End Region
End Class