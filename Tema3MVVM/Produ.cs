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
    
    public partial class Produ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produ()
        {
            this.ProduseVandutes = new HashSet<ProduseVandute>();
            this.Stocs = new HashSet<Stoc>();
        }
    
        public int IDprodus { get; set; }
        public string nume_produs { get; set; }
        public string cod_bare { get; set; }
        public int IDcategorie { get; set; }
        public Nullable<int> IDproducator { get; set; }
        public bool exista { get; set; }
    
        public virtual Categorie Categorie { get; set; }
        public virtual Producator Producator { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProduseVandute> ProduseVandutes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stoc> Stocs { get; set; }
    }
}
