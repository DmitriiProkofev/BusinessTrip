using Client.BusinessTrip.Helpers.HelperReports;
using Core.BusinessTrip.Domain;
using Core.BusinessTrip.Helpers.DomainHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;



namespace Client.BusinessTrip.Reports.Excels
{
    public static class TaskToExcel
    {
        private const string DirectoryName = "Оформление командировок";

        private static Excel.Application _application = null;
        private static Excel.Workbook _workBook = null;
        private static Excel.Worksheet _workSheet = null;
        private static object _missingObj = System.Reflection.Missing.Value;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        //видимость 
        public static bool Visible
        {
            get
            {
                return _application.Visible;
            }
            set
            {
                _application.Visible = value;
            }
        }


        public static bool GetDataTemplete(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {

            Object templatePathObj = System.IO.Path.GetFullPath(string.Format(@"\\fs21402\Templates\WindowsPrograms\IT-Distr\028_BusinessTrip\Content\Excel\{0}\{1}", "TemplateTask", businessTrip.TypeWork != null ? businessTrip.TypeWork.TemplateTask : "Служебное задание.xltx"));//"путь к файлу шаблона";
            try
            {
                if (File.Exists(templatePathObj.ToString()))
                {
                    _application = new Excel.Application();
                    _workBook = _application.Workbooks.Add(templatePathObj);
                    _workSheet = (Excel.Worksheet)_workBook.Worksheets.get_Item(1);

                    Visible = false;

                    return true;
                }
                else
                {
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, "Отсутствует шаблон служебного задания."));
                    MessageBox.Show("Отсутствует шаблон служебного задания.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при экспорте служебного задания.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        public static void ExportToExcel(Core.BusinessTrip.Domain.BusinessTrip businessTrip, Dictionary<string, string> targets)
        {
            if (!GetDataTemplete(businessTrip)) //test vs
                return;

            try
            {
                SetCellValue(businessTrip.HeadStructuralDivision != null ? Regex.Replace(string.Format("{0} {1} {2}",
                    HelperReports.TransformationBeginStr(businessTrip.HeadStructuralDivision.Position.Nominative, false),
                    !HelperReports.IsPositionHead(businessTrip.HeadStructuralDivision.Position.Nominative) && businessTrip.HeadStructuralDivision.Department != null ?
                    HelperReports.GetCategoryGenitiveByDepartmentCategory(businessTrip.HeadStructuralDivision.Department.Category) : "",
                    businessTrip.HeadStructuralDivision.Department != null ?
                    HelperReports.TransformationBeginStr(businessTrip.HeadStructuralDivision.Department.Nominative) : ""), "[ ]+", " ").TrimEnd(' ') : "<не задан руководитель структурного подразделения>", "HeadStructuralDivisionPositionOne");
                SetCellValue(businessTrip.HeadStructuralDivision != null ? Regex.Replace(string.Format("{0} {1} {2}",
                    HelperReports.TransformationBeginStr(businessTrip.HeadStructuralDivision.Position.Nominative, false),
                    !HelperReports.IsPositionHead(businessTrip.HeadStructuralDivision.Position.Nominative) && businessTrip.HeadStructuralDivision.Department != null ?
                    HelperReports.GetCategoryGenitiveByDepartmentCategory(businessTrip.HeadStructuralDivision.Department.Category) : "",
                    businessTrip.HeadStructuralDivision.Department != null ?
                    HelperReports.TransformationBeginStr(businessTrip.HeadStructuralDivision.Department.Nominative) : ""), "[ ]+", " ").TrimEnd(' ') : "<не задан руководитель структурного подразделения>", "HeadStructuralDivisionPositionTwo");

                SetCellValue(businessTrip.HeadStructuralDivision != null ? HelperReports.GetNameShort(businessTrip.HeadStructuralDivision.Name) :
                    "<не задан руководитель структурного подразделения>", "HeadStructuralDivisionNameOne");
                SetCellValue(businessTrip.HeadStructuralDivision != null ? HelperReports.GetNameShort(businessTrip.HeadStructuralDivision.Name) :
                    "<не задан руководитель структурного подразделения>", "HeadStructuralDivisionNameTwo");

                if (businessTrip.HeadStructuralDivision == null)
                {
                    SetCellYellowColor("HeadStructuralDivisionNameOne");
                    SetCellYellowColor("HeadStructuralDivisionNameTwo");
                    SetCellYellowColor("HeadStructuralDivisionPositionOne");
                    SetCellYellowColor("HeadStructuralDivisionPositionTwo");
                }

                //SetCellValue(businessTrip.HeadOrganization != null ? HelperReports.TransformationBeginStr(businessTrip.HeadOrganization.Position.Nominative, false) :
                //    "<не задан руководитель организации>", "HeadOrganizationPosition");
                SetCellValue(businessTrip.HeadOrganization != null ? Regex.Replace(string.Format("{0} {1}",
                    HelperReports.TransformationBeginStr(businessTrip.HeadOrganization.Position.Nominative, false),
                    HelperReports.IsPositionHead(businessTrip.HeadOrganization.Position.Nominative) && businessTrip.HeadOrganization.Department != null ?
                    HelperReports.TransformationBeginStr(businessTrip.HeadOrganization.Department.Nominative) : ""), "[ ]+", " ").TrimEnd(' ') 
                    : "<не задан руководитель организации>", "HeadOrganizationPosition");
                SetCellValue(businessTrip.HeadOrganization != null ? HelperReports.GetNameShort(businessTrip.HeadOrganization.Name) :
                    "<не задан руководитель организации>", "HeadOrganizationName");

                if (businessTrip.HeadOrganization == null)
                {
                    SetCellYellowColor("HeadOrganizationPosition");
                    SetCellYellowColor("HeadOrganizationName");
                }

                SetCellValue(businessTrip.NumberDocument, "NumberDocument");
                SetCellValue(businessTrip.DateFormulationToString, "DateFormulation");

                SetCellValue(string.Format("Выполнение {0} на об.:", businessTrip.TypeWork != null ? HelperReports.TransformationBeginStr(businessTrip.TypeWork.Name) :
                    "<не задан вид работ>"), "TypeWork");

                if (businessTrip.TypeWork == null)
                    SetCellYellowColor("TypeWork");

                PartyPersonComparer partyPersonComparer = new PartyPersonComparer();
                var persons = businessTrip.Party != null ? (businessTrip.Party.PartyPersons != null ? businessTrip.Party.PartyPersons.ToList() : new List<PartyPerson>()) : new List<PartyPerson>();
                persons.Sort(partyPersonComparer);

                DirectionComparer directionComparer = new DirectionComparer();
                var directions = businessTrip.Directions != null ? businessTrip.Directions : new List<Direction>();
                directions.Sort(directionComparer);

                //if (persons.Count > 0)
                //{
                var responsiblePerson = persons.FirstOrDefault(p => p.IsResponsible);
                SetCellValue(responsiblePerson != null ? HelperReports.TransformationBeginStr(responsiblePerson.Position, false) :
                    "<не задан ответственный за командировку>", "PersonPosition");
                SetCellValue(responsiblePerson != null ? HelperReports.GetNameShort(responsiblePerson.Name) :
                    "<не задан ответственный за командировку>", "PersonName");

                SetCellValue(responsiblePerson != null ? (responsiblePerson.Person.Department != null ? Regex.Replace(string.Format("{0} {1} {2}",
                    HelperReports.TransformationBeginStr(responsiblePerson.Person.Department.Category, false),
                    HelperReports.TransformationBeginStr(responsiblePerson.Person.Department.Nominative),
                    !string.IsNullOrWhiteSpace(responsiblePerson.Person.Department.Number) ?
                    string.Format("({0} № {1})", HelperReports.TransformationBeginStr(responsiblePerson.Person.Department.Category),
                    responsiblePerson.Person.Department.Number) : ""), "[ ]+", " ").TrimEnd(' ') : "<не задан отдел для ответственного за командировку>") :
                    "<не задан ответственный за командировку>", "Department");

                if (responsiblePerson == null)
                {
                    SetCellYellowColor("PersonPosition");
                    SetCellYellowColor("PersonName");
                    SetCellYellowColor("Department");
                }
                else
                {
                    if (responsiblePerson.Person.Department == null)
                    {
                        SetCellYellowColor("Department");
                    }
                }
                //}



                var saveRowCount = 0;

                if (persons.Count > 0)
                {
                    var countRowInsert = directions.Count != 0 ? persons.Count * directions.Count - 1 : persons.Count - 1;

                    while (countRowInsert != 0)
                    {
                        InsertRow(16);
                        MergeCell(16, 2, 16, 8);
                        SetBorders(16, 2, 16, 8);
                        MergeCell(16, 26, 16, 49);
                        SetBorders(16, 26, 16, 49);
                        MergeCell(16, 50, 16, 63);
                        SetBorders(16, 50, 16, 63);
                        MergeCell(16, 88, 16, 106);
                        SetBorders(16, 88, 16, 106);
                        MergeCell(16, 107, 16, 124);
                        SetBorders(16, 107, 16, 124);
                        MergeCell(16, 126, 16, 136);
                        SetBorders(16, 126, 16, 136);
                        MergeCell(16, 137, 16, 144);
                        SetBorders(16, 137, 16, 144);
                        MergeCell(16, 149, 16, 162);
                        SetBorders(16, 149, 16, 162);

                        countRowInsert--;
                    }


                    var rowCount = directions.Count != 0 ? persons.Count * directions.Count : persons.Count;
                    saveRowCount = rowCount;

                    var numPerson = 0;

                    if (directions.Count > 0)
                    {
                        for (int i = 0; i < rowCount; i = i + directions.Count)
                        {
                            if (numPerson < persons.Count)
                            {
                                SetCellValue((numPerson + 1).ToString(), i + 15, 2);
                                SetCellValue(persons[numPerson].PersonnelNumber, i + 15, 25);
                                SetCellValue(HelperReports.GetNameShort(persons[numPerson].Name), i + 15, 26);
                                SetCellValue(HelperReports.TransformationBeginStr(persons[numPerson].Position, false), i + 15, 50);

                                numPerson++;
                            }

                            if (directions.Count > 0)
                            {
                                MergeCell(i + 15, 2, i + 15 + directions.Count - 1, 2);
                                SetBorders(i + 15, 2, i + 15 + directions.Count - 1, 2);
                                MergeCell(i + 15, 25, i + 15 + directions.Count - 1, 25);
                                SetBorders(i + 15, 25, i + 15 + directions.Count - 1, 25);
                                MergeCell(i + 15, 26, i + 15 + directions.Count - 1, 26);
                                SetBorders(i + 15, 26, i + 15 + directions.Count - 1, 26);
                                MergeCell(i + 15, 50, i + 15 + directions.Count - 1, 50);
                                SetBorders(i + 15, 50, i + 15 + directions.Count - 1, 50);

                                var numDirections = 0;

                                for (int j = i; j < directions.Count + i; j++)
                                {
                                    if (numDirections < directions.Count)
                                    {
                                        SetCellValue(directions[numDirections].Location != null ? directions[numDirections].Location.ShortAddress : "", i + numDirections + 15, 88);
                                        SetCellValue(directions[numDirections].Organization != null ? directions[numDirections].Organization.ShortName : "", i + numDirections + 15, 107);
                                        SetCellValue(directions[numDirections].DateBeginAsString, i + numDirections + 15, 126);
                                        SetCellValue(directions[numDirections].DateEndAsString, i + numDirections + 15, 137);
                                        if (directions[numDirections].DateBegin != null && directions[numDirections].DateEnd != null)
                                        {
                                            var countEqualDirections = directions.Count(d => d.DateBeginAsString == directions[numDirections].DateBeginAsString && d.DateEndAsString == directions[numDirections].DateEndAsString);
                                            if (countEqualDirections > 1)
                                            {
                                                SetCellValue(Math.Round((double)1 / countEqualDirections, 2).ToString(), i + numDirections + 15, 148);
                                            }
                                            else
                                            {
                                                SetCellValue(Math.Round((directions[numDirections].DateEnd.Value - directions[numDirections].DateBegin.Value).TotalDays + 1).ToString(), i + numDirections + 15, 148);
                                            }
                                        }
                                        SetCellValue(string.Format("Об. {0}", directions[numDirections].Reason), i + numDirections + 15, 149);

                                        numDirections++;
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < persons.Count; i = i + 1)
                        {
                            if (numPerson < persons.Count)
                            {
                                SetCellValue((numPerson + 1).ToString(), i + 15, 2);
                                SetCellValue(persons[i].PersonnelNumber, i + 15, 25);
                                SetCellValue(HelperReports.GetNameShort(persons[i].Name), i + 15, 26);
                                SetCellValue(HelperReports.TransformationBeginStr(persons[i].Position, false), i + 15, 50);

                                numPerson++;
                            }
                        }
                    }
                }

                if (targets != null)
                {
                    if (targets.Count != 0)
                    {
                        if (targets.Count > 3)
                        {
                            var rowCount = targets.Count;

                            while (rowCount != 0)
                            {
                                InsertRow(saveRowCount - 1 + 19);
                                MergeCell(saveRowCount - 1 + 19, 2, saveRowCount - 1 + 19, 124);
                                SetBorders(saveRowCount - 1 + 19, 2, saveRowCount - 1 + 19, 124);
                                MergeCell(saveRowCount - 1 + 19, 126, saveRowCount - 1 + 19, 162);
                                SetBorders(saveRowCount - 1 + 19, 126, saveRowCount - 1 + 19, 162);

                                rowCount--;
                            }
                        }

                        for (int i = 0; i < directions.Count; i++)
                        {
                            if (targets.Keys.Any(d => d == directions[i].Reason))
                            {
                                SetCellValue(targets[directions[i].Reason], i + saveRowCount - 1 + 18, 2);
                            }


                            //_workSheet.Range[_workSheet.Cells[i + saveRowCount - 1 + 18, 2], _workSheet.Cells[i + saveRowCount - 1 + 18, 124]].NamedRange.AutoFit();

                            //_workSheet.Cells[i + saveRowCount - 1 + 18, 2].NamedRange.AutoFit();

                            //_workSheet.Cells[i + saveRowCount - 1 + 18, 2].EntireRow.AutoFit();

                            //double CellHeight = _workSheet.Cells[i + saveRowCount - 1 + 18, 2].RowHeight;//узнать высоту
                            ////objSheet.get_Range(SecondCell, SecondCell).Value2 = ""; //очистить ячейку 

                            //_workSheet.Cells[i + saveRowCount - 1 + 18, 2].RowHeight = CellHeight;
                            ////SetAutoFit(i + saveRowCount - 1 + 18, 2, i + saveRowCount - 1 + 18, 124);


                        }
                    }
                }
                Visible = true;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при экспорте служебного задания.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
            }
        }

        ///// Вычисляем, сколько строк будет занимать текст со шрифтом из указанной ячейки, если ширина печатаемой области экрана нам известна
        ///// Текст должен быть без всяких символов новой строки и табуляций
        //private static int GetLinesCountForTitleInCell(Excel.Range cell, string textWONewLines)
        //{
        //    float width = GetTextWidthInCell(cell, textWONewLines);
        //    int ratio = (int)Math.Ceiling(width / PRINTABLE_AREA_PIXELS_WIDTH);

        //    return ratio + 1;
        //}

        public static void MergeCell(int beginCell1, int beginCell2, int endBegin1, int endBegin2)
        {
            _workSheet.Range[_workSheet.Cells[beginCell1, beginCell2], _workSheet.Cells[endBegin1, endBegin2]].Merge();
        }

        public static void SetBorders(int beginCell1, int beginCell2, int endBegin1, int endBegin2)
        {
            _workSheet.Range[_workSheet.Cells[beginCell1, beginCell2], _workSheet.Cells[endBegin1, endBegin2]].Cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
        }

        public static void SetAutoFit(int beginCell1, int beginCell2, int endBegin1, int endBegin2)
        {
            _workSheet.Range[_workSheet.Cells[beginCell1, beginCell2], _workSheet.Cells[endBegin1, endBegin2]].EntireRow.AutoFit();
        }

        public static void InsertRow(int rowNum)
        {
            Excel.Range cellRange = (Excel.Range)_workSheet.Cells[rowNum, 1];
            Excel.Range rowRange = cellRange.EntireRow;
            rowRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, false);
        }


        public static void SetCellValue(string cellValue, int rowIndex, int columnIndex)
        {
            _workSheet.Cells[rowIndex, columnIndex] = cellValue;
        }

        public static void SetCellValue(string cellValue, string cellName)
        {
            _workBook.Names.Item(cellName).RefersToRange.Value = cellValue;
        }

        public static void SetCellYellowColor(string cellName)
        {
            _workBook.Names.Item(cellName).RefersToRange.Interior.Color = Excel.XlRgbColor.rgbYellow;
        }

        public static void Close()
        {
            if (_workBook != null)
            {
                _workBook.Close(false, _missingObj, _missingObj);
            }
            if (_application != null)
            {
                _application.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(_application);
            }

            _application = null;
            _workBook = null;
            _workSheet = null;

            System.GC.Collect();
        }

    }
}
