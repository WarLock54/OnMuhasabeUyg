using FastReport.Web;
using FaturaOtomasyon.Database;
using FaturaOtomasyon.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaturaOtomasyon.Controllers
{
    public class TeklifController : Controller
    {
        #region TeklifDurum
        public ActionResult TeklifDurumList()
        {
            var db = new Entities();
            ViewBag.Get = db.TeklifDurums.ToList().OrderByDescending(x => x.Adi);
            return View();
        }
        [HttpPost]
        public ActionResult TeklifDurumArama(FormCollection arama)
        {
            var db = new Entities();
            var val = Convert.ToString(arama["Adi"]);
            ViewBag.Get = db.TeklifDurums.Where(x => x.Adi == val).ToList();
            return View("TeklifDurumList");
        }
        public ActionResult GetTeklifDurumById(int id)
        {
       var val=     TeklifManager.getTeklifDurumId(id);
            return View(val);
        }
       
        [HttpPost]
        public ActionResult TeklifDurumEkle(TeklifDurum teklifDurum)
        {
            var db=new Entities();
            TeklifManager.DurumEkle(teklifDurum);
            ViewBag.Get = db.TeklifDurums.ToList().OrderByDescending(x => x.Adi);
            return RedirectToAction("TeklifDurumList", "Teklif");
        }
       
        [HttpPost]
        public ActionResult TeklifDurumGuncelle(TeklifDurum teklifDurum)
        {
            var db = new Entities();
            TeklifManager.DurumGuncelle(teklifDurum);
            ViewBag.Get = db.TeklifDurums.ToList().OrderByDescending(x => x.Adi);
            return RedirectToAction("TeklifDurumList", "Teklif");
        }
        #endregion
      
        #region Teklif
        public DataTable tablo_gonder(string sql,string inputVal)
        {
            DataTable table = new DataTable();
            string connectionString = "data source=DESKTOP-SSV70KU\\SQLEXPRESS;initial catalog=CariDemo;integrated security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Bilgilendirme", inputVal);
                    SqlDataReader reader = command.ExecuteReader();

                    table.Load(reader);
                }
                catch (Exception ex)
                {
                    connection.Close();
                    throw ex;
                }

            }

            return table;
        }
        //PREWİEV KISMINI BURADA PDF HALİNDE GÖSTER...
        public ActionResult PdfDonustur(int id)
        {

            var val = TeklifManager.GetTeklifID(id);
            
            FastReport.Utils.Config.WebMode = true;
            string sql= "select * from Teklif tl INNER JOIN Firma fm on fm.Id = tl.FirmaId  INNER JOIN TeklifDurum td on td.Id = tl.TeklifDurum where tl.Bilgilendirme = @Bilgilendirme";
            DataTable data1 = tablo_gonder(sql,val.Bilgilendirme).Copy();

            using (var webReport = new WebReport())
            {
                webReport.Width = 750;
                webReport.Height = 1000;
                webReport.ToolbarBackgroundStyle = ToolbarBackgroundStyle.Light;
                webReport.ToolbarIconsStyle = ToolbarIconsStyle.Blue;
                webReport.ReportFile = this.Server.MapPath("~/Content/Teklif.frx");
                webReport.RegisterData(data1, "Teklif");
                webReport.Report.SetParameterValue("Bilgilendirme", val.Bilgilendirme);
                ViewBag.WebReport = webReport;
                return View();
            }

        }
        // GET: Teklif
        public ActionResult TeklifList(int page=1)
        {
            var db = new Entities();
            var list = db.Teklifs.Where(x => x.Odendi != true && x.Firma.Sil != true)
    .OrderByDescending(x => x.Id) // Büyükten küçüğe sıralama işlemi burada yapılıyor
    .GroupBy(x => x.Bilgilendirme)
    .Select(group => group.FirstOrDefault()) // Her gruptan en son eklenen teklifi seçiyoruz
    .ToList();

            //pagination
            var itemCount = list.Count();
            ViewBag.Count = itemCount;
            int pageSize = 10;
            int pageNumber = (int)Math.Ceiling((double)itemCount / pageSize); // kaç sayfa olacağı
            int currentPage = page;
            ViewBag.List = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
          

            ViewBag.TeklifDurum = new SelectList(TeklifManager.getTeklifDurums().ToList(), "Id", "Adi");
           

            return View();

        }
        [HttpPost]
        public ActionResult DurumArama(FormCollection arama)
        {
            var db=new Entities();
            //TeklifDurum
            var ara = Convert.ToInt32(arama["TeklifDurum"]);
            ViewBag.List = db.Teklifs.Where(x => x.Odendi != true && x.Firma.Sil != true && x.TeklifDurum == ara)
         .OrderByDescending(x => x.Id) // Büyükten küçüğe sıralama işlemi burada yapılıyor
         .GroupBy(x => x.Bilgilendirme)
         .Select(group => group.FirstOrDefault()) // Her gruptan en son eklenen teklifi seçiyoruz
         .ToList();
            ViewBag.TeklifDurum = new SelectList(TeklifManager.getTeklifDurums().ToList(), "Id", "Adi");
         

            return View("TeklifList");
        }

        [HttpPost]
        public ActionResult Arama(FormCollection arama)
        {
            var db = new Entities();
            var val = Convert.ToString(arama["AdSoyad"]);
            ViewBag.List = db.Teklifs.Where(x => x.Firma.FirmaUnvan == val || x.Firma.AdSoyad == val)
         .OrderByDescending(x => x.Id) // Büyükten küçüğe sıralama işlemi burada yapılıyor
         .GroupBy(x => x.Bilgilendirme)
         .Select(group => group.FirstOrDefault()) // Her gruptan en son eklenen teklifi seçiyoruz
         .ToList();

            ViewBag.TeklifDurum = new SelectList(TeklifManager.getTeklifDurums().ToList(), "Id", "Adi");
            

            return View("TeklifList");
        }
      

        public ActionResult TeklifGuncelle(int id)
        {
            var val = TeklifManager.getTeklifDurumId(id);
            ViewBag.TeklifDurum = new SelectList(TeklifManager.getTeklifDurums().ToList(), "Id", "Adi");
            return View(val);

        }
        public ActionResult getTeklifPrintId(int id)
        {
            var teklif = TeklifManager.GetTeklifID(id);
            teklif.Firma = FirmaManager.getFirmaBilgi(teklif.FirmaId);
             var listele= TeklifManager.FiyatListele(teklif);
            
              var Total= TeklifManager.ToplamTeklif(teklif);
             var urun = TeklifManager.ToplamUrunFiyatTeklif(teklif);
           var x=urun+ (urun/5);
            ViewBag.ToplamIndirim = x - Total;
            ViewBag.Listele = listele;
            ViewBag.ToplamTutar = Total;
            ViewBag.ToplamUrun=urun;
            return View(teklif);
        }
        [HttpPost]
        public ActionResult TeklifGuncelle(Teklif teklif)
        {
          var db=new Entities();
            TeklifManager.DurumDuzenle(teklif);
            
            ViewBag.List = db.Teklifs.Where(x => x.Odendi != true && x.Firma.Sil != true).ToList().OrderByDescending(x => x.Id);
            ViewBag.TeklifDurum = new SelectList(TeklifManager.getTeklifDurums().ToList(), "Id", "Adi");

            return RedirectToAction("TeklifList", "Teklif",new { page=1});
        }
       
        public ActionResult GetTeklifById(int id) 
        {



          
               var teklif= TeklifManager.GetTeklifID(id);
            ViewBag.Liste = TeklifManager.GetTeklis(teklif);

            teklif.Firma = FirmaManager.getFirmaBilgi(teklif.FirmaId);
       
            return View(teklif);

        }
        [HttpPost]
        public ActionResult Guncelle(List<Teklif> teklifs)
        {
                        var db = new Entities();
                foreach(var teklif in teklifs) {
                char lastChar = teklif.Bilgilendirme[teklif.Bilgilendirme.Length - 1];




                int lastDigit = int.Parse(lastChar.ToString());
                lastDigit++; // 1'i artırma
                teklif.Bilgilendirme = teklif.Bilgilendirme.Substring(0, teklif.Bilgilendirme.Length - 1) + lastDigit.ToString();
             
            TeklifManager.Guncelle(teklif);
                }
              
            
            ViewBag.List = db.Teklifs.Where(x => x.Odendi != true && x.Firma.Sil != true).ToList().OrderByDescending(x => x.Id);
            // return RedirectToAction("TeklifList", "Teklif", new { page = 1});
            return View(teklifs);
        }
         public ActionResult TeklifSil(int id)
        {
            TeklifManager.Sil(id);
            using (var db = new Entities())
            {
                ViewBag.List = db.Teklifs.Where(x => x.Odendi != true && x.Firma.Sil != true).ToList().OrderByDescending(x => x.Id);
            }
            return RedirectToAction("TeklifList","Teklif");
        }
        [HttpPost]
      public ActionResult FirmaEkle(Firma firma)
        {
            ViewBag.Firmas = new SelectList(FirmaManager.getFirma().ToList(), "Id", "FirmaUnvan");
             FirmaManager.Ekle(firma);
            return RedirectToAction("TeklifEkle", "Teklif");

        }
        public ActionResult TeklifEkle()
        {
            ViewBag.Firmas = new SelectList(FirmaManager.getFirma().ToList(), "Id", "FirmaUnvan");
            return View();
        }
        [HttpPost]
        public ActionResult Ekle(List<Teklif> teklifs)
        {
            var db = new Entities();
            
            foreach (var teklif in teklifs)
            {
            TeklifManager.Ekle(teklif);
            }
            var firstTeklif = TeklifManager.BilgilendirmeGuncelleme(teklifs[0]);
            TeklifManager.BilgiGuncelles(teklifs, firstTeklif);
                ViewBag.List = db.Teklifs.Where(x => x.Odendi != true && x.Firma.Sil != true)
    .OrderByDescending(x => x.Id) // Büyükten küçüğe sıralama işlemi burada yapılıyor
    .GroupBy(x => x.Bilgilendirme)
    .Select(group => group.FirstOrDefault()) // Her gruptan en son eklenen teklifi seçiyoruz
    .ToList();
            //RedirectToAction("getTeklifPrintId", "Teklif",new { id=id}); 
            return RedirectToAction("TeklifList", "Teklif",new { page=1}); 

        }
       
        [HttpPost]
        public ActionResult GetFirmaSahip(int id)
        {
            var val = FirmaManager.getFirmaBilgi(id);
            return Json(new { 
                value = new
                {
                    val.AdSoyad,
                    val.Telefon,
                    val.VergiNo,
                    val.VergiAdres,
                    val.FirmaUnvan,
                    val.Id
                }
            },JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}