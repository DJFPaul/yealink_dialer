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
        Me.SettingsPanel = New System.Windows.Forms.Panel()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.PhoneConfigBox = New System.Windows.Forms.RichTextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.RichTextBox2 = New System.Windows.Forms.RichTextBox()
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
        Me.SettingsPanel.SuspendLayout()
        Me.BottomPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'SettingsPanel
        '
        Me.SettingsPanel.Controls.Add(Me.Button3)
        Me.SettingsPanel.Controls.Add(Me.Button4)
        Me.SettingsPanel.Controls.Add(Me.WebBrowser1)
        Me.SettingsPanel.Controls.Add(Me.PhoneConfigBox)
        Me.SettingsPanel.Controls.Add(Me.Button2)
        Me.SettingsPanel.Controls.Add(Me.RichTextBox2)
        Me.SettingsPanel.Location = New System.Drawing.Point(222, 3)
        Me.SettingsPanel.Name = "SettingsPanel"
        Me.SettingsPanel.Size = New System.Drawing.Size(205, 366)
        Me.SettingsPanel.TabIndex = 4
        Me.SettingsPanel.Visible = False
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Location = New System.Drawing.Point(3, 123)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(199, 50)
        Me.Button3.TabIndex = 11
        Me.Button3.Text = "Config Folder"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(3, 309)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(199, 50)
        Me.Button4.TabIndex = 11
        Me.Button4.Text = "Save"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(10000, 0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(20, 168)
        Me.WebBrowser1.TabIndex = 0
        '
        'PhoneConfigBox
        '
        Me.PhoneConfigBox.Location = New System.Drawing.Point(3, 3)
        Me.PhoneConfigBox.Name = "PhoneConfigBox"
        Me.PhoneConfigBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.PhoneConfigBox.Size = New System.Drawing.Size(238, 58)
        Me.PhoneConfigBox.TabIndex = 9
        Me.PhoneConfigBox.Text = "PHONEIP=192.168.1.100" & Global.Microsoft.VisualBasic.ChrW(10) & "SIPACCOUNT=10@192.168.1.1" & Global.Microsoft.VisualBasic.ChrW(10) & "USERNAME=admin" & Global.Microsoft.VisualBasic.ChrW(10) & "PASSWORD=admin" & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(3, 67)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(199, 50)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Test"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'RichTextBox2
        '
        Me.RichTextBox2.Location = New System.Drawing.Point(6, 3)
        Me.RichTextBox2.Name = "RichTextBox2"
        Me.RichTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.RichTextBox2.Size = New System.Drawing.Size(181, 58)
        Me.RichTextBox2.TabIndex = 11
        Me.RichTextBox2.Text = ""
        Me.RichTextBox2.Visible = False
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
        Me.SettingsPanel.ResumeLayout(False)
        Me.BottomPanel.ResumeLayout(False)
        Me.BottomPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SettingsPanel As Panel
    Friend WithEvents WebBrowser1 As WebBrowser
    Friend WithEvents IPLabel As Label
    Friend WithEvents CallButton As Button
    Friend WithEvents CloseTimer As Timer
    Friend WithEvents CallDelay As Timer
    Friend WithEvents PhoneConfigBox As RichTextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents RichTextBox2 As RichTextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
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
End Class
