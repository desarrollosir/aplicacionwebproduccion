using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosArzys;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers.ControlUnidadesARZYS
{
    public class arz_semaforosController : Controller
    {
        DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();
        CultureInfo culture = new CultureInfo("es-MX");

        // GET: arz_semaforos
        [AuthorizeUser(IdOperacion: 47)]
        public ActionResult Index()
        {
            List<arz_unidades> lista = new List<arz_unidades>();
            List<arz_unidades> listaTemp = db.arz_unidades.Where(x => x.arz_statusunidad_Id < 3).ToList();

            foreach (var item in listaTemp)
            {
                arz_unidades unidad = SemaforoUnidad(item.Economico);
                lista.Add(unidad);
            }
            
            return View(lista);
        }

        public arz_unidades SemaforoUnidad(string _economico)
        {            
            arz_unidades unidadTemp = new arz_unidades();

            var unidad = (from u in db.arz_unidades
                          where u.Economico == _economico
                          select u).FirstOrDefault();

            var consultadtp = (from u in db.arz_unidades
                               join dtp in db.arz_detunidadproceso on u.Id equals dtp.arz_unidades_Id
                               where u.Economico == _economico
                               select new
                               {
                                   proceso = dtp.arz_proceso_Id,
                                   desproceso = dtp.arz_proceso.Descripcion,
                                   check = dtp.CheckIn
                               }).ToList();

            var consultadtr = (from u in db.arz_unidades
                               join dtr in db.arz_detunidadretrabajo on u.Id equals dtr.arz_unidades_Id
                               where u.Economico == _economico
                               select new
                               {
                                   retrabajo = dtr.arz_retrabajos_Id,
                                   minutos = dtr.arz_retrabajos.Duracion
                               }).FirstOrDefault();

            DateTime citadestino = DateTime.MinValue;
            DateTime horasalida = DateTime.MinValue;

            if (unidad.CitaDestino != null)
            {
                citadestino = (DateTime)unidad.CitaDestino;
                horasalida = citadestino.AddMinutes(-90);
            }

            unidadTemp.horasalida = horasalida.ToString("dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("es-MX"));
            unidadTemp.citadestinostring = citadestino.ToString("dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("es-MX"));
            unidadTemp.arz_lineatransporte = unidad.arz_lineatransporte;
            unidadTemp.Economico = _economico;

            if (citadestino == DateTime.MinValue)
            {
                unidadTemp.statusunidad = "Sin Cita";
                unidadTemp.tiemporestante = "00:00";

                foreach (var item in consultadtp)
                {
                    if (item.proceso == 1 && item.check == false)
                    {
                        unidadTemp.proceso = item.desproceso;                        
                        break;
                    }
                    else if (item.proceso == 2 && item.check == false)
                    {
                        unidadTemp.proceso = item.desproceso;                        
                        break;
                    }
                    else if (item.proceso == 3 && item.check == false)
                    {
                        unidadTemp.proceso = item.desproceso;                        
                        break;
                    }
                    else if (item.proceso == 3 && item.check == true)
                    {
                        unidadTemp.proceso = item.desproceso;                        
                    }
                    else if (item.proceso == 5 && item.check == true)
                    {
                        unidadTemp.proceso = item.desproceso;                        
                    }
                    else if (item.proceso == 6 && item.check == true)
                    {
                        unidadTemp.proceso = item.desproceso;                        
                        break;
                    }
                }
            }
            else
            {
                //DateTime startTime = DateTime.Now;
                DateTime startTime = DateTime.Now.AddHours(2);
                TimeSpan span = citadestino.Subtract(startTime);
                
                double minutos = span.TotalMinutes;
                unidadTemp.Economico = _economico;

                var timeSpan = TimeSpan.FromMinutes(minutos);
                int hh = timeSpan.Hours;
                int mm = timeSpan.Minutes;

                if (mm < 0)
                {
                    mm = mm * -1;
                }

                unidadTemp.tiemporestante = "" + hh + ":" + mm;

                foreach (var item in consultadtp)
                {
                    if (item.proceso == 1 && item.check == false)
                    {
                        unidadTemp.proceso = item.desproceso;
                        unidadTemp.statusunidad = SemaforoArribo(7, minutos);
                        break;
                    }
                    else if (item.proceso == 2 && item.check == false)
                    {
                        unidadTemp.proceso = item.desproceso;
                        unidadTemp.statusunidad = SemaforoArribo(5, minutos);
                        break;
                    }
                    else if (item.proceso == 3 && item.check == false)
                    {
                        unidadTemp.proceso = item.desproceso;
                        unidadTemp.statusunidad = SemaforoArribo(4, minutos);
                        break;
                    }
                    else if (item.proceso == 3 && item.check == true)
                    {
                        unidadTemp.proceso = item.desproceso;
                        unidadTemp.statusunidad = SemaforoArribo(3, minutos);
                    }
                    else if (item.proceso == 5 && item.check == true)
                    {
                        unidadTemp.proceso = item.desproceso;
                        unidadTemp.statusunidad = SemaforoArribo(6, minutos);
                    }
                    else if (item.proceso == 6 && item.check == true)
                    {
                        unidadTemp.proceso = item.desproceso;
                        unidadTemp.statusunidad = SemaforoArribo(8, minutos);
                        break;
                    }
                }
            }

            return unidadTemp;
        }

        public string SemaforoArribo(int idretrabajo, double minutos)
        {
            string estado = "";

            int minutosrojo = 0;
            int minutosamarillo = 0;
            int minutosverde = 0;
            
            foreach (var item in db.arz_retrabajo_semaforo.Where(x => x.arz_retrabajos_Id == idretrabajo).OrderByDescending(x => x.MinutosParaAgregar).ToList())
            {       
                if (item.Semaforo == "Verde")
                {
                    minutosverde = (int)item.MinutosParaAgregar;
                }
                else if (item.Semaforo == "Amarillo")
                {
                    minutosamarillo = (int)item.MinutosParaAgregar;
                }
                else if (item.Semaforo == "Rojo")
                {
                    minutosrojo = (int)item.MinutosParaAgregar;
                }
            }

            if (minutos < minutosrojo)
            {
                estado = "Rojo";
            }
            else if(minutos >= minutosrojo && minutos <= minutosamarillo)
            {
                estado = "Amarillo";
            }
            else
            {
                estado = "Verde";
            }

            return estado;
        }
    }
}