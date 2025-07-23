Public Class SetupWizard
    Private Sub SetupWizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ResetWizard()
    End Sub
    Function ResetWizard()
        WizardTaps.Location = New Point(-4, -22)
        WizardTaps.SelectedIndex = 0
        PhoneIPBox.Text = ""
        SipAccountBox.Text = ""
        WebUsernameBox.Text = ""
        WebPasswordBox.Text = ""
        UseSSLCheckbox.Checked = True
        SkipCertCheckCheckbox.Checked = True
        AutoCloseCheckBox.Checked = True
        AutoCloseDelayBox.Value = 8
        AutoDialCheckBox.Checked = False
        AutoDialDelayBox.Value = 3
        PWShowLockCheckbox.Checked = True
    End Function
#Region "NavigationButtons"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WizardTaps.SelectedIndex = 1
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        WizardTaps.SelectedIndex = 0
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        WizardTaps.SelectedIndex = 2
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        WizardTaps.SelectedIndex = 1
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        WizardTaps.SelectedIndex = 3
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        WizardTaps.SelectedIndex = 2
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        WizardTaps.SelectedIndex = 4
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        WizardTaps.SelectedIndex = 3
    End Sub
#End Region
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        'Finish button
        Me.DialogResult = DialogResult.OK
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        'Show/reveal icon on the web password text field.
        If WebPasswordBox.UseSystemPasswordChar = True Then
            WebPasswordBox.UseSystemPasswordChar = False
            PictureBox2.BackgroundImage = My.Resources.EyeOn
        Else
            WebPasswordBox.UseSystemPasswordChar = True
            PictureBox2.BackgroundImage = My.Resources.EyeGrayedOut
        End If
    End Sub
    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        'Test button
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
            Form1.ProxySendRequestToPhone(WebUsernameBox.Text, WebPasswordBox.Text, PhoneIPBox.Text, "012345678910", SipAccountBox.Text, UseSSLCheckbox.Checked, SkipCertCheckCheckbox.Checked)
        End If
    End Sub
End Class