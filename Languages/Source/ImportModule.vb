Imports System.ComponentModel.Composition
Imports System.Windows

Public Class ImportModule

    <ImportMany(GetType(ResourceDictionary))>
    Property ResourceDictionaryList As IEnumerable(Of Lazy(Of ResourceDictionary, IDictionary(Of String, Object)))
End Class
