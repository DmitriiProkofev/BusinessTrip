using Core.BusinessTrip.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Core.BusinessTrip.Context
{
    public static class LoadExcelPerson
    {
        private const string DirectoryName = "Оформление командировок";

        private static Excel.Application _application = null;
        private static Excel.Workbook _workBook = new Excel.Workbook();
        private static Excel.Worksheet _workSheet = null;
        private static object _missingObj = System.Reflection.Missing.Value;

        public static List<Person> LoadPersons()
        {
            //GetDataTemplete();

            try
            {

                Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
                Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(@"C:\Users\Prokofiev_DA\Documents\ExpPersons.csv", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл
                _workSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
                var lastCell = _workSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейк


                var persons = new List<Person>();
                var listDepartamentShorts = new List<string> { "№119", "№120", "№121" };


                for (int i = 1; i < lastCell.Row; i++)
                //for(int j = 1; j < _workSheet.UsedRange.Columns.Count; j++)
                {
                    var status = GetCellValue(i, 5);
                    var departament = GetCellValue(i, 3);

                    if (status != "")
                    {
                        var departamentShortNew = "";

                        foreach (var departamentShort in listDepartamentShorts)
                        {
                            if (departament.Contains(departamentShort))
                            {
                                var indexNumber = departament.IndexOf('№');
                                departamentShortNew = departament.Substring(indexNumber).TrimEnd(')').TrimStart('№');

                                persons.Add(new Person
                                {
                                    PersonnelNumber = GetCellValue(i, 1),
                                    Name = GetCellValue(i, 2),
                                    Department = departamentShortNew,
                                    Position = GetCellValue(i, 4),
                                    Address = "ул. Вилоновская, 18",
                                    Head = ""
                                });
                            }
                        }
                    }
                }

                ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
                ObjWorkExcel.Quit(); // выйти из экселя
                GC.Collect(); // убрать за собой

                return persons;
            }
            catch
            {
                MessageBox.Show("Ошибка при экспорте служебного задания.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new List<Person>();
            }
        }

        public static string GetCellValue(int rowIndex, int columnIndex)
        {
            string cellValue = "";

            Excel.Range cellRange = (Excel.Range)_workSheet.Cells[rowIndex, columnIndex];
            if (cellRange.Value != null)
            {
                cellValue = cellRange.Value.ToString();
            }
            return cellValue;
        }


    }
}
