using FaturaOtomasyon.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaturaOtomasyon.Manager
{
    public class FirmaManager
    {
        public static List<Firma> getFirma()
        {
            using(var db = new Entities())
            {
                return db.Firmas.Where(x=>x.Sil!=true).ToList();
            }
        }
        
        public static Firma Sil(int id)
        {
            var db = new Entities();
            var val = db.Firmas.Find(id);
            val.Sil = true;
            db.SaveChanges();
            return val;
        }


        public static Firma getFirmaBilgi(int? id) {
        using(var db = new Entities())
            {
                return db.Firmas.Where(x => x.Id == id).FirstOrDefault();
            }
        }
        public static Firma Guncelle(Firma firma)
        {
            using (var db = new Entities())
            {
                var val = db.Firmas.Where(x => x.Id == firma.Id).FirstOrDefault();
                val.FirmaUnvan = firma.FirmaUnvan;
                val.Adres = firma.Adres;
                val.Email = firma.Email;
                val.Telefon = firma.Telefon;
                val.AdSoyad = firma.AdSoyad;
                db.SaveChanges();
                return firma;

            }
        }
        public static Firma Ekle(Firma firma)
        {
            using (var db = new Entities())
            {
                db.Firmas.Add(firma);
                db.SaveChanges();
                return firma;

            }
        }
    }
}