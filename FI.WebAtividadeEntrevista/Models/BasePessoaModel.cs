using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAtividadeEntrevista.Models
{
    public class BasePessoaModel
    {
        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [Remote("IsValid", "Base", HttpMethod = "POST", ErrorMessage = "Este CPF é invalido.")]
        public string CPF { get; set; }
    }
}