using Client.BusinessTrip.Properties;
using Core.BusinessTrip.Domain;
using Data.BusinessTrip.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Client.BusinessTrip.Helpers.HelperReports
{
    public class LoadExcelPersons
    {
        public List<Person> LoadPerson()
        {
            Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(@"C:\Users\Prokofiev_DA\Documents\ExpPersons.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
            string[,] list = new string[lastCell.Column, lastCell.Row]; // массив значений с листа равен по размеру листу


            var persons = new List<Person>();
            var listDepartamentShorts = new List<string> { "№119", "№120", "№121", "№118" };



            Repository<Position> repositoryP = new Repository<Position>();
            repositoryP.ConnectionString = Resources.ConnectionStringDomain;
            var listPositions = repositoryP.GetAll();

            Repository<Department> repositoryD = new Repository<Department>();
            repositoryD.ConnectionString = Resources.ConnectionStringDomain;
            var listDepartments = repositoryD.GetAll();


            for (int i = 1; i < (int)lastCell.Row; i++)
            {

                var status = GetCellValue(ObjWorkSheet, i, 6);
                var departament = GetCellValue(ObjWorkSheet, i, 3);
                var position = GetCellValue(ObjWorkSheet, i, 4);

                if (status == "")
                {
                    var departamentShortNew = "";

                    foreach (var departamentShort in listDepartamentShorts)
                    {
                        if (departament.Contains(departamentShort))
                        {
                            var indexNumber = departament.IndexOf('№');
                            departamentShortNew = departament.Substring(indexNumber).TrimEnd(')').TrimStart('(').TrimStart('№');

                            int? positionId = listPositions.Where(p => p.Nominative == position).Select(p => p.PositionId).FirstOrDefault();

                            if (positionId==null)
                            {
                                positionId = listPositions.Where(p => p.Nominative == "Неизвестна").Select(p => p.PositionId).FirstOrDefault();
                            }

                            string number = "";
                            if (departament.Contains("119"))
                            {
                                number = "119";
                            }
                            else if (departament.Contains("120"))
                            {
                                number = "120";
                            }
                            else if (departament.Contains("121"))
                            {
                                number = "121";
                            }
                            else if (departament.Contains("118"))
                            {
                                number = "118";
                            }
                            int? departmentId = listDepartments.Where(p => p.Number == number).Select(p => p.DepartmentId).FirstOrDefault();



                            persons.Add(new Person
                            {
                                PersonnelNumber = GetCellValue(ObjWorkSheet, i, 1),
                                Name = GetCellValue(ObjWorkSheet, i, 2),
                                DepartmentId = departmentId, /*!= null ? departmentId : listDepartments.Where(p => p.Nominative == "Неизвестна").Select(p => p.DepartmentId).FirstOrDefault();*/
                                PositionId = positionId != null ? (int)positionId : 0,
                                PhoneNumber = ""

                                
                                //Department = new Department { Number = departamentShortNew, Nominative = ""},
                                //Position = new Position { Nominative = GetCellValue(ObjWorkSheet, i, 4), Genitive = "", Dative = "" },
                                //TODO:
                                //Address = "ул. Вилоновская, 18",
                                //Head = ""
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

        public static string GetCellValue(Excel.Worksheet workSheet, int rowIndex, int columnIndex)
        {
            string cellValue = "";

            Excel.Range cellRange = (Excel.Range)workSheet.Cells[rowIndex, columnIndex];
            if (cellRange.Value != null)
            {
                cellValue = cellRange.Value.ToString();
            }
            return cellValue;
        }

        public List<Position> LoadPosition()
        {
            Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(@"C:\Users\Prokofiev_DA\Documents\ExpPersons.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
            string[,] list = new string[lastCell.Column, lastCell.Row]; // массив значений с листа равен по размеру листу


            var positions = new List<Position>();
            var listDepartamentShorts = new List<string> { "№119", "№120", "№121" };

            for (int i = 1; i < (int)lastCell.Row; i++)
            {

                var status = GetCellValue(ObjWorkSheet, i, 6);
                var departament = GetCellValue(ObjWorkSheet, i, 3);

                if (status == "")
                {
                    var departamentShortNew = "";

                    foreach (var departamentShort in listDepartamentShorts)
                    {
                        if (departament.Contains(departamentShort))
                        {
                            var indexNumber = departament.IndexOf('№');
                            departamentShortNew = departament.Substring(indexNumber).TrimEnd(')').TrimStart('(').TrimStart('№');

                            positions.Add(new Position
                            {
                                Nominative = GetCellValue(ObjWorkSheet, i, 4),
                                //Category = "Нет подразделения",
                                Dative = "-",
                                Genitive = "-"
                            });

                            //persons.Add(new Person
                            //{
                            //    PersonnelNumber = GetCellValue(ObjWorkSheet, i, 1),
                            //    Name = GetCellValue(ObjWorkSheet, i, 2),
                            //    Department = new Department { Number = departamentShortNew, Nominative = "" },
                            //    Position = new Position { Nominative = GetCellValue(ObjWorkSheet, i, 4), Genitive = "", Dative = "" },
                            //    //TODO:
                            //    //Address = "ул. Вилоновская, 18",
                            //    //Head = ""
                            //});
                        }
                    }
                }
            }

            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из экселя
            GC.Collect(); // убрать за собой

            return positions;
        }
    }
}
