Public Class LanguageModel

    Private Shared _Instance As LanguageModel
    Private _ImportCatalog As ImportModule

    Shared ReadOnly Property Instance As LanguageModel
        Get
            If _Instance Is Nothing Then
                _Instance = New LanguageModel
            End If
            Return _Instance
        End Get
    End Property

    ReadOnly Property ImportCatalog As ImportModule
        Get
            If _ImportCatalog Is Nothing Then
                _ImportCatalog = New ImportModule
            End If
            Return _ImportCatalog
        End Get
    End Property
End Class
