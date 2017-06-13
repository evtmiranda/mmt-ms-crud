using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassesMarmitex;

namespace ms_crud_rest.DAO
{
    public class DashboardDAO : GenericDAO<Dashboard>
    {
        public DashboardDAO(SqlServer sqlConn, LogDAO logDAO) : base(sqlConn, logDAO) { }

        public override Dashboard BuscarPorId(int idLoja)
        {
            List<Dashboard> dash = new List<Dashboard>();

            try
            {
                sqlConn.StartConnection();

                sqlConn.Command.CommandType = System.Data.CommandType.Text;
                sqlConn.Command.CommandText = @"DROP TABLE IF EXISTS #tmp_dashboard;

                                                CREATE TABLE #tmp_dashboard(
	                                                pedidos int,
	                                                vendas decimal(10,2),
	                                                visitas int,
	                                                parceiros int,
	                                                pedidos_atrasados int,
	                                                pedidos_entregues int
                                                );

                                                DECLARE @pedidos INT;
                                                SET @pedidos = (
	                                                SELECT
		                                                COUNT(1)
	                                                FROM tab_pedido AS tp
	                                                INNER JOIN tab_usuario_parceiro AS tup
	                                                ON tp.id_usuario_parceiro = tup.id_usuario_parceiro
	                                                INNER JOIN tab_parceiro AS tparceiro
	                                                ON tparceiro.id_parceiro = tup.id_parceiro
	                                                WHERE tparceiro.id_loja = @id_loja
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, GETDATE()));

                                                DECLARE @vendas DECIMAL(10,2);
                                                SET @vendas = (
	                                                SELECT
		                                                SUM(vlr_total_pedido)
	                                                FROM tab_pedido AS tp
	                                                INNER JOIN tab_usuario_parceiro AS tup
	                                                ON tp.id_usuario_parceiro = tup.id_usuario_parceiro
	                                                INNER JOIN tab_parceiro AS tparceiro
	                                                ON tparceiro.id_parceiro = tup.id_parceiro
	                                                WHERE tparceiro.id_loja = @id_loja
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, GETDATE()));

                                                DECLARE @visitas INT;
                                                SET @visitas = (
	                                                SELECT 
		                                                COUNT(1)
	                                                FROM tab_acessos_usuario_parceiro AS taup
	                                                INNER JOIN tab_usuario_parceiro AS tup
	                                                ON taup.id_usuario_parceiro = tup.id_usuario_parceiro
	                                                INNER JOIN tab_parceiro AS tp
	                                                ON tp.id_parceiro = tup.id_parceiro
	                                                WHERE tp.id_loja = @id_loja
	                                                AND CONVERT(DATE, dt_acesso) = CONVERT(DATE, GETDATE()));

                                                DECLARE @parceiros INT;
                                                SET @parceiros = (
	                                                SELECT 
		                                                COUNT(DISTINCT tup.id_parceiro)
	                                                FROM tab_acessos_usuario_parceiro AS taup
	                                                INNER JOIN tab_usuario_parceiro AS tup
	                                                ON taup.id_usuario_parceiro = tup.id_usuario_parceiro
	                                                INNER JOIN tab_parceiro AS tp
	                                                ON tp.id_parceiro = tup.id_parceiro
	                                                WHERE tp.id_loja = @id_loja
	                                                AND CONVERT(DATE, dt_acesso) = CONVERT(DATE, GETDATE()));


                                                DECLARE @pedidos_atrasados INT;
                                                SET @pedidos_atrasados = (
	                                                SELECT
		                                                COUNT(1)
	                                                FROM tab_pedido AS tp
	                                                INNER JOIN tab_pedido_status AS tps
	                                                ON tp.id_pedido = tps.id_pedido
	                                                INNER JOIN tab_usuario_parceiro AS tup
	                                                ON tp.id_usuario_parceiro = tup.id_usuario_parceiro
	                                                INNER JOIN tab_parceiro AS tparceiro
	                                                ON tparceiro.id_parceiro = tup.id_parceiro
	                                                WHERE tparceiro.id_loja = @id_loja
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, GETDATE())
	                                                AND tps.id_status = 0
	                                                AND dt_entrega <= GETDATE());

                                                DECLARE @pedidos_entregues INT;
                                                SET @pedidos_entregues = (
	                                                SELECT
		                                                COUNT(1)
	                                                FROM tab_pedido AS tp
	                                                INNER JOIN tab_pedido_status AS tps
	                                                ON tp.id_pedido = tps.id_pedido
	                                                INNER JOIN tab_usuario_parceiro AS tup
	                                                ON tp.id_usuario_parceiro = tup.id_usuario_parceiro
	                                                INNER JOIN tab_parceiro AS tparceiro
	                                                ON tparceiro.id_parceiro = tup.id_parceiro
	                                                WHERE tparceiro.id_loja = @id_loja
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, GETDATE())
	                                                AND tps.id_status = 1);

                                                INSERT INTO #tmp_dashboard(pedidos, vendas, visitas, parceiros, pedidos_atrasados, pedidos_entregues)
                                                VALUES(@pedidos, @vendas, @visitas, @parceiros, @pedidos_atrasados, @pedidos_entregues);

                                                SELECT 
	                                                pedidos AS Pedidos, 
	                                                vendas AS Vendas, 
	                                                visitas AS Visitas, 
	                                                parceiros AS Parceiros, 
	                                                pedidos_atrasados AS PedidosAtrasados, 
	                                                pedidos_entregues AS PedidosEntregues 
                                                FROM #tmp_dashboard;";

                sqlConn.Command.Parameters.Clear();
                sqlConn.Command.Parameters.AddWithValue("@id_loja", idLoja);

                sqlConn.Reader = sqlConn.Command.ExecuteReader();

                if (sqlConn.Reader.HasRows)
                    dash = new ModuloClasse().PreencheClassePorDataReader<Dashboard>(sqlConn.Reader);

                return dash.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logDAO.Adicionar(new Log { IdLoja = idLoja, Mensagem = "Erro ao buscar o dashboard para a loja: " + idLoja, Descricao = ex.Message ?? "", StackTrace = ex.StackTrace ?? "" });
                throw ex;
            }
            finally
            {
                sqlConn.CloseConnection();

                if (sqlConn.Reader != null)
                    sqlConn.Reader.Close();
            }
        }
    }
}