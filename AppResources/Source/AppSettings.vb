Option Strict On
Imports Wells.Base

Public Class AppSettings
    Private Sub New()

    End Sub

    Shared Sub RestoreDefaultSettings()
        My.Settings.CurrentLanguage = "es-ES"

        My.Settings.Save()
    End Sub

    Shared Sub UpgradeSettings()
        My.Settings.Upgrade()
        My.Settings.UpgradeRequired = False
        My.Settings.Save()
    End Sub

    Shared Property CurrentLanguage As String
        Get
            Return My.Settings.CurrentLanguage
        End Get
        Set(value As String)
            My.Settings.CurrentLanguage = value
            My.Settings.Save()
        End Set
    End Property

    Shared ReadOnly Property UpgradeRequired As Boolean
        Get
            Return My.Settings.UpgradeRequired
        End Get
    End Property

End Class
