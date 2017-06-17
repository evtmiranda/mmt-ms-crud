using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesMarmitex.Utils
{
    public class LogDAO
    {
        private SqlServer sqlConn;

        public LogDAO(SqlServer sqlConn)
        {
            this.sqlConn = sqlConn;
        }

        public void Adicionar(Log log)
        {
            try
            {
                sqlConn.StartConnection();
                sqlConn.Command.CommandType = System.Data.CommandType.Text;

                sqlConn.Command.CommandText = string.Format(@"INSERT INTO tab_log(id_loja, nm_descricao, nm_mensagem, nm_stack_trace)
                                                                    VALUES(@id_loja, @nm_descricao, @nm_mensagem, @nm_stack_trace);");

                sqlConn.Command.Parameters.AddWithValue("@id_loja", log.IdLoja);
                sqlConn.Command.Parameters.AddWithValue("@nm_descricao", log.Descricao ?? "");
                sqlConn.Command.Parameters.AddWithValue("@nm_mensagem", log.Mensagem ?? "");
                sqlConn.Command.Parameters.AddWithValue("@nm_stack_trace", log.StackTrace ?? "");

                sqlConn.Command.ExecuteNonQuery();
            }
            catch (System.Exception)
            {
                //lascou
            }
            finally
            {
                sqlConn.CloseConnection();
            }
        }
    }
}
