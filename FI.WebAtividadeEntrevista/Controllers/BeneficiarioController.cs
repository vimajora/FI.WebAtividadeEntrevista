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
    public class BeneficiarioController : BaseController
    {
        #region"Metodos Publicos"

        #region"HttpPost"

        [HttpPost]
        public JsonResult IncluirBenef(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            if (!this.ModelState.IsValid || !Convert.ToBoolean(VerificarCPFBenef(model.CPF, model.Nome).Data))
            {
               return Json(string.Join(Environment.NewLine, AddError()));
            }
            else
            {
                model.CPF = new string(model.CPF.Where(char.IsDigit).ToArray());

                var beneficiarioId = bo.Incluir(new Beneficiario()
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    IdCliente = model.IdCliente,
                });
                model.Id = beneficiarioId;
                List<dynamic> result = new List<dynamic>();
                result.Add(model);
                result.Add("Cadastro efetuado com sucesso");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region"HttpPut"
        [HttpPut]
        public JsonResult AlterarBenef(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            if (!this.ModelState.IsValid || !bo.IdCpfCondiz(model.CPF, model.Id) || !IsValid(model.CPF))
            {
                this.ModelState.AddModelError("CPF", "Este CPF é invalido ou esta em uso.");
                return Json(string.Join(Environment.NewLine, AddError()));
            }
            else
            {
                model.CPF = new string(model.CPF.Where(char.IsDigit).ToArray());

                var beneficiarioId = bo.Alterar(new Beneficiario()
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    IdCliente = model.IdCliente,
                });
                List<dynamic> result = new List<dynamic>();
                result.Add(model);
                result.Add("Cadastro efetuado com sucesso");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region"HttpDelete"
        [HttpDelete]
        public JsonResult DeleteBenef(long id)
        {
            BoBeneficiario bo = new BoBeneficiario();
            return Json(bo.DeleteBenef(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

    }


}