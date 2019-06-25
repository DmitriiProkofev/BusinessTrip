using Word = Microsoft.Office.Interop.Word;
using System;
using Client.BusinessTrip.Helpers.HelperReports;
using Core.BusinessTrip.Domain;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Windows.Forms;
using Client.BusinessTrip.Properties;
using System.Text.RegularExpressions;
using Core.BusinessTrip.Helpers.DomainHelpers;

namespace Client.BusinessTrip.Reports.Words
{
    public static class DecreeToWord
    {
        private const string DirectoryName = "Оформление командировок";

        private static List<string> _categories = new List<string> { "Группа", "Отдел", "Управление" };

        private static Word.Application _application;
        private static Word._Document _document;

        private static Object _missingObj = System.Reflection.Missing.Value;
        private static Object _trueObj = true;
        private static Object _falseObj = false;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void ExportToWord(Core.BusinessTrip.Domain.BusinessTrip businesstrip)
        {
            GetDataTemplete(businesstrip);

            try
            {
                // обьектные строки для Word
                var strToFindObjs = new string[] {"@ObjectD", "@Composition", "@Ending", "@SaidWorker" , "@NumberDocument", "@DateFormulation", "@DayDoc", "@MonthDoc", "@YearDoc", "@TypeWork", "@ResponsibleForEquipment", "@ResponsibleForData",
                    "@ResponsibleForIndustrialSecurity", "@ResponsibleForRigInspection", "@ResponsibleForCommunication", "@ResponsibleForTechnicalSecurity", "@ResponsibleForFireSecurity",
                    "@DateBegin","@DateEnd", "@ResponsibleForDealer", "@ResponsibleForReceiving", "@ResponsibleForMonitoring", "@ResponsibleForInformation", "@PositionHeadOrganization",
                    "@NameHeadOrganization", "@Directions", "@Target", "@IsHoliday","@IsOrenburgWork", "@Persons" };

                var notSpecifieds = new Dictionary<string, string>();
                notSpecifieds.Add("typeWork", "<не задан тип работ>");


                //// диапазон документа Word
                Word.Range wordRange;

                _application.Visible = false;

                // обходим все разделы документа
                for (int i = 1; i <= _document.Sections.Count; i++)
                {
                    // берем всю секцию диапазоном
                    wordRange = _document.Sections[i].Range;

                    wordRange.Find.ClearFormatting();
                    //wordRange.Find.Replacement.ClearFormatting();

                    foreach (object strToFindObj in strToFindObjs)
                    {
                        switch ((string)strToFindObj)
                        {
                            case "@NumberDocument":
                                {
                                    wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: 
                                        businesstrip.NumberDocument, Replace: Word.WdReplace.wdReplaceAll);
                                    break;
                                }
                            case "@DayDoc":
                                {
                                    wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: 
                                        HelperReports.TransformationDay(businesstrip.DateFormulation.Day), Replace: Word.WdReplace.wdReplaceAll);
                                    break;
                                }
                            case "@MonthDoc":
                                {
                                    wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: 
                                        HelperReports.GetNameMonthByNumber(businesstrip.DateFormulation.Month, HelperReports.Cases.Genitive), Replace: Word.WdReplace.wdReplaceAll);
                                    break;
                                }
                            case "@YearDoc":
                                {

                                    wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: 
                                        businesstrip.DateFormulation.Year.ToString(), Replace: Word.WdReplace.wdReplaceAll);
                                    break;
                                }
                            case "@TypeWork":
                                {
                                    if (businesstrip.TypeWork != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith:
                                             HelperReports.TransformationBeginStr(businesstrip.TypeWork.ToString()), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан тип работ>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForEquipment":
                                {
                                    if (businesstrip.Command.ResponsibleForEquipment != null)
                                    {
                                        wordRange.Find.ClearFormatting();
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} – {1} {2} {3}", 
                                            HelperReports.GetNameDativeByName(businesstrip.Command.ResponsibleForEquipment.Name),
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForEquipment.Position.Dative),
                                            !HelperReports.IsPositionHead(businesstrip.Command.ResponsibleForEquipment.Position.Nominative) && businesstrip.Command.ResponsibleForEquipment.Department != null ?
                                            HelperReports.GetCategoryGenitiveByDepartmentCategory(businesstrip.Command.ResponsibleForEquipment.Department.Category) : "",
                                            businesstrip.Command.ResponsibleForEquipment.Department != null ?
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForEquipment.Department.Nominative) : ""), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за оборудование>", Replace: Word.WdReplace.wdReplaceAll);

                                        var que = wordRange.Find.Replacement.Highlight;

                                        wordRange.Find.Replacement.ClearFormatting();
                                    }

                                    break;
                                }
                            case "@ResponsibleForData":
                                {
                                    if (businesstrip.Command.ResponsibleForData != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} – {1} {2} {3}", 
                                            HelperReports.GetNameDativeByName(businesstrip.Command.ResponsibleForData.Name),
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForData.Position.Dative),
                                            !HelperReports.IsPositionHead(businesstrip.Command.ResponsibleForData.Position.Nominative) && businesstrip.Command.ResponsibleForData.Department != null ?
                                            HelperReports.GetCategoryGenitiveByDepartmentCategory(businesstrip.Command.ResponsibleForData.Department.Category) : "",
                                            businesstrip.Command.ResponsibleForData.Department != null ?
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForData.Department.Nominative) : ""), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за данные>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForCommunication":
                                {
                                    if (businesstrip.Command.ResponsibleForCommunication != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1}", 
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForCommunication.Position.Dative),
                                            HelperReports.GetNameDativeByName(businesstrip.Command.ResponsibleForCommunication.Name)), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за связь>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForTechnicalSecurity":
                                {
                                    if (businesstrip.Command.ResponsibleForTechnicalSecurity != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1}", 
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForTechnicalSecurity.Position.Genitive),
                                            HelperReports.GetGenderByName(businesstrip.Command.ResponsibleForTechnicalSecurity.Name) ?
                                            HelperReports.GetNameAccusativeByName(businesstrip.Command.ResponsibleForTechnicalSecurity.Name) :
                                            HelperReports.GetNameGenitiveByName(businesstrip.Command.ResponsibleForTechnicalSecurity.Name)), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за техническую безопасность>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForFireSecurity":
                                {
                                    if (businesstrip.Command.ResponsibleForFireSecurity != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1}", 
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForFireSecurity.Position.Genitive),
                                            HelperReports.GetGenderByName(businesstrip.Command.ResponsibleForFireSecurity.Name) ?
                                            HelperReports.GetNameAccusativeByName(businesstrip.Command.ResponsibleForFireSecurity.Name) :
                                            HelperReports.GetNameGenitiveByName(businesstrip.Command.ResponsibleForFireSecurity.Name)), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за пожарную безопасность>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForIndustrialSecurity":
                                {
                                    if (businesstrip.Command.ResponsibleForIndustrialSecurity != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1}", 
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForIndustrialSecurity.Position.Genitive),
                                            HelperReports.GetGenderByName(businesstrip.Command.ResponsibleForIndustrialSecurity.Name) ?
                                            HelperReports.GetNameAccusativeByName(businesstrip.Command.ResponsibleForIndustrialSecurity.Name) :
                                            HelperReports.GetNameGenitiveByName(businesstrip.Command.ResponsibleForIndustrialSecurity.Name)), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за промышленную безопасность>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForRigInspection":
                                {
                                    if (businesstrip.Command.ResponsibleForRigInspection != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1}", 
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForRigInspection.Position.Genitive),
                                            HelperReports.GetGenderByName(businesstrip.Command.ResponsibleForRigInspection.Name) ?
                                            HelperReports.GetNameAccusativeByName(businesstrip.Command.ResponsibleForRigInspection.Name) :
                                            HelperReports.GetNameGenitiveByName(businesstrip.Command.ResponsibleForRigInspection.Name)), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за осмотр буровой>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@DateBegin":
                                {
                                    var dateBegins = businesstrip.Directions != null ? businesstrip.Directions.Select(r => r.DateBegin) : null;
                                    DateTime? minDate = dateBegins != null ? dateBegins.Where(d => d == dateBegins.Min(x => x)).FirstOrDefault() : null;

                                    if (minDate != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: minDate.Value.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задана дата начала командировки>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }

                                    break;
                                }
                            case "@DateEnd":
                                {
                                    var dateEnds = businesstrip.Directions != null ? businesstrip.Directions.Select(r => r.DateEnd) : null;
                                    DateTime? maxDate = dateEnds != null ? dateEnds.Where(d => d == dateEnds.Max(x => x)).FirstOrDefault() : null;

                                    if (maxDate != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: maxDate.Value.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задана дата окончания командировки>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForDealer":
                                {
                                    if (businesstrip.Command.ResponsibleForDealer != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1}", 
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForDealer.Position.Dative),
                                            HelperReports.GetNameDativeByName(businesstrip.Command.ResponsibleForDealer.Name)), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за сдачу материала>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForReceiving":
                                {
                                    if (businesstrip.Command.ResponsibleForReceiving != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1} {2} {3}", 
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForReceiving.Position.Dative),
                                            !HelperReports.IsPositionHead(businesstrip.Command.ResponsibleForReceiving.Position.Nominative) && businesstrip.Command.ResponsibleForReceiving.Department != null ?
                                            HelperReports.GetCategoryGenitiveByDepartmentCategory(businesstrip.Command.ResponsibleForReceiving.Department.Category) : "",
                                            businesstrip.Command.ResponsibleForReceiving.Department != null ?
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForReceiving.Department.Nominative) : "",
                                            HelperReports.GetNameDativeByName(businesstrip.Command.ResponsibleForReceiving.Name)), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за прием материала>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForMonitoring":
                                {
                                    if (businesstrip.Command.ResponsibleForMonitoring != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1} {2} – {3}", 
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForMonitoring.Position.Genitive),
                                            !HelperReports.IsPositionHead(businesstrip.Command.ResponsibleForMonitoring.Position.Nominative) && businesstrip.Command.ResponsibleForMonitoring.Department != null ?
                                            HelperReports.GetCategoryGenitiveByDepartmentCategory(businesstrip.Command.ResponsibleForMonitoring.Department.Category) : "",
                                            businesstrip.Command.ResponsibleForMonitoring.Department != null ?
                                            HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForMonitoring.Department.Nominative) : "",
                                            HelperReports.GetGenderByName(businesstrip.Command.ResponsibleForMonitoring.Name) ?
                                            HelperReports.GetNameAccusativeByName(businesstrip.Command.ResponsibleForMonitoring.Name) :
                                            HelperReports.GetNameGenitiveByName(businesstrip.Command.ResponsibleForMonitoring.Name)), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за контроль выполнения приказа>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ResponsibleForInformation":
                                {
                                    if (businesstrip.Command.ResponsibleForInformation != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1} {2} {3}", 
                                            HelperReports.GetNameDativeByName(businesstrip.Command.ResponsibleForInformation.Name),
                                             HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForInformation.Position.Dative),
                                             !HelperReports.IsPositionHead(businesstrip.Command.ResponsibleForInformation.Position.Nominative) && businesstrip.Command.ResponsibleForInformation.Department != null ?
                                             HelperReports.GetCategoryGenitiveByDepartmentCategory(businesstrip.Command.ResponsibleForInformation.Department.Category) : "",
                                             businesstrip.Command.ResponsibleForInformation.Department != null ?
                                             HelperReports.TransformationBeginStr(businesstrip.Command.ResponsibleForInformation.Department.Nominative) : ""), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан отв. за оповещение>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@PositionHeadOrganization":
                                {
                                    if (businesstrip.HeadOrganization != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: Regex.Replace(string.Format("{0} {1}\r{2}", 
                                            HelperReports.TransformationBeginStr(businesstrip.HeadOrganization.Position.Nominative, false),
                                             HelperReports.IsPositionHead(businesstrip.HeadOrganization.Position.Nominative) && businesstrip.HeadOrganization.Department != null ?
                                             HelperReports.TransformationBeginStr(businesstrip.HeadOrganization.Department.Nominative) : "",
                                             !string.IsNullOrWhiteSpace(businesstrip.ProxyHeadO) ? string.Format("({0})",HelperReports.TransformationBeginStr(businesstrip.ProxyHeadO)) : ""), "[ ]+", " ").TrimEnd(' '), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан начальник организации>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@NameHeadOrganization":
                                {
                                    if (businesstrip.HeadOrganization != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: 
                                            HelperReports.GetNameShort(businesstrip.HeadOrganization.Name), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задан начальник организации>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@DateFormulation":
                                {
                                    if (businesstrip.DateFormulation != null)
                                    {
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: 
                                            businesstrip.DateFormulation.ToShortDateString(), Replace: Word.WdReplace.wdReplaceAll);
                                    }
                                    break;
                                }
                            case "@Target":
                                {
                                    if (businesstrip.Target != "")
                                    {
                                        wordRange.Find.Execute(strToFindObj);
                                        wordRange.Text = businesstrip.Target;
                                        wordRange.Collapse();
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не задана цель>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    
                                    break;
                                }
                            case "@Directions":
                                {
                                    if (businesstrip.Directions != null)
                                    {
                                        if (businesstrip.Directions.Count != 0)
                                        {
                                            _application.Selection.Find.Execute(strToFindObj);

                                            Word.Range wrdRng = _application.Selection.Range;
                                            wrdRng.Text = "";

                                            Word.Paragraph assets = wrdRng.Paragraphs.Add();//_document.Content.Paragraphs.Add();

                                            DirectionComparer directionComparer = new DirectionComparer();
                                            List<Direction> directions = businesstrip.Directions != null ? businesstrip.Directions : new List<Direction>();
                                            directions.Sort(directionComparer);

                                            assets.Range.ListFormat.ApplyBulletDefault();

                                            for (int j = 0; j < directions.Count; j++)
                                            {
                                                if (j == directions.Count - 1)
                                                    assets.Range.InsertBefore(string.Format("{0}.", directions[j].ToString()));
                                                else
                                                    assets.Range.InsertBefore(string.Format("{0};\r", directions[j].ToString()));
                                            }
                                        }
                                        else
                                        {
                                            wordRange.Find.Replacement.Highlight = 14;
                                            wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не заданы направления>", Replace: Word.WdReplace.wdReplaceAll);
                                            wordRange.Find.Replacement.ClearFormatting();
                                        }
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: "<не заданы направления>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }
                                    break;
                                }
                            case "@ObjectD":
                                {
                                    if (businesstrip.Directions != null)
                                    {
                                        var directionsDistinct = businesstrip.Directions.Select(d => d.Reason).Distinct();

                                        if (directionsDistinct.Count() != 0)
                                        {
                                            wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: directionsDistinct.Count() > 1 ? "объектам" : "объекту", Replace: Word.WdReplace.wdReplaceAll);
                                        }
                                        else
                                        {
                                            wordRange.Find.Replacement.Highlight = 14;
                                            wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано объектам\объекту>", Replace: Word.WdReplace.wdReplaceAll);
                                            wordRange.Find.Replacement.ClearFormatting();
                                        }
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано объектам\объекту>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }


                                    break;
                                }
                            case "@Composition":
                                {
                                    if (businesstrip.Party != null)
                                    {
                                        if (businesstrip.Party.PartyPersons != null)
                                        {
                                            if (businesstrip.Party.PartyPersons.Count != 0)
                                            {
                                                wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: 
                                                    businesstrip.Party.PartyPersons.Count > 1 ? "в следующем составе" : "следующего работника", Replace: Word.WdReplace.wdReplaceAll);
                                            }
                                            else
                                            {
                                                wordRange.Find.Replacement.Highlight = 14;
                                                wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано в следующем составе\следующего работника>", Replace: Word.WdReplace.wdReplaceAll);
                                                wordRange.Find.Replacement.ClearFormatting();
                                            }
                                        }
                                        else
                                        {
                                            wordRange.Find.Replacement.Highlight = 14;
                                            wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано в следующем составе\следующего работника>", Replace: Word.WdReplace.wdReplaceAll);
                                            wordRange.Find.Replacement.ClearFormatting();
                                        }
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано в следующем составе\следующего работника>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }

                                    break;
                                }
                            case "@Ending":
                                {
                                    if (businesstrip.Party != null)
                                    {
                                        if (businesstrip.Party.PartyPersons != null)
                                        {
                                            if (businesstrip.Party.PartyPersons.Count != 0)
                                            {
                                                wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith:
                                                    businesstrip.Party.PartyPersons.Count > 1 ? "ов" : "а", Replace: Word.WdReplace.wdReplaceAll);
                                            }
                                            else
                                            {
                                                wordRange.Find.Replacement.Highlight = 14;
                                                wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано ов\а>", Replace: Word.WdReplace.wdReplaceAll);
                                                wordRange.Find.Replacement.ClearFormatting();
                                            }
                                        }
                                        else
                                        {
                                            wordRange.Find.Replacement.Highlight = 14;
                                            wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано ов\а>", Replace: Word.WdReplace.wdReplaceAll);
                                            wordRange.Find.Replacement.ClearFormatting();
                                        }
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано ов\а>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }

                                    break;
                                }
                            case "@IsHoliday":
                                {
                                    wordRange.Find.Execute(strToFindObj);
                                    wordRange.Text = businesstrip.IsHoliday ? 
                                        "\rРаботу выполнять в праздничный день – @Holiday. \rОплату за работу в выходные нерабочие дни производить" +
                                        " в размере двойной дневной части оклада за день работы сверх должностного оклада, согласно части 1 статьи 153 Трудового"+
                                        " Кодекса Российской Федерации. \rОтделу по работе с персоналом ознакомить сотрудников с правом отказаться от работы в выходные" +
                                        " и нерабочие праздничные дни, согласно части 5 статьи 99 и части 7 статьи 113 Трудового Кодекса Российской Федерации." : "";
                                    wordRange.Collapse();

                                    //wordRange.Find.Replacement.Highlight = 14;
                                    //wordRange.Find.Execute(FindText: "<праздничный день>");//, ReplaceWith: "<праздничный день>", Replace: Word.WdReplace.wdReplaceAll);
                                    //wordRange.Find.Replacement.ClearFormatting();

                                    //wordRange.Find.ClearFormatting();

                                    break;
                                }
                            case "@IsOrenburgWork":
                                {
                                    wordRange.Find.Execute(strToFindObj);
                                    wordRange.Text = businesstrip.IsOrenburgWork ?
                                        "\rООО «РН-Учёт» при расчете заработной платы сотрудника, указанных в п. 1 настоящего приказа, в период проведения инженерно - геологических"+
                                        " работ в Оренбургской области, применять районный коэффициент в размере 1, 15 на основания Постановления Госкомтруда СССР, Секретариата ВЦСПС" +
                                        " от 02.07.1987 №403 / 20 - 155." : "";
                                    wordRange.Collapse();

                                    break;
                                }
                            case "@SaidWorker":
                                {
                                    if (businesstrip.Party != null)
                                    {
                                        if (businesstrip.Party.PartyPersons != null)
                                        {
                                            if (businesstrip.Party.PartyPersons.Count != 0)
                                            {
                                                wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith:
                                                    businesstrip.Party.PartyPersons.Count > 1 ? "вышеуказанных сотрудников" : "вышеуказанного сотрудника", Replace: Word.WdReplace.wdReplaceAll);
                                            }
                                            else
                                            {
                                                wordRange.Find.Replacement.Highlight = 14;
                                                wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано вышеуказанных сотрудников\вышеуказанного сотрудника>", Replace: Word.WdReplace.wdReplaceAll);
                                                wordRange.Find.Replacement.ClearFormatting();
                                            }
                                        }
                                        else
                                        {
                                            wordRange.Find.Replacement.Highlight = 14;
                                            wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано вышеуказанных сотрудников\вышеуказанного сотрудника>", Replace: Word.WdReplace.wdReplaceAll);
                                            wordRange.Find.Replacement.ClearFormatting();
                                        }
                                    }
                                    else
                                    {
                                        wordRange.Find.Replacement.Highlight = 14;
                                        wordRange.Find.Execute(FindText: strToFindObj, ReplaceWith: @"<не задано вышеуказанных сотрудников\вышеуказанного сотрудника>", Replace: Word.WdReplace.wdReplaceAll);
                                        wordRange.Find.Replacement.ClearFormatting();
                                    }

                                    break;
                                }
                        }
                    }
                }

                for (int i = 1; i <= _document.Sections.Count; i++)
                {
                    // берем всю секцию диапазоном
                    wordRange = _document.Sections[i].Range;
                    wordRange.Find.ClearFormatting();

                    wordRange.Find.Replacement.Highlight = 14;
                    wordRange.Find.Execute(FindText: "@Holiday", ReplaceWith: "<выходной день>", Replace: Word.WdReplace.wdReplaceAll);

                }

                Word.Table wordTable = _document.Tables[2];
                int row = 2, k = 0;

                PartyPersonComparer partyPersonComparer = new PartyPersonComparer();
                List<PartyPerson> persons = businesstrip.Party != null ? (businesstrip.Party.PartyPersons != null ?
                    businesstrip.Party.PartyPersons.ToList() : new List<PartyPerson>()) : new List<PartyPerson>();
                persons.Sort(partyPersonComparer);

                foreach (var person in persons)
                {
                    if (person != null)
                    {
                        row++; k++;
                        wordTable.Rows.Add();
                        wordTable.Cell(row, 1).Range.Text = k.ToString();
                        wordTable.Cell(row, 2).Range.Text = HelperReports.GetNameShort(person.Name);
                        wordTable.Cell(row, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        wordTable.Cell(row, 3).Range.Text = HelperReports.TransformationBeginStr(person.Position);
                        wordTable.Cell(row, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        wordTable.Cell(row, 4).Range.Text = person.Person.Department != null ? person.Person.Department.Number : "";
                        wordTable.Cell(row, 5).Range.Text = "";
                    }
                }

                object bookmark_name = "NumberDocument";
                if (_document.Bookmarks.Exists(bookmark_name.ToString()))
                {
                    wordRange = _document.Bookmarks.get_Item(ref bookmark_name).Range;
                    wordRange.Text = businesstrip.NumberDocument;
                }

                bookmark_name = "DateFormulation";
                if (_document.Bookmarks.Exists(bookmark_name.ToString()))
                {
                    wordRange = _document.Bookmarks.get_Item(ref bookmark_name).Range;
                    wordRange.Text = businesstrip.DateFormulation != null ? businesstrip.DateFormulation.ToShortDateString() : "";
                }

                bookmark_name = "NumberDocumentOne";
                if (_document.Bookmarks.Exists(bookmark_name.ToString()))
                {
                    wordRange = _document.Bookmarks.get_Item(ref bookmark_name).Range;
                    wordRange.Text = businesstrip.NumberDocument;
                }

                bookmark_name = "DateFormulationOne";
                if (_document.Bookmarks.Exists(bookmark_name.ToString()))
                {
                    wordRange = _document.Bookmarks.get_Item(ref bookmark_name).Range;
                    wordRange.Text = businesstrip.DateFormulation != null ? businesstrip.DateFormulation.ToShortDateString() : "";
                }

                bookmark_name = "Page";
                if (_document.Bookmarks.Exists(bookmark_name.ToString()))
                {
                    Word.WdStatistic PagesCountStat = Word.WdStatistic.wdStatisticPages;
                    int pagesCount = _document.ComputeStatistics(PagesCountStat);

                    _document.Application.Selection.GoTo(Word.WdGoToItem.wdGoToPage, Word.WdGoToDirection.wdGoToAbsolute, pagesCount);
                    _document.Bookmarks.get_Item(ref bookmark_name).Range.Text = "";
                }
                _application.Visible = true;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при экспорте приказа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                _document.Close(ref _falseObj, ref _missingObj, ref _missingObj);
                _application.Quit(ref _missingObj, ref _missingObj, ref _missingObj);
                _document = null;
                _application = null;


            }
        }

        public static void GetDataTemplete(Core.BusinessTrip.Domain.BusinessTrip businessTrip)
        {
            //создаем обьект приложения word
            _application = new Word.Application();

            // если вылетим не этом этапе, приложение останется открытым
            try
            {
                // создаем путь к файлу
                //Object templatePathObj = System.IO.Path.GetFullPath(string.Format(@"\\fs21402\Templates\WindowsPrograms\IT-Distr\028_BusinessTrip\Content\Doc\{0}\{1}", businessTrip.IsHoliday ? "TempleteDecreeHoliday" : "TempleteDecree", businessTrip.TypeWork != null ? businessTrip.TypeWork.TemplateDecree : "Decree.dotx"));//"путь к файлу шаблона";
                Object templatePathObj = System.IO.Path.GetFullPath(string.Format(@"\\fs21402\Templates\WindowsPrograms\IT-Distr\028_BusinessTrip\Content\Doc\{0}\{1}", "TempleteDecree", businessTrip.TypeWork != null ? businessTrip.TypeWork.TemplateDecree : "Decree.dotx"));

                _document = _application.Documents.Add(ref templatePathObj, ref _missingObj, ref _missingObj, ref _missingObj);
            }
            catch (Exception ex)
            {
                _document.Close(ref _falseObj, ref _missingObj, ref _missingObj);
                _application.Quit(ref _missingObj, ref _missingObj, ref _missingObj);
                _document = null;
                _application = null;

                _logger.Error(string.Format("{0} | {1} | {2}", DirectoryName, ex.Message, ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : ""));
                MessageBox.Show("Ошибка при экспорте приказа.", DirectoryName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
