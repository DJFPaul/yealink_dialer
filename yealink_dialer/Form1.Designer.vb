<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.IPLabel = New System.Windows.Forms.Label()
        Me.CallButton = New System.Windows.Forms.Button()
        Me.CloseTimer = New System.Windows.Forms.Timer(Me.components)
        Me.CallDelay = New System.Windows.Forms.Timer(Me.components)
        Me.Dial1 = New System.Windows.Forms.Button()
        Me.Dial2 = New System.Windows.Forms.Button()
        Me.Dial3 = New System.Windows.Forms.Button()
        Me.Dial4 = New System.Windows.Forms.Button()
        Me.Dial5 = New System.Windows.Forms.Button()
        Me.Dial6 = New System.Windows.Forms.Button()
        Me.Dial7 = New System.Windows.Forms.Button()
        Me.Dial8 = New System.Windows.Forms.Button()
        Me.Dial9 = New System.Windows.Forms.Button()
        Me.DialPlus = New System.Windows.Forms.Button()
        Me.Dial0 = New System.Windows.Forms.Button()
        Me.DialErase = New System.Windows.Forms.Button()
        Me.TelNumberBox = New System.Windows.Forms.TextBox()
        Me.SettingsLabel = New System.Windows.Forms.Label()
        Me.BottomPanel = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PhoneIPBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SipAccountBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.WebUsernameBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.WebPasswordBox = New System.Windows.Forms.TextBox()
        Me.PhoneConfigBox = New System.Windows.Forms.RichTextBox()
        Me.AutoCloseCheckBox = New System.Windows.Forms.CheckBox()
        Me.AutoDialCheckBox = New System.Windows.Forms.CheckBox()
        Me.AutoCloseDelayBox = New System.Windows.Forms.NumericUpDown()
        Me.AutoDialDelayBox = New System.Windows.Forms.NumericUpDown()
        Me.SettingsPanel = New System.Windows.Forms.Panel()
        Me.SkipCertCheckCheckbox = New System.Windows.Forms.CheckBox()
        Me.UseSSLCheckbox = New System.Windows.Forms.CheckBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PWShowLockCheckbox = New System.Windows.Forms.CheckBox()
        Me.SetupWizardLabel = New System.Windows.Forms.Label()
        Me.ConfigInfoToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.BottomPanel.SuspendLayout()
        CType(Me.AutoCloseDelayBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AutoDialDelayBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SettingsPanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'IPLabel
        '
        Me.IPLabel.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IPLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.IPLabel.Location = New System.Drawing.Point(13, 39)
        Me.IPLabel.Name = "IPLabel"
        Me.IPLabel.Size = New System.Drawing.Size(150, 15)
        Me.IPLabel.TabIndex = 7
        Me.IPLabel.Text = "no account"
        '
        'CallButton
        '
        Me.CallButton.Enabled = False
        Me.CallButton.Location = New System.Drawing.Point(13, 4)
        Me.CallButton.Name = "CallButton"
        Me.CallButton.Size = New System.Drawing.Size(204, 32)
        Me.CallButton.TabIndex = 8
        Me.CallButton.Text = "Call"
        Me.CallButton.UseVisualStyleBackColor = True
        '
        'CloseTimer
        '
        Me.CloseTimer.Interval = 1000
        '
        'CallDelay
        '
        Me.CallDelay.Interval = 3000
        '
        'Dial1
        '
        Me.Dial1.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial1.Location = New System.Drawing.Point(12, 44)
        Me.Dial1.Name = "Dial1"
        Me.Dial1.Size = New System.Drawing.Size(64, 64)
        Me.Dial1.TabIndex = 12
        Me.Dial1.Text = "1"
        Me.Dial1.UseVisualStyleBackColor = True
        '
        'Dial2
        '
        Me.Dial2.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial2.Location = New System.Drawing.Point(82, 44)
        Me.Dial2.Name = "Dial2"
        Me.Dial2.Size = New System.Drawing.Size(64, 64)
        Me.Dial2.TabIndex = 13
        Me.Dial2.Text = "2"
        Me.Dial2.UseVisualStyleBackColor = True
        '
        'Dial3
        '
        Me.Dial3.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial3.Location = New System.Drawing.Point(152, 44)
        Me.Dial3.Name = "Dial3"
        Me.Dial3.Size = New System.Drawing.Size(64, 64)
        Me.Dial3.TabIndex = 14
        Me.Dial3.Text = "3"
        Me.Dial3.UseVisualStyleBackColor = True
        '
        'Dial4
        '
        Me.Dial4.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial4.Location = New System.Drawing.Point(12, 114)
        Me.Dial4.Name = "Dial4"
        Me.Dial4.Size = New System.Drawing.Size(64, 64)
        Me.Dial4.TabIndex = 15
        Me.Dial4.Text = "4"
        Me.Dial4.UseVisualStyleBackColor = True
        '
        'Dial5
        '
        Me.Dial5.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial5.Location = New System.Drawing.Point(82, 114)
        Me.Dial5.Name = "Dial5"
        Me.Dial5.Size = New System.Drawing.Size(64, 64)
        Me.Dial5.TabIndex = 16
        Me.Dial5.Text = "5"
        Me.Dial5.UseVisualStyleBackColor = True
        '
        'Dial6
        '
        Me.Dial6.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial6.Location = New System.Drawing.Point(152, 114)
        Me.Dial6.Name = "Dial6"
        Me.Dial6.Size = New System.Drawing.Size(64, 64)
        Me.Dial6.TabIndex = 17
        Me.Dial6.Text = "6"
        Me.Dial6.UseVisualStyleBackColor = True
        '
        'Dial7
        '
        Me.Dial7.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial7.Location = New System.Drawing.Point(12, 184)
        Me.Dial7.Name = "Dial7"
        Me.Dial7.Size = New System.Drawing.Size(64, 64)
        Me.Dial7.TabIndex = 18
        Me.Dial7.Text = "7"
        Me.Dial7.UseVisualStyleBackColor = True
        '
        'Dial8
        '
        Me.Dial8.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial8.Location = New System.Drawing.Point(82, 184)
        Me.Dial8.Name = "Dial8"
        Me.Dial8.Size = New System.Drawing.Size(64, 64)
        Me.Dial8.TabIndex = 19
        Me.Dial8.Text = "8"
        Me.Dial8.UseVisualStyleBackColor = True
        '
        'Dial9
        '
        Me.Dial9.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial9.Location = New System.Drawing.Point(152, 184)
        Me.Dial9.Name = "Dial9"
        Me.Dial9.Size = New System.Drawing.Size(64, 64)
        Me.Dial9.TabIndex = 20
        Me.Dial9.Text = "9"
        Me.Dial9.UseVisualStyleBackColor = True
        '
        'DialPlus
        '
        Me.DialPlus.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DialPlus.Location = New System.Drawing.Point(12, 254)
        Me.DialPlus.Name = "DialPlus"
        Me.DialPlus.Size = New System.Drawing.Size(64, 64)
        Me.DialPlus.TabIndex = 21
        Me.DialPlus.Text = "+"
        Me.DialPlus.UseVisualStyleBackColor = True
        '
        'Dial0
        '
        Me.Dial0.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dial0.Location = New System.Drawing.Point(82, 254)
        Me.Dial0.Name = "Dial0"
        Me.Dial0.Size = New System.Drawing.Size(64, 64)
        Me.Dial0.TabIndex = 22
        Me.Dial0.Text = "0"
        Me.Dial0.UseVisualStyleBackColor = True
        '
        'DialErase
        '
        Me.DialErase.Font = New System.Drawing.Font("Consolas", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DialErase.Location = New System.Drawing.Point(152, 254)
        Me.DialErase.Name = "DialErase"
        Me.DialErase.Size = New System.Drawing.Size(64, 64)
        Me.DialErase.TabIndex = 23
        Me.DialErase.Text = "C"
        Me.DialErase.UseVisualStyleBackColor = True
        '
        'TelNumberBox
        '
        Me.TelNumberBox.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TelNumberBox.Location = New System.Drawing.Point(12, 12)
        Me.TelNumberBox.MaxLength = 22
        Me.TelNumberBox.Name = "TelNumberBox"
        Me.TelNumberBox.Size = New System.Drawing.Size(204, 26)
        Me.TelNumberBox.TabIndex = 24
        '
        'SettingsLabel
        '
        Me.SettingsLabel.AutoSize = True
        Me.SettingsLabel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SettingsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SettingsLabel.ForeColor = System.Drawing.Color.RoyalBlue
        Me.SettingsLabel.Location = New System.Drawing.Point(160, 39)
        Me.SettingsLabel.Name = "SettingsLabel"
        Me.SettingsLabel.Size = New System.Drawing.Size(61, 13)
        Me.SettingsLabel.TabIndex = 25
        Me.SettingsLabel.Text = "[Settings]"
        '
        'BottomPanel
        '
        Me.BottomPanel.Controls.Add(Me.CallButton)
        Me.BottomPanel.Controls.Add(Me.IPLabel)
        Me.BottomPanel.Controls.Add(Me.SettingsLabel)
        Me.BottomPanel.Location = New System.Drawing.Point(0, 324)
        Me.BottomPanel.Name = "BottomPanel"
        Me.BottomPanel.Size = New System.Drawing.Size(230, 58)
        Me.BottomPanel.TabIndex = 26
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(17, 191)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(195, 50)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Test"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(15, 316)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(199, 50)
        Me.Button4.TabIndex = 11
        Me.Button4.Text = "Save"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Phone IP"
        Me.ConfigInfoToolTip.SetToolTip(Me.Label1, "IP where the phones web interface can be reached." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'PhoneIPBox
        '
        Me.PhoneIPBox.Location = New System.Drawing.Point(18, 22)
        Me.PhoneIPBox.Name = "PhoneIPBox"
        Me.PhoneIPBox.Size = New System.Drawing.Size(194, 20)
        Me.PhoneIPBox.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Sip Account"
        Me.ConfigInfoToolTip.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'SipAccountBox
        '
        Me.SipAccountBox.Location = New System.Drawing.Point(17, 87)
        Me.SipAccountBox.Name = "SipAccountBox"
        Me.SipAccountBox.Size = New System.Drawing.Size(195, 20)
        Me.SipAccountBox.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 110)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "WebUI Username"
        Me.ConfigInfoToolTip.SetToolTip(Me.Label3, "Username used to login to the WebUI.")
        '
        'WebUsernameBox
        '
        Me.WebUsernameBox.Location = New System.Drawing.Point(17, 126)
        Me.WebUsernameBox.Name = "WebUsernameBox"
        Me.WebUsernameBox.Size = New System.Drawing.Size(195, 20)
        Me.WebUsernameBox.TabIndex = 17
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 149)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "WebUI Password"
        Me.ConfigInfoToolTip.SetToolTip(Me.Label4, "Password used to login to the WebUI.")
        '
        'WebPasswordBox
        '
        Me.WebPasswordBox.Location = New System.Drawing.Point(17, 165)
        Me.WebPasswordBox.Name = "WebPasswordBox"
        Me.WebPasswordBox.Size = New System.Drawing.Size(195, 20)
        Me.WebPasswordBox.TabIndex = 19
        Me.WebPasswordBox.UseSystemPasswordChar = True
        '
        'PhoneConfigBox
        '
        Me.PhoneConfigBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.PhoneConfigBox.Location = New System.Drawing.Point(12, 10)
        Me.PhoneConfigBox.Name = "PhoneConfigBox"
        Me.PhoneConfigBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.PhoneConfigBox.Size = New System.Drawing.Size(223, 153)
        Me.PhoneConfigBox.TabIndex = 9
        Me.PhoneConfigBox.Text = "MINVERSION=3.0" & Global.Microsoft.VisualBasic.ChrW(10) & "PHONEIP=192.168.1.100" & Global.Microsoft.VisualBasic.ChrW(10) & "SIPACCOUNT=10@192.168.1.1" & Global.Microsoft.VisualBasic.ChrW(10) & "USERNAME=admin" & Global.Microsoft.VisualBasic.ChrW(10) & "PAS" &
    "SWORD=admin" & Global.Microsoft.VisualBasic.ChrW(10) & "autoclose=true" & Global.Microsoft.VisualBasic.ChrW(10) & "closedelay=8" & Global.Microsoft.VisualBasic.ChrW(10) & "autodial=false" & Global.Microsoft.VisualBasic.ChrW(10) & "dialdelay=3" & Global.Microsoft.VisualBasic.ChrW(10) & "pwreveallock=" &
    "false" & Global.Microsoft.VisualBasic.ChrW(10) & "usessl=true" & Global.Microsoft.VisualBasic.ChrW(10) & "ignorecert=true"
        Me.PhoneConfigBox.Visible = False
        '
        'AutoCloseCheckBox
        '
        Me.AutoCloseCheckBox.AutoSize = True
        Me.AutoCloseCheckBox.Checked = True
        Me.AutoCloseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoCloseCheckBox.Location = New System.Drawing.Point(18, 247)
        Me.AutoCloseCheckBox.Name = "AutoCloseCheckBox"
        Me.AutoCloseCheckBox.Size = New System.Drawing.Size(73, 17)
        Me.AutoCloseCheckBox.TabIndex = 20
        Me.AutoCloseCheckBox.Text = "Autoclose"
        Me.ConfigInfoToolTip.SetToolTip(Me.AutoCloseCheckBox, "Only when the app is launched trough call links." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Automatically closes the Yeal" &
        "ink Dialer window after x seconds.")
        Me.AutoCloseCheckBox.UseVisualStyleBackColor = True
        '
        'AutoDialCheckBox
        '
        Me.AutoDialCheckBox.AutoSize = True
        Me.AutoDialCheckBox.Location = New System.Drawing.Point(18, 269)
        Me.AutoDialCheckBox.Name = "AutoDialCheckBox"
        Me.AutoDialCheckBox.Size = New System.Drawing.Size(64, 17)
        Me.AutoDialCheckBox.TabIndex = 21
        Me.AutoDialCheckBox.Text = "Autodial"
        Me.ConfigInfoToolTip.SetToolTip(Me.AutoDialCheckBox, "Only when the app is launched trough call links." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Automatically dials a clicked" &
        " number after x seconds." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please use this function with absolute caution!")
        Me.AutoDialCheckBox.UseVisualStyleBackColor = True
        '
        'AutoCloseDelayBox
        '
        Me.AutoCloseDelayBox.Location = New System.Drawing.Point(177, 244)
        Me.AutoCloseDelayBox.Name = "AutoCloseDelayBox"
        Me.AutoCloseDelayBox.Size = New System.Drawing.Size(35, 20)
        Me.AutoCloseDelayBox.TabIndex = 22
        Me.AutoCloseDelayBox.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'AutoDialDelayBox
        '
        Me.AutoDialDelayBox.Location = New System.Drawing.Point(177, 269)
        Me.AutoDialDelayBox.Name = "AutoDialDelayBox"
        Me.AutoDialDelayBox.Size = New System.Drawing.Size(35, 20)
        Me.AutoDialDelayBox.TabIndex = 23
        Me.AutoDialDelayBox.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'SettingsPanel
        '
        Me.SettingsPanel.Controls.Add(Me.SkipCertCheckCheckbox)
        Me.SettingsPanel.Controls.Add(Me.UseSSLCheckbox)
        Me.SettingsPanel.Controls.Add(Me.PictureBox1)
        Me.SettingsPanel.Controls.Add(Me.PWShowLockCheckbox)
        Me.SettingsPanel.Controls.Add(Me.SetupWizardLabel)
        Me.SettingsPanel.Controls.Add(Me.AutoDialDelayBox)
        Me.SettingsPanel.Controls.Add(Me.AutoCloseDelayBox)
        Me.SettingsPanel.Controls.Add(Me.AutoDialCheckBox)
        Me.SettingsPanel.Controls.Add(Me.AutoCloseCheckBox)
        Me.SettingsPanel.Controls.Add(Me.WebPasswordBox)
        Me.SettingsPanel.Controls.Add(Me.Label4)
        Me.SettingsPanel.Controls.Add(Me.WebUsernameBox)
        Me.SettingsPanel.Controls.Add(Me.Label3)
        Me.SettingsPanel.Controls.Add(Me.SipAccountBox)
        Me.SettingsPanel.Controls.Add(Me.Label2)
        Me.SettingsPanel.Controls.Add(Me.PhoneIPBox)
        Me.SettingsPanel.Controls.Add(Me.Label1)
        Me.SettingsPanel.Controls.Add(Me.Button4)
        Me.SettingsPanel.Controls.Add(Me.Button2)
        Me.SettingsPanel.Controls.Add(Me.PhoneConfigBox)
        Me.SettingsPanel.Location = New System.Drawing.Point(0, 0)
        Me.SettingsPanel.Name = "SettingsPanel"
        Me.SettingsPanel.Size = New System.Drawing.Size(230, 369)
        Me.SettingsPanel.TabIndex = 4
        Me.SettingsPanel.Visible = False
        '
        'SkipCertCheckCheckbox
        '
        Me.SkipCertCheckCheckbox.AutoSize = True
        Me.SkipCertCheckCheckbox.Checked = True
        Me.SkipCertCheckCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.SkipCertCheckCheckbox.Location = New System.Drawing.Point(82, 48)
        Me.SkipCertCheckCheckbox.Name = "SkipCertCheckCheckbox"
        Me.SkipCertCheckCheckbox.Size = New System.Drawing.Size(138, 17)
        Me.SkipCertCheckCheckbox.TabIndex = 30
        Me.SkipCertCheckCheckbox.Text = "Ignore certificate check"
        Me.ConfigInfoToolTip.SetToolTip(Me.SkipCertCheckCheckbox, "When checked, ignores the SSL certificate validation." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This for example means i" &
        "t will accept self signed certificates.")
        Me.SkipCertCheckCheckbox.UseVisualStyleBackColor = True
        '
        'UseSSLCheckbox
        '
        Me.UseSSLCheckbox.AutoSize = True
        Me.UseSSLCheckbox.Checked = True
        Me.UseSSLCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.UseSSLCheckbox.Location = New System.Drawing.Point(18, 48)
        Me.UseSSLCheckbox.Name = "UseSSLCheckbox"
        Me.UseSSLCheckbox.Size = New System.Drawing.Size(68, 17)
        Me.UseSSLCheckbox.TabIndex = 29
        Me.UseSSLCheckbox.Text = "Use SSL"
        Me.ConfigInfoToolTip.SetToolTip(Me.UseSSLCheckbox, "Uses the HTTPS endpoint on the phone." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note: Most phones have a self signed/off" &
        "line certificate." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This will require the certificate check to be ignored.")
        Me.UseSSLCheckbox.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.yealink_dialer.My.Resources.Resources.EyeGrayedOut
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.Location = New System.Drawing.Point(188, 150)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(24, 13)
        Me.PictureBox1.TabIndex = 28
        Me.PictureBox1.TabStop = False
        Me.ConfigInfoToolTip.SetToolTip(Me.PictureBox1, resources.GetString("PictureBox1.ToolTip"))
        '
        'PWShowLockCheckbox
        '
        Me.PWShowLockCheckbox.AutoSize = True
        Me.PWShowLockCheckbox.Checked = True
        Me.PWShowLockCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.PWShowLockCheckbox.Location = New System.Drawing.Point(18, 292)
        Me.PWShowLockCheckbox.Name = "PWShowLockCheckbox"
        Me.PWShowLockCheckbox.Size = New System.Drawing.Size(142, 17)
        Me.PWShowLockCheckbox.TabIndex = 27
        Me.PWShowLockCheckbox.Text = "Password reveal lockout"
        Me.ConfigInfoToolTip.SetToolTip(Me.PWShowLockCheckbox, resources.GetString("PWShowLockCheckbox.ToolTip"))
        Me.PWShowLockCheckbox.UseVisualStyleBackColor = True
        '
        'SetupWizardLabel
        '
        Me.SetupWizardLabel.AutoSize = True
        Me.SetupWizardLabel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SetupWizardLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SetupWizardLabel.ForeColor = System.Drawing.Color.RoyalBlue
        Me.SetupWizardLabel.Location = New System.Drawing.Point(125, 6)
        Me.SetupWizardLabel.Name = "SetupWizardLabel"
        Me.SetupWizardLabel.Size = New System.Drawing.Size(91, 13)
        Me.SetupWizardLabel.TabIndex = 26
        Me.SetupWizardLabel.Text = "[Setup Wizard]"
        '
        'ConfigInfoToolTip
        '
        Me.ConfigInfoToolTip.AutoPopDelay = 32000
        Me.ConfigInfoToolTip.InitialDelay = 500
        Me.ConfigInfoToolTip.ReshowDelay = 100
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(229, 381)
        Me.Controls.Add(Me.SettingsPanel)
        Me.Controls.Add(Me.TelNumberBox)
        Me.Controls.Add(Me.BottomPanel)
        Me.Controls.Add(Me.DialErase)
        Me.Controls.Add(Me.Dial0)
        Me.Controls.Add(Me.DialPlus)
        Me.Controls.Add(Me.Dial9)
        Me.Controls.Add(Me.Dial8)
        Me.Controls.Add(Me.Dial7)
        Me.Controls.Add(Me.Dial6)
        Me.Controls.Add(Me.Dial5)
        Me.Controls.Add(Me.Dial4)
        Me.Controls.Add(Me.Dial3)
        Me.Controls.Add(Me.Dial2)
        Me.Controls.Add(Me.Dial1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = " Yealink Dialer"
        Me.TopMost = True
        Me.BottomPanel.ResumeLayout(False)
        Me.BottomPanel.PerformLayout()
        CType(Me.AutoCloseDelayBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AutoDialDelayBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SettingsPanel.ResumeLayout(False)
        Me.SettingsPanel.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents IPLabel As Label
    Friend WithEvents CallButton As Button
    Friend WithEvents CloseTimer As Timer
    Friend WithEvents CallDelay As Timer
    Friend WithEvents Dial1 As Button
    Friend WithEvents Dial2 As Button
    Friend WithEvents Dial3 As Button
    Friend WithEvents Dial4 As Button
    Friend WithEvents Dial5 As Button
    Friend WithEvents Dial6 As Button
    Friend WithEvents Dial7 As Button
    Friend WithEvents Dial8 As Button
    Friend WithEvents Dial9 As Button
    Friend WithEvents DialPlus As Button
    Friend WithEvents Dial0 As Button
    Friend WithEvents DialErase As Button
    Friend WithEvents TelNumberBox As TextBox
    Friend WithEvents SettingsLabel As Label
    Friend WithEvents BottomPanel As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents PhoneIPBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents SipAccountBox As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents WebUsernameBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents WebPasswordBox As TextBox
    Friend WithEvents AutoCloseCheckBox As CheckBox
    Friend WithEvents AutoDialCheckBox As CheckBox
    Friend WithEvents AutoCloseDelayBox As NumericUpDown
    Friend WithEvents AutoDialDelayBox As NumericUpDown
    Friend WithEvents SettingsPanel As Panel
    Friend WithEvents ConfigInfoToolTip As ToolTip
    Friend WithEvents SetupWizardLabel As Label
    Friend WithEvents PhoneConfigBox As RichTextBox
    Friend WithEvents PWShowLockCheckbox As CheckBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents UseSSLCheckbox As CheckBox
    Friend WithEvents SkipCertCheckCheckbox As CheckBox
End Class
