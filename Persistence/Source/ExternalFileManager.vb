Imports System.IO

Public Class ExternalFileManager

    Private Const ExtraFileFolder As String = "Extra"
    Private ReadOnly _projectFolder As String

    Sub New(projectFolder As String)
        _projectFolder = projectFolder
        If Not Directory.Exists(_projectFolder) Then
            Directory.CreateDirectory(_projectFolder)
        End If
    End Sub

    Function Add(originalFilePath As String, wellName As String) As String
        Dim filename = Path.GetFileName(originalFilePath)
        Dim destinationFolder = Path.Combine(_projectFolder, ExtraFileFolder, wellName)
        If Not Directory.Exists(destinationFolder) Then
            Directory.CreateDirectory(destinationFolder)
        End If
        Dim destinationFilename = Path.Combine(destinationFolder, filename)
        File.Copy(originalFilePath, destinationFilename, True)
        Return destinationFilename
    End Function

    Sub RemoveFile(filename As String)
        If File.Exists(filename) Then
            Try
                File.Delete(filename)
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub

    Sub RemoveWellFolder(wellName As String)
        Dim folderPath = Path.Combine(_projectFolder, ExtraFileFolder, wellName)
        If Directory.Exists(folderPath) Then
            Try
                Directory.Delete(folderPath, True)
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub
End Class
