// wwwroot/js/site.js

// Configurações globais
const API_BASE_URL = '';

// Função auxiliar para formatar datas
function formatarData(data) {
    return new Date(data).toLocaleDateString('pt-BR');
}

// Função auxiliar para formatar data e hora
function formatarDataHora(data) {
    return new Date(data).toLocaleString('pt-BR');
}

// Função auxiliar para formatar moeda
function formatarMoeda(valor) {
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    }).format(valor);
}

// Função para exibir mensagens de erro
function exibirErro(mensagem) {
    Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: mensagem
    });
}

// Função para exibir mensagens de sucesso
function exibirSucesso(mensagem) {
    Swal.fire({
        icon: 'success',
        title: 'Sucesso!',
        text: mensagem
    });
}

// Função para confirmar ação
function confirmarAcao(titulo, texto, callback) {
    Swal.fire({
        title: titulo,
        text: texto,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim, confirmar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed && callback) {
            callback();
        }
    });
}

// Configuração padrão do DataTables
$.extend(true, $.fn.dataTable.defaults, {
    language: {
        url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json'
    },
    responsive: true,
    pageLength: 10,
    lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Todos"]]
});

// Configuração do AJAX global
$.ajaxSetup({
    error: function (xhr, status, error) {
        if (xhr.status === 401) {
            window.location.href = '/Auth/Login';
        } else if (xhr.status === 403) {
            window.location.href = '/Auth/AccessDenied';
        }
    }
});