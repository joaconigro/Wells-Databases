Imports System.IO

Public Class ExternalFileManager

    Private Const ExtraFileFolder As String = "Extra"
    Private _projectFolder As String

    Sub New(projectFolder As String)
        _projectFolder = projectFolder
        If Not Directory.Exists(_projectFolder) Then
            Directory.CreateDirectory(_projectFolder)
        End If
    End Sub

    Sub Add(originalFilePath As String, databasePath As String, wellName As String)
        Dim filename = Path.GetFileName(originalFilePath)
        Dim destinationFolder = Path.Combine(databasePath, ExtraFileFolder, wellName)
        If Not Directory.Exists(destinationFolder) Then
            Directory.CreateDirectory(destinationFolder)
        End If
        Dim destinationFilename = Path.Combine(destinationFolder, filename)
        File.Copy(originalFilePath, destinationFilename, True)
    End Sub

    Sub RemoveFile(filename As String)
        If File.Exists(filename) Then
            Try
                File.Delete(filename)
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub

    Sub RemoveWellFolder(databasePath As String, wellName As String)
        Dim folderPath = Path.Combine(databasePath, ExtraFileFolder, wellName)
        If Directory.Exists(folderPath) Then
            Try
                Directory.Delete(folderPath, True)
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub
End Class
