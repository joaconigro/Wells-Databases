Public Class WaitingView

    Property Message As String
        Get
            Return MessageTextBlock.Text
        End Get
        Set(value As String)
            MessageTextBlock.Text = value
        End Set
    End Property

    Sub New(message As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Message = message
    End Sub
End Class
