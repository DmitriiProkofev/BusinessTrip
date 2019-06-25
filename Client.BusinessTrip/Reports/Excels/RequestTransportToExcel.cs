using Client.BusinessTrip.Helpers.HelperReports;
using Core.BusinessTrip.Domain;
using Core.BusinessTrip.Helpers.DomainHelpers;
using Cyriller;
using Cyriller.Model;
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
    public static class RequestTransportToExcel
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

            Object templatePathObj = System.IO.Path.GetFullPath(string.Format(@"\\fs21402\Templates\WindowsPrograms\IT-Distr\028_BusinessTrip\Content\Excel\{0}\{1}", "TemplateRequestTransport", businessTrip.TypeWork != null ? businessTrip.TypeWork.TemplateRequestTransport : "Заявка на транспортное средство.xltx"));//"путь к файлу шаблона";
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
                    _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, "Отсутствует шаблон заявки на транспорт."));
                    MessageBox.Show("Отсутствует шаблон заявки на транспорт.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при экспорте заявки на транспорт.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        public static void ExportToExcel(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            if (!GetDataTemplete(businessTrip))
                return;

            try
            {
                SetCellValue(businessTrip.HeadStructuralDivision != null ? Regex.Replace(string.Format("от {0} {1} {2}",
                    HelperReports.TransformationBeginStr(businessTrip.HeadStructuralDivision.Position.Genitive),
                    !HelperReports.IsPositionHead(businessTrip.HeadStructuralDivision.Position.Nominative) && businessTrip.HeadStructuralDivision.Department != null ?
                    HelperReports.GetCategoryGenitiveByDepartmentCategory(businessTrip.HeadStructuralDivision.Department.Category) : "",
                    string.Format("№ {0}", !string.IsNullOrWhiteSpace(businessTrip.HeadStructuralDivision.Department.Number) ?
                    businessTrip.HeadStructuralDivision.Department.Number : "<не задан номер отдела>")), "[ ]+", " ").TrimEnd(' ') : 
                    "<не задан руководитель структурного подразделения>", "HeadStructuralDivisionPosition");
                SetCellValue(businessTrip.HeadStructuralDivision != null ? HelperReports.GetNameGenitiveByName(businessTrip.HeadStructuralDivision.Name) : 
                    "<не задан руководитель структурного подразделения>", "HeadStructuralDivisionNameOne");
                SetCellValue(businessTrip.HeadStructuralDivision != null ? HelperReports.GetNameShort(businessTrip.HeadStructuralDivision.Name) : 
                    "<не задан руководитель структурного подразделения>", "HeadStructuralDivisionName");

                if (businessTrip.HeadStructuralDivision == null)
                {
                    SetCellYellowColor("HeadStructuralDivisionPosition");
                    SetCellYellowColor("HeadStructuralDivisionNameOne");
                    SetCellYellowColor("HeadStructuralDivisionName");
                }
                else
                {
                    if (businessTrip.HeadStructuralDivision.Department != null)
                    {
                        if (string.IsNullOrWhiteSpace(businessTrip.HeadStructuralDivision.Department.Number))
                        {
                            SetCellYellowColor("HeadStructuralDivisionPosition");
                        }
                    }
                }

                SetCellValue(businessTrip.RequestTransport.DateFormulation != null ? businessTrip.RequestTransport.DateFormulation.Value.ToShortDateString() : 
                    "<не задана дата формирования заявки на транспорт>", "DateFormulation");

                if (businessTrip.RequestTransport.Date == null)
                    SetCellYellowColor("DateFormulation");

                SetCellValue(businessTrip.HeadOrganization != null ? Regex.Replace(string.Format("{0} {1}",
                    HelperReports.TransformationBeginStr(businessTrip.HeadOrganization.Position.Nominative, false),
                    HelperReports.IsPositionHead(businessTrip.HeadOrganization.Position.Nominative) && businessTrip.HeadOrganization.Department != null ?
                    HelperReports.TransformationBeginStr(businessTrip.HeadOrganization.Department.Nominative) : ""), "[ ]+", " ").TrimEnd(' ') : 
                    "<не задан руководитель организации>", "HeadOrganizationPosition");
                SetCellValue(businessTrip.HeadOrganization != null ? HelperReports.GetNameShort(businessTrip.HeadOrganization.Name) : 
                    "<не задан руководитель организации>", "HeadOrganizationName");

                if (businessTrip.HeadOrganization == null)
                {
                    SetCellYellowColor("HeadOrganizationPosition");
                    SetCellYellowColor("HeadOrganizationName");
                }

                SetCellValue(businessTrip.RequestTransport.ProjectManager != null ? HelperReports.TransformationBeginStr(businessTrip.RequestTransport.ProjectManager.Position.Nominative, false) : 
                    "<не задан сотрудник отдела управления проектами>", "ProjectManagerPosition");

                SetCellValue(businessTrip.RequestTransport.ProjectManager != null ? HelperReports.GetNameShort(businessTrip.RequestTransport.ProjectManager.Name) : 
                    "<не задан сотрудник отдела управления проектами>", "ProjectManagerName");

                if (businessTrip.RequestTransport.ProjectManager == null)
                {
                    SetCellYellowColor("ProjectManagerPosition");
                    SetCellYellowColor("ProjectManagerName");
                }

                SetCellValue(businessTrip.RequestTransport.Date != null ? businessTrip.RequestTransport.Date.Value.ToShortDateString() : "<не задана дата выдачи транспорта>", "Date");

                if (businessTrip.RequestTransport.Date == null)
                    SetCellYellowColor("Date");

                SetCellValue(businessTrip.RequestTransport.TimeHour != null ? businessTrip.RequestTransport.TimeHour.ToString() : "<не задано время>", "TimeHour");

                if (businessTrip.RequestTransport.TimeHour == null)
                    SetCellYellowColor("TimeHour");

                SetCellValue(businessTrip.RequestTransport.Address != null ? businessTrip.RequestTransport.Address.ShortAddress : "<не задан адрес>", "Address");

                if (businessTrip.RequestTransport.Address == null)
                    SetCellYellowColor("Address");

                PartyPersonComparer partyPersonComparer = new PartyPersonComparer();
                var persons = businessTrip.Party != null ? (businessTrip.Party.PartyPersons != null ? businessTrip.Party.PartyPersons.ToList() : new List<PartyPerson>()) : new List<PartyPerson>();
                persons.Sort(partyPersonComparer);

                DirectionComparer directionComparer = new DirectionComparer();
                var directions = businessTrip.Directions != null ? businessTrip.Directions : new List<Direction>();
                directions.Sort(directionComparer);

                var responsiblePerson = persons.FirstOrDefault(p => p.IsResponsible);
                SetCellValue(responsiblePerson != null ? HelperReports.GetNameShort(responsiblePerson.Name) :
                    "<не задан ответственный за командировку>", "ContactName");
                SetCellValue(responsiblePerson != null ? (!string.IsNullOrWhiteSpace(responsiblePerson.Person.PhoneNumber) ? responsiblePerson.Person.PhoneNumber : "<не задан телефон ответственного за командировку>") :
                    "<не задан ответственный за командировку>", "ContactPhone");

                if (responsiblePerson == null)
                {
                    SetCellYellowColor("ContactName");
                    SetCellYellowColor("ContactPhone");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(responsiblePerson.Person.PhoneNumber))
                    {
                        SetCellYellowColor("ContactPhone");
                    }
                }

                if (directions.Count > 0)
                {
                    var countRowInsert = directions.Count != 0 ? directions.Count - 1 : 0;

                    while (countRowInsert != 0)
                    {
                        InsertRow(19);
                        countRowInsert--;
                    }

                    MergeCell(18, 1, 18 + directions.Count - 1, 1);
                    MergeCell(18, 6, 18 + directions.Count - 1, 6);
                    MergeCell(18, 7, 18 + directions.Count - 1, 7);
                    MergeCell(18, 8, 18 + directions.Count - 1, 8);
                    MergeCell(18, 9, 18 + directions.Count - 1, 9);
                    MergeCell(18, 10, 18 + directions.Count - 1, 10);

                    SetBorders(18, 1, 18 + directions.Count - 1, 11);

                    SetCellValue(businessTrip.RequestTransport.Transport != null ? HelperReports.TransformationBeginStr(businessTrip.RequestTransport.Transport.Mark, false) : "", 18, 1);
                    SetCellValue(string.Format("Обеспечение {0} работ", businessTrip.TypeWork != null ? HelperReports.TransformationBeginStr(businessTrip.TypeWork.Name) : ""), 18, 6);
                    SetCellValue(businessTrip.RequestTransport.Mileage != null ? businessTrip.RequestTransport.Mileage.Value.ToString() : "", 18, 7);
                    SetCellValue(businessTrip.RequestTransport.TimeWork != null ? businessTrip.RequestTransport.TimeWork.Value.ToString() : "", 18, 8);
                    SetCellValue(persons.Count.ToString(), 18, 9);
                    SetCellValue(persons.Count != 0 ? HelperReports.GetNameShort(persons.FirstOrDefault().Name) : "", 18, 10);

                    for (int i = 0; i < directions.Count; i++)
                    {
                        SetCellValue(directions[i].DateBeginAsString, i + 18, 2);
                        SetCellValue(directions[i].DateEndAsString, i + 18, 3);
                        SetCellValue(directions[i].Location != null ? directions[i].Location.ShortAddress : "", i + 18, 4);
                        SetCellValue(directions[i].Organization != null ? directions[i].Organization.ShortName : "", i + 18, 5);
                        SetCellValue(string.Format("Об. {0}",directions[i].Reason), i + 18, 11);
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
