using Microsoft.VisualBasic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Wells.BaseModel.Models;
using Wells.Model;

namespace Wells.View.Importer
{
    public static class ExcelReader
    {
        static readonly string _OriginalRow = "Fila original";
        static readonly string _Reason = "Razón";

        public static List<IRow> RejectedRows { get; private set; }

        public static List<Well> ReadWells(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            Dictionary<string, Well> wells = Persistence.Repositories.RepositoryWrapper.Instance.Wells.Wells;
            var newWells = new List<Well>();
            int indexError = 1;
            CreateRejectedRows(sheet.GetRow(0));

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var well = new Well
                    {
                        Name = ReadCellAsString(row, 0).ToUpper(CultureInfo.InvariantCulture),
                        Latitude = ReadCellAsDouble(row, 1),
                        Longitude = ReadCellAsDouble(row, 2),
                        Z = ReadCellAsDouble(row, 3),
                    };
                    well.Height = ReadCellAsDouble(row, 4);
                    var exists = ReadCellAsString(row, 5).ToUpper(CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(exists))
                    {
                        well.Exists = exists == "SI" ? true : false;
                    }
                    well.Bottom = ReadCellAsDouble(row, 6);

                    if (IsValid(well, wells, out var reason))
                    {
                        wells.Add(well.Name, well);
                        newWells.Add(well);
                    }
                    else
                    {
                        RejectedRows.Add(CreateRejectedRow(row, 7, i, reason));
                    }

                    progress?.Report(i / maxCount * 100);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error leyendo la fila " + indexError.ToString(), ex);
            }

            return newWells;
        }      

        static bool IsValid(Well well, Dictionary<string, Well> wells, out RejectedReasons reason)
        {
            reason = RejectedReasons.Unknown;
            if (!string.IsNullOrEmpty(well.Name) && !wells.ContainsKey(well.Name))
            {
                reason = RejectedReasons.None;
                return true;
            }
            else if (string.IsNullOrEmpty(well.Name))
            {
                reason = RejectedReasons.WellNameEmpty;
            }
            else if (wells.ContainsKey(well.Name))
            {
                reason = RejectedReasons.DuplicatedName;
            }
            return false;
        }

        public static List<Precipitation> ReadPrecipitations(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            List<Precipitation> precipitations = new List<Precipitation>();
            int indexError = 1;
            CreateRejectedRows(sheet.GetRow(0));

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var prec = new Precipitation
                    {
                        Date = ParseStringDate(ReadCellAsDateString(row, 0)),
                        Millimeters = ReadCellAsDouble(row, 1)
                    };


                    prec.Millimeters = prec.Millimeters == BusinessObject.NumericNullValue ? 0 : prec.Millimeters;

                    precipitations.Add(prec);

                    progress?.Report(i / maxCount * 100);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error leyendo la fila " + indexError.ToString(), ex);
            }

            return precipitations;
        }


        public static List<Measurement> ReadMeasurements(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            List<Measurement> measurements = new List<Measurement>();
            int indexError = 1;
            CreateRejectedRows(sheet.GetRow(0));

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var meas = new Measurement
                    {
                        Well = GetWell(ReadCellAsString(row, 0)?.ToUpper(CultureInfo.InvariantCulture)),
                        Date = ParseStringDate(ReadCellAsDateString(row, 1)),
                        WaterDepth = ReadCellAsDouble(row, 2),
                        Caudal = ReadCellAsDouble(row, 3),
                        Comment = ReadCellAsString(row, 4)
                    };

                    if (meas.Well != null)
                    {
                        measurements.Add(meas);
                    }
                    else
                    {
                        RejectedRows.Add(CreateRejectedRow(row, 6, i, RejectedReasons.WellNotFound));
                    }

                    progress?.Report(i / maxCount * 100);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error leyendo la fila " + indexError.ToString(), ex);
            }

            return measurements;
        }

        public static List<WaterAnalysis> ReadWaterAnalysis(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            List<WaterAnalysis> analyses = new List<WaterAnalysis>();
            int indexError = 1;
            CreateRejectedRows(sheet.GetRow(0));

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var analysis = new WaterAnalysis
                    {
                        Well = GetWell(ReadCellAsString(row, 0)?.ToUpper(CultureInfo.InvariantCulture)),
                        Date = ParseStringDate(ReadCellAsDateString(row, 1))
                    };

                    int j = 2;
                    foreach (var p in WaterAnalysis.DoubleProperties.Keys.ToList())
                    {
                        Interaction.CallByName(analysis, WaterAnalysis.DoubleProperties[p].Name, CallType.Set, ReadCellAsDouble(row, j));
                        j += 1;
                    }

                    if (analysis.Well != null)
                    {
                        analyses.Add(analysis);
                    }
                    else
                    {
                        RejectedRows.Add(CreateRejectedRow(row, j, i, RejectedReasons.WellNotFound));
                    }

                    progress?.Report(i / maxCount * 100);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error leyendo la fila " + indexError.ToString(), ex);
            }

            return analyses;
        }


        public static void ExportRejected(string filename)
        {
            if (RejectedRows != null && RejectedRows.Count > 1)
            {
                //Create Excel workbook
                var wb = new XSSFWorkbook();

                //Create Excel sheet
                var sheet = wb.CreateSheet("Rechazados");

                var dateFormat = wb.CreateDataFormat();
                var style = wb.CreateCellStyle();
                style.DataFormat = dateFormat.GetFormat("m/d/yy");

                int rowIndex = 0;
                foreach (var r in RejectedRows)
                {
                    var newRow = sheet.CreateRow(rowIndex);
                    foreach (var c in r.Cells)
                    {
                        var newCell = newRow.CreateCell(c.ColumnIndex);
                        switch (c.CellType)
                        {
                            case CellType.Numeric:
                            case CellType.Formula:
                                bool useDateValue = DateUtil.IsCellDateFormatted(c);
                                if (useDateValue)
                                {
                                    newCell.SetCellValue(c.DateCellValue);
                                    newCell.CellStyle = style;
                                }
                                else
                                {
                                    newCell.SetCellValue(c.NumericCellValue);
                                }
                                break;
                            default:
                                newCell.SetCellValue(c.StringCellValue);
                                break;
                        }
                    }
                    rowIndex += 1;
                }

                using var stream = File.Open(filename, FileMode.Create, FileAccess.Write);
                wb.Write(stream);
                wb.Close();
                RejectedRows.Clear();
            }
        }

        static void CreateRejectedRows(IRow header)
        {
            RejectedRows = new List<IRow>
            {
                CreateRejectedRow(header, _OriginalRow, _Reason)
            };
        }

        static IRow CreateRejectedRow(IRow row, int column, int originalRow, RejectedReasons reason)
        {
            row.CreateCell(column, CellType.Numeric).SetCellValue(originalRow + 1);
            row.CreateCell(column + 1, CellType.String).SetCellValue(Base.Common.GetEnumDescription(reason));
            return row;
        }

        static IRow CreateRejectedRow(IRow row, string originalRow, string reason)
        {
            int column = row.LastCellNum;
            row.CreateCell(column).SetCellValue(originalRow);
            row.CreateCell(column + 1).SetCellValue(reason);
            return row;
        }



        public static void ExportEntities<T>(IEnumerable<T> entities, string filename)
        {
            //Create Excel workbook
            var wb = new XSSFWorkbook();

            //Create Excel sheet
            var sheet = wb.CreateSheet("Datos");

            var dateFormat = wb.CreateDataFormat();
            var style = wb.CreateCellStyle();
            style.DataFormat = dateFormat.GetFormat("m/d/yy");

            if (typeof(T) == typeof(Well))
            {
                ExportWells(entities.Cast<Well>().ToList(), sheet);
            }
            else if (typeof(T) == typeof(Measurement))
            {
                ExportMeasurements(entities.Cast<Measurement>().ToList(), sheet, style);
            }
            else if (typeof(T) == typeof(Precipitation))
            {
                ExportPrecipitations(entities.Cast<Precipitation>().ToList(), sheet, style);
            }
            else if (typeof(T) == typeof(WaterAnalysis))
            {
                ExportWaterAnalyses(entities.Cast<WaterAnalysis>().ToList(), sheet, style);
            }

            using var stream = File.Open(filename, FileMode.Create, FileAccess.Write);

            wb.Write(stream);
            wb.Close();
        }

        static IRow CreateAnalysisRow(ChemicalAnalysis analysis, ISheet sheet, int rowIndex, ICellStyle cellDateStyle)
        {
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue(analysis.WellName);
            var cell = row.CreateCell(1, CellType.Numeric);
            cell.SetCellValue(analysis.Date);
            cell.CellStyle = cellDateStyle;
            return row;
        }

        #region Export Water analyses
        static void ExportWaterAnalyses(List<WaterAnalysis> waterAnalyses, ISheet sheet, ICellStyle cellDateStyle)
        {
            WriteWaterAnalysisHeader(sheet);
            int row = 1;
            foreach (var w in waterAnalyses)
            {
                WriteWaterAnalysis(w, sheet, row, cellDateStyle);
                row++;
            }
        }

        static void WriteWaterAnalysisHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);

            header.CreateCell(0).SetCellValue(WaterAnalysis.GetDisplayName(nameof(WaterAnalysis.WellName)));
            header.CreateCell(1).SetCellValue(WaterAnalysis.GetDisplayName(nameof(WaterAnalysis.Date)));

            int i = 2;
            foreach (var p in WaterAnalysis.DoubleProperties.Keys.ToList())
            {
                header.CreateCell(i).SetCellValue(p);
                i += 1;
            }
        }


        static void WriteWaterAnalysis(WaterAnalysis analysis, ISheet sheet, int rowIndex, ICellStyle cellDateStyle)
        {
            var row = CreateAnalysisRow(analysis, sheet, rowIndex, cellDateStyle);

            int i = 2;
            foreach (var p in WaterAnalysis.DoubleProperties.Keys.ToList())
            {
                row.CreateCell(i, CellType.Numeric).SetCellValue((double)Interaction.CallByName(analysis, WaterAnalysis.DoubleProperties[p].Name, CallType.Get));
                i += 1;
            }
        }
        #endregion

        #region Export precipitations
        static void ExportPrecipitations(List<Precipitation> precipitations, ISheet sheet, ICellStyle cellDateStyle)
        {
            WritePrecipitationHeader(sheet);
            int row = 1;
            foreach (var w in precipitations)
            {
                WritePrecipitation(w, sheet, row, cellDateStyle);
                row++;
            }
        }

        static void WritePrecipitationHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);

            header.CreateCell(0).SetCellValue(Precipitation.GetDisplayName(nameof(Precipitation.Date)));
            header.CreateCell(1).SetCellValue(Precipitation.GetDisplayName(nameof(Precipitation.Millimeters)));
        }

        static void WritePrecipitation(Precipitation precipitation, ISheet sheet, int rowIndex, ICellStyle cellDateStyle)
        {
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue(precipitation.Date);
            var cell = row.CreateCell(0, CellType.Numeric);
            cell.SetCellValue(precipitation.Date);
            cell.CellStyle = cellDateStyle;
            row.CreateCell(1, CellType.Numeric).SetCellValue(precipitation.Millimeters);
        }
        #endregion

        #region Export measurements
        static void ExportMeasurements(List<Measurement> measurements, ISheet sheet, ICellStyle cellDateStyle)
        {
            WriteMeasurementHeader(sheet);
            int row = 1;
            foreach (var w in measurements)
            {
                WriteMeasurement(w, sheet, row, cellDateStyle);
                row++;
            }
        }

        static void WriteMeasurementHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);
            int i = 0;
            header.CreateCell(i).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.WellName)));
            header.CreateCell(++i).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.Date)));
            header.CreateCell(++i).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.WaterDepth)));
            header.CreateCell(++i).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.Caudal)));
            header.CreateCell(++i).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.Comment)));
        }

        static void WriteMeasurement(Measurement measurement, ISheet sheet, int rowIndex, ICellStyle cellDateStyle)
        {
            var row = sheet.CreateRow(rowIndex);
            int i = 0;
            row.CreateCell(i).SetCellValue(measurement.WellName);
            var cell= row.CreateCell(++i, CellType.Numeric);
            cell.SetCellValue(measurement.Date);
            cell.CellStyle = cellDateStyle;
            row.CreateCell(++i, CellType.Numeric).SetCellValue(measurement.WaterDepth);
            row.CreateCell(++i, CellType.Numeric).SetCellValue(measurement.Caudal);
            row.CreateCell(++i).SetCellValue(measurement.Comment);
        }
        #endregion

        #region Export Wells
        static void ExportWells(List<Well> wells, ISheet sheet)
        {
            WriteWellHeader(sheet);
            int row = 1;
            foreach (var w in wells)
            {
                WriteWell(w, sheet, row);
                row++;
            }
        }

        static void WriteWellHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);
            int i = 0;
            header.CreateCell(i).SetCellValue(Well.GetDisplayName(nameof(Well.Name)));
            header.CreateCell(++i).SetCellValue(Well.GetDisplayName(nameof(Well.Latitude)));
            header.CreateCell(++i).SetCellValue(Well.GetDisplayName(nameof(Well.Longitude)));
            header.CreateCell(++i).SetCellValue(Well.GetDisplayName(nameof(Well.Z)));
            header.CreateCell(++i).SetCellValue(Well.GetDisplayName(nameof(Well.Height)));
            header.CreateCell(++i).SetCellValue(Well.GetDisplayName(nameof(Well.Exists)));
            header.CreateCell(++i).SetCellValue(Well.GetDisplayName(nameof(Well.Bottom)));
        }

        static void WriteWell(Well well, ISheet sheet, int rowIndex)
        {
            var row = sheet.CreateRow(rowIndex);
            int i = 0;
            row.CreateCell(i).SetCellValue(well.Name);
            row.CreateCell(++i, CellType.Numeric).SetCellValue(well.Latitude);
            row.CreateCell(++i, CellType.Numeric).SetCellValue(well.Longitude);
            row.CreateCell(++i, CellType.Numeric).SetCellValue(well.Z);           
            row.CreateCell(++i, CellType.Numeric).SetCellValue(well.Height);
            row.CreateCell(++i).SetCellValue(well.Exists ? "SI" : "NO");
            row.CreateCell(++i, CellType.Numeric).SetCellValue(well.Bottom);
        }
        #endregion


        #region Common methods
        static Well GetWell(string wellName)
        {
            if (!string.IsNullOrEmpty(wellName) && Persistence.Repositories.RepositoryWrapper.Instance.Wells.Wells.ContainsKey(wellName))
            {
                return Persistence.Repositories.RepositoryWrapper.Instance.Wells.Wells[wellName];
            }
            return null;
        }

        static string ReadCellAsString(IRow row, int cellIndex)
        {
            var cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                    case CellType.Formula:
                        return cell.NumericCellValue.ToString().Trim();
                    default:
                        return cell.StringCellValue.Trim();
                }
            }
            return string.Empty;
        }

        static string ReadCellAsDateString(IRow row, int cellIndex)
        {
            var cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                    case CellType.Formula:
                        return cell.DateCellValue.ToString("dd/MM/yyyy");
                    default:
                        DateTime cellDate;
                        string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/MM/yy", "d/M/yy" };
                        var ok = DateTime.TryParseExact(cell.StringCellValue.Trim(), formats, null, DateTimeStyles.None, out cellDate);
                        if (ok) { return cellDate.ToString("dd/MM/yyyy"); }
                        break;
                }
            }
            return new DateTime(1900, 1, 1).ToString("dd/MM/yyyy");
        }

        static DateTime ParseStringDate(string date)
        {
            var values = date.Split("/");
            var intValues = values.Select(s => Convert.ToInt32(s)).ToList();
            return new DateTime(intValues[2], intValues[1], intValues[0]);
        }

        static double ReadCellAsDouble(IRow row, int cellIndex)
        {
            var cell = row.GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                    case CellType.Formula:
                        return cell.NumericCellValue;
                    default:
                        double result;
                        var stringValue = cell.StringCellValue;
                        bool isLowerThan = false;
                        if (stringValue.Contains("<"))
                        {
                            stringValue = stringValue.Replace("<", "");
                            isLowerThan = true;
                        }
                        if (double.TryParse(stringValue, out result))
                        {
                            if (isLowerThan) { result -= result * 0.1; }
                        }
                        return result;

                }
            }
            return BusinessObject.NumericNullValue;
        }
        #endregion
    }


    public enum RejectedReasons
    {
        [Description("Ninguna")] None,
        [Description("Id duplicado")] DuplicatedId,
        [Description("Nombre duplicado")] DuplicatedName,
        [Description("Pozo sin nombre")] WellNameEmpty,
        [Description("Pozo no encontrado")] WellNotFound,
        [Description("Profundidad de FLNA mayor al del agua")] FLNADepthGreaterThanWaterDepth,
        [Description("Fecha duplicada")] DuplicatedDate,
        [Description("Desconocido")] Unknown
    }
}
