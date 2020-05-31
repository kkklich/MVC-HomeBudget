//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationHomeBudget.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    
    public partial class Transact
    {
        public int id_trans { get; set; }

        [DataType(DataType.Currency)]
        public Nullable<float> Amount { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> date_Transaction { get; set; }

        [StringLength(100)]
        public string descript { get; set; }
        public Nullable<bool> IfIncome { get; set; }
        public Nullable<int> id_client { get; set; }
        public Nullable<int> id_Client_subcategories { get; set; }
        public Nullable<int> id_Client_wallet { get; set; }
    
        public virtual client client { get; set; }
        public virtual Client_subcategories Client_subcategories { get; set; }
        public virtual Client_wallet Client_wallet { get; set; }
    }
}
