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

    Protected _repositories As Repositories

    Sub New(repo As Repositories)
        _repositories = repo
    End Sub

    Function Apply() As IEnumerable(Of IBusinessObject)
        Select Case ShowedDatasource
            Case DatasourceType.Wells
                Return WellApply(_repositories.Wells)
            Case DatasourceType.Measurements
                Return MeasurementApply(_repositories.Measurements)
            Case DatasourceType.ChemicalAnalysis
                'Datasource = repo.ChemicalAnalysis.All
            Case DatasourceType.Precipitations
                ' Datasource = repo.Precipitations.All
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
                Dim list = From w In repo.All
                           Where w.Type = WellType.MeasurementWell
                           Select w

                Return list.ToList
            Case WellQuery.OnlySounding
                Dim list = From w In repo.All
                           Where w.Type = WellType.Sounding
                           Select w

                Return list.ToList
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

        Return datedList

    End Function
End Class
