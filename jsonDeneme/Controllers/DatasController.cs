using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using jsonDeneme.Models;

namespace jsonDeneme.Controllers
{
    public class DatasController : Controller
    {
        private jsonTestEntities db = new jsonTestEntities();

        // GET: Datas
        public ActionResult Index()
        {
            return View(db.JsonDatas.ToList());
        }
        [HttpPost]
        public ActionResult Index(string url)
        {
            string a = Request["quary"].ToString();
            if (a!=null)
            {
                db.JsonDatas.AddRange(GetApiData(a));
                db.SaveChanges();
            }
           
           

            return View(db.JsonDatas.ToList());
        }
        // GET: Datas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JsonDatas jsonDatas = db.JsonDatas.Find(id);
            if (jsonDatas == null)
            {
                return HttpNotFound();
            }
            return View(jsonDatas);
        }

        // GET: Datas/Create
        public ActionResult Getxml()
        {

            MemoryStream ms = new MemoryStream();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;
            
            var xml = new XElement("datas", db.JsonDatas.ToList().Select(x => new XElement("data",
                                                  new XAttribute("Id", x.id),
                                                  new XAttribute("name", x.name)
                                                  )));
            Response.ContentType = "text/xml";

            xml.Save(Response.Output);

            return new EmptyResult();

           // return RedirectToAction("Index");
        }

        // POST: Datas/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dataID,data,id")] JsonDatas jsonDatas)
        {
            if (ModelState.IsValid)
            {
                db.JsonDatas.Add(jsonDatas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jsonDatas);
        }

        // GET: Datas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JsonDatas jsonDatas = db.JsonDatas.Find(id);
            if (jsonDatas == null)
            {
                return HttpNotFound();
            }
            return View(jsonDatas);
        }

        // POST: Datas/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dataID,data,id")] JsonDatas jsonDatas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jsonDatas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jsonDatas);
        }

        // GET: Datas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JsonDatas jsonDatas = db.JsonDatas.Find(id);
            if (jsonDatas == null)
            {
                return HttpNotFound();
            }
            return View(jsonDatas);
        }

        // POST: Datas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JsonDatas jsonDatas = db.JsonDatas.Find(id);
            db.JsonDatas.Remove(jsonDatas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public List<JsonDatas> GetApiData(string jsonUrl)
        {

            var apiUrl = jsonUrl;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            string a = (json.Split('[')[1]).Split(']')[0];
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<JsonDatas> jsonList = ser.Deserialize<List<JsonDatas>>("[" + a + "]");
            //END

            return jsonList;
        }
    }
}
