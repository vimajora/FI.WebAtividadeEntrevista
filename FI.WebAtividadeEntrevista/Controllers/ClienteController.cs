using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Threading.Tasks;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : BaseController
    {
        #region"Metodos Publicos"

        #region"HttpGet"
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    CPF = cliente.CPF,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                };
            }

            List<Beneficiario> beneficiarios = boBeneficiario.ConsuPCliente(id);
            model.Beneficiarios = new List<BeneficiarioModel>();

            if (beneficiarios == null)
                return View(model);

            foreach (var item in beneficiarios)
            {
                model.Beneficiarios.Add(MapperBenefMToVm(item));
            };

            return View(model);
        }
        #endregion

        #region"HttpPost"

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            if (!this.ModelState.IsValid || !Convert.ToBoolean(VerificarCPF(model.CPF).Data))
            {
                return Json(string.Join(Environment.NewLine, AddError()));
            }
            else
            {
                model.CPF = new string(model.CPF.Where(char.IsDigit).ToArray());

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    CPF = model.CPF,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                IncluirBenef(model.Beneficiarios, model.Id);

                if (this.ModelState.IsValid)
                    return Json("Cadastro efetuado com sucesso");

                return Json(string.Join(Environment.NewLine, AddError()));
            }
        }


        [HttpPost]
        public void IncluirBenef(List<BeneficiarioModel> beneficiarios, long idCliente)
        {
            BoBeneficiario bo = new BoBeneficiario();
            if (beneficiarios == null)
                return;

            foreach (var model in beneficiarios)
            {
                model.CPF = new string(model.CPF.Where(char.IsDigit).ToArray());
                var cpfOk = Convert.ToBoolean(VerificarCPFBenef(model.CPF, model.Nome).Data);
                if (cpfOk)
                {
                    var beneficiarioId = bo.Incluir(new Beneficiario()
                    {
                        Nome = model.Nome,
                        CPF = model.CPF,
                        IdCliente = idCliente,

                    });
                }
            }
        }

        [HttpPut]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid || !IsValid(model.CPF))
            {
                return Json(string.Join(Environment.NewLine, AddError()));
            }
            else
            {
                model.CPF = new string(model.CPF.Where(char.IsDigit).ToArray());

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    CPF = model.CPF,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                return Json("Cadastro alterado com sucesso");
            }
        }       

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        #endregion
    }
}