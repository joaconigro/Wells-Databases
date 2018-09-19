Imports NPOI.SS.UserModel
Imports Wells.Model

Public Class ExcelReader


    Shared Function ReadWells(workbook As IWorkbook, sheetIndex As Integer) As List(Of Well)
        Dim sheet = workbook.GetSheetAt(sheetIndex)
        Dim row As IRow
        Dim wells As New List(Of Well)

        For i = 1 To sheet.LastRowNum
            row = sheet.GetRow(i)
            Dim well As New Well()

            well.Name = ReadCellAsString(row, 0).ToUpper
            well.Location.X = ReadCellAsDouble(row, 1)
            well.Location.Y = ReadCellAsDouble(row, 2)
            well.Location.Z = ReadCellAsDouble(row, 3)
            well.Location.Latitude = ReadCellAsDouble(row, 4)
            well.Location.Longitude = ReadCellAsDouble(row, 5)
            Dim wellType = ReadCellAsDouble(row, 6)
            well.Type = If(wellType = 1, Model.WellType.Sounding, Model.WellType.MeasurementWell)
            well.Height = ReadCellAsDouble(row, 7)
            Dim exists = ReadCellAsString(row, 8).ToUpper
            If Not String.IsNullOrEmpty(exists) Then
                well.Exists = If(exists = "SI", True, False)
            End If
            well.Bottom = ReadCellAsDouble(row, 9)

            wells.Add(well)
        Next

        Return wells
    End Function

    Shared Function ReadMeasurements(workbook As IWorkbook, sheetIndex As Integer) As List(Of Measurement)
        Dim sheet = workbook.GetSheetAt(sheetIndex)
        Dim row As IRow
        Dim measurements As New List(Of Measurement)
        Dim indexError As Integer
        Try
            For i = 1 To sheet.LastRowNum - 1
                indexError = i
                row = sheet.GetRow(i)
                Dim meas As New Measurement
                meas.WellName = ReadCellAsString(row, 0).ToUpper
                meas.SampleDate = ReadCellAsDateString(row, 1)
                meas.FLNADepth = ReadCellAsDouble(row, 2)
                meas.WaterDepth = ReadCellAsDouble(row, 3)
                meas.Caudal = ReadCellAsDouble(row, 4)
                meas.Comment = ReadCellAsString(row, 5)

                measurements.Add(meas)
            Next

        Catch ex As Exception
            Throw New Exception("Error en la fila " & indexError)
        End Try

        Return measurements
    End Function

    Private Shared Function ReadCellAsString(row As IRow, cellIndex As Integer) As String
        Dim cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL)
        If cell IsNot Nothing Then
            Select Case cell.CellType
                Case CellType.Numeric, CellType.Formula
                    Return cell.NumericCellValue.ToString
                Case Else
                    Return cell.StringCellValue
            End Select
        End If
        Return ""
    End Function

    Private Shared Function ReadCellAsDateString(row As IRow, cellIndex As Integer) As String
        Dim cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL)
        If cell IsNot Nothing Then
            Select Case cell.CellType
                Case CellType.Numeric, CellType.Formula
                    Return cell.DateCellValue.ToShortDateString.ToUpper
                Case Else
                    Return cell.StringCellValue.ToUpper
            End Select
        End If
        Return ""
    End Function

    Private Shared Function ReadCellAsDouble(row As IRow, cellIndex As Integer) As Double
        Dim cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL)
        If cell IsNot Nothing Then
            Select Case cell.CellType
                Case CellType.Numeric, CellType.Formula
                    Return cell.NumericCellValue
                Case Else
                    Dim result As Double
                    If Double.TryParse(cell.StringCellValue, result) Then
                        Return result
                    End If
            End Select
        End If
        Return BusinessObject.NullNumericValue
    End Function
End Class
