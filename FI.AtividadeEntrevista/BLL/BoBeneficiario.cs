using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="Beneficiario">Objeto de cliente</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.Incluir(beneficiario);
        }

        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="Beneficiario">Objeto de cliente</param>
        public long Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.Alterar(beneficiario);
        }


        /// <summary>
        /// Deleta beneficiario
        /// </summary>
        /// <param name="id">Objeto de cliente</param>
        public bool DeleteBenef(long id)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.DeleteBenef(id);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string cpf)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.VerificarExistencia(cpf);
        }

        /// <summary>
        /// verifica se o cpf incluso é do id informado
        /// </summary>
        /// <param name="CPF"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IdCpfCondiz(string cpf, long id)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            var result = benef.IdCpfCondiz(cpf);
            if (result == null)
                return true;
            else if(result.Id == id)
                return true;

            return false;
        }

        /// <summary>
        /// Consulta o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public DML.Beneficiario Consultar(long id)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.Consultar(id);
        }

        /// <summary>
        /// Consulta o beneficiario pelo id do cliente
        /// </summary>
        /// <param name="idCli">id do cliente</param>
        /// <returns></returns>
        public List<DML.Beneficiario> ConsuPCliente(long idCli)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.ConsuPCliente(idCli);
        }
    }
}
