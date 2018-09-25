Imports Wells.Model
Imports Wells.Persistence

Public Class BaseFilter

    Public Enum WellQuery
        All
        OnlyMeasurementWell
        OnlySounding
        ByName
        ZoneA
        ZoneB
        ZoneC
        ZoneD
    End Enum

    Private _measurementPropeties As New Dictionary(Of String, String) From {
        {"Ninguna", "None"},
        {"Profundidad FLNA", NameOf(Measurement.FLNADepth)},
        {"Profundidad Agua", NameOf(Measurement.WaterDepth)},
        {"Caudal", NameOf(Measurement.Caudal)},
        {"Espesor FLNA", NameOf(Measurement.FLNAThickness)},
        {"Cota Agua", NameOf(Measurement.WaterElevation)},
        {"Cota FLNA", NameOf(Measurement.FLNAElevation)}}

    Private _chemicalAnalysisPropeties As New Dictionary(Of String, String) From {
        {"Ninguna", "None"},
        {"Valor", NameOf(ChemicalAnalysis.Value)}}

    Private _precipitationPropeties As New Dictionary(Of String, String) From {
        {"Ninguna", "None"},
        {"Milímetros", NameOf(Precipitation.Millimeters)}}

    ReadOnly Property PropertiesNames As List(Of String)
        Get
            Select Case ShowedDatasource
                Case DatasourceType.Measurements
                    Return _measurementPropeties.Keys.ToList
                Case DatasourceType.ChemicalAnalysis
                    Return _chemicalAnalysisPropeties.Keys.ToList
                Case DatasourceType.Precipitations
                    Return _precipitationPropeties.Keys.ToList
                Case Else
                    Return New List(Of String)
            End Select
        End Get
    End Property

    Public Enum MeasurementQuery
        FLNADepth
        WaterDepth
        Caudal
        FLNAThickness
        WaterElevation
        FLNAElevation
    End Enum

    Public Enum CriteriaQuery
        ExactValue
        LessThan
        LessOrEqualThan
        GreaterThan
        GreaterOrEqualThan
    End Enum

    Event FilterChanged()

    Event DatasoureceTypeChanged()

    Private _showedDatasource As DatasourceType
    Property ShowedDatasource As DatasourceType
        Get
            Return _showedDatasource
        End Get
        Set
            If _showedDatasource <> Value Then
                _showedDatasource = Value
                My.Settings.ShowedDatasource = Value
                My.Settings.Save()
                RaiseEvent FilterChanged()
                RaiseEvent DatasoureceTypeChanged()
            End If
        End Set
    End Property

    Private _SelectedWellFilterOption As Integer
    Property SelectedWellFilterOption As Integer
        Get
            Return _SelectedWellFilterOption
        End Get
        Set
            If _SelectedWellFilterOption <> Value Then
                _SelectedWellFilterOption = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _SelectedWellName As String
    Property SelectedWellName As String
        Get
            Return _SelectedWellName
        End Get
        Set
            If _SelectedWellName <> Value Then
                _SelectedWellName = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _WellFilter As WellQuery
    Property WellFilter As WellQuery
        Get
            Return _WellFilter
        End Get
        Set
            If _WellFilter <> Value Then
                _WellFilter = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _StartDate As Date
    Property StartDate As Date
        Get
            Return _StartDate
        End Get
        Set
            If _StartDate <> Value Then
                _StartDate = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _EndDate As Date
    Property EndDate As Date
        Get
            Return _EndDate
        End Get
        Set
            If _EndDate <> Value Then
                _EndDate = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _PropertyName As String
    Property PropertyName As String
        Get
            Return _PropertyName
        End Get
        Set
            If _PropertyName <> Value Then
                _PropertyName = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _StringValue As String
    Property StringValue As String
        Get
            Return _doubleValue
        End Get
        Set
            If _StringValue <> Value Then
                _StringValue = Value
                If IsNumeric(Value) Then
                    _doubleValue = Double.Parse(Value, Globalization.NumberStyles.Any)
                Else
                    _StringValue = String.Empty
                End If
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _doubleValue As Double

    Private filterFunction As Func(Of Object, String, Double, Boolean)

    Private _parameterFilter As CriteriaQuery
    Property ParameterFilter As CriteriaQuery
        Get
            Return _parameterFilter
        End Get
        Set
            If _parameterFilter <> Value Then
                _parameterFilter = Value
                Select Case _parameterFilter
                    Case CriteriaQuery.ExactValue
                        filterFunction = AddressOf ExactValue
                    Case CriteriaQuery.LessThan
                        filterFunction = AddressOf LessThan
                    Case CriteriaQuery.LessOrEqualThan
                        filterFunction = AddressOf LessOrEqualThan
                    Case CriteriaQuery.GreaterThan
                        filterFunction = AddressOf GreaterThan
                    Case CriteriaQuery.GreaterOrEqualThan
                        filterFunction = AddressOf GreaterOrEqualThan
                End Select
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Protected _repositories As Repositories

    Sub New()
        _repositories = Repositories.Instance
        _showedDatasource = My.Settings.ShowedDatasource
        _StartDate = New Date(2000, 1, 1)
        _EndDate = Today
        filterFunction = AddressOf ExactValue
    End Sub

    Function Apply() As IEnumerable(Of IBusinessObject)
        Select Case ShowedDatasource
            Case DatasourceType.Wells
                Return WellApply(_repositories.Wells)
            Case DatasourceType.Measurements
                Return MeasurementApply(_repositories.Measurements)
            Case DatasourceType.ChemicalAnalysis
                Return ChemicalAnalysisApply(_repositories.ChemicalAnalysis)
            Case Else
                Return PrecipitationsApply(_repositories.Precipitations)
        End Select
    End Function

    Private Function WellApply(repo As WellsRepository) As List(Of Well)
        Select Case WellFilter
            Case WellQuery.All
                Return repo.All
            Case WellQuery.ByName
                If String.IsNullOrEmpty(SelectedWellName) Then
                    Return repo.All
                Else
                    Return {repo.FindName(SelectedWellName)}.ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                Return (From w In repo.All
                        Where w.Type = WellType.MeasurementWell
                        Select w).ToList
            Case WellQuery.OnlySounding
                Return (From w In repo.All
                        Where w.Type = WellType.Sounding
                        Select w).ToList
            Case Else
                Return repo.All
        End Select
    End Function

    Private Function MeasurementApply(repo As MeasurementsRepository) As List(Of Measurement)
        Dim list As List(Of Measurement)
        Select Case WellFilter
            Case WellQuery.All
                list = repo.All
            Case WellQuery.ByName
                If String.IsNullOrEmpty(SelectedWellName) Then
                    list = repo.All
                Else
                    list = (From m In repo.All
                            Where m.Well.Name = SelectedWellName
                            Select m).ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                list = (From m In repo.All
                        Where m.Well.Type = WellType.MeasurementWell
                        Select m).ToList
            Case WellQuery.OnlySounding
                list = (From m In repo.All
                        Where m.Well.Type = WellType.Sounding
                        Select m).ToList
            Case Else
                list = repo.All
        End Select

        Dim datedList = (From m In list
                         Where m.RealDate >= StartDate AndAlso m.RealDate <= EndDate
                         Select m).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso _measurementPropeties(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In datedList
                                Where filterFunction(e, _measurementPropeties(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return datedList
        End If

    End Function

    Private Function ChemicalAnalysisApply(repo As ChemicalAnalysisRepository) As List(Of ChemicalAnalysis)
        Dim list As List(Of ChemicalAnalysis)
        Select Case WellFilter
            Case WellQuery.All
                list = repo.All
            Case WellQuery.ByName
                If String.IsNullOrEmpty(SelectedWellName) Then
                    list = repo.All
                Else
                    list = (From e In repo.All
                            Where e.Well.Name = SelectedWellName
                            Select e).ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                list = (From e In repo.All
                        Where e.Well.Type = WellType.MeasurementWell
                        Select e).ToList
            Case WellQuery.OnlySounding
                list = (From e In repo.All
                        Where e.Well.Type = WellType.Sounding
                        Select e).ToList
            Case Else
                list = repo.All
        End Select

        Dim datedList = (From e In list
                         Where e.RealDate >= StartDate AndAlso e.RealDate <= EndDate
                         Select e).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso _chemicalAnalysisPropeties(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In datedList
                                Where filterFunction(e, _chemicalAnalysisPropeties(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return datedList
        End If

    End Function

    Private Function PrecipitationsApply(repo As PrecipitationsRepository) As List(Of Precipitation)

        Dim list = (From e In repo.All
                    Where e.RealDate >= StartDate AndAlso e.RealDate <= EndDate
                    Select e).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso _precipitationPropeties(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In list
                                Where filterFunction(e, _precipitationPropeties(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return list
        End If

    End Function

    Private Function ExactValue(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue = value
    End Function

    Private Function LessThan(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue < value
    End Function

    Private Function LessOrEqualThan(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue <= value
    End Function

    Private Function GreaterThan(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue > value
    End Function

    Private Function GreaterOrEqualThan(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue >= value
    End Function

End Class
