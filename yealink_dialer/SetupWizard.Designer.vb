<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SetupWizard
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SetupWizard))
        Me.WizardTaps = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.SkipCertCheckCheckbox = New System.Windows.Forms.CheckBox()
        Me.UseSSLCheckbox = New System.Windows.Forms.CheckBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PhoneIPBox = New System.Windows.Forms.TextBox()
        Me.WebPasswordBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SipAccountBox = New System.Windows.Forms.TextBox()
        Me.WebUsernameBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.PWShowLockCheckbox = New System.Windows.Forms.CheckBox()
        Me.AutoDialDelayBox = New System.Windows.Forms.NumericUpDown()
        Me.AutoCloseDelayBox = New System.Windows.Forms.NumericUpDown()
        Me.AutoDialCheckBox = New System.Windows.Forms.CheckBox()
        Me.AutoCloseCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.WizardTaps.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.AutoDialDelayBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AutoCloseDelayBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'WizardTaps
        '
        Me.WizardTaps.Controls.Add(Me.TabPage1)
        Me.WizardTaps.Controls.Add(Me.TabPage2)
        Me.WizardTaps.Controls.Add(Me.TabPage3)
        Me.WizardTaps.Controls.Add(Me.TabPage4)
        Me.WizardTaps.Controls.Add(Me.TabPage5)
        Me.WizardTaps.Location = New System.Drawing.Point(0, -8)
        Me.WizardTaps.Name = "WizardTaps"
        Me.WizardTaps.SelectedIndex = 0
        Me.WizardTaps.Size = New System.Drawing.Size(806, 476)
        Me.WizardTaps.TabIndex = 0
        Me.WizardTaps.TabStop = False
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(798, 450)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(175, 120)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(453, 280)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = resources.GetString("Label6.Text")
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(90, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(624, 55)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Welcome to Yealink Dialer!"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(722, 424)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Next"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage2.Controls.Add(Me.PictureBox1)
        Me.TabPage2.Controls.Add(Me.Label7)
        Me.TabPage2.Controls.Add(Me.Button4)
        Me.TabPage2.Controls.Add(Me.Button2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(798, 450)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.yealink_dialer.My.Resources.Resources.ActionURIExample
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.InitialImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(210, 1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(585, 424)
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(2, 3)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(211, 416)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = resources.GetString("Label7.Text")
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(722, 424)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "Next"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(3, 424)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Back"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage3.Controls.Add(Me.GroupBox1)
        Me.TabPage3.Controls.Add(Me.Label8)
        Me.TabPage3.Controls.Add(Me.Button5)
        Me.TabPage3.Controls.Add(Me.Button6)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(798, 450)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "TabPage3"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.SkipCertCheckCheckbox)
        Me.GroupBox1.Controls.Add(Me.UseSSLCheckbox)
        Me.GroupBox1.Controls.Add(Me.PictureBox2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.PhoneIPBox)
        Me.GroupBox1.Controls.Add(Me.WebPasswordBox)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.SipAccountBox)
        Me.GroupBox1.Controls.Add(Me.WebUsernameBox)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 32)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(756, 377)
        Me.GroupBox1.TabIndex = 56
        Me.GroupBox1.TabStop = False
        '
        'SkipCertCheckCheckbox
        '
        Me.SkipCertCheckCheckbox.AutoSize = True
        Me.SkipCertCheckCheckbox.Checked = True
        Me.SkipCertCheckCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.SkipCertCheckCheckbox.Location = New System.Drawing.Point(205, 117)
        Me.SkipCertCheckCheckbox.Name = "SkipCertCheckCheckbox"
        Me.SkipCertCheckCheckbox.Size = New System.Drawing.Size(138, 17)
        Me.SkipCertCheckCheckbox.TabIndex = 57
        Me.SkipCertCheckCheckbox.Text = "Ignore certificate check"
        Me.SkipCertCheckCheckbox.UseVisualStyleBackColor = True
        '
        'UseSSLCheckbox
        '
        Me.UseSSLCheckbox.AutoSize = True
        Me.UseSSLCheckbox.Checked = True
        Me.UseSSLCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.UseSSLCheckbox.Location = New System.Drawing.Point(22, 117)
        Me.UseSSLCheckbox.Name = "UseSSLCheckbox"
        Me.UseSSLCheckbox.Size = New System.Drawing.Size(68, 17)
        Me.UseSSLCheckbox.TabIndex = 56
        Me.UseSSLCheckbox.Text = "Use SSL"
        Me.UseSSLCheckbox.UseVisualStyleBackColor = True
        '
        'PictureBox2
        '
        Me.PictureBox2.BackgroundImage = Global.yealink_dialer.My.Resources.Resources.EyeGrayedOut
        Me.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox2.Location = New System.Drawing.Point(311, 326)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(24, 13)
        Me.PictureBox2.TabIndex = 55
        Me.PictureBox2.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(515, 65)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'PhoneIPBox
        '
        Me.PhoneIPBox.Location = New System.Drawing.Point(21, 91)
        Me.PhoneIPBox.Name = "PhoneIPBox"
        Me.PhoneIPBox.Size = New System.Drawing.Size(315, 20)
        Me.PhoneIPBox.TabIndex = 48
        '
        'WebPasswordBox
        '
        Me.WebPasswordBox.Location = New System.Drawing.Point(19, 339)
        Me.WebPasswordBox.Name = "WebPasswordBox"
        Me.WebPasswordBox.Size = New System.Drawing.Size(316, 20)
        Me.WebPasswordBox.TabIndex = 54
        Me.WebPasswordBox.UseSystemPasswordChar = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 154)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(577, 52)
        Me.Label2.TabIndex = 49
        Me.Label2.Text = resources.GetString("Label2.Text")
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(17, 323)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 53
        Me.Label4.Text = "WebUI Password"
        '
        'SipAccountBox
        '
        Me.SipAccountBox.Location = New System.Drawing.Point(19, 209)
        Me.SipAccountBox.Name = "SipAccountBox"
        Me.SipAccountBox.Size = New System.Drawing.Size(316, 20)
        Me.SipAccountBox.TabIndex = 50
        '
        'WebUsernameBox
        '
        Me.WebUsernameBox.Location = New System.Drawing.Point(19, 300)
        Me.WebUsernameBox.Name = "WebUsernameBox"
        Me.WebUsernameBox.Size = New System.Drawing.Size(316, 20)
        Me.WebUsernameBox.TabIndex = 52
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 245)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(415, 52)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "Last the app needs the username and password used to login to the webinterface." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
    "This is nesseary for the app to authenticate with the phone when making API requ" &
    "ests." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WebUI Username"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(12, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(779, 20)
        Me.Label8.TabIndex = 55
        Me.Label8.Text = "In this step we will configure the nesseary data for Yealink Dialer to communicat" &
    "e to your phone."
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(722, 424)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 23)
        Me.Button5.TabIndex = 5
        Me.Button5.Text = "Next"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(3, 424)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(75, 23)
        Me.Button6.TabIndex = 4
        Me.Button6.Text = "Back"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage4.Controls.Add(Me.GroupBox2)
        Me.TabPage4.Controls.Add(Me.Label9)
        Me.TabPage4.Controls.Add(Me.Button7)
        Me.TabPage4.Controls.Add(Me.Button8)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(798, 450)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "TabPage4"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button3)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 32)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(756, 377)
        Me.GroupBox2.TabIndex = 57
        Me.GroupBox2.TabStop = False
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Location = New System.Drawing.Point(526, 183)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(196, 54)
        Me.Button3.TabIndex = 48
        Me.Button3.Text = "Test"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(33, 37)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(689, 320)
        Me.Label10.TabIndex = 47
        Me.Label10.Text = resources.GetString("Label10.Text")
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(296, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(194, 20)
        Me.Label9.TabIndex = 56
        Me.Label9.Text = "Phone connection test."
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(722, 424)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(75, 23)
        Me.Button7.TabIndex = 47
        Me.Button7.Text = "Next"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(3, 424)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(75, 23)
        Me.Button8.TabIndex = 46
        Me.Button8.Text = "Back"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage5.Controls.Add(Me.GroupBox3)
        Me.TabPage5.Controls.Add(Me.Label12)
        Me.TabPage5.Controls.Add(Me.Button9)
        Me.TabPage5.Controls.Add(Me.Button10)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(798, 450)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "TabPage5"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label14)
        Me.GroupBox3.Controls.Add(Me.Label13)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.PWShowLockCheckbox)
        Me.GroupBox3.Controls.Add(Me.AutoDialDelayBox)
        Me.GroupBox3.Controls.Add(Me.AutoCloseDelayBox)
        Me.GroupBox3.Controls.Add(Me.AutoDialCheckBox)
        Me.GroupBox3.Controls.Add(Me.AutoCloseCheckBox)
        Me.GroupBox3.Location = New System.Drawing.Point(16, 32)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(756, 377)
        Me.GroupBox3.TabIndex = 59
        Me.GroupBox3.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(18, 216)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(555, 120)
        Me.Label14.TabIndex = 35
        Me.Label14.Text = resources.GetString("Label14.Text")
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(19, 112)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(568, 40)
        Me.Label13.TabIndex = 34
        Me.Label13.Text = "Automatically dial a clicked number after x seconds when launched by a call link." &
    "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please use this function with absolute caution!"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(19, 25)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(663, 20)
        Me.Label11.TabIndex = 33
        Me.Label11.Text = "Automatically close the Yealink Dialer window after x seconds when launched troug" &
    "h call links"
        '
        'PWShowLockCheckbox
        '
        Me.PWShowLockCheckbox.AutoSize = True
        Me.PWShowLockCheckbox.Checked = True
        Me.PWShowLockCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.PWShowLockCheckbox.Location = New System.Drawing.Point(23, 339)
        Me.PWShowLockCheckbox.Name = "PWShowLockCheckbox"
        Me.PWShowLockCheckbox.Size = New System.Drawing.Size(142, 17)
        Me.PWShowLockCheckbox.TabIndex = 32
        Me.PWShowLockCheckbox.Text = "Password reveal lockout"
        Me.PWShowLockCheckbox.UseVisualStyleBackColor = True
        '
        'AutoDialDelayBox
        '
        Me.AutoDialDelayBox.Location = New System.Drawing.Point(92, 154)
        Me.AutoDialDelayBox.Name = "AutoDialDelayBox"
        Me.AutoDialDelayBox.Size = New System.Drawing.Size(35, 20)
        Me.AutoDialDelayBox.TabIndex = 31
        '
        'AutoCloseDelayBox
        '
        Me.AutoCloseDelayBox.Location = New System.Drawing.Point(92, 47)
        Me.AutoCloseDelayBox.Name = "AutoCloseDelayBox"
        Me.AutoCloseDelayBox.Size = New System.Drawing.Size(35, 20)
        Me.AutoCloseDelayBox.TabIndex = 30
        '
        'AutoDialCheckBox
        '
        Me.AutoDialCheckBox.AutoSize = True
        Me.AutoDialCheckBox.Location = New System.Drawing.Point(23, 155)
        Me.AutoDialCheckBox.Name = "AutoDialCheckBox"
        Me.AutoDialCheckBox.Size = New System.Drawing.Size(64, 17)
        Me.AutoDialCheckBox.TabIndex = 29
        Me.AutoDialCheckBox.Text = "Autodial"
        Me.AutoDialCheckBox.UseVisualStyleBackColor = True
        '
        'AutoCloseCheckBox
        '
        Me.AutoCloseCheckBox.AutoSize = True
        Me.AutoCloseCheckBox.Checked = True
        Me.AutoCloseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoCloseCheckBox.Location = New System.Drawing.Point(22, 48)
        Me.AutoCloseCheckBox.Name = "AutoCloseCheckBox"
        Me.AutoCloseCheckBox.Size = New System.Drawing.Size(73, 17)
        Me.AutoCloseCheckBox.TabIndex = 28
        Me.AutoCloseCheckBox.Text = "Autoclose"
        Me.AutoCloseCheckBox.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(299, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(163, 20)
        Me.Label12.TabIndex = 58
        Me.Label12.Text = "Additional settings."
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(722, 424)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(75, 23)
        Me.Button9.TabIndex = 49
        Me.Button9.Text = "Finish"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(3, 424)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(75, 23)
        Me.Button10.TabIndex = 48
        Me.Button10.Text = "Back"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'SetupWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.WizardTaps)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SetupWizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Yealink Dialer - Setup Wizard"
        Me.WizardTaps.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.AutoDialDelayBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AutoCloseDelayBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents WizardTaps As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents WebPasswordBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents WebUsernameBox As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents SipAccountBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents PhoneIPBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button3 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents Label9 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label10 As Label
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents Button9 As Button
    Friend WithEvents Button10 As Button
    Friend WithEvents UseSSLCheckbox As CheckBox
    Friend WithEvents SkipCertCheckCheckbox As CheckBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label12 As Label
    Friend WithEvents PWShowLockCheckbox As CheckBox
    Friend WithEvents AutoDialDelayBox As NumericUpDown
    Friend WithEvents AutoCloseDelayBox As NumericUpDown
    Friend WithEvents AutoDialCheckBox As CheckBox
    Friend WithEvents AutoCloseCheckBox As CheckBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label11 As Label
End Class
