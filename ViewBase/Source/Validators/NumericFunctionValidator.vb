Imports System.ComponentModel
Imports FluentValidation

Public Class NumericFunctionValidator
    Inherits AbstractValidator(Of Double)

    Private _FuntionType As NumericFunctions
    Private _LimitValue As Double

    Private _LowerFunctionRange As NumericFunctions
    Private _UpperFunctionRange As NumericFunctions
    Private _LowerValue As Double
    Private _UpperValue As Double

    Sub New(propertyDisplayName As String, functionType As NumericFunctions, limitValue As Double)
        _FuntionType = functionType
        _LimitValue = limitValue

        RuleFor(Function(number) number).
            Must(Function(number) CompareValue(number)).WithMessage($"{propertyDisplayName}: el valor no es correcto.")
    End Sub


    Sub New(propertyDisplayName As String, lowerLimit As Double, inclusiveLowerLimit As Boolean, upperLimit As Double, inclusiveUpperLimit As Boolean)
        _LowerFunctionRange = If(inclusiveLowerLimit, NumericFunctions.GreaterOrEqual, NumericFunctions.Greater)
        _UpperFunctionRange = If(inclusiveUpperLimit, NumericFunctions.LowerOrEqual, NumericFunctions.Lower)
        _LowerValue = lowerLimit
        _UpperValue = upperLimit

        RuleFor(Function(number) number).
            Must(Function(number) CompareRange(number)).WithMessage($"{propertyDisplayName}: el valor está en el rango esperado.")
    End Sub

    Private Function CompareRange(aValue As Double) As Boolean
        Dim lowerResult As Boolean
        If _LowerFunctionRange = NumericFunctions.Greater Then
            lowerResult = aValue > _LowerValue
        Else
            lowerResult = aValue >= _LowerValue
        End If

        Dim upperResult As Boolean
        If _UpperFunctionRange = NumericFunctions.Lower Then
            upperResult = aValue < _UpperValue
        Else
            upperResult = aValue <= _UpperValue
        End If

        Return lowerResult AndAlso upperResult
    End Function

    Private Function CompareValue(aValue As Double) As Boolean
        Select Case _FuntionType
            Case NumericFunctions.Equal
                Return _LimitValue = Math.Round(aValue)
            Case NumericFunctions.Greater
                Return aValue > _LimitValue
            Case NumericFunctions.GreaterOrEqual
                Return aValue >= _LimitValue
            Case NumericFunctions.Lower
                Return aValue < _LimitValue
            Case NumericFunctions.LowerOrEqual
                Return aValue <= _LimitValue
            Case Else
                Return False
        End Select
    End Function
End Class

Public Enum NumericFunctions
    <Description("Igual")> Equal
    <Description("Menor")> Lower
    <Description("Menor o igual")> LowerOrEqual
    <Description("Mayor")> Greater
    <Description("Mayor o igual")> GreaterOrEqual
End Enum

