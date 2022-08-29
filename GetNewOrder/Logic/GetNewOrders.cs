using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace GetNewOrder.Logic
{
    class GetNewOrders
    {

        public static void GetOrders()
        {
            FileUploader.Upload();
            SendInformation();
        }

        public static void SendInformation()
        {
            using (SqlConnection connection = new SqlConnection(FileUploader.GetConnectionString()))
            {
                Console.WriteLine("Generando Informe");
                connection.Open();
                BuckCopyDb();
                SendFile sendFilers = new SendFile();
                int cantidadColumnas = 1;
                DateTime fecha = DateTime.Now;
                string FechaArchivo = DateTime.Now.ToString("dd-MM-yyyy");
                string horaArchivo = DateTime.Now.ToString("HH-mm");
                string fecha_menos = fecha.AddDays(-1).ToString("d/MM/yyyy");
                SqlDataAdapter getNewOrderSql = new SqlDataAdapter("select * from MM_DTMovistarP where FECHA_CREACION like '%" + fecha_menos + "%'", connection);
                DataTable dataTableGetNewOrder = new DataTable();
                getNewOrderSql.Fill(dataTableGetNewOrder);
                if (dataTableGetNewOrder.Rows.Count == 0)
                {
                    try
                    {
                        //CREAMOS ARCHIVO CSV
                        //StreamWriter sw = new StreamWriter(@"E:\Documentos\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv", false, Encoding.UTF8);
                        StreamWriter sw = new StreamWriter(@"C:\Users\jsgomezpe2\Desktop\Trabajo Celula Axia\OneDrive - fractalia.es\archivosCSV\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv", false, Encoding.UTF8);
                        for (int ncolumna = 0; ncolumna < cantidadColumnas; ncolumna++)
                        {
                            sw.Write("Error");
                            if (ncolumna < cantidadColumnas - 1)
                            {
                                sw.Write("|");
                            }
                        }
                        sw.Write(sw.NewLine); //saltamos linea
                        sw.Write("no se encontraron registros");
                        sw.Write(sw.NewLine); //saltamos linea
                        sw.Close();
                        connection.Close();
                        //sendFilers.Send(@"E:\Documentos\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv");
                        sendFilers.Send(@"C:\Users\jsgomezpe2\Desktop\Trabajo Celula Axia\OneDrive - fractalia.es\archivosCSV\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv");
                        connection.Dispose();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {

                    try
                    {
                        // CREAMOS ARCHIVO CSV
                      //StreamWriter sw = new StreamWriter(@"E:\Documentos\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv", false, Encoding.UTF8);
                       StreamWriter sw = new StreamWriter(@"C:\Users\jsgomezpe2\Desktop\Trabajo Celula Axia\OneDrive - fractalia.es\archivosCSV\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv", false, Encoding.UTF8);
                        //copiar encabezados de la consulta
                        cantidadColumnas = dataTableGetNewOrder.Columns.Count;

                        for (int ncolumna = 0; ncolumna < cantidadColumnas; ncolumna++)
                        {
                            sw.Write(dataTableGetNewOrder.Columns[ncolumna]);
                            if (ncolumna < cantidadColumnas - 1)
                            {
                                sw.Write("|");
                            }
                        }
                        sw.Write(sw.NewLine); //saltamos linea

                        // copiar info linea por linea
                        foreach (DataRow renglon in dataTableGetNewOrder.Rows)
                        {
                            for (int ncolumna = 0; ncolumna < cantidadColumnas; ncolumna++)
                            {
                                if (!Convert.IsDBNull(renglon[ncolumna]))
                                {
                                    sw.Write(renglon[ncolumna]);
                                }
                                if (ncolumna < cantidadColumnas)
                                {
                                    sw.Write("|");
                                }
                            }
                            sw.Write(sw.NewLine); //saltamos linea
                        }
                        sw.Close();
                        connection.Close();
                        //sendFilers.Send(@"E:\Documentos\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv");
                        sendFilers.Send(@"C:\Users\jsgomezpe2\Desktop\Trabajo Celula Axia\OneDrive - fractalia.es\archivosCSV\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv");
                        connection.Dispose();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }

                }

            }

        }

        public static void BuckCopyDb()
        {
            //Realiza carga de archivo consulta_ODS a la base de datos para correcta ejecucion del UpdateOrders
            //using (SqlConnection connectionSql = new SqlConnection(FileUploader.GetConnectionString()))
            //{
            //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionSql))
            //    {
            //        connectionSql.Open();
            //        DataTable dataTablexd = ConvertCSVtoDataTable(@"\\WBOGVMAPP115\ProyectoPortalBI\VisionReportes\3105714 - CONCILIACION AMAZON\Temporal\Consulta_ODS.csv");
            //        bulkCopy.DestinationTableName = "dbo.MM_DTMovistarP";
            //        try
            //        {
            //            //Se copia las columnas de consulta a nuestra base de datos.
            //            bulkCopy.WriteToServer(dataTablexd);
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //        connectionSql.Close();
            //    }
            //}
            //Realiza cargue de informacion para ejecucion de GetNetOrder
            using (SqlConnection SqlConnection = new SqlConnection(FileUploader.GetConnectionString()))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlConnection))
                {
                    SqlConnection.Open();
                    DataTable dataTablexd = ConvertCSVtoDataTable(@"C:\Users\jsgomezpe2\Desktop\Trabajo Celula Axia\OneDrive - fractalia.es\archivos prueba\Consulta_ODS_GetNewOrder.csv");
                   //DataTable dataTablexd = ConvertCSVtoDataTable(@"\\WBOGVMAPP115\ProyectoPortalBI\VisionReportes\3105714 - CONCILIACION AMAZON\Temporal\Consulta_ODS_GetNewOrder.csv");
                    bulkCopy.DestinationTableName = "dbo.MM_DTMovistarP";
                    try
                    {

                        //Se copia las columnas de consulta a nuestra base de datos.
                        bulkCopy.WriteToServer(dataTablexd);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    SqlConnection.Close();
                }
            }
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }


    }

}
