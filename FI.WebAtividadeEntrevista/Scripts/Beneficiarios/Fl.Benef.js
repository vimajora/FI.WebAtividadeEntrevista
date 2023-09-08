$(document).ready(function () {
    $('#CPF, #cpf').mask('000.000.000-00');
});

var beneficiarioEditar;
var beneficiarios = [];
var editandoBeneficiario = null;
$("#btnIncluir").click(function () {
    debugger
    $("#btnIncluir").prop("disabled", true);
    var nome = $("#nome").val();
    var cpf = $("#cpf").val();
    if (nome && nome.length) {
        verificarCPF(cpf, function (data) {
            if (data) {
                adicionarOuEditarBeneficiario(nome, cpf);
            } else {
                exibirMensagem("Aviso", "Este CPF já está em uso ou é inválido.");
                limparCampos();
            }
            limparCampos();
        }, function () {
            exibirMensagem("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            limparCampos();
        });

    } else {
        exibirMensagem("Aviso", "O campo nome é obrigatório.");
        limparCampos();
    }

});

function adicionarOuEditarBeneficiario(nome, cpf) {
    debugger
    if (editandoBeneficiario === null && !cpfNoArray(cpf)) {
        if (alterar == true) {
            Incluir();
        } else {
            beneficiarios.push({ Nome: nome, CPF: cpf });
        }
    } else if (alterar == true) {
        Alterar(editandoBeneficiario);
    }
    else if (alterar == false && cpfNoArray(cpf)) {
        exibirMensagem("Aviso", "CPF em uso.");
    }
    else {
        beneficiarios.push({ Nome: nome, CPF: cpf });
        editandoBeneficiario = null;
    }
    limparCampos();
    atualizarTabelaBeneficiarios();
}

function limparCampos() {
    $("#nome").val("");
    $("#cpf").val("");
    $("#btnIncluir").prop("disabled", false);
    editandoBeneficiario = null;
    $("#btnIncluir").text("Incluir");
}

function cpfNoArray(cpf) {
    debugger
    return beneficiarios.some(function (beneficiario) {
        return beneficiario.CPF.replaceAll('-', '').replaceAll('.', '') === cpf.replaceAll('-', '').replaceAll('.', '');
    });
}

function atualizarTabelaBeneficiarios() {
    var tabela = $("#tabelaBeneficiarios");
    tabela.empty();

    beneficiarios.forEach(function (beneficiario, index) {
        tabela.append(
            "<tr><td>" + beneficiario.Nome + "</td><td>" + beneficiario.CPF +
            "</td><td><button class='btn btn-info btn-editar' data-id='" + index + "' type='button'>Editar</button> " +
            "<button class='btn btn-info btn-excluir' data-id='" + index + "' type='button'>Excluir</button></td></tr>"
        );
    });
}

$("#tabelaBeneficiarios").on("click", ".btn-editar", function () {
    debugger
    var id = $(this).data("id");
    editandoBeneficiario = id;
    var beneficiario = beneficiarios[id];
    beneficiarioEditar = beneficiario.Id;
    $("#nome").val(beneficiario.Nome);
    $("#cpf").val(beneficiario.CPF);
    $("#btnIncluir").text("Editar");
    if (alterar == false) {
        beneficiarios.splice(id, 1);
        atualizarTabelaBeneficiarios();
    }
});

$("#tabelaBeneficiarios").on("click", ".btn-excluir", function () {
    debugger
    var id = $(this).data("id");
    if (alterar == true) {
        $.ajax({
            type: "DELETE",
            url: "/Beneficiario/DeleteBenef",
            data: { id: beneficiarios[id].Id },
            success: function (r) { ModalDialog("Ocorreu um erro", "Excluido com sucesso.") },
            error: function (r) { ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.") }
        });
    }
    beneficiarios.splice(id, 1);
    editandoBeneficiario = null;
    atualizarTabelaBeneficiarios();
});

$("#myModal").on("show.bs.modal", function () {
    $("#btnIncluir").text("Incluir");
    $("#nome").val("");
    $("#cpf").val("");
    editandoBeneficiario = null;
});

function verificarCPF(cpf, successCallback, errorCallback) {
    if (editandoBeneficiario != null && alterar == true) {
        $.ajax({
            type: "POST",
            url: "/Base/IdCpfCondiz",
            data: { cpf: cpf, id: beneficiarioEditar },
            success: successCallback,
            error: errorCallback
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Base/VerificarCPFBenef",
            data: { CPF: cpf },
            success: successCallback,
            error: errorCallback
        });
    }
}

function Incluir() {
    $.ajax({
        type: "POST",
        url: "/Beneficiario/IncluirBenef",
        data: {
            "NOME": $("#nome").val(),
            "CPF": $("#cpf").val(),
            "IdCliente": obj.Id
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                debugger
                beneficiarios.push({ Id: r[0].Id, Nome: r[0].Nome, CPF: r[0].CPF.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4'), IdCliente: r[0].IdCliente });
                limparCampos();
                atualizarTabelaBeneficiarios();
                ModalDialog("Sucesso!", r[1])

            }
    });
}

function Alterar(id) {
    $.ajax({
        type: "PUT",
        url: "/Beneficiario/AlterarBenef",
        data: {
            "Id": beneficiarioEditar,
            "NOME": $("#nome").val(),
            "CPF": $("#cpf").val(),
            "IdCliente": obj.Id
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                debugger
                beneficiarios.splice(id, 1);
                beneficiarios.push({ Id: r[0].Id, Nome: r[0].Nome, CPF: r[0].CPF.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4'), IdCliente: r[0].IdCliente });
                limparCampos();
                atualizarTabelaBeneficiarios();
                ModalDialog("Sucesso!", r[1])

            }
    });
    beneficiarioEditar = 0;
}

function exibirMensagem(titulo, mensagem) {
    ModalDialog(titulo, mensagem);
}
