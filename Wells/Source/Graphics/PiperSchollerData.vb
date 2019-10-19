Public Class PiperSchollerData

    Property Calcium As Double
    Property PotassiumAndSodium As Double
    Property Magnesium As Double
    Property Carbonate As Double
    Property Sulfates As Double
    Property Chlorides As Double
    Property Label As String
    Property PointColor As Media.Color
    Property IsVisible As Boolean

    ReadOnly Property Cation As Point
    ReadOnly Property Anion As Point
    ReadOnly Property Diamond As Point

    Sub CalculateXYPositions(lowerLeftCations As Point, lowerLeftAnions As Point, triangleSideLength As Double)
        Dim scale = triangleSideLength / 100
        _Cation.X = lowerLeftCations.X + triangleSideLength - (Calcium + Magnesium / 2) * scale
        _Cation.Y = lowerLeftCations.Y - (Math.Sqrt(3.0) * Magnesium / 2) * scale

        _Anion.X = lowerLeftAnions.X + (Chlorides + Sulfates / 2) * scale
        _Anion.Y = lowerLeftAnions.Y - (Math.Sqrt(3.0) * Sulfates / 2) * scale

        _Diamond.X = 0.5 * (Cation.X + Anion.X - (Anion.Y - Cation.Y) / Math.Sqrt(3.0))
        _Diamond.Y = 0.5 * (Anion.Y + Cation.Y - Math.Sqrt(3.0) * (Anion.X - Cation.X))
    End Sub

    Public Overrides Function ToString() As String
        Return Label
    End Function
End Class
