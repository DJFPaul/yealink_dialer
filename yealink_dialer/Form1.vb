Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class Form1
    Inherits System.Windows.Forms.Form
    Dim DIAL As String
    Dim PHONEIP As String
    Dim SIPACCOUNT As String
    Dim USERNAME As String
    Dim PASSWORD As String
    Dim CTime As Integer = 8
    Dim CRDTA As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ConfigContent As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath() & "\data\config.ini")
        RichTextBox2.Text = ConfigContent
        CTime = RichTextBox2.Lines(1).Replace("closedelay=", "") - 1
        CallDelay.Interval = RichTextBox2.Lines(3).Replace("dialdelay=", "") & "000"

        Dim PhoneContent As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath() & "\data\phone.cfg")

        Dim rd As New RijndaelManaged
        Dim rijndaelIvLength As Integer = 16
        Dim md5 As New MD5CryptoServiceProvider
        Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes("duSK(57&5fHPskg8%$/3jalskg90DGNA51"))
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

        PHONEIP = RichTextBox1.Lines(0).Replace("PHONEIP=", "")
        SIPACCOUNT = RichTextBox1.Lines(1).Replace("SIPACCOUNT=", "")
        USERNAME = RichTextBox1.Lines(2).Replace("USERNAME=", "")
        PASSWORD = RichTextBox1.Lines(3).Replace("PASSWORD=", "")

        Try
            DIAL = My.Application.CommandLineArgs(0).Replace("tel:", "")
            DIAL = DIAL.Replace("/", "")
            DIAL = DIAL.Replace("-", "")
            Label1.Text = "Calling: " & DIAL
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
        If Button1.Text = "Save" Then

            Dim rd As New RijndaelManaged
            Dim md5 As New MD5CryptoServiceProvider
            Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes("duSK(57&5fHPskg8%$/3jalskg90DGNA51"))
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

            Using writer As New System.IO.StreamWriter(Application.StartupPath() & "\data\phone.cfg")
                For Each line In "A"
                    writer.Write(CRDTA)
                Next
            End Using
        Else
            CloseTimer.Stop()
            WebBrowser1.Navigate("http://" & USERNAME & ":" & PASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")
            Closer.Start()
            Me.Hide()

        End If
    End Sub

    Private Sub CloseTimer_Tick(sender As Object, e As EventArgs) Handles CloseTimer.Tick
        If CTime > 0 Then
            CTime = CTime - 1
        Else
            Close()
        End If
    End Sub

    Private Sub CallDelay_Tick(sender As Object, e As EventArgs) Handles CallDelay.Tick
        WebBrowser1.Navigate("http://" & USERNAME & ":" & PASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")
        CallDelay.Stop()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MsgBox("Initiate test call?" & vbNewLine & "(Calls 01234567890)", MsgBoxStyle.YesNo)
        PHONEIP = RichTextBox1.Lines(0).Replace("PHONEIP=", "")
        SIPACCOUNT = RichTextBox1.Lines(1).Replace("SIPACCOUNT=", "")
        USERNAME = RichTextBox1.Lines(2).Replace("USERNAME=", "")
        PASSWORD = RichTextBox1.Lines(3).Replace("PASSWORD=", "")

        WebBrowser1.Navigate("http://" & USERNAME & ":" & PASSWORD & "@" & PHONEIP & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")
    End Sub

    Private Sub Closer_Tick(sender As Object, e As EventArgs) Handles Closer.Tick
        Close()
    End Sub

End Class
