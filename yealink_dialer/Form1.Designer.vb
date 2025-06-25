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
        Me.SipUsernameBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SipPasswordBox = New System.Windows.Forms.TextBox()
        Me.PhoneConfigBox = New System.Windows.Forms.RichTextBox()
        Me.AutoCloseCheckBox = New System.Windows.Forms.CheckBox()
        Me.AutoDialCheckBox = New System.Windows.Forms.CheckBox()
        Me.AutoCloseDelayBox = New System.Windows.Forms.NumericUpDown()
        Me.AutoDialDelayBox = New System.Windows.Forms.NumericUpDown()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.SettingsPanel = New System.Windows.Forms.Panel()
        Me.BottomPanel.SuspendLayout()
        CType(Me.AutoCloseDelayBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AutoDialDelayBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SettingsPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'IPLabel
        '
        Me.IPLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.IPLabel.Location = New System.Drawing.Point(15, 39)
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
        Me.SettingsLabel.Location = New System.Drawing.Point(164, 39)
        Me.SettingsLabel.Name = "SettingsLabel"
        Me.SettingsLabel.Size = New System.Drawing.Size(53, 13)
        Me.SettingsLabel.TabIndex = 25
        Me.SettingsLabel.Text = "Settings"
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
        Me.Button2.Location = New System.Drawing.Point(3, 211)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(199, 50)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Test"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(4, 304)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(199, 50)
        Me.Button4.TabIndex = 11
        Me.Button4.Text = "Save"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 1)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Phone IP"
        '
        'PhoneIPBox
        '
        Me.PhoneIPBox.Location = New System.Drawing.Point(7, 17)
        Me.PhoneIPBox.Name = "PhoneIPBox"
        Me.PhoneIPBox.Size = New System.Drawing.Size(194, 20)
        Me.PhoneIPBox.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Sip Account"
        '
        'SipAccountBox
        '
        Me.SipAccountBox.Location = New System.Drawing.Point(6, 56)
        Me.SipAccountBox.Name = "SipAccountBox"
        Me.SipAccountBox.Size = New System.Drawing.Size(195, 20)
        Me.SipAccountBox.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(4, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Sip Username"
        '
        'SipUsernameBox
        '
        Me.SipUsernameBox.Location = New System.Drawing.Point(6, 95)
        Me.SipUsernameBox.Name = "SipUsernameBox"
        Me.SipUsernameBox.Size = New System.Drawing.Size(195, 20)
        Me.SipUsernameBox.TabIndex = 17
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 118)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Sip Password"
        '
        'SipPasswordBox
        '
        Me.SipPasswordBox.Location = New System.Drawing.Point(6, 134)
        Me.SipPasswordBox.Name = "SipPasswordBox"
        Me.SipPasswordBox.Size = New System.Drawing.Size(195, 20)
        Me.SipPasswordBox.TabIndex = 19
        Me.SipPasswordBox.UseSystemPasswordChar = True
        '
        'PhoneConfigBox
        '
        Me.PhoneConfigBox.Location = New System.Drawing.Point(3, 3)
        Me.PhoneConfigBox.Name = "PhoneConfigBox"
        Me.PhoneConfigBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.PhoneConfigBox.Size = New System.Drawing.Size(199, 112)
        Me.PhoneConfigBox.TabIndex = 9
        Me.PhoneConfigBox.Text = "PHONEIP=192.168.1.100" & Global.Microsoft.VisualBasic.ChrW(10) & "SIPACCOUNT=10@192.168.1.1" & Global.Microsoft.VisualBasic.ChrW(10) & "USERNAME=admin" & Global.Microsoft.VisualBasic.ChrW(10) & "PASSWORD=admin" & Global.Microsoft.VisualBasic.ChrW(10) & "aut" &
    "oclose=true" & Global.Microsoft.VisualBasic.ChrW(10) & "closedelay=8" & Global.Microsoft.VisualBasic.ChrW(10) & "autodial=false" & Global.Microsoft.VisualBasic.ChrW(10) & "dialdelay=3"
        Me.PhoneConfigBox.Visible = False
        '
        'AutoCloseCheckBox
        '
        Me.AutoCloseCheckBox.AutoSize = True
        Me.AutoCloseCheckBox.Checked = True
        Me.AutoCloseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoCloseCheckBox.Location = New System.Drawing.Point(6, 161)
        Me.AutoCloseCheckBox.Name = "AutoCloseCheckBox"
        Me.AutoCloseCheckBox.Size = New System.Drawing.Size(73, 17)
        Me.AutoCloseCheckBox.TabIndex = 20
        Me.AutoCloseCheckBox.Text = "Autoclose"
        Me.AutoCloseCheckBox.UseVisualStyleBackColor = True
        '
        'AutoDialCheckBox
        '
        Me.AutoDialCheckBox.AutoSize = True
        Me.AutoDialCheckBox.Location = New System.Drawing.Point(6, 184)
        Me.AutoDialCheckBox.Name = "AutoDialCheckBox"
        Me.AutoDialCheckBox.Size = New System.Drawing.Size(64, 17)
        Me.AutoDialCheckBox.TabIndex = 21
        Me.AutoDialCheckBox.Text = "Autodial"
        Me.AutoDialCheckBox.UseVisualStyleBackColor = True
        '
        'AutoCloseDelayBox
        '
        Me.AutoCloseDelayBox.Location = New System.Drawing.Point(166, 160)
        Me.AutoCloseDelayBox.Name = "AutoCloseDelayBox"
        Me.AutoCloseDelayBox.Size = New System.Drawing.Size(35, 20)
        Me.AutoCloseDelayBox.TabIndex = 22
        '
        'AutoDialDelayBox
        '
        Me.AutoDialDelayBox.Location = New System.Drawing.Point(166, 185)
        Me.AutoDialDelayBox.Name = "AutoDialDelayBox"
        Me.AutoDialDelayBox.Size = New System.Drawing.Size(35, 20)
        Me.AutoDialDelayBox.TabIndex = 23
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(4, 267)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(95, 31)
        Me.Button1.TabIndex = 24
        Me.Button1.Text = "Import"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Location = New System.Drawing.Point(108, 267)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(95, 31)
        Me.Button3.TabIndex = 25
        Me.Button3.Text = "Export"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'SettingsPanel
        '
        Me.SettingsPanel.Controls.Add(Me.Button3)
        Me.SettingsPanel.Controls.Add(Me.Button1)
        Me.SettingsPanel.Controls.Add(Me.AutoDialDelayBox)
        Me.SettingsPanel.Controls.Add(Me.AutoCloseDelayBox)
        Me.SettingsPanel.Controls.Add(Me.AutoDialCheckBox)
        Me.SettingsPanel.Controls.Add(Me.AutoCloseCheckBox)
        Me.SettingsPanel.Controls.Add(Me.PhoneConfigBox)
        Me.SettingsPanel.Controls.Add(Me.SipPasswordBox)
        Me.SettingsPanel.Controls.Add(Me.Label4)
        Me.SettingsPanel.Controls.Add(Me.SipUsernameBox)
        Me.SettingsPanel.Controls.Add(Me.Label3)
        Me.SettingsPanel.Controls.Add(Me.SipAccountBox)
        Me.SettingsPanel.Controls.Add(Me.Label2)
        Me.SettingsPanel.Controls.Add(Me.PhoneIPBox)
        Me.SettingsPanel.Controls.Add(Me.Label1)
        Me.SettingsPanel.Controls.Add(Me.Button4)
        Me.SettingsPanel.Controls.Add(Me.Button2)
        Me.SettingsPanel.Location = New System.Drawing.Point(11, 5)
        Me.SettingsPanel.Name = "SettingsPanel"
        Me.SettingsPanel.Size = New System.Drawing.Size(391, 377)
        Me.SettingsPanel.TabIndex = 4
        Me.SettingsPanel.Visible = False
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
    Friend WithEvents SipUsernameBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents SipPasswordBox As TextBox
    Friend WithEvents PhoneConfigBox As RichTextBox
    Friend WithEvents AutoCloseCheckBox As CheckBox
    Friend WithEvents AutoDialCheckBox As CheckBox
    Friend WithEvents AutoCloseDelayBox As NumericUpDown
    Friend WithEvents AutoDialDelayBox As NumericUpDown
    Friend WithEvents Button1 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents SettingsPanel As Panel
End Class
