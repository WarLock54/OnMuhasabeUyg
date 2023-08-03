using FaturaOtomasyon.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaturaOtomasyon.Manager
{
    public class TeklifManager
    {
        public static List<TeklifDurum> getTeklifDurums()
        {
            using (var db = new Entities())
            {
                return db.TeklifDurums.ToList();
            }
        }
        public static TeklifDurum getTeklifDurumId(int id)
        {
            using (var db = new Entities())
            {
                return db.TeklifDurums.Find(id);
            }
        }
        public static TeklifDurum DurumEkle(TeklifDurum teklifDurum)
        {
            var db=new Entities();
            db.TeklifDurums.Add(teklifDurum);
            db.SaveChanges();
            return teklifDurum;
        }
        public static TeklifDurum DurumGuncelle(TeklifDurum teklifDurum)
        {
            var db = new Entities();
           var val= db.TeklifDurums.Where(x=>x.Id==teklifDurum.Id).FirstOrDefault();
            val.Adi = teklifDurum.Adi;
            db.SaveChanges();
            return val;
        }
        public static Teklif DurumDuzenle(Teklif teklif)
        {
            var db=new Entities();
            var val = db.Teklifs.Where(x => x.Id == teklif.Id).FirstOrDefault();
            val.TeklifDurum = teklif.TeklifDurum;
            db.SaveChanges();
            return val;
        }
        public static Teklif BilgilendirmeGuncelleme(Teklif teklif)
        {
           var db = new Entities();
                var val = db.Teklifs.Where(x=>x.Id==teklif.Id).FirstOrDefault();
                val.Bilgilendirme = teklif.Id + teklif.Bilgilendirme;
                db.SaveChanges();
                    
                return val;
        }
    

        public static Teklif BilgiGuncelles(List<Teklif> teklifs,Teklif teklif)
        {
            var db = new Entities();
            for (int i = 1; i < teklifs.Count; i++)
            {
                var val = db.Teklifs.Find(teklifs[i].Id);
                val.Bilgilendirme = teklif.Bilgilendirme;
            }
            db.SaveChanges();
            return teklif;
        }
        public static Teklif Ekle(Teklif teklif)
        {
            var db = new Entities();
            
            teklif.TeklifDurum = 2;
            db.Teklifs.Add(teklif);
            db.SaveChanges();

/*            if (!char.IsDigit(teklif.Bilgilendirme[0]))
            {
                BilgilendirmeGuncelleme(teklif);
            }*/
    
            return teklif;
        }
        public static Teklif Sil(int id)
        {
            var db = new Entities();
            var val = db.Teklifs.Find(id);
            val.Odendi = true;
            db.SaveChanges();
            return val;
        }
       
       public static Teklif BilgiGetir(int id)
        {
            using (var db = new Entities())
            {
                var val = db.Teklifs.Find(id);
                val.Id = 0;
       
                val.Indirim = 0;
                val.Miktar = 0;
               
                val.UrunFiyat = null;
                val.Total = 0;
                val.Firma = null;
                return val;
            }

        }
        public static Teklif GetTeklifID(int id)
        {
            using (var db = new Entities())
            {
                return db.Teklifs.Find(id);
            }
        }
        public static List<Teklif> GetTeklis(Teklif teklif)
        {
            using (var db = new Entities())
            {
                return db.Teklifs.Where(x=>x.Bilgilendirme==teklif.Bilgilendirme).ToList();
            }
        }
        public static Teklif Guncelle(Teklif teklif)
        {
            using (var db = new Entities())
            {
                var val = db.Teklifs.Where(x => x.Id == teklif.Id).FirstOrDefault();
                val.GecerlilikTarih = teklif.GecerlilikTarih;
                val.BasTarih = teklif.BasTarih;
                
                val.Total = teklif.Total;
                val.Indirim = teklif.Indirim;
                val.Miktar = teklif.Miktar;
                val.FirmaId = teklif.FirmaId;
                val.Bilgilendirme = teklif.Bilgilendirme;
                val.UrunBaslik = teklif.UrunBaslik;
                val.UrunFiyat = teklif.UrunFiyat;
                val.Note = teklif.Note;
                db.SaveChanges();
                return teklif;
               
            }
        }

        public static List<Teklif> FiyatListele(Teklif teklif)
        {
            using (var db=new Entities())
            {
                return db.Teklifs.Where(x => x.Bilgilendirme == teklif.Bilgilendirme&& x.Odendi != true).ToList();
            }
        }

        public static decimal? ToplamTeklif(Teklif teklif)
        {
            
            using (var db = new Entities())
            {
                decimal? toplam=0;
                var liste= db.Teklifs.Where(x => x.Bilgilendirme == teklif.Bilgilendirme && x.Odendi!=true).ToList();
                foreach(var item in liste)
                {
                    toplam += item.Total;
                }
                return toplam;
            }
        }
        public static decimal? ToplamUrunFiyatTeklif(Teklif teklif)
        {

            using (var db = new Entities())
            {
                decimal? toplam = 0;
                var liste = db.Teklifs.Where(x => x.Bilgilendirme == teklif.Bilgilendirme && x.Odendi != true).ToList();
                foreach (var item in liste)
                {
                    toplam += (item.UrunFiyat*item.Miktar);
                }
                return toplam;
            }
        }
        public static decimal? IndirimHesapla(Teklif teklif)
        {

            using (var db = new Entities())
            {
                decimal? toplam = 0;
                var liste = db.Teklifs.Where(x => x.Bilgilendirme == teklif.Bilgilendirme && x.Odendi != true).ToList();
                foreach (var item in liste)
                {
                    toplam = item.Total-(item.UrunFiyat*item.Miktar);
                }
                return toplam;
            }
        }

    }
}