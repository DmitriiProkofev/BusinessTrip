using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.BusinessTrip.Domain
{
    /// <summary>
    /// Вид работ.
    /// </summary>
    [DataContract]
    public class TypeWork
    {
        #region Constructors

        public TypeWork()
        {
        }

        public TypeWork(string name)
        {
            Name = name;
        }

        #endregion //Constructors

        #region Data Properties

        /// <summary>
        /// Ид.
        /// </summary>
        [DataMember]
        [Key]
        public int TypeWorkId { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Шаблон приказа.
        /// </summary>
        [DataMember]
        [MaxLength(300)]
        public string TemplateDecree { get; set; }

        /// <summary>
        /// Шаблон задания.
        /// </summary>
        [DataMember]
        [MaxLength(300)]
        public string TemplateTask { get; set; }

        /// <summary>
        /// Шаблон запрос автотранспорта.
        /// </summary>
        [DataMember]
        [MaxLength(300)]
        public string TemplateRequestTransport { get; set; }

        #endregion //Data Properties

        #region Override Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion //Override Methods
    }
}
