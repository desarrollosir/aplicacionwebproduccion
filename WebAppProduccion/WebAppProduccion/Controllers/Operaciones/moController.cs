using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosOperaciones;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Management;
using System.IO;
using System.Text.RegularExpressions;

namespace WebAppProduccion.Controllers.Operaciones
{
    public class moController : Controller
    {
        DB_A3F19C_producccionEntities3 db = new DB_A3F19C_producccionEntities3();
        // GET: mo
        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerMO() 
        {
            var Draw = Request.Form.GetValues("draw").FirstOrDefault();
            var Start = Request.Form.GetValues("start").FirstOrDefault();
            var Length = Request.Form.GetValues("length").FirstOrDefault();
            var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
            var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            var moveorder = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
            int Skip = Start != null ? Convert.ToInt32(Start) : 0;
            int TotalRecords = 0;

            List<wh_LineasMO> lista = new List<wh_LineasMO>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
            {
                con.Open();

                string sql = "exec SP_Mo_Index_ParametrosOpcionales @order";
                var query = new SqlCommand(sql, con);

                if (moveorder != "")
                {
                    query.Parameters.AddWithValue("@order", moveorder);
                }
                else
                {
                    query.Parameters.AddWithValue("@order", DBNull.Value);
                }

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // facturas
                        var linea = new wh_LineasMO();

                        linea.Id = Convert.ToInt32(dr["Id"]);
                        linea.Order = dr["NumOrder"].ToString();
                        linea.Status = dr["Status"].ToString();
                        linea.Line = dr["Line"].ToString();
                        linea.Item = dr["Item"].ToString();
                        linea.Rev = dr["Rev"].ToString();
                        linea.SrcLocator = dr["SrcLocator"].ToString();
                        linea.SrcUbicacion = dr["SrcUbicacion"].ToString();
                        linea.DestLocator = dr["DestLocator"].ToString();
                        linea.DestUbicacion = dr["DestUbicacion"].ToString();
                        linea.Requester = dr["Requester"].ToString();
                        linea.Ref = dr["Ref"].ToString();
                        linea.UoM = dr["UoM"].ToString();
                        linea.Qty = dr["Qty"].ToString();
                        
                        string lote = dr["Lote"].ToString();
                        string lotqty = dr["LotQty"].ToString();
                        
                        if (lote.Equals(string.Empty))
                        {
                            linea.Lote = "NA";
                        }
                        else
                        {
                            linea.Lote = lote;                            
                        }

                        if (lotqty.Equals(string.Empty))
                        {
                            linea.QtyLote = linea.Qty;
                        }
                        else
                        {
                            linea.QtyLote = lotqty;
                        }

                        string fecha = dr["FechaExpiracion"].ToString(); ;

                        if (fecha.Equals(string.Empty))
                        {
                            linea.FechaExpiracion = "NA";
                        }
                        else
                        {
                            linea.FechaExpiracion = Convert.ToDateTime(fecha).ToShortDateString();
                        }
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

        public ActionResult CargarMO(string mensaje) 
        {
            ViewBag.Msn = mensaje;
            return View();
        }

        [HttpPost]
        public ActionResult ImportarMO(HttpPostedFileBase postedFileBase) 
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
                    string numeromo = Path.GetFileName(postedFileBase.FileName);
                    numeromo = numeromo.Replace(".txt", "");

                    var numeromorepetido = db.wh_moveorder.Where(x => x.NumOrder.Equals(numeromo)).FirstOrDefault();

                    if (numeromorepetido == null)
                    {
                        postedFileBase.SaveAs(filePath);
                        //string csvData = System.IO.File.ReadAllText(filePath);
                        string[] lines = System.IO.File.ReadAllLines(filePath);

                        List<string> listalineamo = new List<string>();
                        int idmo = AgregarMoveOrder(numeromo);

                        foreach (string line in lines)
                        {
                            if (line.Contains(numeromo))
                            {
                                listalineamo.Add(line);
                            }

                            if (line.Contains("SURTIDO RESUPPLY") || line.Contains("ETIQUETADO"))
                            {
                                listalineamo.Add(line);
                            }

                            if (line.Contains("Lot Number"))
                            {
                                listalineamo.Add(line);
                            }
                        }

                        int id = db.wh_LineasMO.Count();
                        List<wh_LineasMO> listaBD = new List<wh_LineasMO>();
                        List<wh_LotesMO> listaLotes = new List<wh_LotesMO>();

                        for (int i = 0; i < listalineamo.Count(); i++)
                        {
                            if (i > 0)
                            {
                                string lineamolista = listalineamo[i].ToString(); //+ "" + listalinealote[i].ToString();
                                string lineamolistacorregido = Operacion.RemoveSpaces(lineamolista);
                                string lineamolistacorregidoconcomas = lineamolistacorregido.Replace(" ", ",");

                                wh_LineasMO mO = new wh_LineasMO();

                                if (lineamolistacorregidoconcomas.Contains(numeromo))
                                {
                                    string[] lineas;
                                    lineas = lineamolistacorregidoconcomas.Split(',');

                                    mO.wh_moveorder_Id = idmo;
                                    mO.Status = lineas[1].ToString();
                                    mO.Line = lineas[2].ToString();
                                    mO.Item = lineas[3].ToString();
                                    mO.Rev = lineas[4].ToString();
                                    mO.SrcLocator = lineas[5].ToString();
                                    mO.DestLocator = lineas[6].ToString();
                                    mO.Requester = lineas[7].ToString();
                                    mO.Ref = lineas[8].ToString();
                                    mO.UoM = lineas[9].ToString();
                                    mO.Qty = lineas[10].ToString();
                                    mO.Asterisk = lineas[11].ToString();
                                    mO.SrcUbicacion = "";
                                    mO.DestUbicacion = "";

                                    id++;

                                    mO.Id = id;

                                    listaBD.Add(mO);
                                }
                                else if (lineamolistacorregidoconcomas.Contains("SURTIDO") || lineamolistacorregidoconcomas.Contains("ETIQUETADO"))
                                {
                                    string[] lineas;
                                    lineas = lineamolistacorregidoconcomas.Split(',');

                                    wh_LineasMO linea = listaBD.Where(x => x.Id == id).FirstOrDefault();

                                    if (lineas.Length == 3)
                                    {
                                        if (lineamolistacorregidoconcomas.Contains("ETIQUETADO"))
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += lineas[1].ToString();
                                            linea.DestUbicacion = linea.DestUbicacion += lineas[2].ToString();
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else if (lineas.Length == 4)
                                    {
                                        if (lineamolistacorregidoconcomas.Contains("ETIQUETADO"))
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += lineas[2].ToString();
                                            linea.DestUbicacion = linea.DestUbicacion += lineas[3].ToString();
                                        }
                                        else
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += (lineas[1].ToString());
                                            linea.DestUbicacion = linea.DestUbicacion += (lineas[2].ToString() + " " + lineas[3].ToString());
                                        }
                                    }
                                    else if (lineas.Length == 5)
                                    {
                                        if (lineamolistacorregidoconcomas.Contains("ETIQUETADO"))
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += lineas[3].ToString();
                                            linea.DestUbicacion = linea.DestUbicacion += lineas[4].ToString();
                                        }
                                        else
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += (lineas[2].ToString());
                                            linea.DestUbicacion = linea.DestUbicacion += (lineas[3].ToString() + " " + lineas[4].ToString());
                                        }
                                    }
                                    else if (lineas.Length == 6)
                                    {
                                        if (lineamolistacorregidoconcomas.Contains("ETIQUETADO"))
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += lineas[4].ToString();
                                            linea.DestUbicacion = linea.DestUbicacion += lineas[5].ToString();
                                        }
                                        else
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += (lineas[3].ToString());
                                            linea.DestUbicacion = linea.DestUbicacion += (lineas[4].ToString() + " " + lineas[5].ToString());
                                        }
                                    }
                                    else if (lineas.Length == 7)
                                    {
                                        if (lineamolistacorregidoconcomas.Contains("ETIQUETADO"))
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += lineas[5].ToString();
                                            linea.DestUbicacion = linea.DestUbicacion += lineas[6].ToString();
                                        }
                                        else
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += (lineas[4].ToString());
                                            linea.DestUbicacion = linea.DestUbicacion += (lineas[5].ToString() + " " + lineas[6].ToString());
                                        }
                                    }
                                    else if (lineas.Length == 8)
                                    {
                                        if (lineamolistacorregidoconcomas.Contains("ETIQUETADO"))
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += lineas[6].ToString();
                                            linea.DestUbicacion = linea.DestUbicacion += lineas[7].ToString();
                                        }
                                        else
                                        {
                                            linea.SrcUbicacion = linea.SrcUbicacion += (lineas[5].ToString());
                                            linea.DestUbicacion = linea.DestUbicacion += (lineas[6].ToString() + " " + lineas[7].ToString());
                                        }
                                    }
                                }
                                else if (lineamolistacorregidoconcomas.Contains("Lot"))
                                {
                                    string[] lineas;
                                    lineas = lineamolistacorregidoconcomas.Split(',');

                                    wh_LotesMO lotes = new wh_LotesMO();
                                    lotes.Lote = lineas[3].ToString();
                                    lotes.FechaExpiracion = DateTime.Parse(lineas[7].ToString());
                                    lotes.Qty = int.Parse(lineas[11].ToString());
                                    lotes.wh_LineasMO_Id = id;

                                    listaLotes.Add(lotes);
                                }
                            }
                        }

                        db.wh_LineasMO.AddRange(listaBD);
                        db.wh_LotesMO.AddRange(listaLotes);
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception _ex)
            {
                return RedirectToAction("CargarMO", new { mensaje = _ex.Message.ToString() });
            }
        }

        public int AgregarMoveOrder(string order)
        {
            wh_moveorder mo = new wh_moveorder();
            mo.NumOrder = order;
            mo.FechaAlta = DateTime.Now;
            mo.wh_statusmo_Id = 1;

            db.wh_moveorder.Add(mo);
            db.SaveChanges();

            return mo.Id;
        }
    }

    static class Operacion
    {
        public static string RemoveSpaces(this String Value)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            return regex.Replace(Value.Trim(), @" ");
        }
    }
}