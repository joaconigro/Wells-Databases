Imports System.IO
Imports NPOI.SS.UserModel
Imports NPOI.XSSF.UserModel
Imports Wells.YPFModel
Imports Wells.CorePersistence
Imports Wells.StandardModel.Models

Public Class ExcelReader

    Private Shared _OriginalRow As String = "Fila original"
    Private Shared _Reason As String = "Razón"

    Shared Function ReadWells(workbook As IWorkbook, sheetIndex As Integer, progress As IProgress(Of Integer)) As List(Of Well)
        Dim sheet = workbook.GetSheetAt(sheetIndex)
        Dim row As IRow
        Dim wells As New Dictionary(Of String, Well)

        Dim indexError As Integer
        Try
            Dim maxCount = sheet.LastRowNum
            For i = 1 To maxCount
                indexError = i
                row = sheet.GetRow(i)
                Dim well As New Well With {
                    .Name = ReadCellAsString(row, 0).ToUpper,
                    .X = ReadCellAsDouble(row, 1),
                    .Y = ReadCellAsDouble(row, 2),
                    .Z = ReadCellAsDouble(row, 3),
                    .Latitude = ReadCellAsDouble(row, 4),
                    .Longitude = ReadCellAsDouble(row, 5)
                }
                Dim wellType = ReadCellAsDouble(row, 6)
                well.WellType = If(wellType = 1, StandardModel.Models.WellType.Sounding, StandardModel.Models.WellType.MeasurementWell)
                well.Height = ReadCellAsDouble(row, 7)
                Dim exists = ReadCellAsString(row, 8).ToUpper
                If Not String.IsNullOrEmpty(exists) Then
                    well.Exists = If(exists = "SI", True, False)
                End If
                well.Bottom = ReadCellAsDouble(row, 9)

                If Not wells.ContainsKey(well.Name) AndAlso Not String.IsNullOrEmpty(well.Name) Then
                    wells.Add(well.Name, well)
                End If
                progress.Report(i / maxCount * 100)
            Next
        Catch ex As Exception
            Throw New Exception("Error leyendo la fila " & indexError)
        End Try

        Return wells.Values.ToList
    End Function

    Shared Function ReadPrecipitations(workbook As IWorkbook, sheetIndex As Integer, progress As IProgress(Of Integer)) As List(Of Precipitation)
        Dim sheet = workbook.GetSheetAt(sheetIndex)
        Dim row As IRow
        Dim precipitations As New List(Of Precipitation)
        Dim indexError As Integer
        Try
            Dim maxCount = sheet.LastRowNum
            For i = 1 To maxCount
                indexError = i
                row = sheet.GetRow(i)
                Dim prec As New Precipitation With {
                    .PrecipitationDate = ReadCellAsDateString(row, 0),
                    .Millimeters = ReadCellAsDouble(row, 1)}
                prec.Millimeters = If(prec.Millimeters = BusinessObject.NumericNullValue, 0, prec.Millimeters)

                precipitations.Add(prec)
                progress.Report(i / maxCount * 100)
            Next
        Catch ex As Exception
            Throw New Exception("Error leyendo la fila " & indexError)
        End Try

        Return precipitations
    End Function

    Private Shared Function GetWell(wellName As String) As Well
        If Not String.IsNullOrEmpty(wellName) AndAlso Repositories.RepositoryWrapper.Instance.Wells.Wells.ContainsKey(wellName) Then
            Return Repositories.RepositoryWrapper.Instance.Wells.Wells(wellName)
        End If
        Return Nothing
    End Function

    Shared Function ReadMeasurements(workbook As IWorkbook, sheetIndex As Integer, progress As IProgress(Of Integer)) As List(Of Measurement)
        Dim sheet = workbook.GetSheetAt(sheetIndex)
        Dim row As IRow
        Dim measurements As New List(Of Measurement)
        Dim indexError As Integer
        Try
            Dim maxCount = sheet.LastRowNum
            For i = 1 To maxCount
                indexError = i
                row = sheet.GetRow(i)
                Dim meas As New Measurement With {
                    .Well = GetWell(ReadCellAsString(row, 0).ToUpper),
                    .Date = ReadCellAsDateString(row, 1),
                    .FLNADepth = ReadCellAsDouble(row, 2),
                    .WaterDepth = ReadCellAsDouble(row, 3),
                    .Caudal = ReadCellAsDouble(row, 4),
                    .Comment = ReadCellAsString(row, 5)
                }

                If meas.Well IsNot Nothing Then
                    measurements.Add(meas)
                End If
                progress.Report(i / maxCount * 100)
            Next
        Catch ex As Exception
            Throw New Exception("Error leyendo la fila " & indexError)
        End Try

        Return measurements
    End Function

    Shared Function ReadFLNAAnalysis(workbook As IWorkbook, sheetIndex As Integer, progress As IProgress(Of Integer)) As List(Of FLNAAnalysis)
        Dim sheet = workbook.GetSheetAt(sheetIndex)
        Dim row As IRow
        Dim analysis As New List(Of FLNAAnalysis)
        Dim indexError As Integer
        Try
            Dim maxCount = sheet.LastRowNum
            For i = 1 To maxCount
                indexError = i
                row = sheet.GetRow(i)
                Dim flna As New FLNAAnalysis With {
                    .Well = GetWell(ReadCellAsString(row, 0).ToUpper),
                    .[Date] = ReadCellAsDateString(row, 1)
                }

                Dim j As Integer = 2
                For Each p In FLNAAnalysis.Properties.Keys.ToList
                    CallByName(flna, FLNAAnalysis.Properties(p).Name, CallType.Set, ReadCellAsDouble(row, j))
                    j += 1
                Next

                analysis.Add(flna)
                progress.Report(i / maxCount * 100)
            Next
        Catch ex As Exception
            Throw New Exception("Error leyendo la fila " & indexError)
        End Try

        Return analysis
    End Function

    Shared Function ReadWaterAnalysis(workbook As IWorkbook, sheetIndex As Integer, progress As IProgress(Of Integer)) As List(Of WaterAnalysis)
        Dim sheet = workbook.GetSheetAt(sheetIndex)
        Dim row As IRow
        Dim analysis As New List(Of WaterAnalysis)
        Dim indexError As Integer
        Try
            Dim maxCount = sheet.LastRowNum
            For i = 1 To maxCount
                indexError = i
                row = sheet.GetRow(i)
                Dim water As New WaterAnalysis With {
                    .Well = GetWell(ReadCellAsString(row, 0).ToUpper),
                    .[Date] = ReadCellAsDateString(row, 1)
                }

                Dim j As Integer = 2
                For Each p In WaterAnalysis.Properties.Keys.ToList
                    CallByName(water, WaterAnalysis.Properties(p).Name, CallType.Set, ReadCellAsDouble(row, j))
                    j += 1
                Next

                analysis.Add(water)
                progress.Report(i / maxCount * 100)
            Next
        Catch ex As Exception
            Throw New Exception("Error leyendo la fila " & indexError)
        End Try

        Return analysis
    End Function

    Shared Function ReadSoilAnalysis(workbook As IWorkbook, sheetIndex As Integer, progress As IProgress(Of Integer)) As List(Of SoilAnalysis)
        Dim sheet = workbook.GetSheetAt(sheetIndex)
        Dim row As IRow
        Dim analysis As New List(Of SoilAnalysis)
        Dim indexError As Integer
        Try
            Dim maxCount = sheet.LastRowNum
            For i = 1 To maxCount
                indexError = i
                row = sheet.GetRow(i)
                Dim soil As New SoilAnalysis With {
                    .Well = GetWell(ReadCellAsString(row, 0).ToUpper),
                    .Date = ReadCellAsDateString(row, 1)
                }

                Dim j As Integer = 2
                For Each p In SoilAnalysis.Properties.Keys.ToList
                    CallByName(soil, SoilAnalysis.Properties(p).Name, CallType.Set, ReadCellAsDouble(row, j))
                    j += 1
                Next

                analysis.Add(soil)
                progress.Report(i / maxCount * 100)
            Next
        Catch ex As Exception
            Throw New Exception("Error leyendo la fila " & indexError)
        End Try

        Return analysis
    End Function

    Shared Sub ExportRejectedToExcel(rejected As List(Of RejectedEntity), filename As String)
        Try
            Using stream = File.Open(filename, FileMode.Create, FileAccess.Write)
                'Create Excel workbook
                Dim wb As New XSSFWorkbook()

                'Create Excel sheet
                Dim sheet = wb.CreateSheet("Rechazados")

                Dim type = rejected.First.Entity.GetType

                If type = GetType(Well) Then
                    ExportsRejectedWells(rejected, sheet)
                ElseIf type = GetType(Measurement) Then
                    ExportsRejectedMeasurements(rejected, sheet)
                ElseIf type = GetType(Precipitation) Then
                    ExportsRejectedPrecipitations(rejected, sheet)
                ElseIf type = GetType(FLNAAnalysis) Then
                    ExportsRejectedFLNAAnalysis(rejected, sheet)
                ElseIf type = GetType(WaterAnalysis) Then
                    ExportsRejectedWaterAnalysis(rejected, sheet)
                ElseIf type = GetType(SoilAnalysis) Then
                    ExportsRejectedSoilAnalysis(rejected, sheet)
                End If

                wb.Write(stream)
                wb.Close()
            End Using

        Catch ex As Exception
            Throw New Exception("Se produjo un error al guardar los datos")
        End Try
    End Sub

    Private Shared Sub ExportsRejectedSoilAnalysis(rejected As List(Of RejectedEntity), sheet As ISheet)
        WriteSoilAnalysisHeader(sheet)

        For i = 1 To rejected.Count
            Dim r = rejected(i - 1)
            WriteSoilAnalysisToExcelRow(CType(r.Entity, SoilAnalysis), sheet, i, r.OriginalRow, r.Reason)
        Next
    End Sub

    Private Shared Sub WriteSoilAnalysisHeader(sheet As ISheet)
        Dim header = sheet.CreateRow(0)

        header.CreateCell(0).SetCellValue(SoilAnalysis.GetDisplayName(NameOf(SoilAnalysis.WellName)))
        header.CreateCell(1).SetCellValue(SoilAnalysis.GetDisplayName(NameOf(SoilAnalysis.Date)))

        Dim i As Integer = 2
        For Each p In SoilAnalysis.Properties.Keys.ToList
            header.CreateCell(i).SetCellValue(p)
            i += 1
        Next

        header.CreateCell(i).SetCellValue(_OriginalRow)
        header.CreateCell(i + 1).SetCellValue(_Reason)
    End Sub

    Private Shared Sub WriteSoilAnalysisToExcelRow(analysis As SoilAnalysis, sheet As ISheet, rowIndex As Integer, originalRowIndex As Integer, reason As RejectedReasons)
        Dim row = sheet.CreateRow(rowIndex)

        row.CreateCell(0).SetCellValue(analysis.WellName)
        row.CreateCell(1).SetCellValue(analysis.Date)

        Dim i As Integer = 2
        For Each p In SoilAnalysis.Properties.Keys.ToList
            row.CreateCell(i, CellType.Numeric).SetCellValue(CDbl(CallByName(analysis, SoilAnalysis.Properties(p).Name, CallType.Get)))
            i += 1
        Next

        row.CreateCell(i, CellType.Numeric).SetCellValue(originalRowIndex)
        row.CreateCell(i + 1, CellType.String).SetCellValue([Enum].GetName(GetType(RejectedReasons), reason))
    End Sub

    Private Shared Sub ExportsRejectedWaterAnalysis(rejected As List(Of RejectedEntity), sheet As ISheet)
        WriteWaterAnalysisHeader(sheet)

        For i = 1 To rejected.Count
            Dim r = rejected(i - 1)
            WriteWaterAnalysisToExcelRow(CType(r.Entity, WaterAnalysis), sheet, i, r.OriginalRow, r.Reason)
        Next
    End Sub

    Private Shared Sub WriteWaterAnalysisHeader(sheet As ISheet)
        Dim header = sheet.CreateRow(0)

        header.CreateCell(0).SetCellValue(WaterAnalysis.GetDisplayName(NameOf(WaterAnalysis.WellName)))
        header.CreateCell(1).SetCellValue(WaterAnalysis.GetDisplayName(NameOf(WaterAnalysis.Date)))

        Dim i As Integer = 2
        For Each p In WaterAnalysis.Properties.Keys.ToList
            header.CreateCell(i).SetCellValue(p)
            i += 1
        Next

        header.CreateCell(i).SetCellValue(_OriginalRow)
        header.CreateCell(i + 1).SetCellValue(_Reason)
    End Sub

    Private Shared Sub WriteWaterAnalysisToExcelRow(analysis As WaterAnalysis, sheet As ISheet, rowIndex As Integer, originalRowIndex As Integer, reason As RejectedReasons)
        Dim row = sheet.CreateRow(rowIndex)

        row.CreateCell(0).SetCellValue(analysis.WellName)
        row.CreateCell(1).SetCellValue(analysis.Date)

        Dim i As Integer = 2
        For Each p In WaterAnalysis.Properties.Keys.ToList
            row.CreateCell(i, CellType.Numeric).SetCellValue(CDbl(CallByName(analysis, WaterAnalysis.Properties(p).Name, CallType.Get)))
            i += 1
        Next

        row.CreateCell(i, CellType.Numeric).SetCellValue(originalRowIndex)
        row.CreateCell(i + 1, CellType.String).SetCellValue([Enum].GetName(GetType(RejectedReasons), reason))
    End Sub

    Private Shared Sub ExportsRejectedFLNAAnalysis(rejected As List(Of RejectedEntity), sheet As ISheet)
        WriteFLNAAnalysisHeader(sheet)

        For i = 1 To rejected.Count
            Dim r = rejected(i - 1)
            WriteFLNAAnalysisToExcelRow(CType(r.Entity, FLNAAnalysis), sheet, i, r.OriginalRow, r.Reason)
        Next
    End Sub

    Private Shared Sub WriteFLNAAnalysisHeader(sheet As ISheet)
        Dim header = sheet.CreateRow(0)

        header.CreateCell(0).SetCellValue(FLNAAnalysis.GetDisplayName(NameOf(FLNAAnalysis.WellName)))
        header.CreateCell(1).SetCellValue(FLNAAnalysis.GetDisplayName(NameOf(FLNAAnalysis.Date)))

        Dim i As Integer = 2
        For Each p In FLNAAnalysis.Properties.Keys.ToList
            header.CreateCell(i).SetCellValue(p)
            i += 1
        Next

        header.CreateCell(i).SetCellValue(_OriginalRow)
        header.CreateCell(i + 1).SetCellValue(_Reason)
    End Sub

    Private Shared Sub WriteFLNAAnalysisToExcelRow(analysis As FLNAAnalysis, sheet As ISheet, rowIndex As Integer, originalRowIndex As Integer, reason As RejectedReasons)
        Dim row = sheet.CreateRow(rowIndex)

        row.CreateCell(0).SetCellValue(analysis.WellName)
        row.CreateCell(1).SetCellValue(analysis.Date)

        Dim i As Integer = 2
        For Each p In FLNAAnalysis.Properties.Keys.ToList
            row.CreateCell(i, CellType.Numeric).SetCellValue(CDbl(CallByName(analysis, FLNAAnalysis.Properties(p).Name, CallType.Get)))
            i += 1
        Next

        row.CreateCell(i, CellType.Numeric).SetCellValue(originalRowIndex)
        row.CreateCell(i + 1, CellType.String).SetCellValue([Enum].GetName(GetType(RejectedReasons), reason))

    End Sub

    Private Shared Sub ExportsRejectedPrecipitations(rejected As List(Of RejectedEntity), sheet As ISheet)
        WritePrecipitationHeader(sheet)

        For i = 1 To rejected.Count
            Dim r = rejected(i - 1)
            Dim row = WritePrecipitationToExcelRow(CType(r.Entity, Precipitation), sheet, i)

            row.CreateCell(2, CellType.Numeric).SetCellValue(r.OriginalRow)
            row.CreateCell(3, CellType.String).SetCellValue([Enum].GetName(GetType(RejectedReasons), r.Reason))
        Next
    End Sub

    Private Shared Sub WritePrecipitationHeader(sheet As ISheet)
        Dim header = sheet.CreateRow(0)

        header.CreateCell(0).SetCellValue(Precipitation.GetDisplayName(NameOf(Precipitation.PrecipitationDate)))
        header.CreateCell(1).SetCellValue(Precipitation.GetDisplayName(NameOf(Precipitation.Millimeters)))
        header.CreateCell(2).SetCellValue(_OriginalRow)
        header.CreateCell(3).SetCellValue(_Reason)
    End Sub

    Private Shared Function WritePrecipitationToExcelRow(precip As Precipitation, sheet As ISheet, rowIndex As Integer) As IRow
        Dim row = sheet.CreateRow(rowIndex)

        row.CreateCell(0).SetCellValue(precip.PrecipitationDate)
        row.CreateCell(1, CellType.Numeric).SetCellValue(precip.Millimeters)

        Return row
    End Function

    Private Shared Sub ExportsRejectedMeasurements(rejected As List(Of RejectedEntity), sheet As ISheet)
        WriteMeasurementHeader(sheet)

        For i = 1 To rejected.Count
            Dim r = rejected(i - 1)
            Dim row = WriteMeasurementToExcelRow(CType(r.Entity, Measurement), sheet, i)

            row.CreateCell(6, CellType.Numeric).SetCellValue(r.OriginalRow)
            row.CreateCell(7, CellType.String).SetCellValue([Enum].GetName(GetType(RejectedReasons), r.Reason))
        Next
    End Sub

    Private Shared Sub WriteMeasurementHeader(sheet As ISheet)
        Dim header = sheet.CreateRow(0)

        header.CreateCell(0).SetCellValue(Measurement.GetDisplayName(NameOf(Measurement.WellName)))
        header.CreateCell(1).SetCellValue(Measurement.GetDisplayName(NameOf(Measurement.Date)))
        header.CreateCell(2).SetCellValue(Measurement.GetDisplayName(NameOf(Measurement.FLNADepth)))
        header.CreateCell(3).SetCellValue(Measurement.GetDisplayName(NameOf(Measurement.WaterDepth)))
        header.CreateCell(4).SetCellValue(Measurement.GetDisplayName(NameOf(Measurement.Caudal)))
        header.CreateCell(5).SetCellValue(Measurement.GetDisplayName(NameOf(Measurement.Comment)))
        header.CreateCell(6).SetCellValue(_OriginalRow)
        header.CreateCell(7).SetCellValue(_Reason)
    End Sub

    Private Shared Function WriteMeasurementToExcelRow(measurement As Measurement, sheet As ISheet, rowIndex As Integer) As IRow
        Dim row = sheet.CreateRow(rowIndex)

        row.CreateCell(0).SetCellValue(measurement.WellName)
        row.CreateCell(1).SetCellValue(measurement.Date)
        row.CreateCell(2, CellType.Numeric).SetCellValue(measurement.FLNADepth)
        row.CreateCell(3, CellType.Numeric).SetCellValue(measurement.WaterDepth)
        row.CreateCell(4, CellType.Numeric).SetCellValue(measurement.Caudal)
        row.CreateCell(5).SetCellValue(measurement.Comment)

        Return row
    End Function

    Private Shared Sub ExportsRejectedWells(rejected As List(Of RejectedEntity), sheet As ISheet)
        WriteWellHeader(sheet)

        For i = 1 To rejected.Count
            Dim r = rejected(i - 1)
            Dim row = WriteWellToExcelRow(CType(r.Entity, Well), sheet, i)

            row.CreateCell(10, CellType.Numeric).SetCellValue(r.OriginalRow)
            row.CreateCell(11, CellType.String).SetCellValue([Enum].GetName(GetType(RejectedReasons), r.Reason))
        Next
    End Sub

    Private Shared Sub WriteWellHeader(sheet As ISheet)
        Dim header = sheet.CreateRow(0)

        header.CreateCell(0).SetCellValue(Well.GetDisplayName(NameOf(Well.Name)))
        header.CreateCell(1).SetCellValue(Well.GetDisplayName(NameOf(Well.X)))
        header.CreateCell(2).SetCellValue(Well.GetDisplayName(NameOf(Well.Y)))
        header.CreateCell(3).SetCellValue(Well.GetDisplayName(NameOf(Well.Z)))
        header.CreateCell(4).SetCellValue(Well.GetDisplayName(NameOf(Well.Latitude)))
        header.CreateCell(5).SetCellValue(Well.GetDisplayName(NameOf(Well.Longitude)))
        header.CreateCell(6).SetCellValue("Tipo")
        header.CreateCell(7).SetCellValue(Well.GetDisplayName(NameOf(Well.Height)))
        header.CreateCell(8).SetCellValue(Well.GetDisplayName(NameOf(Well.Exists)))
        header.CreateCell(9).SetCellValue(Well.GetDisplayName(NameOf(Well.Bottom)))
        header.CreateCell(10).SetCellValue(_OriginalRow)
        header.CreateCell(11).SetCellValue(_Reason)
    End Sub

    Private Shared Function WriteWellToExcelRow(well As Well, sheet As ISheet, rowIndex As Integer) As IRow
        Dim row = sheet.CreateRow(rowIndex)

        row.CreateCell(0).SetCellValue(well.Name)
        row.CreateCell(1, CellType.Numeric).SetCellValue(well.X)
        row.CreateCell(2, CellType.Numeric).SetCellValue(well.Y)
        row.CreateCell(3, CellType.Numeric).SetCellValue(well.Z)
        row.CreateCell(4, CellType.Numeric).SetCellValue(well.Latitude)
        row.CreateCell(5, CellType.Numeric).SetCellValue(well.Longitude)
        row.CreateCell(6, CellType.Numeric).SetCellValue(CInt(well.WellType))
        row.CreateCell(7, CellType.Numeric).SetCellValue(well.Height)
        row.CreateCell(8).SetCellValue(If(well.Exists, "SI", "NO"))
        row.CreateCell(9, CellType.Numeric).SetCellValue(well.Bottom)

        Return row
    End Function

    Private Shared Function ReadCellAsString(row As IRow, cellIndex As Integer) As String
        Dim cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL)
        If cell IsNot Nothing Then
            Select Case cell.CellType
                Case CellType.Numeric, CellType.Formula
                    Return cell.NumericCellValue.ToString.Trim
                Case Else
                    Return cell.StringCellValue.Trim
            End Select
        End If
        Return String.Empty
    End Function

    Private Shared Function ReadCellAsDateString(row As IRow, cellIndex As Integer) As String
        Dim cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL)
        If cell IsNot Nothing Then
            Select Case cell.CellType
                Case CellType.Numeric, CellType.Formula
                    Return cell.DateCellValue.ToString("dd/MM/yyyy").ToUpper
                Case Else
                    Dim cellDate As Date
                    Dim formats() As String = {"dd/MM/yyyy", "d/M/yyyy", "dd/MM/yy", "d/M/yy"}
                    Dim ok = Date.TryParseExact(cell.StringCellValue.Trim, formats, Nothing, Globalization.DateTimeStyles.None, cellDate)
                    If ok Then
                        Return cellDate.ToString("dd/MM/yyyy").ToUpper
                    End If
            End Select
        End If
        Return New Date(1900, 1, 1).ToString("dd/MM/yyyy")
    End Function

    Private Shared Function ReadCellAsDouble(row As IRow, cellIndex As Integer) As Double
        Dim cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL)
        If cell IsNot Nothing Then
            Select Case cell.CellType
                Case CellType.Numeric, CellType.Formula
                    Return cell.NumericCellValue
                Case Else
                    Dim result As Double
                    Dim stringValue = cell.StringCellValue
                    Dim isLowerThan As Boolean = False
                    If stringValue.Contains("<") Then
                        stringValue = stringValue.Replace("<", "")
                        isLowerThan = True
                    End If
                    If Double.TryParse(stringValue, result) Then
                        If isLowerThan Then
                            result -= result * 0.1
                        End If
                        Return result
                    End If
            End Select
        End If
        Return BusinessObject.NumericNullValue
    End Function
End Class
