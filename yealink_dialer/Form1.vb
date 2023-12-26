Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports System.Management
Public Class Form1
    Inherits System.Windows.Forms.Form

    Dim CFGSecret As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath() & "\data\secret.cfg")

    Dim DIAL As String
    Dim PHONEIP As String
    Dim SIPACCOUNT As String
    Dim USERNAME As String
    Dim PASSWORD As String
    Dim CTime As Integer = 8
    Dim CRDTA As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Load application settings.
        Dim ConfigContent As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath() & "\data\config.ini")
        RichTextBox2.Text = ConfigContent
        CTime = RichTextBox2.Lines(1).Replace("closedelay=", "") - 1
        CallDelay.Interval = RichTextBox2.Lines(3).Replace("dialdelay=", "") & "000"

        'Load phone configuration file and decrypt it.
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
            RichTextBox1.Text = CRDTA

        Catch ex As Exception

        Finally
        End Try

        'Configure parameters
        PHONEIP = RichTextBox1.Lines(0).Replace("PHONEIP=", "")
        SIPACCOUNT = RichTextBox1.Lines(1).Replace("SIPACCOUNT=", "")
        USERNAME = RichTextBox1.Lines(2).Replace("USERNAME=", "")
        PASSWORD = RichTextBox1.Lines(3).Replace("PASSWORD=", "")



        'Grab command line args and sanitize them.
        Try
            DIAL = My.Application.CommandLineArgs(0).Replace("tel:", "")
            DIAL = DIAL.Replace("/", "")
            DIAL = DIAL.Replace("-", "")
            Label1.Text = "Calling: " & DIAL

            'Start timers depending on config file.
            If RichTextBox2.Lines(0).Replace("autoclose=", "") = "true" Then CloseTimer.Start()
            If RichTextBox2.Lines(2).Replace("autodial=", "") = "true" Then CallDelay.Start()
        Catch
            Label1.Text = ""
            Button2.Show()
            Button1.Text = "Save"
            RichTextBox1.Show()
            DIAL = "012345678910"
        Finally
            Label2.Text = "Phone IP: " & PHONEIP
        End Try
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        'This button serves 2 functions, depending on the mode.

        If Button1.Text = "Save" Then

            'If application was not launched by a callto link, this saves the configuration with encryption.

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
            Dim data() As Byte = System.Text.Encoding.UTF8.GetBytes(RichTextBox1.Text)
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

        Else

            'If launched with callto link, this button instead is the CALL button and initiates the call once pressed.

            CloseTimer.Stop()
            WebBrowser1.Navigate("http://" & USERNAME & ":" & PASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")
            Closer.Start()
            Me.Hide()

        End If
    End Sub

    'Autoclose after delay, if autoclose is enabled.
    Private Sub CloseTimer_Tick(sender As Object, e As EventArgs) Handles CloseTimer.Tick
        If CTime > 0 Then
            CTime = CTime - 1
        Else
            Close()
        End If
    End Sub

    'Execute call after delay, if autocall is enabled.
    Private Sub CallDelay_Tick(sender As Object, e As EventArgs) Handles CallDelay.Tick
        WebBrowser1.Navigate("http://" & USERNAME & ":" & PASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")
        CallDelay.Stop()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'Test Call Button

        Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim Result As DialogResult
        Result = MessageBox.Show("Initiate test call?" & vbNewLine & "(Calls 01234567890)", "Test call?", Buttons, MessageBoxIcon.Information)

        If Result = DialogResult.Yes Then
            PHONEIP = RichTextBox1.Lines(0).Replace("PHONEIP=", "")
            SIPACCOUNT = RichTextBox1.Lines(1).Replace("SIPACCOUNT=", "")
            USERNAME = RichTextBox1.Lines(2).Replace("USERNAME=", "")
            PASSWORD = RichTextBox1.Lines(3).Replace("PASSWORD=", "")

            WebBrowser1.Navigate("http://" & USERNAME & ":" & PASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")

        End If
    End Sub

    Private Sub Closer_Tick(sender As Object, e As EventArgs) Handles Closer.Tick
        Close()
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
End Class
