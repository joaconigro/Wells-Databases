Imports Wells.YPFModel
Imports Wells.CorePersistence
Imports Wells.CorePersistence.Repositories
Imports Wells.StandardModel.Models

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
        Torches
    End Enum

    Private _measurementPropeties As Dictionary(Of String, String)

    Private _flnaAnalysisPropeties As Dictionary(Of String, String)

    Private _soilAnalysisPropeties As Dictionary(Of String, String)

    Private _waterAnalysisPropeties As Dictionary(Of String, String)

    Private _precipitationPropeties As Dictionary(Of String, String)

    ReadOnly Property PropertiesNames As List(Of String)
        Get
            Select Case ShowedDatasource
                Case DatasourceType.Measurements
                    Return _measurementPropeties.Keys.ToList
                Case DatasourceType.FLNAAnalysis
                    Return _flnaAnalysisPropeties.Keys.ToList
                Case DatasourceType.SoilAnalysis
                    Return _soilAnalysisPropeties.Keys.ToList
                Case DatasourceType.WaterAnalysis
                    Return _waterAnalysisPropeties.Keys.ToList
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
                ParameterFilter = 0
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

    Protected _repositories As RepositoryWrapper

    Private Sub CreatePropertiesDictionaries()
        _measurementPropeties = New Dictionary(Of String, String) From {{"Ninguna", "None"}}
        For Each k In Measurement.Properties
            _measurementPropeties.Add(k.Key, k.Value)
        Next

        _precipitationPropeties = New Dictionary(Of String, String) From {{"Ninguna", "None"}}
        For Each k In Precipitation.Propeties
            _precipitationPropeties.Add(k.Key, k.Value)
        Next

        _flnaAnalysisPropeties = New Dictionary(Of String, String) From {{"Ninguna", "None"}}
        For Each k In FLNAAnalysis.Properties
            _flnaAnalysisPropeties.Add(k.Key, k.Value)
        Next

        _soilAnalysisPropeties = New Dictionary(Of String, String) From {{"Ninguna", "None"}}
        For Each k In SoilAnalysis.Properties
            _soilAnalysisPropeties.Add(k.Key, k.Value)
        Next

        _waterAnalysisPropeties = New Dictionary(Of String, String) From {{"Ninguna", "None"}}
        For Each k In WaterAnalysis.Properties
            _waterAnalysisPropeties.Add(k.Key, k.Value)
        Next
    End Sub

    Sub New()
        _repositories = RepositoryWrapper.Instance
        CreatePropertiesDictionaries()
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
            Case DatasourceType.FLNAAnalysis
                Return FLNAAnalysisApply(_repositories.ChemicalAnalysis)
            Case DatasourceType.WaterAnalysis
                Return WaterAnalysisApply(_repositories.ChemicalAnalysis)
            Case DatasourceType.SoilAnalysis
                Return SoilAnalysisApply(_repositories.ChemicalAnalysis)
            Case Else
                Return PrecipitationsApply(_repositories.Precipitations)
        End Select
    End Function

    Private Function WellApply(repo As WellsRepository) As List(Of Well)
        Select Case WellFilter
            Case WellQuery.ByName
                If String.IsNullOrEmpty(SelectedWellName) Then
                    Return repo.All
                Else
                    Return {repo.FindByName(SelectedWellName)}.ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                Return (From w In repo.All
                        Where w.WellType = WellType.MeasurementWell
                        Select w).ToList
            Case WellQuery.OnlySounding
                Return (From w In repo.All
                        Where w.WellType = WellType.Sounding
                        Select w).ToList
            Case WellQuery.ZoneA
                Return (From w In repo.All
                        Where Rectangle2D.ZoneA.Contains(w)
                        Select w).ToList
            Case WellQuery.ZoneB
                Return (From w In repo.All
                        Where Rectangle2D.ZoneB.Contains(w)
                        Select w).ToList
            Case WellQuery.ZoneC
                Return (From w In repo.All
                        Where Rectangle2D.ZoneC.Contains(w)
                        Select w).ToList
            Case WellQuery.ZoneD
                Return (From w In repo.All
                        Where Rectangle2D.ZoneD.Contains(w)
                        Select w).ToList
            Case WellQuery.Torches
                Return (From w In repo.All
                        Where Rectangle2D.Torches.Contains(w)
                        Select w).ToList
            Case Else
                Return repo.All
        End Select
    End Function

    Private Function MeasurementApply(repo As MeasurementsRepository) As List(Of Measurement)
        Dim list As List(Of Measurement)
        Select Case WellFilter
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
                        Where m.Well.WellType = WellType.MeasurementWell
                        Select m).ToList
            Case WellQuery.OnlySounding
                list = (From m In repo.All
                        Where m.Well.WellType = WellType.Sounding
                        Select m).ToList
            Case WellQuery.ZoneA
                list = (From e In repo.All
                        Where Rectangle2D.ZoneA.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneB
                list = (From e In repo.All
                        Where Rectangle2D.ZoneB.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneC
                list = (From e In repo.All
                        Where Rectangle2D.ZoneC.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneD
                list = (From e In repo.All
                        Where Rectangle2D.ZoneD.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.Torches
                list = (From e In repo.All
                        Where Rectangle2D.Torches.Contains(e.Well)
                        Select e).ToList
            Case Else
                list = repo.All
        End Select

        Dim datedList = (From m In list
                         Where m.Date >= StartDate AndAlso m.Date <= EndDate
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

    Private Function FLNAAnalysisApply(repo As AnalysesRepository) As List(Of FLNAAnalysis)
        Dim list As List(Of FLNAAnalysis) = (From c In repo.FindAll(Function(a) a.SampleOf = SampleType.FLNA)
                                             Select CType(c, FLNAAnalysis)).ToList
        Dim dict = _flnaAnalysisPropeties

        Select Case WellFilter
            Case WellQuery.ByName
                If Not String.IsNullOrEmpty(SelectedWellName) Then
                    list = (From e In list
                            Where e.Well.Name = SelectedWellName
                            Select e).ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                list = (From e In list
                        Where e.Well.WellType = WellType.MeasurementWell
                        Select e).ToList
            Case WellQuery.OnlySounding
                list = (From e In list
                        Where e.Well.WellType = WellType.Sounding
                        Select e).ToList
            Case WellQuery.ZoneA
                list = (From e In list
                        Where Rectangle2D.ZoneA.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneB
                list = (From e In list
                        Where Rectangle2D.ZoneB.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneC
                list = (From e In list
                        Where Rectangle2D.ZoneC.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneD
                list = (From e In list
                        Where Rectangle2D.ZoneD.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.Torches
                list = (From e In list
                        Where Rectangle2D.Torches.Contains(e.Well)
                        Select e).ToList
        End Select

        Dim datedList = (From e In list
                         Where e.Date >= StartDate AndAlso e.Date <= EndDate
                         Select e).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso dict(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In datedList
                                Where filterFunction(e, dict(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return datedList
        End If

    End Function

    Private Function WaterAnalysisApply(repo As AnalysesRepository) As List(Of WaterAnalysis)
        Dim list As List(Of WaterAnalysis) = (From c In repo.FindAll(Function(a) a.SampleOf = SampleType.Water)
                                              Select CType(c, WaterAnalysis)).ToList
        Dim dict = _waterAnalysisPropeties

        Select Case WellFilter
            Case WellQuery.ByName
                If Not String.IsNullOrEmpty(SelectedWellName) Then
                    list = (From e In list
                            Where e.Well.Name = SelectedWellName
                            Select e).ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                list = (From e In list
                        Where e.Well.WellType = WellType.MeasurementWell
                        Select e).ToList
            Case WellQuery.OnlySounding
                list = (From e In list
                        Where e.Well.WellType = WellType.Sounding
                        Select e).ToList
            Case WellQuery.ZoneA
                list = (From e In list
                        Where Rectangle2D.ZoneA.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneB
                list = (From e In list
                        Where Rectangle2D.ZoneB.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneC
                list = (From e In list
                        Where Rectangle2D.ZoneC.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneD
                list = (From e In list
                        Where Rectangle2D.ZoneD.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.Torches
                list = (From e In list
                        Where Rectangle2D.Torches.Contains(e.Well)
                        Select e).ToList
        End Select

        Dim datedList = (From e In list
                         Where e.Date >= StartDate AndAlso e.Date <= EndDate
                         Select e).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso dict(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In datedList
                                Where filterFunction(e, dict(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return datedList
        End If

    End Function

    Private Function SoilAnalysisApply(repo As AnalysesRepository) As List(Of SoilAnalysis)
        Dim list As List(Of SoilAnalysis) = (From c In repo.FindAll(Function(a) a.SampleOf = SampleType.Soil)
                                             Select CType(c, SoilAnalysis)).ToList
        Dim dict = _soilAnalysisPropeties

        Select Case WellFilter
            Case WellQuery.ByName
                If Not String.IsNullOrEmpty(SelectedWellName) Then
                    list = (From e In list
                            Where e.Well.Name = SelectedWellName
                            Select e).ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                list = (From e In list
                        Where e.Well.WellType = WellType.MeasurementWell
                        Select e).ToList
            Case WellQuery.OnlySounding
                list = (From e In list
                        Where e.Well.WellType = WellType.Sounding
                        Select e).ToList
            Case WellQuery.ZoneA
                list = (From e In list
                        Where Rectangle2D.ZoneA.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneB
                list = (From e In list
                        Where Rectangle2D.ZoneB.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneC
                list = (From e In list
                        Where Rectangle2D.ZoneC.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneD
                list = (From e In list
                        Where Rectangle2D.ZoneD.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.Torches
                list = (From e In list
                        Where Rectangle2D.Torches.Contains(e.Well)
                        Select e).ToList
        End Select

        Dim datedList = (From e In list
                         Where e.Date >= StartDate AndAlso e.Date <= EndDate
                         Select e).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso dict(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In datedList
                                Where filterFunction(e, dict(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return datedList
        End If

    End Function


    Private Function PrecipitationsApply(repo As PrecipitationsRepository) As List(Of Precipitation)

        Dim list = (From e In repo.All
                    Where e.PrecipitationDate >= StartDate AndAlso e.PrecipitationDate <= EndDate
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
