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
    
    public partial class Client_subcategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client_subcategories()
        {
            this.Transact = new HashSet<Transact>();
        }
    
        public int id_Client_subcategories { get; set; }
        public Nullable<int> id_subcategories { get; set; }
        public Nullable<int> id_client { get; set; }
    
        public virtual client client { get; set; }
        public virtual subcategories subcategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transact> Transact { get; set; }
    }
}