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
    
    public partial class subcategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public subcategories()
        {
            this.Client_subcategories = new HashSet<Client_subcategories>();
        }
    
        public int id_subcategories { get; set; }
        [StringLength(50)]
        public string Name_subcategory { get; set; }
        [StringLength(100)]
        public string description_Catgory { get; set; }
        public Nullable<int> id_categories { get; set; }
    
        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client_subcategories> Client_subcategories { get; set; }
    }
}
