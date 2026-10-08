Namespace My
    ' Für MyApplication sind folgende Ereignisse verfügbar:
    ' Startup: Wird beim Starten der Anwendung noch vor dem Erstellen des Startformulars ausgelöst.
    ' Shutdown: Wird nach dem Schließen aller Anwendungsformulare ausgelöst.  Dieses Ereignis wird nicht ausgelöst, wenn die Anwendung mit einem Fehler beendet wird.
    ' UnhandledException: Wird bei einem Ausnahmefehler ausgelöst.
    ' StartupNextInstance: Wird beim Starten einer Einzelinstanzanwendung ausgelöst, wenn die Anwendung bereits aktiv ist. 
    ' NetworkAvailabilityChanged: Wird beim Herstellen oder Trennen der Netzwerkverbindung ausgelöst.
    Partial Friend Class MyApplication
        Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            e.BringToForeground = True
            Dim launch = Global.yealink_dialer.Form1.ParseLaunchArgs(e.CommandLine)
            Dim dialer = TryCast(MainForm, Global.yealink_dialer.Form1)
            If dialer Is Nothing Then Return
            If launch.EventName <> "" Then
                If dialer.InvokeRequired Then
                    dialer.BeginInvoke(New Action(Of String, String, String)(AddressOf dialer.ApplyPhoneEvent), launch.EventName, launch.Number, launch.Name)
                Else
                    dialer.ApplyPhoneEvent(launch.EventName, launch.Number, launch.Name)
                End If
            ElseIf launch.Number <> "" Then
                If dialer.InvokeRequired Then
                    dialer.BeginInvoke(New Action(Of String, String, Boolean)(AddressOf dialer.ShowIncomingNumber), launch.Number, launch.Name, launch.Incoming)
                Else
                    dialer.ShowIncomingNumber(launch.Number, launch.Name, launch.Incoming)
                End If
            End If
        End Sub
    End Class
End Namespace
