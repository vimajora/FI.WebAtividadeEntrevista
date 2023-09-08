using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe de Beneficiarios que representa o registo na tabela Beneficiarios do Banco de Dados
    /// </summary>
    public class Beneficiario : BasePessoa
    {
        public long Id { get; set; }

        /// <summary>
        /// Id do Cliente
        /// </summary>
        public long IdCliente { get; set; }
    }
}
