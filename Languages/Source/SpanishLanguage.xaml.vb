Imports System.ComponentModel.Composition
Imports System.Windows

<ExportMetadata("Culture", "es-ES")>
<Export(GetType(ResourceDictionary))>
Public Class SpanishLanguage
    Inherits ResourceDictionary

    Sub New()
        InitializeComponent()
    End Sub
End Class
