using FaturaOtomasyon.Database;
using FaturaOtomasyon.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaturaOtomasyon.Controllers
{
    public class FirmaController : Controller
    {
        // GET: Firma
        public ActionResult FirmaList()
        {
            var db = new Entities();
            ViewBag.Get= db.Firmas.Where(x=>x.Sil!=true).ToList().OrderByDescending(x => x.FirmaUnvan);
            return View();
        }
        [HttpPost]
        public ActionResult Arama(FormCollection arama)
        {
            var db = new Entities();
            var val = Convert.ToString(arama["FirmaUnvan"]);
            ViewBag.Get = db.Firmas.Where(x => x.FirmaUnvan == val&& x.Sil != true).ToList();
            return View("FirmaList");
        }
        public ActionResult GetFirmafById(int id)
        {
            var val = FirmaManager.getFirmaBilgi(id);
            ViewBag.Get = val;
            return View(val);
        }
        [HttpPost]
        public ActionResult Guncelle(Firma firma)
        {
            var db = new Entities();
            FirmaManager.Guncelle(firma);
            ViewBag.Get = db.Firmas.Where(x => x.Sil != true).ToList().OrderByDescending(x => x.FirmaUnvan);

            return RedirectToAction("FirmaList", "Firma");
        }
        
        public ActionResult FirmaEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FirmaEkle(Firma firma)
        {
            FirmaManager.Ekle(firma);
            return RedirectToAction("TeklifList", "Teklif");
        }
        
        public ActionResult FirmaSil(int id)
        {
            FirmaManager.Sil(id);
            return RedirectToAction("TeklifList", "Teklif");
        }
    }
}