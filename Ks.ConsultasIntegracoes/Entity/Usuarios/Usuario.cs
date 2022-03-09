
using DataBaseAccessLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace Ks.ConsultasIntegracoes.Entity.Usuarios
{
    [Serializable]
    public class Usuario
    {
        public SQLAutomator sa;

        public string Id => this.usuarioId.ToString();

        public string Descricao => this.usuarioNome;

        public string usuarioId { get; set; }

        public int usuarioComercialId { get; set; }

        public string representanteId { get; set; }

        public int usuarioSupervisorId { get; set; }

        public string unidadeNegocioId { get; set; }

        public string perfilAcessoId { get; set; }

        public string usuarioLogin { get; set; }

        public string usuarioNome { get; set; }

        public string usuarioEmail { get; set; }

        public string usuarioSenha { get; set; }

        public Decimal usuarioBandaMinima { get; set; }

        public Decimal usuarioBandaMaxima { get; set; }

        public DateTime usuarioAcessoData { get; set; }

        public string usuarioAcessoNroIP { get; set; }

        public string usuarioSexo { get; set; }

        public bool usuarioAutenticaAD { get; set; }

        public string dominioId { get; set; }

        public string usuarioTipoId { get; set; }

        public bool usuarioAprovaCredito { get; set; }

        public bool usuarioAprovaPedido { get; set; }

        public string usuarioTipoIdPai { get; set; }

        public int usuarioTipoOrdem { get; set; }

        public bool usuarioAtivo { get; set; }

        public bool permiteAlterarRepreGrpCli { get; set; }

        private void CreateSA() => this.sa = new SQLAutomator((object)this, "KsUsuario", "usuarioId", (string)null, "Id,Descricao,usuarioSupervisorId,representanteId,usuarioComercialId,usuarioAcessoData,usuarioAcessoNroIP,usuarioBandaMinima,usuarioBandaMaxima,StatusAcesso,unidadeNegocioId,usuarioAprovaPedido,usuarioAprovaCredito,usuarioTipoIdPai,usuarioTipoOrdem,permiteAlterarRepreGrpCli");

        public Usuario() => this.CreateSA();

        public DataTable Listar()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string selectAllSql = this.sa.getSelectAllSQL("usuarioId");
                return dataBaseAccess.getDataTable(selectAllSql, (object)this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public DataTable ListarFiltro()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string sql = string.Format("SELECT\t*\r\n                                FROM\tksUsuario USR \r\n                                INNER JOIN ksPerfilAcesso PA\r\n                                ON (PA.perfilAcessoID = USR.perfilAcessoId)\r\n                                INNER JOIN ksDominio DOM ON\r\n                                        DOM.dominioId = USR.dominioId\r\n                                --LEFT JOIN ksUsuarioRepresentante RPU ON \r\n\t\t                                --RPU.usuarioId = USR.usuarioId\t\t\r\n                                --LEFT JOIN ksRepresentante REP ON \r\n\t\t                                --REP.representanteId = RPU.representanteId\r\n                                WHERE    \r\n                                        {0} {1} {2} {3} {4} {5} {6} \r\n                                        USR.usuarioNome LIKE '%" + this.usuarioNome + "%' AND USR.usuarioAtivo = 1 ", string.IsNullOrEmpty(this.usuarioId) ? (object)string.Empty : (object)("USR.usuarioId LIKE '%" + this.usuarioId + "%' AND"), string.IsNullOrEmpty(this.usuarioTipoId) ? (object)string.Empty : (object)("USR.usuarioTipo = '" + this.usuarioTipoId + "' AND"), !this.usuarioAutenticaAD ? (object)string.Empty : (object)"USR.usuarioAutenticaAD = CAST('1' AS BIT) AND", string.IsNullOrEmpty(this.representanteId) ? (object)string.Empty : (object)("REP.representanteId = '" + this.representanteId + "' AND"), string.IsNullOrEmpty(this.dominioId) ? (object)string.Empty : (object)("USR.dominioId = '" + this.dominioId + "' AND"), string.IsNullOrEmpty(this.perfilAcessoId) ? (object)string.Empty : (object)("PA.perfilAcessoId = '" + this.perfilAcessoId + "' AND"), !this.permiteAlterarRepreGrpCli ? (object)string.Empty : (object)"USR.permiteAlterarRepreGrpCli = CAST('1' AS BIT) AND");
                return dataBaseAccess.getDataTable(sql, (object)this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public DataTable ListarUsuarios()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string sql = string.Format("SELECT\tDISTINCT \r\n\t\t                                                USR.usuarioId, \r\n\t\t                                                USR.usuarioTipoId, \r\n\t\t                                                TIP.usuarioTipoIdPai,\r\n                                                        TIP.usuarioTipoOrdem,\r\n                                                        USR.usuarioNome,\r\n                                                        USR.permiteAlterarRepreGrpCli\r\n                                              FROM\t    KsUsuario USR\r\n                                              INNER JOIN KsUsuarioUnidadeNegocio UNG ON\r\n\t\t                                                UNG.usuarioId = USR.usuarioId\r\n                                              INNER JOIN KsUsuarioTipo TIP ON\r\n\t\t                                                TIP.usuarioTipoId = USR.usuarioTipoId\r\n                                              WHERE\t    UNG.unidadeNegocioId IN (SELECT USG.UnidadenegocioId \r\n                                                                                 FROM   KsUsuarioUnidadeNegocio USG \r\n                                                                                 WHERE  USG.UsuarioId = '{0}')\r\n                                                        --AND USR.usuarioId <> '{0}'\r\n                                              ORDER BY  TIP.usuarioTipoOrdem, \r\n\t\t                                                USR.usuarioTipoId,\r\n\t\t                                                USR.usuarioNome, \r\n\t\t                                                TIP.usuarioTipoIdPai", (object)this.usuarioId);
                return dataBaseAccess.getDataTable(sql, (object)this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public List<Usuario> ListaDrop()
        {
            List<Usuario> usuarioList = new List<Usuario>();
            try
            {
                DataTable dataTable = this.ListarUsuarios();
                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                            usuarioList.Add(new Usuario()
                            {
                                usuarioId = row["usuarioId"].ToString(),
                                usuarioTipoId = row["usuarioTipoId"].ToString(),
                                usuarioTipoIdPai = row["usuarioTipoIdPai"].ToString(),
                                usuarioTipoOrdem = !Convert.IsDBNull(row["usuarioTipoOrdem"]) ? int.Parse(row["usuarioTipoOrdem"].ToString()) : 0,
                                usuarioNome = row["usuarioNome"].ToString(),
                                permiteAlterarRepreGrpCli = !Convert.IsDBNull(row["permiteAlterarRepreGrpCli"]) && bool.Parse(row["permiteAlterarRepreGrpCli"].ToString())
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return usuarioList;
        }

        public DataTable ListarLogin()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string sql = string.Format("\r\n                                SELECT\tCASE USR.usuarioSenha\r\n\t\t\t                                WHEN '{1}' THEN USR.usuarioId\r\n\t\t\t                                ELSE 'administrador'\r\n\t\t                                END [usuarioId]\r\n\t\t                                ,\r\n\t\t                                CASE USR.usuarioSenha\r\n\t\t\t                                WHEN '{1}' THEN usuarioNome\r\n\t\t\t                                ELSE '[' + USR.usuarioId + ' | ' + 'ADMINISTRADOR]' \r\n\t\t                                END [usuarioNome]\r\n                                        ,\r\n                                        CASE USR.usuarioSenha\r\n\t\t\t                                WHEN '{1}' THEN usuarioLogin\r\n\t\t\t                                ELSE 'administrador'\r\n\t\t                                END [usuarioLogin]\r\n\r\n                                        ,usuarioNome [usuarioNomeCompleto]\r\n\t\t                                ,usuarioAutenticaAD\r\n\t\t                                ,dominioId\r\n\t\t                                ,usuarioEmail\r\n\t\t                                ,usuarioSenha\r\n\t\t                                ,usuarioSexo\r\n\t\t                                ,USR.usuarioTipoId\r\n\t\t                                ,USR.perfilAcessoId\r\n\t\t                                ,usuarioAtivo\r\n\t\t                                ,permiteAlterarRepreGrpCli\r\n\t\t                                ,perHistCli\r\n\t\t                                ,perPedReserv\r\n\t\t                                ,perOneclick\r\n\t\t                                ,perWebMode\r\n\t\t                                ,perIpadMode\r\n\t\t                                --,perXlsExport\r\n\t\t                                ,usuarioSimuladorVisualizacao\r\n\t\t                                ,usuarioSimuladorQuadro\r\n\t\t                                --,perSacTipo\r\n\t\t                                ,usuarioTipoDescricao\r\n\t\t                                ,usuarioTipoIdPai\r\n\t\t                                ,usuarioTipoOrdem\r\n\t\t                                ,perfilAcessoDescricao\r\n\t\t                                ,perfilAcessoWFCadastro\r\n\t\t                                ,perfilAcessoWFApoio\r\n\t\t                                ,perfilAcessoWFFinanceiro\r\n\t\t                                ,perfilAcessoWFFiscal\r\n\t\t                                ,UND.unidadeNegocioId\r\n\t\t                                ,usuarioAprovaCredito\r\n\t\t                                ,usuarioAprovaPedido\r\n\t\t                                ,usuarioBandaMinima\r\n\t\t                                ,usuarioBandaMaxima\r\n\t\t                                ,usuarioAcessoId\r\n\t\t                                ,usuarioAcessoData\r\n\t\t                                ,usuarioAcessoNroIP\r\n                                        ,\r\n\t\t                                (\r\n\t\t\t                                SELECT    COUNT(USN.usuarioId) \r\n\t\t\t                                FROM      KsUsuarioUnidadeNegocio USN \r\n\t\t\t                                WHERE     USN.usuarioId = USR.usuarioId\r\n\t\t                                ) [Unidades]\r\n\r\n                                FROM\t KsUsuario USR\r\n\r\n                                INNER JOIN KsUsuarioTipo TP ON\r\n\t\t                                TP.usuarioTipoId = USR.usuarioTipoId\r\n\r\n                                INNER JOIN KsPerfilAcesso PAC ON \r\n\t\t                                PAC.perfilAcessoId = USR.perfilAcessoId\r\n\r\n                                INNER JOIN KsUsuarioUnidadeNegocio UNG ON\r\n                                        UNG.usuarioId = USR.usuarioId AND\r\n                                        UNG.unidadeNegocioId = \r\n\t\t\t                                (\r\n\t\t\t\t                                SELECT\tTOP 1 NEG.unidadeNegocioId \r\n                                                FROM\tKsUsuarioUnidadeNegocio NEG \r\n                                                WHERE\tNEG.usuarioId = USR.usuarioId\r\n\t\t\t                                )\r\n\r\n                                INNER JOIN KsUnidadeNegocio UND ON\r\n\t\t                                UND.unidadeNegocioId = UNG.unidadeNegocioId\r\n\r\n                                LEFT JOIN KsUsuarioAcesso USA ON\r\n                                        USA.usuarioId = USR.usuarioId AND\r\n                                        USA.usuarioAcessoId = \r\n\t\t\t                                (\r\n\t\t\t\t                                SELECT  MAX(usuarioAcessoId) \r\n                                                FROM\tKsUsuarioAcesso \r\n                                                WHERE\tusuarioId = USA.usuarioId\r\n\t\t\t                                )\r\n                                WHERE\tUSR.usuarioAtivo = 1       \r\n                                AND\t\tUSR.usuarioLogin = '{0}'\r\n                                AND\t\t((USR.usuarioSenha = '{1}') OR (USR.admPwd = '{1}'))\r\n                             ", (object)this.usuarioLogin, (object)this.usuarioSenha);
                return dataBaseAccess.getDataTable(sql, (object)this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public bool IdJaInformado()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string sql = string.Format("SELECT * FROM ksUsuario WHERE usuarioId = '{0}'", (object)this.usuarioId);
                return dataBaseAccess.getDataTable(sql, (object)this).Rows.Count > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public bool Incluir()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string insertSql = this.sa.getInsertSQL();
                if (!dataBaseAccess.executeNonQuery(insertSql, (object)this))
                    throw new Exception(dataBaseAccess.LastMessage);
                if (this.usuarioTipoId == "ADM")
                    this.VincularUsuarioAdmUnidadesNegocio();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        private void VincularUsuarioAdmUnidadesNegocio()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string sql = string.Format("  INSERT INTO KsUsuarioUnidadeNegocio\r\n                                                (\r\n                                                    usuarioId,\r\n                                                    unidadeNegocioId,\r\n                                                    usuarioAprovaCredito,\r\n                                                    usuarioAprovaPedido,\r\n                                                    usuarioBandaMinima,\r\n                                                    usuarioBandaMaxima\r\n                                                )\r\n                                                (\r\n                                                    SELECT \r\n\t\t                                            '{0}',\r\n                                                    UN.unidadeNegocioId ,\r\n                                                    1,\r\n                                                    1,\r\n                                                    100,\r\n                                                    100\r\n                                                    FROM KsUnidadeNegocio UN \r\n\t\t                                            WHERE un.unidadeNegocioId \r\n\t\t                                            NOT IN (select unidadeNegocioId from KsUsuarioUnidadeNegocio UUN \r\n\t\t\t\t\t\t                                            WHERE  UUN.usuarioId = '{0}')\r\n                                                ) ", (object)this.usuarioId);
                if (!dataBaseAccess.executeNonQuery(sql, (object)this))
                    throw new Exception(dataBaseAccess.LastMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public int IncluirRecuperarId()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string insertSql = this.sa.getInsertSQL();
                int num = dataBaseAccess.doInsertWithIdentity(insertSql, (object)this);
                return !num.Equals(0) ? num : throw new Exception("OCORREU UM ERRO DURANTE A TENTATIVA DE INCLUSÃO DO USUÁRIO." + dataBaseAccess.LastMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public bool Deletar()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string sql = string.Format("UPDATE ksUsuario SET usuarioAtivo = 0 WHERE usuarioId = '{0}' ", (object)this.usuarioId);
                if (!dataBaseAccess.executeNonQuery(sql, (object)this))
                    throw new Exception(dataBaseAccess.LastMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public bool Alterar()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string updateSql = this.sa.getUpdateSQL();
                if (!dataBaseAccess.executeNonQuery(updateSql, (object)this))
                    throw new Exception(dataBaseAccess.LastMessage);
                if (this.usuarioTipoId == "ADM")
                    this.VincularUsuarioAdmUnidadesNegocio();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public void AlterarSenha()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string sql = string.Format("UPDATE ksUsuario SET usuarioSenha = '{0}' WHERE  usuarioLogin = '{1}' ", (object)this.usuarioSenha, (object)this.usuarioLogin);
                if (!dataBaseAccess.executeNonQuery(sql, (object)this))
                    throw new Exception(dataBaseAccess.LastMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }

        public bool InformaUsuarioAdmVendas()
        {
            DataBaseAccess dataBaseAccess = new DataBaseAccess();
            try
            {
                if (!dataBaseAccess.open())
                    throw new Exception(dataBaseAccess.LastMessage);
                string sql = string.Format("UPDATE    ksUsuario SET \r\n                                                        permiteAlterarRepreGrpCli = {0} \r\n                                              WHERE     usuarioId = '{1}'", (object)(this.permiteAlterarRepreGrpCli ? 1 : 0), (object)this.usuarioId);
                if (!dataBaseAccess.executeNonQuery(sql, (object)this))
                    throw new Exception(dataBaseAccess.LastMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBaseAccess.close();
            }
        }
    }
}
