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
    
    public partial class Utilizator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utilizator()
        {
            this.Bons = new HashSet<Bon>();
        }
    
        public int IDutilizator { get; set; }
        public string nume_utilizator { get; set; }
        public string parola { get; set; }
        public string tip_utilizator { get; set; }
        public bool exista { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bon> Bons { get; set; }
    }
}
