using Microsoft.VisualBasic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Wells.BaseModel.Models;
using Wells.Model;
using Wells.Persistence;

namespace Wells.View.Importer
{
    public static class ExcelReader
    {
        static readonly string _OriginalRow = "Fila original";
        static readonly string _Reason = "Razón";

        public static List<RejectedEntity> RejectedEntities { get; private set; }

        public static List<Well> ReadWells(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            Dictionary<string, Well> wells = new Dictionary<string, Well>();
            int indexError = 1;
            RejectedEntities = new List<RejectedEntity>();

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var well = new Well()
                    {
                        Name = ReadCellAsString(row, 0).ToUpper(CultureInfo.InvariantCulture),
                        X = ReadCellAsDouble(row, 1),
                        Y = ReadCellAsDouble(row, 2),
                        Z = ReadCellAsDouble(row, 3),
                        Latitude = ReadCellAsDouble(row, 4),
                        Longitude = ReadCellAsDouble(row, 5)
                    };
                    var wellType = ReadCellAsDouble(row, 6);
                    well.WellType = (wellType == 1 ? WellType.Sounding : WellType.MeasurementWell);
                    well.Height = ReadCellAsDouble(row, 7);
                    var exists = ReadCellAsString(row, 8).ToUpper(CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(exists))
                    {
                        well.Exists = exists == "SI" ? true : false;
                    }
                    well.Bottom = ReadCellAsDouble(row, 9);

                    if (!string.IsNullOrEmpty(well.Name) && !wells.ContainsKey(well.Name))
                    {
                        wells.Add(well.Name, well);
                    }
                    else if (string.IsNullOrEmpty(well.Name))
                    {
                        RejectedEntities.Add(new RejectedEntity(well, i, RejectedReasons.WellNameEmpty));
                    }
                    else if (wells.ContainsKey(well.Name))
                    {
                        RejectedEntities.Add(new RejectedEntity(well, i, RejectedReasons.DuplicatedName));
                    }

                    progress?.Report(i / maxCount * 100);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error leyendo la fila " + indexError.ToString(), ex);
            }

            return wells.Values.ToList();
        }


        public static List<Precipitation> ReadPrecipitations(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            List<Precipitation> precipitations = new List<Precipitation>();
            int indexError = 1;
            RejectedEntities = new List<RejectedEntity>();

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var prec = new Precipitation()
                    {
                        PrecipitationDate = ParseStringDate(ReadCellAsDateString(row, 0)),
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
            RejectedEntities = new List<RejectedEntity>();

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var meas = new Measurement()
                    {
                        Well = GetWell(ReadCellAsString(row, 0)?.ToUpper(CultureInfo.InvariantCulture)),
                        Date = ParseStringDate(ReadCellAsDateString(row, 1)),
                        FlnaDepth = ReadCellAsDouble(row, 2),
                        WaterDepth = ReadCellAsDouble(row, 3),
                        Comment = ReadCellAsString(row, 4)
                    };

                    if (meas.Well != null)
                    {
                        measurements.Add(meas);
                    }
                    else
                    {
                        RejectedEntities.Add(new RejectedEntity(meas, i, RejectedReasons.WellNotFound));
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

        public static List<FlnaAnalysis> ReadFlnaAnalysis(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            List<FlnaAnalysis> analyses = new List<FlnaAnalysis>();
            int indexError = 1;
            RejectedEntities = new List<RejectedEntity>();

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var analysis = new FlnaAnalysis()
                    {
                        Well = GetWell(ReadCellAsString(row, 0)?.ToUpper(CultureInfo.InvariantCulture)),
                        Date = ParseStringDate(ReadCellAsDateString(row, 1))
                    };

                    int j = 2;
                    foreach (var p in FlnaAnalysis.DoubleProperties.Keys.ToList())
                    {
                        Interaction.CallByName(analysis, FlnaAnalysis.DoubleProperties[p].Name, CallType.Set, ReadCellAsDouble(row, j));
                        j += 1;
                    }

                    if (analysis.Well != null)
                    {
                        analyses.Add(analysis);
                    }
                    else
                    {
                        RejectedEntities.Add(new RejectedEntity(analysis, i, RejectedReasons.WellNotFound));
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


        public static List<WaterAnalysis> ReadWaterAnalysis(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            List<WaterAnalysis> analyses = new List<WaterAnalysis>();
            int indexError = 1;
            RejectedEntities = new List<RejectedEntity>();

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var analysis = new WaterAnalysis()
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
                        RejectedEntities.Add(new RejectedEntity(analysis, i, RejectedReasons.WellNotFound));
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


        public static List<SoilAnalysis> ReadSoilAnalysis(IWorkbook workbook, int sheetIndex, IProgress<int> progress)
        {
            var sheet = workbook.GetSheetAt(sheetIndex);
            IRow row;
            List<SoilAnalysis> analyses = new List<SoilAnalysis>();
            int indexError = 1;
            RejectedEntities = new List<RejectedEntity>();

            try
            {
                var maxCount = sheet.LastRowNum;
                for (int i = 1; i < maxCount + 1; i++)
                {
                    indexError = i;
                    row = sheet.GetRow(i);
                    var analysis = new SoilAnalysis()
                    {
                        Well = GetWell(ReadCellAsString(row, 0)?.ToUpper(CultureInfo.InvariantCulture)),
                        Date = ParseStringDate(ReadCellAsDateString(row, 1))
                    };

                    int j = 2;
                    foreach (var p in SoilAnalysis.DoubleProperties.Keys.ToList())
                    {
                        Interaction.CallByName(analysis, SoilAnalysis.DoubleProperties[p].Name, CallType.Set, ReadCellAsDouble(row, j));
                        j += 1;
                    }

                    if (analysis.Well != null)
                    {
                        analyses.Add(analysis);
                    }
                    else
                    {
                        RejectedEntities.Add(new RejectedEntity(analysis, i, RejectedReasons.WellNotFound));
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



        static Well GetWell(string wellName)
        {
            if (!string.IsNullOrEmpty(wellName) && Persistence.Repositories.RepositoryWrapper.Instance.Wells.Wells.ContainsKey(wellName))
            {
                return Persistence.Repositories.RepositoryWrapper.Instance.Wells.Wells[wellName];
            }
            return null;
        }


        public static void ExportRejectedToExcel(string filename)
        {
            try
            {
                if (RejectedEntities != null && RejectedEntities.Any())
                {
                    using (var stream = File.Open(filename, FileMode.Create, FileAccess.Write))
                    {
                        //Create Excel workbook
                        var wb = new XSSFWorkbook();

                        //Create Excel sheet
                        var sheet = wb.CreateSheet("Rechazados");

                        var type = RejectedEntities.First().Entity.GetType();

                        if (type == typeof(Well))
                        {
                            ExportsRejectedWells(RejectedEntities, sheet);
                        }
                        else if (type == typeof(Measurement))
                        {
                            ExportsRejectedMeasurements(RejectedEntities, sheet);
                        }
                        else if (type == typeof(Precipitation))
                        {
                            ExportsRejectedPrecipitations(RejectedEntities, sheet);
                        }
                        else if (type == typeof(FlnaAnalysis))
                        {
                            ExportsRejectedFLNAAnalysis(RejectedEntities, sheet);
                        }
                        else if (type == typeof(WaterAnalysis))
                        {
                            ExportsRejectedWaterAnalysis(RejectedEntities, sheet);
                        }
                        else if (type == typeof(SoilAnalysis))
                        {
                            ExportsRejectedSoilAnalysis(RejectedEntities, sheet);
                        }

                        RejectedEntities.Clear();
                        wb.Write(stream);
                        wb.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error al guardar los datos", ex);
            }
        }




        static void ExportsRejectedSoilAnalysis(List<RejectedEntity> rejected, ISheet sheet)
        {
            WriteSoilAnalysisHeader(sheet);

            for (int i = 1; i < rejected.Count; i++)
            {
                var r = rejected[i - 1];
                WriteSoilAnalysisToExcelRow(r.Entity as SoilAnalysis, sheet, i, r.OriginalRow, r.Reason);
            }
        }

        static void WriteSoilAnalysisHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);

            header.CreateCell(0).SetCellValue(SoilAnalysis.GetDisplayName(nameof(SoilAnalysis.WellName)));
            header.CreateCell(1).SetCellValue(SoilAnalysis.GetDisplayName(nameof(SoilAnalysis.Date)));

            int i = 2;
            foreach (var p in SoilAnalysis.Properties.Keys.ToList())
            {
                header.CreateCell(i).SetCellValue(p);
                i += 1;
            }

            header.CreateCell(i).SetCellValue(_OriginalRow);
            header.CreateCell(i + 1).SetCellValue(_Reason);
        }


        static void WriteSoilAnalysisToExcelRow(SoilAnalysis analysis, ISheet sheet, int rowIndex, int originalRowIndex, RejectedReasons reason)
        {
            var row = sheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue(analysis.WellName);
            row.CreateCell(1).SetCellValue(analysis.Date);

            int i = 2;
            foreach (var p in SoilAnalysis.Properties.Keys.ToList())
            {
                row.CreateCell(i, CellType.Numeric).SetCellValue((double)Interaction.CallByName(analysis, SoilAnalysis.Properties[p].Name, CallType.Get));
                i += 1;
            }

            row.CreateCell(i, CellType.Numeric).SetCellValue(originalRowIndex);
            row.CreateCell(i + 1, CellType.String).SetCellValue(Enum.GetName(typeof(RejectedReasons), reason));
        }

        static void ExportsRejectedWaterAnalysis(List<RejectedEntity> rejected, ISheet sheet)
        {
            WriteWaterAnalysisHeader(sheet);

            for (int i = 1; i < rejected.Count; i++)
            {
                var r = rejected[i - 1];
                WriteWaterAnalysisToExcelRow(r.Entity as WaterAnalysis, sheet, i, r.OriginalRow, r.Reason);
            }
        }

        static void WriteWaterAnalysisHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);

            header.CreateCell(0).SetCellValue(WaterAnalysis.GetDisplayName(nameof(WaterAnalysis.WellName)));
            header.CreateCell(1).SetCellValue(WaterAnalysis.GetDisplayName(nameof(WaterAnalysis.Date)));

            int i = 2;
            foreach (var p in WaterAnalysis.Properties.Keys.ToList())
            {
                header.CreateCell(i).SetCellValue(p);
                i += 1;
            }

            header.CreateCell(i).SetCellValue(_OriginalRow);
            header.CreateCell(i + 1).SetCellValue(_Reason);
        }


        static void WriteWaterAnalysisToExcelRow(WaterAnalysis analysis, ISheet sheet, int rowIndex, int originalRowIndex, RejectedReasons reason)
        {
            var row = sheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue(analysis.WellName);
            row.CreateCell(1).SetCellValue(analysis.Date);

            int i = 2;
            foreach (var p in WaterAnalysis.Properties.Keys.ToList())
            {
                row.CreateCell(i, CellType.Numeric).SetCellValue((double)Interaction.CallByName(analysis, WaterAnalysis.Properties[p].Name, CallType.Get));
                i += 1;
            }

            row.CreateCell(i, CellType.Numeric).SetCellValue(originalRowIndex);
            row.CreateCell(i + 1, CellType.String).SetCellValue(Enum.GetName(typeof(RejectedReasons), reason));
        }

        static void ExportsRejectedFLNAAnalysis(List<RejectedEntity> rejected, ISheet sheet)
        {
            WriteFLNAAnalysisHeader(sheet);

            for (int i = 1; i < rejected.Count; i++)
            {
                var r = rejected[i - 1];
                WriteFLNAAnalysisToExcelRow(r.Entity as FlnaAnalysis, sheet, i, r.OriginalRow, r.Reason);
            }
        }

        static void WriteFLNAAnalysisHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);

            header.CreateCell(0).SetCellValue(FlnaAnalysis.GetDisplayName(nameof(FlnaAnalysis.WellName)));
            header.CreateCell(1).SetCellValue(FlnaAnalysis.GetDisplayName(nameof(FlnaAnalysis.Date)));

            int i = 2;
            foreach (var p in FlnaAnalysis.Properties.Keys.ToList())
            {
                header.CreateCell(i).SetCellValue(p);
                i += 1;
            }

            header.CreateCell(i).SetCellValue(_OriginalRow);
            header.CreateCell(i + 1).SetCellValue(_Reason);
        }

        static void WriteFLNAAnalysisToExcelRow(FlnaAnalysis analysis, ISheet sheet, int rowIndex, int originalRowIndex, RejectedReasons reason)
        {
            var row = sheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue(analysis.WellName);
            row.CreateCell(1).SetCellValue(analysis.Date);

            int i = 2;
            foreach (var p in FlnaAnalysis.Properties.Keys.ToList())
            {
                row.CreateCell(i, CellType.Numeric).SetCellValue((double)Interaction.CallByName(analysis, FlnaAnalysis.Properties[p].Name, CallType.Get));
                i += 1;
            }

            row.CreateCell(i, CellType.Numeric).SetCellValue(originalRowIndex);
            row.CreateCell(i + 1, CellType.String).SetCellValue(Enum.GetName(typeof(RejectedReasons), reason));
        }

        static void ExportsRejectedPrecipitations(List<RejectedEntity> rejected, ISheet sheet)
        {
            WritePrecipitationHeader(sheet);

            for (int i = 1; i < rejected.Count; i++)
            {
                var r = rejected[i - 1];
                var row = WritePrecipitationToExcelRow((r.Entity as Precipitation), sheet, i);

                row.CreateCell(2, CellType.Numeric).SetCellValue(r.OriginalRow);
                row.CreateCell(3, CellType.String).SetCellValue(Enum.GetName(typeof(RejectedReasons), r.Reason));
            }
        }

        static void WritePrecipitationHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);

            header.CreateCell(0).SetCellValue(Precipitation.GetDisplayName(nameof(Precipitation.PrecipitationDate)));
            header.CreateCell(1).SetCellValue(Precipitation.GetDisplayName(nameof(Precipitation.Millimeters)));
            header.CreateCell(2).SetCellValue(_OriginalRow);
            header.CreateCell(3).SetCellValue(_Reason);
        }

        static IRow WritePrecipitationToExcelRow(Precipitation precipitation, ISheet sheet, int rowIndex)
        {
            var row = sheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue(precipitation.PrecipitationDate);
            row.CreateCell(1, CellType.Numeric).SetCellValue(precipitation.Millimeters);

            return row;
        }

        static void ExportsRejectedMeasurements(List<RejectedEntity> rejected, ISheet sheet)
        {
            WriteMeasurementHeader(sheet);

            for (int i = 1; i < rejected.Count; i++)
            {
                var r = rejected[i - 1];
                var row = WriteMeasurementToExcelRow((r.Entity as Measurement), sheet, i);

                row.CreateCell(6, CellType.Numeric).SetCellValue(r.OriginalRow);
                row.CreateCell(7, CellType.String).SetCellValue(Enum.GetName(typeof(RejectedReasons), r.Reason));
            }
        }

        static void WriteMeasurementHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);
            int i = 0;
            header.CreateCell(i).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.WellName)));
            header.CreateCell(i++).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.Date)));
            header.CreateCell(i++).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.FlnaDepth)));
            header.CreateCell(i++).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.WaterDepth)));
            header.CreateCell(i++).SetCellValue(Measurement.GetDisplayName(nameof(Measurement.Comment)));
            header.CreateCell(i++).SetCellValue(_OriginalRow);
            header.CreateCell(i++).SetCellValue(_Reason);
        }

        static IRow WriteMeasurementToExcelRow(Measurement measurement, ISheet sheet, int rowIndex)
        {
            var row = sheet.CreateRow(rowIndex);
            int i = 0;
            row.CreateCell(0).SetCellValue(measurement.WellName);
            row.CreateCell(i++).SetCellValue(measurement.Date);
            row.CreateCell(i++, CellType.Numeric).SetCellValue(measurement.FlnaDepth);
            row.CreateCell(i++, CellType.Numeric).SetCellValue(measurement.WaterDepth);
            row.CreateCell(i++).SetCellValue(measurement.Comment);

            return row;
        }

        static void ExportsRejectedWells(List<RejectedEntity> rejected, ISheet sheet)
        {
            WriteWellHeader(sheet);

            for (int i = 1; i < rejected.Count; i++)
            {
                var r = rejected[i - 1];
                var row = WriteWellToExcelRow((r.Entity as Well), sheet, i);

                row.CreateCell(10, CellType.Numeric).SetCellValue(r.OriginalRow);
                row.CreateCell(11, CellType.String).SetCellValue(Enum.GetName(typeof(RejectedReasons), r.Reason));
            }
        }




        static void WriteWellHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);
            int i = 0;
            header.CreateCell(0).SetCellValue(Well.GetDisplayName(nameof(Well.Name)));
            header.CreateCell(i++).SetCellValue(Well.GetDisplayName(nameof(Well.X)));
            header.CreateCell(i++).SetCellValue(Well.GetDisplayName(nameof(Well.Y)));
            header.CreateCell(i++).SetCellValue(Well.GetDisplayName(nameof(Well.Z)));
            header.CreateCell(i++).SetCellValue(Well.GetDisplayName(nameof(Well.Latitude)));
            header.CreateCell(i++).SetCellValue(Well.GetDisplayName(nameof(Well.Longitude)));
            header.CreateCell(i++).SetCellValue("Tipo");
            header.CreateCell(i++).SetCellValue(Well.GetDisplayName(nameof(Well.Height)));
            header.CreateCell(i++).SetCellValue(Well.GetDisplayName(nameof(Well.Exists)));
            header.CreateCell(i++).SetCellValue(Well.GetDisplayName(nameof(Well.Bottom)));
            header.CreateCell(i++).SetCellValue(_OriginalRow);
            header.CreateCell(i++).SetCellValue(_Reason);
        }

        static IRow WriteWellToExcelRow(Well well, ISheet sheet, int rowIndex)
        {
            var row = sheet.CreateRow(rowIndex);
            int i = 0;
            row.CreateCell(0).SetCellValue(well.Name);
            row.CreateCell(i++, CellType.Numeric).SetCellValue(well.X);
            row.CreateCell(i++, CellType.Numeric).SetCellValue(well.Y);
            row.CreateCell(i++, CellType.Numeric).SetCellValue(well.Z);
            row.CreateCell(i++, CellType.Numeric).SetCellValue(well.Latitude);
            row.CreateCell(i++, CellType.Numeric).SetCellValue(well.Longitude);
            row.CreateCell(i++, CellType.Numeric).SetCellValue((int)well.WellType);
            row.CreateCell(i++, CellType.Numeric).SetCellValue(well.Height);
            row.CreateCell(i++).SetCellValue(well.Exists ? "SI" : "NO");
            row.CreateCell(i++, CellType.Numeric).SetCellValue(well.Bottom);

            return row;
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
    }
}
