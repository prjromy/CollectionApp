using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.CustomHelper
{
    public class ExcelExportHelper
    {
        public static string ExcelContentType
        {


            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }

        }
            public static DataTable ListToDataTable<T>(List<T> data, params string[] columnsToTake)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                DataTable dataTable = new DataTable();

                for (int i = 0; i < properties.Count; i++)
                {
                    PropertyDescriptor property = properties[i];
                    dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                }

                object[] values = new object[properties.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = properties[i].GetValue(item);
                    }

                    dataTable.Rows.Add(values);
                }
                return dataTable;
            }

        public static byte[] ExportExcel(DataTable dataTable, string[] parameterSearch, string heading = "")
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("Sheet1"));
                int startRowFrom = 1;


                if (parameterSearch.Count() == 1)
                {
                    startRowFrom = 7;
                }
                else if (parameterSearch.Count() == 2)
                {
                    startRowFrom = 8;
                }
                else
                {
                    startRowFrom = 9;
                }
                DataColumn dataColumn = dataTable.Columns.Add("S.N", typeof(int));
                dataColumn.SetOrdinal(0);
                int index = 1;
                foreach (DataRow item in dataTable.Rows)
                {
                    item[0] = index;
                    index++;
                }



                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    if (column.ToString() == "Date")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    if (column.ToString() == "EffectiveDate")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    if (column.ToString() == "CollectionDate")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                     if (column.ToString() == "PostedOn")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }

                    workSheet.Column(columnIndex).AutoFit();



                    columnIndex++;
                }


                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  


                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Bold = true;
                    workSheet.Cells["A1"].Style.Font.Size = 18;
                    if (parameterSearch.Length == 1)
                    {
                        workSheet.Cells["A3"].Value = parameterSearch[0];

                    }
                    else if (parameterSearch.Length == 2)
                    {
                        workSheet.Cells["A3"].Value = parameterSearch[0];
                        workSheet.Cells["A4"].Value = parameterSearch[1];
                    }
                    else if (parameterSearch.Length == 3)
                    {
                        workSheet.Cells["A3"].Value = parameterSearch[0];
                        workSheet.Cells["A4"].Value = parameterSearch[1];
                        workSheet.Cells["A5"].Value = parameterSearch[2];
                    }
                    else if (parameterSearch.Length == 4)
                    {
                        workSheet.Cells["A3"].Value = parameterSearch[0];
                        workSheet.Cells["A4"].Value = parameterSearch[1];
                        workSheet.Cells["A5"].Value = parameterSearch[2];
                        workSheet.Cells["A6"].Value = parameterSearch[3];
                    }
                    else if (parameterSearch.Length == 5)
                    {
                        workSheet.Cells["A3"].Value = parameterSearch[0];
                        workSheet.Cells["A4"].Value = parameterSearch[1];
                        workSheet.Cells["A5"].Value = parameterSearch[2];
                        workSheet.Cells["A6"].Value = parameterSearch[3];
                        workSheet.Cells["A7"].Value = parameterSearch[4];
                    }
                    else if (parameterSearch.Length == 6)
                    {
                        workSheet.Cells["A3"].Value = parameterSearch[0];
                        workSheet.Cells["A4"].Value = parameterSearch[1];
                        workSheet.Cells["A5"].Value = parameterSearch[2];
                        workSheet.Cells["A6"].Value = parameterSearch[3];
                        workSheet.Cells["A7"].Value = parameterSearch[4];
                        workSheet.Cells["A8"].Value = parameterSearch[5];
                    }
                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

                return result;
            
        }

            public static byte[] ExportExcel<T>(List<T> data, string[] parameterSearch, string Heading="", params string[] ColumnsToTake)
            {
            return ExportExcel(ListToDataTable<T>(data, ColumnsToTake), parameterSearch, Heading );
        }

        }
    }
    

