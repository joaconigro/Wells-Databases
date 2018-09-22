Imports System.Text

Public Class ExceptionHandler
    Shared Sub Handle(ex As Exception, Optional rethrow As Boolean = True)
        Handle(ex, TraceEventType.Information, "", rethrow)
    End Sub

    Shared Sub Handle(ex As Exception, severity As TraceEventType, additionalInfo As String, Optional rethrow As Boolean = True)
        If ex IsNot Nothing Then
            My.Application.Log.WriteEntry($"Exception type {ex.GetType().Name}", severity)
            Log(ex, severity, additionalInfo)
            Dim inner = ex.InnerException
            While inner IsNot Nothing
                My.Application.Log.WriteEntry($"Exception type {ex.InnerException.GetType().Name}", severity)
                Log(ex.InnerException, severity, additionalInfo)
                inner = inner.InnerException
            End While
            If rethrow Then
                Throw ex
            End If
        End If
    End Sub

    Private Shared Sub Log(ex As Exception, severity As TraceEventType, additionalInfo As String)
        My.Application.Log.WriteEntry("", severity)
        My.Application.Log.WriteException(ex, severity, additionalInfo)
        My.Application.Log.WriteEntry(ex.StackTrace, severity)
        My.Application.Log.TraceSource.Flush()
    End Sub

    Shared Function GetString(ex As Exception) As String
        Dim msg As New StringBuilder
        msg.AppendLine(ex.Message)
        msg.AppendLine(ex.StackTrace)
        If ex.InnerException IsNot Nothing Then
            msg.AppendLine(ex.InnerException.Message)
            msg.AppendLine(ex.InnerException.StackTrace)
        End If
        Return msg.ToString
    End Function

    Shared Function GetAllMessages(ex As Exception) As String
        Dim msg As New StringBuilder
        msg.AppendLine(ex.Message)
        Dim inner = ex.InnerException
        While inner IsNot Nothing
            msg.AppendLine(inner.Message)
            inner = inner.InnerException
        End While
        Return msg.ToString
    End Function
End Class
