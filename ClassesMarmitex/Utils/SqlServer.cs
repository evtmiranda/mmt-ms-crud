namespace ClassesMarmitex.Utils
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;

    public class SqlServer
    {
        #region "Propriedades"

        private string ConnectionString;
        private SqlConnection Connection { get; set; }
        public SqlTransaction Trans { get; set; }
        public SqlDataReader Reader { get; set; }

        private SqlCommand _command;
        public SqlCommand Command
        {
            get
            {
                if (_command == null)
                {
                    if (Connection != null)
                    {
                        _command = Connection.CreateCommand();
                        if ((Trans != null))
                        {
                            _command.Transaction = Trans;
                        }
                    }
                }
                else if (Connection.State == System.Data.ConnectionState.Open)
                {
                    _command.Connection = Connection;
                    if ((Trans != null))
                    {
                        _command.Transaction = Trans;
                    }
                }
                return _command;
            }
            set
            {
                _command = value;
                _command = Connection.CreateCommand();
            }
        }

        #endregion

        #region "Métodos"

        public bool ExisteStringConexao()
        {
            return ConfigurationManager.ConnectionStrings["marmitex"] != null ? true : false;
        }

        public string RetornaStringConexao()
        {
            string strConnection = string.Empty;

            strConnection = ConfigurationManager.ConnectionStrings["marmitex"].ConnectionString;

            //1 - String criptografada | 2 - String aberta
            //if (ConfigurationManager.AppSettings["ModoDesenvolvimento"] == "1")
            //{
            //    //Aplicar a rotina de descriptografia
            //    ConnectionString = Criptografia.Descriptografar(strConnection);
            //}
            //else
            //{
            //    ConnectionString = strConnection;
            //}

            ConnectionString = strConnection;

            return ConnectionString;
        }

        public void StartConnection()
        {
            if (Connection == null || Connection.State == System.Data.ConnectionState.Closed)
            {
                ConnectionString = RetornaStringConexao();

                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
            }
            if ((!(Connection.State == System.Data.ConnectionState.Open)))
            {
                throw new Exception("falha ao abrir conexão com o banco de dados");
            }
        }

        public void CloseConnection()
        {
            if (((Connection != null) && Connection.State == System.Data.ConnectionState.Open))
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        public void BeginTransaction()
        {
            if ((Trans == null))
            {
                Trans = Connection.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (((Connection != null) && Connection.State == System.Data.ConnectionState.Open))
            {
                if (((Trans != null) && (Trans.Connection != null)))
                {
                    Trans.Commit();
                }
            }
            ClearObjectDatabase();
        }

        public void Rollback()
        {
            if (((Connection != null) && Connection.State == System.Data.ConnectionState.Open))
            {
                if (((Trans != null) && (Trans.Connection != null)))
                {
                    Trans.Rollback();
                }
                ClearObjectDatabase();
            }
        }

        private void ClearObjectDatabase()
        {
            if (((Command != null)))
            {
                Command.Dispose();
            }
        }

        #endregion        

    }
}
