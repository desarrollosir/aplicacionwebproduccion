using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosOperaciones;
using System.Linq.Dynamic;
using WebAppProduccion.Entities.ModulosEscaneos;
using System.Threading.Tasks;

namespace WebAppProduccion.Controllers.Operaciones
{
    public class masterskusController : Controller
    {
        DB_A3F19C_producccionEntities3 db = new DB_A3F19C_producccionEntities3();
        DB_A3F19C_producccionEntities db1 = new DB_A3F19C_producccionEntities();
        // GET: masterskus
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int? id)
        {
            wh_masterskus skumaster = db.wh_masterskus.Find(id);
            var sku = db1.skus.Where(x => x.id == skumaster.skus_id).FirstOrDefault();
            skumaster.sku = sku.codigobarras;
            return View(skumaster);
        }

        [HttpPost]
        public ActionResult Edit(wh_masterskus model)
        {
            try
            {
                wh_masterskus skumaster = db.wh_masterskus.Find(model.Id);
                skumaster.PiezasPorCaja = model.PiezasPorCaja;
                skumaster.CajasPorCama = model.CajasPorCama;
                skumaster.PiezasPorTarima = model.PiezasPorTarima;

                db.SaveChanges();

                return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { respuesta = false }, JsonRequestBehavior.AllowGet);                
            }           
        }

        public async Task<ActionResult> Create()
        {
            await AgrarSKUSFromMOLines();
            await AgregarSKUSFromMO();
            await AgregarAMasterDesdeSKUS();
            await AgregarSKUSHDDesdeSKUS();
            return View();
        }

        public async Task AgrarSKUSFromMOLines()
        {
            await Task.Run(() =>
            {
                DB_A3F19C_producccionEntities3 dbMAster = new DB_A3F19C_producccionEntities3();

                DB_A3F19C_producccionEntities dbSKUS = new DB_A3F19C_producccionEntities();

                List<skus> listaSkus = new List<skus>();

                var skusmo = dbMAster.wh_molinesdetalles.GroupBy(x => x.Item).Select(x => x.Key).ToList();

                foreach (var item in skusmo)
                {
                    var sku = dbSKUS.skus.Where(x => x.codigobarras.Equals(item)).FirstOrDefault();

                    if (sku == null)
                    {
                        skus skus = new skus();
                        skus.SKU = item;
                        skus.codigobarras = item;
                        skus.uom_id = 1;
                        listaSkus.Add(skus);
                    }
                }

                dbSKUS.skus.AddRange(listaSkus);
                dbSKUS.SaveChangesAsync();
            });
        }

        public async Task AgregarSKUSFromMO() 
        {
            await Task.Run(() =>
            {
                DB_A3F19C_producccionEntities3 dbMAster = new DB_A3F19C_producccionEntities3();

                DB_A3F19C_producccionEntities dbSKUS = new DB_A3F19C_producccionEntities();

                List<skus> listaSkus = new List<skus>();

                var skusmo = dbMAster.wh_LineasMO.GroupBy(x => x.Item).Select(x => x.Key).ToList();

                foreach (var item in skusmo)
                {
                    var sku = dbSKUS.skus.Where(x => x.codigobarras.Equals(item)).FirstOrDefault();

                    if (sku == null)
                    {
                        skus skus = new skus();
                        skus.SKU = item;
                        skus.codigobarras = item;
                        skus.uom_id = 1;
                        listaSkus.Add(skus);
                    }
                }

                dbSKUS.skus.AddRange(listaSkus);
                dbSKUS.SaveChangesAsync();
            });
        }

        public async Task AgregarAMasterDesdeSKUS() 
        {
            await Task.Run(() =>
            {
                DB_A3F19C_producccionEntities3 dbMaster = new DB_A3F19C_producccionEntities3();
                DB_A3F19C_producccionEntities dbSKUS = new DB_A3F19C_producccionEntities();
                List<wh_masterskus> lista = new List<wh_masterskus>();
                var skus = dbSKUS.skus.ToList();

                foreach (var item in skus)
                {
                    var skumaster = dbMaster.wh_masterskus.Where(x => x.skus_id.Equals(item.id)).FirstOrDefault();

                    if (skumaster == null)
                    {
                        wh_masterskus wh = new wh_masterskus();
                        wh.skus_id = item.id;
                        wh.PiezasPorCaja = 0;
                        wh.PiezasPorTarima = 0;
                        wh.CajasPorCama = 0;

                        lista.Add(wh);                                                
                    }
                }

                dbMaster.wh_masterskus.AddRange(lista);
                dbMaster.SaveChangesAsync();
            });
        }

        public async Task AgregarSKUSHDDesdeSKUS() 
        {
            await Task.Run(() => 
            {
                DB_A3F19C_producccionEntities dbSKUS = new DB_A3F19C_producccionEntities();
                var skus = dbSKUS.skus.ToList();
                List<hd_skushomedelivery> lista = new List<hd_skushomedelivery>();

                foreach (var item in skus)
                {
                    var skumaster = dbSKUS.hd_skushomedelivery.Where(x => x.skus_Id.Equals(item.id)).FirstOrDefault();

                    if (skumaster == null)
                    {
                        hd_skushomedelivery wh = new hd_skushomedelivery();
                        wh.QtyManual = false;
                        wh.QrCode = false;
                        wh.skus_Id = item.id;

                        lista.Add(wh);
                    }
                }
                dbSKUS.hd_skushomedelivery.AddRange(lista);
                dbSKUS.SaveChangesAsync();
            });
        }

        [HttpPost]
        public ActionResult ObtenerMaster()
        {
            var Draw = Request.Form.GetValues("draw").FirstOrDefault();
            var Start = Request.Form.GetValues("start").FirstOrDefault();
            var Length = Request.Form.GetValues("length").FirstOrDefault();
            var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
            var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            var sku = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
            int Skip = Start != null ? Convert.ToInt32(Start) : 0;
            int TotalRecords = 0;

            List<wh_masterskus> lista = new List<wh_masterskus>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
            {
                con.Open();

                string sql = "exec [SP_MasterSKUS_Index_ParametrosOpcionales] @sku";
                var query = new SqlCommand(sql, con);

                if (sku != "")
                {
                    query.Parameters.AddWithValue("@sku", sku);
                }
                else
                {
                    query.Parameters.AddWithValue("@sku", DBNull.Value);
                }

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // facturas
                        var linea = new wh_masterskus();

                        linea.Id = Convert.ToInt32(dr["Id"]);
                        linea.sku = dr["SKU"].ToString();
                        linea.descripcion = dr["Descripcion"].ToString();
                        linea.codigobarras = dr["codigobarras"].ToString();
                        linea.PiezasPorCaja = Convert.ToInt32(dr["PiezasPorCaja"]);
                        linea.CajasPorCama = Convert.ToInt32(dr["CajasPorCama"]);
                        linea.PiezasPorTarima = Convert.ToInt32(dr["PiezasPorTarima"]);

                        lista.Add(linea);
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
    }
}