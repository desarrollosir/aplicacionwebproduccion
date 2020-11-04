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
using System.Globalization;
using System.Web.Helpers;
using System.Data;
using WebAppProduccion.Entities.ModulosSistemas;
using System.Runtime.CompilerServices;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers.Operaciones
{
    public class moController : Controller
    {
        DB_A3F19C_producccionEntities3 db = new DB_A3F19C_producccionEntities3();
        // GET: mo
        [AuthorizeUser(IdOperacion: 33)]
        public ActionResult Index()
        {            
            return View();
        }

        [AuthorizeUser(IdOperacion: 34)]
        public ActionResult CargarMO(string mensaje)
        {
            ViewBag.Msn = mensaje;
            return View();
        }

        [AuthorizeUser(IdOperacion: 35)]
        public ActionResult IndexMOLines()
        {
            return View();
        }

        [AuthorizeUser(IdOperacion: 36)]
        public ActionResult CargarMOLines(string mensaje)
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

                        int id = (Convert.ToInt32(db.wh_LineasMO.OrderByDescending(x => x.Id).FirstOrDefault().Id) + 1);
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

                                try
                                {
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
                                catch (Exception _ex)
                                {
                                    Console.WriteLine(_ex.Message.ToString());
                                    Console.WriteLine(lineamolistacorregidoconcomas);
                                    throw;
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
                            DateTime date1 = DateTime.Parse(fecha);

                            linea.FechaExpiracion = date1.ToString(CultureInfo.GetCultureInfo("es-MX").DateTimeFormat.ShortDatePattern);
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

        [HttpPost]
        public ActionResult ImportarMOLines(HttpPostedFileBase postedFileBase)
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

                    int idmoline = AgregarMoLines(numeromo);

                    postedFileBase.SaveAs(filePath);

                    DataTable dtCharge = new DataTable();

                    StreamReader streamreader = new StreamReader(@"" + filePath);
                    char[] delimiter = new char[] { '\t' };
                    string[] columnheaders = streamreader.ReadLine().Split(delimiter);

                    foreach (string columnheader in columnheaders)
                    {
                        dtCharge.Columns.Add(columnheader); // I've added the column headers here.
                    }

                    while (streamreader.Peek() > 0)
                    {
                        DataRow datarow = dtCharge.NewRow();
                        datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                        dtCharge.Rows.Add(datarow);
                    }

                    AgregarDetalleMOLines(idmoline, dtCharge);
                }

                return RedirectToAction("IndexMOLines");
            }
            catch (Exception _ex)
            {
                return RedirectToAction("CargarMOLines", new { mensaje = _ex.Message.ToString() });
            }
        }

        [HttpPost]
        public ActionResult ObtenerMOLines()
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

            List<wh_molinesdetalles> lista = new List<wh_molinesdetalles>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
            {
                con.Open();

                string sql = "exec [SP_MOLinesIndex_ParametrosOpcionales] @numeroorden";
                var query = new SqlCommand(sql, con);


                if (orden != "")
                {
                    query.Parameters.AddWithValue("@numeroorden", orden);
                }
                else
                {
                    query.Parameters.AddWithValue("@numeroorden", DBNull.Value);
                }

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // facturas
                        var linea = new wh_molinesdetalles();

                        linea.Id = Convert.ToInt32(dr["Id"]);
                        linea.MoNumber = dr["NumOrder"].ToString();
                        linea.Type = dr["Type"].ToString();
                        linea.Line = dr["Line"].ToString();
                        linea.TransactionType = dr["TransactionType"].ToString();
                        linea.Item = dr["Item"].ToString();
                        linea.Rev = dr["Rev"].ToString();
                        linea.SourceSubinv = dr["SourceSubinv"].ToString();
                        linea.SourceLocator = dr["SourceLocator"].ToString();
                        linea.DestinationSubinv = dr["DestinationSubinv"].ToString();
                        linea.DestinationLocator = dr["DestinationLocator"].ToString();
                        linea.UOM = dr["UOM"].ToString();
                        linea.TransactionQty = int.Parse(dr["TransactionQty"].ToString());
                        linea.RequestedQty = int.Parse(dr["RequestedQty"].ToString());
                        linea.AllocatedQty = int.Parse(dr["AllocatedQty"].ToString());
                        //linea.DateRequired = dr["DateRequired"].ToString();
                        linea.Reason = dr["Reason"].ToString();
                        linea.Reference = dr["Reference"].ToString();
                        linea.LineStatus = dr["LineStatus"].ToString();
                        //linea.StatusDate = dr["StatusDate"].ToString();
                        linea.CreatedBy = dr["CreatedBy"].ToString();

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

        public int AgregarMoLines(string numorder)
        {
            DB_A3F19C_producccionEntities1 dbEmpleados = new DB_A3F19C_producccionEntities1();
            var empleado = dbEmpleados.empleados.Where(x => x.Email.Equals(User.Identity.Name));

            wh_molines molines = new wh_molines();
            molines.Empleados_Id = 1;
            molines.FechaCreacion = DateTime.Now;
            molines.NumOrder = numorder;

            db.wh_molines.Add(molines);
            db.SaveChanges();

            return molines.Id;
        }

        public bool AgregarDetalleMOLines(int idmolines, DataTable dtCharge)
        {
            List<wh_molinesdetalles> lista = new List<wh_molinesdetalles>();
            foreach (DataRow row in dtCharge.Rows)
            {
                wh_molinesdetalles linea = new wh_molinesdetalles();
                linea.Type = row[2].ToString();
                linea.Line = row[3].ToString();
                linea.TransactionType = row[4].ToString();
                linea.Item = row[5].ToString();
                linea.Rev = row[6].ToString();
                linea.SourceSubinv = row[7].ToString();
                linea.SourceLocator = row[8].ToString();
                linea.DestinationSubinv = row[9].ToString();
                linea.DestinationLocator = row[10].ToString();
                linea.UOM = row[17].ToString();
                linea.TransactionQty = int.Parse(row[18].ToString());
                linea.RequestedQty = int.Parse(row[19].ToString());
                linea.AllocatedQty = int.Parse(row[22].ToString());
                linea.DateRequired = DateTime.Parse(row[30].ToString());
                linea.Reason = row[31].ToString();
                linea.Reference = row[32].ToString();
                linea.LineStatus = row[40].ToString();
                linea.StatusDate = DateTime.Parse(row[41].ToString());
                linea.CreatedBy = row[42].ToString();

                linea.wh_molines_Id = idmolines;
                lista.Add(linea);
            }

            db.wh_molinesdetalles.AddRange(lista);
            db.SaveChanges();
            return true;
        }

        public JsonResult CantidadesPorMO(string mo)
        {
            var moid = db.wh_moveorder.Where(x => x.NumOrder.Equals(mo)).FirstOrDefault();

            if (moid != null)
            {
                var conteolineas = db.wh_LineasMO.Where(x => x.wh_moveorder_Id.Equals(moid.Id)).Select(x => x.Item).Count();

                var cantidadessku = db.wh_LineasMO.Where(x => x.wh_moveorder_Id.Equals(moid.Id)).ToList();

                int sumalineas = 0;
                foreach (var item in cantidadessku)
                {
                    sumalineas = sumalineas + int.Parse(item.Qty);
                }

                int? sumalote = 0;
                int? sumalinea = 0;
                foreach (var item in cantidadessku)
                {
                    var lot = db.wh_LotesMO.Where(x => x.wh_LineasMO_Id == item.Id).Sum(x => x.Qty);

                    if (lot != null)
                    {
                        sumalote += lot.Value;
                    }
                    else
                    {
                        sumalinea += int.Parse(item.Qty);
                    }
                }

                int? sumatotal = sumalinea + sumalote;

                int? sumamoline = 0;
                var molines = db.wh_molines.Where(x => x.NumOrder.Equals(mo)).FirstOrDefault();
                if (molines != null)
                {
                    sumamoline = db.wh_molinesdetalles.Where(x => x.wh_molines_Id.Equals(molines.Id)).Sum(x => x.AllocatedQty);
                }

                return Json(new { qtysku = conteolineas, sumlineas = sumalineas, sumalotes = sumalote, sumasinlote = sumalinea, sumatotales = sumatotal, skumoline = sumamoline }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { qtysku = 0, sumqtysku = 0, sumatotales = 0, skumoline = 0 }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult VistaDetalles(string mo) 
        {
            List<wh_molinesdetalles> listaMalos = new List<wh_molinesdetalles>();
            var motemp = db.wh_LineasMO.Where(x => x.wh_moveorder.NumOrder.Equals(mo)).ToList();
            var linemo = db.wh_molinesdetalles.Where(x => x.wh_molines.NumOrder.Equals(mo)).ToList();

            foreach (var item in linemo)
            {
                var skutemp = motemp.Where(x => x.Item.Contains(item.Item)).FirstOrDefault();
                
                wh_molinesdetalles wh = new wh_molinesdetalles();
                
                if (skutemp == null)
                {
                    wh.Item = item.Item;
                    wh.AllocatedQty = item.AllocatedQty;
                    wh.TransactionQty = 0;
                    listaMalos.Add(wh);
                }
                else
                {
                    int? qtymoline = item.AllocatedQty;
                    int qtymo = motemp.Where(x => x.Item.Contains(item.Item)).Sum(x => int.Parse(x.Qty));
                    
                    if (qtymo != qtymoline)
                    {
                        wh.Item = item.Item;
                        wh.AllocatedQty = qtymoline;
                        wh.TransactionQty = qtymo;
                        listaMalos.Add(wh);
                    }
                }                
            }

            return View(listaMalos);
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