namespace GetNewOrder.DataDB
{
    internal class Connection
    {
        public string GetConnectionOracle()
        {
            //el usuario debe de cambiar para el del desarrollador
            //connection to DB Oracle 
            //string getConnectionDBOracle = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (" +
            //        "ADDRESS = (PROTOCOL = TCP)(HOST = 10.203.100.160)(PORT = 1527)))" +
            //        "(CONNECT_DATA =(SERVICE_NAME =  odsdb)));" +
            //        "User Id=AUTAXIA;Password=rz$NPj2q!zvg;";
            string getConnectionDBOracle = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (" +
                    "ADDRESS = (PROTOCOL = TCP)(HOST = 10.203.100.133)(PORT = 1527)))" +
                    "(CONNECT_DATA =(SERVICE_NAME =  crmdb)));" +
                    "User Id=E2E_SUSPENSION_GESCODE;Password=TEmpoRMoV!!s;";
            //string getConnectionDBOracle = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (" +
            //        "ADDRESS = (PROTOCOL = TCP)(HOST = 10.203.100.160)(PORT = 1527)))" +
            //        "(CONNECT_DATA =(SERVICE_NAME = odsdb)));" +
            //        "User Id=SQL_JSGOMEZPE2; Password=*!4jSC$2jAbc; ";
            return getConnectionDBOracle;
        }

        //parameter of connection DB SQL
        public string GetConnectionMySql()
        {
            //connection to DB SQL
            string getConnecionDBSql = @"Server=10.203.200.31,53100;Database=E2E_MovistarMoney_PROD;User Id=E2E_MovistarMoney;Password=Tele2021*!May#;";
            return getConnecionDBSql;
        }
    }
}
