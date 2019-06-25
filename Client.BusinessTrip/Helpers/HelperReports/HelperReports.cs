using Cyriller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BusinessTrip.Helpers.HelperReports
{
    public static class HelperReports
    {
        private static List<string> _categoriesShort = new List<string> { "групп", "отдел", "управлен" };
    
        public enum Cases
        {
            None = 1,
            Genitive = 2
        }

        public static string GetNameShort(string name)
        {
            var names = name.Split(new char[] { ' ' });
            string nameShort = "";

            if (names.Length > 0)
            {
                nameShort += names[0];

                if (names.Length > 1)
                {
                    if (names[1].Length > 0)
                    {
                        nameShort += string.Format(" {0}.", names[1][0]).ToUpper();

                        if (names.Length > 2)
                        {
                            if (names[2].Length > 0)
                            {
                                nameShort += string.Format(" {0}.", names[2][0]).ToUpper();
                            }
                        }
                    }
                }
            }

            return nameShort;
        }

        public static string GetInitialsByName(string name)
        {
            var names = name.Split(new char[] { ' ' });
            string nameInitials = "";

            if (names.Length > 1)
            {
                if (names[1].Length > 0)
                {
                    nameInitials += string.Format(" {0}.", names[1][0]).ToUpper();

                    if (names.Length > 2)
                    {
                        if (names[2].Length > 0)
                        {
                            nameInitials += string.Format(" {0}.", names[2][0]).ToUpper();
                        }
                    }
                }
            }

            return nameInitials;
        }

        public static string GetNameGenitiveByName(string name)
        {
            // Создаем коллекцию всех существительных.
            CyrNounCollection nouns = new CyrNounCollection();
            // Создаем коллекцию всех прилагательных.
            CyrAdjectiveCollection adjectives = new CyrAdjectiveCollection();
            // Создаем фразу с использование созданных коллекций.
            CyrPhrase phrase = new CyrPhrase(nouns, adjectives);
            CyrName cyrName = new CyrName();

            var newName = string.Format("{0}{1}", cyrName.DeclineSurnameGenitive(GetLastNameByName(name), GetGenderByName(name)), GetInitialsByName(name));

            return newName;
        }

        public static string GetNameDativeByName(string name)
        {
            // Создаем коллекцию всех существительных.
            CyrNounCollection nouns = new CyrNounCollection();
            // Создаем коллекцию всех прилагательных.
            CyrAdjectiveCollection adjectives = new CyrAdjectiveCollection();
            // Создаем фразу с использование созданных коллекций.
            CyrPhrase phrase = new CyrPhrase(nouns, adjectives);
            CyrName cyrName = new CyrName();

            var newName = string.Format("{0}{1}", cyrName.DeclineSurnameDative(GetLastNameByName(name), GetGenderByName(name)), GetInitialsByName(name));

            return newName;
        }

        public static string GetNameAccusativeByName(string name)
        {
            // Создаем коллекцию всех существительных.
            CyrNounCollection nouns = new CyrNounCollection();
            // Создаем коллекцию всех прилагательных.
            CyrAdjectiveCollection adjectives = new CyrAdjectiveCollection();
            // Создаем фразу с использование созданных коллекций.
            CyrPhrase phrase = new CyrPhrase(nouns, adjectives);
            CyrName cyrName = new CyrName();

            var newName = string.Format("{0}{1}", cyrName.DeclineSurnameAccusative(GetLastNameByName(name), GetGenderByName(name)), GetInitialsByName(name));

            return newName;
        }

        /// <summary>
        /// Получение пола по имени.
        /// </summary>
        /// <param name="name">Имя.</param>
        /// <returns>true - женский, false - мужской.</returns>
        public static bool GetGenderByName(string name)
        {
            if (name.EndsWith("на") || name.StartsWith("кызы"))
                return true;
            return false;
        }

        public static string GetLastNameByName(string name)
        {
            var names = name.Split(new char[] { ' ' });
            string lastName = "";

            if (names.Length > 0)
            {
                lastName = names[0];
            }

            return lastName;
        }

        public static string GetCategoryGenitiveByDepartmentCategory(string category) //"Группа", "Отдел", "Управление"
        {
            category = category.ToLower();

            if (category.Contains("отдел"))
            {
                return "отдела";
            }
            else if (category.Contains("управление"))
            {
                return "управления";
            }
            else if (category.Contains("группа"))
            {
                return "группы";
            }
            else
            {
                return "";
            }   
        }

        public static bool IsPositionHead(string position)
        {
            position = position.ToLower();

            foreach (var categoryShort in _categoriesShort)
            {
                if (position.Contains(categoryShort))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetNameMonthByNumber(int number, Cases cases)
        {
            string nameMonth = "";

            switch (number)
            {
                case 1:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "январь";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "января";
                                    break;
                                }
                        }
                        break;
                    }
                case 2:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "февраль";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "февраля";
                                    break;
                                }
                        }
                        break;
                    }
                case 3:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "март";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "марта";
                                    break;
                                }
                        }
                        break;
                    }
                case 4:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "апрель";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "апреля";
                                    break;
                                }
                        }
                        break;
                    }
                case 5:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "май";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "мая";
                                    break;
                                }
                        }
                        break;
                    }
                case 6:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "июнь";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "июня";
                                    break;
                                }
                        }
                        break;
                    }
                case 7:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "июль";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "июля";
                                    break;
                                }
                        }
                        break;
                    }
                case 8:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "август";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "августа";
                                    break;
                                }
                        }
                        break;
                    }
                case 9:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "сентябрь";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "сентября";
                                    break;
                                }
                        }
                        break;
                    }
                case 10:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "октябрь";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "октября";
                                    break;
                                }
                        }
                        break;
                    }
                case 11:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "ноябрь";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "ноября";
                                    break;
                                }
                        }
                        break;
                    }
                case 12:
                    {
                        switch (cases)
                        {
                            case Cases.None:
                                {
                                    nameMonth = "декабрь";
                                    break;
                                }
                            case Cases.Genitive:
                                {
                                    nameMonth = "декабря";
                                    break;
                                }
                        }
                        break;
                    }
            }

            return nameMonth;
        }

        public static string TransformationDay(int day)
        {
            var dayString = day.ToString();

            if (dayString.Length == 1)
            {
                dayString = string.Format("0{0}", dayString);
            }

            return dayString;
        }

        public static string TransformationBeginStr(string str, bool isLower = true)
        {
            if (str.Length > 0)
            {
                string oneChar = "";

                if (isLower)
                {
                    oneChar = str[0].ToString().ToLower();
                }
                else
                {
                    oneChar = str[0].ToString().ToUpper();
                }
                str = string.Format("{0}{1}", oneChar, str.Substring(1));
            }

            return str;
        }
    }
}
