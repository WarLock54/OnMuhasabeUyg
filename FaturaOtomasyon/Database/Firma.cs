//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FaturaOtomasyon.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Firma
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Firma()
        {
            this.Teklif = new HashSet<Teklif>();
        }
    
        public int Id { get; set; }
        public string FirmaUnvan { get; set; }
        public string Adres { get; set; }
        public string VergiNo { get; set; }
        public string VergiAdres { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string AdSoyad { get; set; }
        public Nullable<bool> Sil { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teklif> Teklif { get; set; }
    }
}