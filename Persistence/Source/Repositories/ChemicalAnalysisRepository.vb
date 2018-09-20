Imports Wells.Model

Public Class ChemicalAnalysisRepository
    Inherits Repository(Of ChemicalAnalysis)

    Sub New(context As Context, repositories As Repositories, entities As IEnumerable(Of ChemicalAnalysis))
        MyBase.New(context, repositories, entities)
    End Sub

    Public Overrides Function Add(entity As ChemicalAnalysis) As Boolean
        If Not _repositories.Wells.ContainsName(entity.WellName) OrElse _entities.ContainsKey(entity.Id) Then
            Return False
        Else
            _entities.Add(entity.Id, entity)
            Dim well = _repositories.Wells.FindName(entity.WellName)
            well.Analysis.Add(entity)
            entity.Well = well
            Return True
        End If
    End Function
End Class
