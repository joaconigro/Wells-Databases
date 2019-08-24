Imports System.ComponentModel.Composition
Imports System.Windows

<ExportMetadata("Culture", "en-US")>
<Export(GetType(ResourceDictionary))>
Public Class EnglishLanguage
    Inherits ResourceDictionary

    Sub New()
        InitializeComponent()
    End Sub
End Class
