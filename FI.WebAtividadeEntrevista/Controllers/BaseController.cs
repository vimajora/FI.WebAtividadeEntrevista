using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class BaseController : Controller
    {
        #region"Metodos Publicos"

        #region"HttpPost"

        [HttpPost]
        public JsonResult VerificarCPF(string CPF)
        {
            BoCliente bo = new BoCliente();

            CPF = new string(CPF.Where(char.IsDigit).ToArray());
            bool isValid = IsValid(CPF);
            bool cpfExiste = bo.VerificarExistencia(CPF);

            if (isValid)
            {
                if (cpfExiste)
                {
                    this.ModelState.AddModelError("CPF", "Este CPF já está em uso.");
                    return Json(false);
                }
                return Json(true);
            }
            this.ModelState.AddModelError("CPF", "Este CPF é invalido.");
            return Json(false);

        }

        [HttpPost]
        public JsonResult VerificarCPFBenef(string CPF, string nome)
        {
            BoBeneficiario bo = new BoBeneficiario();

            CPF = new string(CPF.Where(char.IsDigit).ToArray());
            bool isValid = IsValid(CPF);
            bool cpfExiste = bo.VerificarExistencia(CPF);

            if (isValid)
            {
                if (cpfExiste)
                {

                    this.ModelState.AddModelError("CPF", String.Concat("Ocorreu um erro ao cadastrar o(a) beneficiário ", nome, " \n Motivo: Este CPF já está em uso. \n Outros foram incluidos com sucesso."));
                    return Json(false);
                }
                return Json(true);
            }
            this.ModelState.AddModelError("CPF", String.Concat("Ocorreu um erro ao cadastrar o(a) beneficiário ", nome, " \n Motivo: Este CPF é invalido. \n Outros foram incluidos com sucesso."));
            return Json(false);

        }

        [HttpPost]
        public JsonResult IdCpfCondiz(string cpf, long id)
        {
            BoBeneficiario bo = new BoBeneficiario();

            return Json(bo.IdCpfCondiz(cpf, id), JsonRequestBehavior.AllowGet);

        }

        #region"IsValid(string)"
        public bool IsValid(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            BoCliente bo = new BoCliente();

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            }
            int resto = soma % 11;
            int digitoVerificador1 = (resto < 2) ? 0 : 11 - resto;

            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            }
            resto = soma % 11;
            int digitoVerificador2 = (resto < 2) ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digitoVerificador1 || int.Parse(cpf[10].ToString()) != digitoVerificador2)
            {
                return false;
            }

            return true;
        }
        #endregion

        [HttpPost]
        public List<string> AddError()
        {
            List<string> erros = (from item in ModelState.Values
                                  from error in item.Errors
                                  select error.ErrorMessage).ToList();

            Response.StatusCode = 400;

            return erros;
        }

        [HttpPost]
        public BeneficiarioModel MapperBenefMToVm(Beneficiario beneficiarios)
        {
            var beneModel = new BeneficiarioModel()
            {
                Id = beneficiarios.Id,
                CPF = Convert.ToUInt64(beneficiarios.CPF).ToString(@"000\.000\.000\-00"),
                Nome = beneficiarios.Nome,
                IdCliente = beneficiarios.IdCliente,
            };
            return beneModel;
        }
        #endregion

        #endregion

    }
}