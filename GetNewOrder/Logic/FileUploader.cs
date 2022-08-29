using GetNewOrder.DataDB;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GetNewOrder.Logic
{
    class FileUploader
    {
        public static void Upload()
        {
            GetCSV();
        }
        public static string GetCSV()
        {
            SqlConnection conSql = new SqlConnection(GetConnectionString());
            conSql.Open();
            string query = "select * from MM_PGeneral where id = '8'";
            SqlDataAdapter adap = new SqlDataAdapter(query, conSql);
            DataTable tablaOracle = new DataTable();
            adap.Fill(tablaOracle);
            string queryMV = "";
            OracleConnection conOracle = new OracleConnection(GetConnectionOracle());
            conOracle.Open();
            queryMV = tablaOracle.Rows[0]["query"].ToString();
            return CreateCSV(new OracleCommand(queryMV, conOracle).ExecuteReader());
        }

        public static string CreateCSV(IDataReader reader)
        {
            Console.WriteLine("Generando Archivo");
            //string file = @"\\WBOGVMAPP115\ProyectoPortalBI\VisionReportes\3105714 - CONCILIACION AMAZON\Temporal\Consulta_ODS_GetNewOrder.csv";
           string file = @"C:\Users\jsgomezpe2\Desktop\Trabajo Celula Axia\OneDrive - fractalia.es\archivos prueba\Consulta_ODS_GetNewOrder.csv";
            List<string> lines = new List<string>();
            string headerLine = "";
            if (reader.Read())
            {

                string[] columns = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns[i] = reader.GetName(i);
                }
                headerLine = string.Join(",", columns);
                lines.Add(headerLine);
            }
            //Datos
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);
                lines.Add(string.Join(",", values));
            }

            //creat File
            System.IO.File.WriteAllLines(file, lines);
            return file;
        }
        public static string GetConnectionString()
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
                    $"values ('{FechaArchivo()}','problema al conectar con base de datos SQL','FileUpload')";
                SqlCommand comando = new SqlCommand(insert, connectionSql);
                comando.ExecuteNonQuery();
                connectionSql.Close();
                throw;
            }

        }
        public static string GetConnectionOracle()
        {
            Connection connection = new Connection();

            try
            {
                OracleConnection connectionOracle = new OracleConnection();
                return connectionOracle.ConnectionString = connection.GetConnectionOracle(); ;
            }
            catch (Exception)
            {
                Console.WriteLine("Error en conexion con oracle");
                using (SqlConnection cn = new SqlConnection(GetConnectionString()))
                {
                    cn.Open();
                    string insert = $"insert into MM_Log(Fecha,Problema,Consola) values ('{FechaArchivo()}','problema al conectar con base de datos ORACLE','FileUpload')";
                    SqlCommand comando = new SqlCommand(insert, cn);
                    comando.ExecuteNonQuery();
                    cn.Close();
                }
                return null;

            }

        }
        public static string FechaArchivo()
        {
            DateTime fechaArchivo = DateTime.Now;
            string fecha_menos = fechaArchivo.AddDays(-1).ToString("dd/MM/yyyy/hh:mm");
            return fecha_menos;
        }

    }
}
