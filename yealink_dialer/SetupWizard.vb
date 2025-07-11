Public Class SetupWizard
    Dim DIAL As String
    Dim PHONEIP As String
    Dim SIPACCOUNT As String
    Dim APIUSERNAME As String
    Dim APIPASSWORD As String
    Private Sub SetupWizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WizardTaps.Location = New Point(-4, -22)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WizardTaps.SelectedIndex = 1
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        WizardTaps.SelectedIndex = 0
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim Buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim Result As DialogResult
        Result = MessageBox.Show("Initiate test call?" & vbNewLine & "(Calls 01234567890)" & vbNewLine & vbNewLine & "Phone IP: " & PhoneIPBox.Text & vbNewLine & "Account: " & SipAccountBox.Text & vbNewLine & "Username: " & WebUsernameBox.Text & vbNewLine & "Password: " & WebPasswordBox.Text, "Test call?", Buttons, MessageBoxIcon.Information)

        If Result = DialogResult.Yes Then
            Form1.ProxySendRequestToPhone(WebUsernameBox.Text, WebPasswordBox.Text, PhoneIPBox.Text & "/servlet?key=number=" & DIAL & "&outgoing_uri=" & SIPACCOUNT & "")
        End If
    End Sub
End Class