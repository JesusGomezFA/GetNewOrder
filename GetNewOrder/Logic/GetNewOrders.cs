using GetNewOrder.DataDB;
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
                int contador = 0;
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

                        ErrorMessage(GenerateDate(), ex.Message);
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
                            contador++;
                        }
                        if(contador < dataTableGetNewOrder.Rows.Count)
                        {
                            string error = "No se envian todos los datos en el archivos";
                            ErrorMessage(GenerateDate(), error);
                        }
                        sw.Close();
                        connection.Close();
                        //sendFilers.Send(@"E:\Documentos\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv");
                        sendFilers.Send(@"C:\Users\jsgomezpe2\Desktop\Trabajo Celula Axia\OneDrive - fractalia.es\archivosCSV\GetNewOrders_" + FechaArchivo + "_" + horaArchivo + ".csv");
                        connection.Dispose();
                    }
                    catch (Exception ex)
                    {

                        ErrorMessage(GenerateDate(), ex.Message);
                    }

                }

            }

        }
        //realiza un copiado de informacion tomada desde el archivo generado por el FileUploader a la tabla de Sql
        public static void BuckCopyDb()
        {
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
        //convierte el archivo tomado desde la clase FileUploader y la pasa a csv
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
        //Obtiene conexion con base de datos SQL server
        public static string GetConnectionSql()
        {
            Connection connection = new Connection();
            try
            {
                SqlConnection connectionSql = new SqlConnection();
                return connectionSql.ConnectionString = connection.GetConnectionMySql(); ;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error en conexion con SQL");
                SqlConnection connectionSql = new SqlConnection();
                connectionSql.Open();
                string insert = $"insert into MM_Log(Fecha,Problema,Consola) " +
                    $"values ('{GenerateDate()}','problema al conectar con base de datos SQL','FileUpload')";
                SqlCommand comando = new SqlCommand(insert, connectionSql);
                comando.ExecuteNonQuery();
                connectionSql.Close();
                Console.WriteLine("error: " + ex.Message);
                return null;
            }
        }
        public static void ErrorMessage(string archivo, string error)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionSql()))
            {
                con.Open();
                string FechaArchivo = DateTime.Now.ToString("dd-MM-yyyy");
                string horaArchivo = DateTime.Now.ToString("HH-mm");
                string insert = "insert into Errors(Fecha,Problema,Consola) values ('" + FechaArchivo + "_" + horaArchivo + "','Error_ " + error + "_al crear el archivo GetNewOrders" + archivo + ".csv','GetNewOrders)";
                SqlCommand comando = new SqlCommand(insert, con);
                comando.ExecuteNonQuery();
                con.Close();
            }
            Console.WriteLine("Error Enviado a DB");
        }
        //Genera la fecha con la cual se va a guardar el archivo
        public static string GenerateDate()
        {
            string fechaArchivo = DateTime.Now.ToString("dd-MM-yyyy");
            string horaArchivo = DateTime.Now.ToString("HH-mm");
            string archivo = "" + fechaArchivo + "_" + horaArchivo + "";
            return archivo;
        }
    }

}
