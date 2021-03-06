﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassesMarmitex;
using ClassesMarmitex.Utils;

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
                sqlConn.Command.CommandText = @"DECLARE @pedidos INT;
                                                SET @pedidos = (
	                                                SELECT
		                                                COUNT(1)
	                                                FROM tab_pedido AS tp
	                                                INNER JOIN tab_usuario_parceiro AS tup
	                                                ON tp.id_usuario_parceiro = tup.id_usuario_parceiro
	                                                INNER JOIN tab_parceiro AS tparceiro
	                                                ON tparceiro.id_parceiro = tup.id_parceiro
	                                                WHERE tparceiro.id_loja = @id_loja
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, DATEADD(HOUR, -3, GETDATE())));

                                                DECLARE @vendas DECIMAL(10,2);
                                                SET @vendas = (
	                                                SELECT
		                                                SUM(vlr_total_pedido)
	                                                FROM tab_pedido AS tp
	                                                INNER JOIN tab_pedido_status AS tps
	                                                ON tp.id_pedido = tps.id_pedido
	                                                INNER JOIN tab_usuario_parceiro AS tup
	                                                ON tp.id_usuario_parceiro = tup.id_usuario_parceiro
	                                                INNER JOIN tab_parceiro AS tparceiro
	                                                ON tparceiro.id_parceiro = tup.id_parceiro
	                                                WHERE tparceiro.id_loja = @id_loja
	                                                AND tps.bol_ativo = 1
	                                                AND tps.id_status <> 2
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, DATEADD(HOUR, -3, GETDATE())));

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
	                                                AND CONVERT(DATE, dt_acesso) = CONVERT(DATE, DATEADD(HOUR, -3, GETDATE())));

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
	                                                AND CONVERT(DATE, dt_acesso) = CONVERT(DATE, DATEADD(HOUR, -3, GETDATE())));


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
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, DATEADD(HOUR, -3, GETDATE()))
	                                                AND tps.id_status = 0
	                                                AND tps.bol_ativo = 1
	                                                AND dt_entrega <= DATEADD(hh, -3, GETDATE()));

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
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, DATEADD(HOUR, -3, GETDATE()))
	                                                AND tps.id_status = 1
	                                                AND tps.bol_ativo = 1);

                                                DECLARE @pedidos_fila INT;
                                                SET @pedidos_fila = (
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
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, DATEADD(HOUR, -3, GETDATE()))
	                                                AND tps.id_status = 0
	                                                AND tps.bol_ativo = 1);

                                                DECLARE @pedidos_cancelados INT;
                                                SET @pedidos_cancelados = (
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
	                                                AND CONVERT(DATE, dt_pedido) = CONVERT(DATE, DATEADD(HOUR, -3, GETDATE()))
	                                                AND tps.id_status = 2
	                                                AND tps.bol_ativo = 1);

                                                SELECT 
	                                                @pedidos AS Pedidos, 
	                                                @vendas AS Vendas, 
	                                                @visitas AS Visitas, 
	                                                @parceiros AS Parceiros, 
	                                                @pedidos_atrasados AS PedidosAtrasados,
	                                                @pedidos_fila AS PedidosFila,
	                                                @pedidos_entregues AS PedidosEntregues,
	                                                @pedidos_cancelados AS PediosCancelados";

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