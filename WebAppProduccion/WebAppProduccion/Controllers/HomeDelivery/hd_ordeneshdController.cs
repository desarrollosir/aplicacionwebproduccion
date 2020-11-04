using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Configuration;

namespace WebAppProduccion.Controllers.HomeDelivery
{
    public class hd_ordeneshdController : Controller
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();
        // GET: hd_ordeneshd
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObtenerOrdenes() 
        {
            try
            {
                var Draw = Request.Form.GetValues("draw").FirstOrDefault();
                var Start = Request.Form.GetValues("start").FirstOrDefault();
                var Length = Request.Form.GetValues("length").FirstOrDefault();
                var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
                var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                var orden = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

                int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
                int Skip = Start != null ? Convert.ToInt32(Start) : 0;
                int TotalRecords = 0;

                List<hd_ordenes> lista = new List<hd_ordenes>();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
                {
                    con.Open();

                    string sql = "exec SP_Ordenes_Index @orden";
                    var query = new SqlCommand(sql, con);

                    if (orden != "")
                    {
                        query.Parameters.AddWithValue("@orden", orden);
                    }
                    else
                    {
                        query.Parameters.AddWithValue("@orden", DBNull.Value);
                    }

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // facturas
                            var ordenesTemp = new hd_ordenes();

                            ordenesTemp.Id = Convert.ToInt32(dr["Id"]);
                            ordenesTemp.NumeroOrden = dr["NumeroOrden"].ToString();
                            ordenesTemp.Guia = dr["Guia"].ToString();
                            ordenesTemp.OracleID = dr["OracleID"].ToString();


                            lista.Add(ordenesTemp);
                        }
                    }
                }

                if (!(string.IsNullOrEmpty(SortColumn) && string.IsNullOrEmpty(SortColumnDir)))
                {
                    lista = lista.OrderBy(SortColumn + " " + SortColumnDir).ToList();
                }

                TotalRecords = lista.ToList().Count();
                var NewItems = lista.Skip(Skip).Take(PageSize == -1 ? TotalRecords : PageSize).ToList();

                return Json(new { draw = Draw, recordsFiltered = TotalRecords, recordsTotal = TotalRecords, data = NewItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult CargarOrdenes()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportarOrdenes(HttpPostedFileBase postedFileBase)
        {
            try
            {
                string filePath = string.Empty;

                if (postedFileBase != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFileBase.FileName);
                    string extension = Path.GetExtension(postedFileBase.FileName);
                    postedFileBase.SaveAs(filePath);

                    DataTable dtCharge = new DataTable();

                    dtCharge.Columns.AddRange(new DataColumn[5] {
                        //0
                        new DataColumn("sku", typeof(string)),
                        //1                        
                        new DataColumn("qty", typeof(int)),
                        //2
                        new DataColumn("orden", typeof(string)),
                        //3
                        new DataColumn("oracleid", typeof(string)),
                        //4
                        new DataColumn("guia", typeof(string)),                       
                    });

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            dtCharge.Rows.Add();
                            int i = 0;

                            //Execute a loop over the columns.
                            foreach (string cell in row.Split(','))
                            {
                                dtCharge.Rows[dtCharge.Rows.Count - 1][i] = cell;

                                i++;
                            }
                        }
                    }

                    bool validarskus = ValidarSKUS(dtCharge);
                    bool validarskushd = ValidarSKUSHD(dtCharge);
                    if (ValidarOrdenes(dtCharge).Count() > 0)
                    {
                        AgregarSkusOrden(dtCharge)
;                    } 

                }
                return RedirectToAction("Index");
            }
            catch (Exception _ex)
            {
                return RedirectToAction("Index");
            }            
        }

        public bool ValidarSKUS(DataTable dt) 
        {
            try
            {
                List<skus> listaSKUS = new List<skus>();
                List<skus> listabusqueda = db.skus.ToList();
                foreach (DataRow row in dt.Rows)
                {
                    string sku = row[0].ToString().Trim().ToUpper();

                    var busquedasku = listabusqueda.Where(x => x.codigobarras.Equals(sku)).FirstOrDefault();                    

                    if (busquedasku == null)
                    {
                        if (listaSKUS.Where(x => x.SKU == sku).FirstOrDefault() == null)
                        {
                            skus skus = new skus();
                            skus.codigobarras = sku;
                            skus.SKU = sku;
                            skus.uom_id = 1;

                            listaSKUS.Add(skus);
                        }                       
                    }                   
                }

                db.skus.AddRange(listaSKUS);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidarSKUSHD(DataTable dt)
        {
            List<hd_skushomedelivery> listaSKUSHD = new List<hd_skushomedelivery>();
            List<hd_skushomedelivery> listabusauqeda = db.hd_skushomedelivery.ToList();
            List<skus> listabusquedasku = db.skus.ToList();
            foreach (DataRow row in dt.Rows)
            {
                string sku = row[0].ToString().Trim().ToUpper();

                var busquedaskuhd = listabusauqeda.Where(x => x.skus.codigobarras == sku).FirstOrDefault();

                if (busquedaskuhd == null)
                {
                    int idskuhd = listabusquedasku.Where(x => x.codigobarras == sku).FirstOrDefault().id;

                    if (listaSKUSHD.Where(x => x.skus_Id == idskuhd) == null)
                    {
                        hd_skushomedelivery hd_sku = new hd_skushomedelivery();
                        hd_sku.QRCode = false;
                        hd_sku.QtyManual = false;
                        hd_sku.skus_Id = idskuhd;
                        listaSKUSHD.Add(hd_sku);
                    }
                }
            }

            db.hd_skushomedelivery.AddRange(listaSKUSHD);
            db.SaveChanges();

            return true;
        }

        public List<hd_ordenes> ValidarOrdenes(DataTable dt) 
        {            
            List<hd_ordenes> listaordenes = db.hd_ordenes.Take(5000).ToList();
            List<hd_ordenes> listaAgregar = new List<hd_ordenes>();
            
            foreach (DataRow row in dt.Rows)
            {
                string orden = row[2].ToString().Trim().ToUpper();
                if (listaordenes.Where(x => x.NumeroOrden.Equals(orden)).FirstOrDefault() == null)
                {
                    if (listaAgregar.Where(x => x.NumeroOrden == orden).FirstOrDefault() == null)
                    {
                        hd_ordenes ordenes = new hd_ordenes();
                        ordenes.NumeroOrden = orden;
                        ordenes.OracleID = row[3].ToString().Trim().ToUpper();
                        ordenes.Guia = row[4].ToString().Trim().ToUpper();
                        ordenes.Paquetes = 1;
                        ordenes.FechaAlta = DateTime.Now;
                        ordenes.Pickers_Id = 1;
                        ordenes.Auditores_Id = 1;
                        ordenes.hd_statusorden_Id = 1;
                        ordenes.hd_statusquickship_Id = 1;
                        ordenes.Empleados_Id = 1;

                        listaAgregar.Add(ordenes);
                    }                   
                }
            }

            db.hd_ordenes.AddRange(listaAgregar);
            db.SaveChanges();
            return listaAgregar;
        }

        public void AgregarSkusOrden(DataTable dt) 
        {
            List<hd_detordenesskus> detalle = new List<hd_detordenesskus>();
            List<hd_ordenes> listaordenes = db.hd_ordenes.Take(5000).ToList();
            List<hd_skushomedelivery> listabusauqedaskuhd = db.hd_skushomedelivery.ToList();

            foreach (DataRow row in dt.Rows)
            {
                string sku = row[0].ToString().Trim().ToUpper();                
                string orden = row[2].ToString().Trim().ToUpper();                               
                int qty = (int.Parse(row[1].ToString().Trim().ToUpper()) * -1);

                hd_detordenesskus detordenesskus = new hd_detordenesskus();
                detordenesskus.hd_ordenes_Id = listaordenes.Where(x => x.NumeroOrden == orden).FirstOrDefault().Id;
                detordenesskus.hd_skushomedelivery_Id = listabusauqedaskuhd.Where(x => x.skus.codigobarras == sku).FirstOrDefault().Id;
                detordenesskus.Cantidad = qty;

                detalle.Add(detordenesskus);
            }

            db.hd_detordenesskus.AddRange(detalle);
            db.SaveChanges();
        }
    }
}