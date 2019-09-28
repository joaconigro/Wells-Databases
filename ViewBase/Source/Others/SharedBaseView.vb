﻿Imports System.Windows

Public Class SharedBaseView
    Inherits Window

    Public Shared Sub ShowErrorMessageBox(owner As Window, message As String)
        MessageBox.Show(owner, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
    End Sub

    Public Shared Function OpenFileDialog(filter As String, title As String, Optional initialDirectory As String = "") As String
        Dim ofd As New Microsoft.Win32.OpenFileDialog With {.Filter = filter, .Title = title, .InitialDirectory = initialDirectory}
        If ofd.ShowDialog = True Then
            Return ofd.FileName
        End If
        Return Nothing
    End Function

    Public Shared Function OpenMultipleFileDialog(filter As String, title As String) As List(Of String)
        Dim ofd As New Microsoft.Win32.OpenFileDialog With {.Filter = filter, .Title = title, .Multiselect = True}
        If ofd.ShowDialog = True Then
            Return ofd.FileNames.ToList
        End If
        Return Nothing
    End Function

    Public Shared Function SaveFileDialog(filter As String, title As String, Optional filename As String = "", Optional initialDirectory As String = "") As String
        Dim sfd As New Microsoft.Win32.SaveFileDialog With {.Filter = filter, .Title = title, .FileName = filename, .InitialDirectory = initialDirectory}
        If sfd.ShowDialog = True Then
            Return sfd.FileName
        End If
        Return Nothing
    End Function

    Public Shared Function ShowMessageBox(owner As Window, message As String, title As String) As Boolean
        Return MessageBox.Show(owner, message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes
    End Function

    Public Shared Sub ShowOkOnkyMessageBox(owner As Window, message As String, title As String)
        MessageBox.Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    Public Shared Function ShowInputBox(prompt As String, Optional title As String = "", Optional defaultResponse As String = "") As String
        Return Interaction.InputBox(prompt, title, defaultResponse)
    End Function

    Public Shared Function ShowFolderSelectionDialog() As String
        Using diag As New Forms.FolderBrowserDialog
            If diag.ShowDialog = Forms.DialogResult.OK Then
                Return diag.SelectedPath
            End If
        End Using
        Return Nothing
    End Function

End Class