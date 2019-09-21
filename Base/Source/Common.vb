Imports System.ComponentModel
Imports System.Reflection

Public Module Common
    ''' <summary>
    ''' Genera una lista a partir de las descripciones de los valores de la enumeración pasada
    ''' </summary>
    ''' <param name="enumType"></param>
    ''' <returns></returns>
    Public Function EnumDescriptionsToList(enumType As Type) As List(Of String)
        Dim descriptions As New List(Of String)
        Dim names = [Enum].GetNames(enumType).ToList
        For Each name In names
            Dim attr = CType(enumType.GetField([Enum].GetName(enumType, names.IndexOf(name))).
                        GetCustomAttribute(GetType(DescriptionAttribute)),
                        DescriptionAttribute)

            If attr IsNot Nothing Then
                descriptions.Add(attr.Description)
            Else
                descriptions.Add(name)
            End If
        Next

        Return descriptions
    End Function

    ''' <summary>
    ''' Obtiene el valor de la descripción de un valor de una enumeración. Si no tiene descripción, se devuelve el valor de la enumeración pasado a string.
    ''' </summary>
    ''' <param name="e">Valor de una enumeración</param>
    ''' <returns></returns>
    Public Function GetEnumDescription(e As [Enum]) As String
        Dim t As Type = e.GetType()
        Dim attr = CType(t.GetField([Enum].GetName(t, e)).
                        GetCustomAttribute(GetType(DescriptionAttribute)),
                        DescriptionAttribute)

        If attr IsNot Nothing Then
            Return attr.Description
        Else
            Return e.ToString
        End If
    End Function

    ''' <summary>
    ''' Obtiene el valor de la descripción de un valor de una enumeración. Si no tiene descripción, se devuelve el valor de la enumeración pasado a string.
    ''' </summary>
    ''' <param name="enumType">Tipo de enumeración</param>
    ''' <returns></returns>
    Public Function GetEnumDescription(enumType As Type, value As Integer) As String
        Return EnumDescriptionsToList(enumType)(value)
    End Function

    Public Function IsNumericType(aType As Type) As Boolean
        If aType.IsEnum Then Return False
        Dim typeCode = Type.GetTypeCode(aType)
        Select Case typeCode
            Case TypeCode.Byte, TypeCode.Char, TypeCode.Decimal, TypeCode.Double, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, TypeCode.SByte,
                  TypeCode.Single, TypeCode.UInt16, TypeCode.UInt32, TypeCode.UInt64
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Function IsIntegerNumericType(aType As Type) As Boolean
        If aType.IsEnum Then Return False
        Dim typeCode = Type.GetTypeCode(aType)
        Select Case typeCode
            Case TypeCode.Byte, TypeCode.Char, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, TypeCode.SByte,
                  TypeCode.UInt16, TypeCode.UInt32, TypeCode.UInt64
                Return True
            Case Else
                Return False
        End Select
    End Function
End Module
