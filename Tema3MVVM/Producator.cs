//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tema3MVVM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Producator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producator()
        {
            this.Produs = new HashSet<Produ>();
        }
    
        public int IDproducator { get; set; }
        public string nume_producator { get; set; }
        public string tara_origine { get; set; }
        public bool exista { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Produ> Produs { get; set; }
    }
}
