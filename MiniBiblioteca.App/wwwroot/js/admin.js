
$(document).ready(function () {
    carregarDashboard();
});

function carregarDashboard() {
    $.ajax({
        url: '/Admin/GetDashboardData',
        type: 'GET',
        success: function (response) {
            if (response.success) {
                atualizarCards(response.data);
                renderizarAlugueisRecentes(response.data.alugueisRecentes);
            }
        },
        error: function () {
            exibirErro('Erro ao carregar dados do dashboard');
        }
    });
}

function atualizarCards(data) {
    debugger;
    $('#totalLivros').text(data.totalLivros);
    $('#livrosDisponiveis').text(data.livrosDisponiveis);
    $('#livrosAlugados').text(data.livrosAlugados);
    $('#alugueisAtivos').text(data.alugueisAtivos);
    $('#alugueisAtrasados').text(data.alugueisAtrasados);
    $('#totalUsuarios').text(data.totalUsuarios);
    $('#multasAcumuladas').text(formatarMoeda(data.multasAcumuladas));
}

function renderizarAlugueisRecentes(alugueis) {
    const tbody = $('#tableAlugueisRecentes tbody');
    tbody.empty();

    if (alugueis.length === 0) {
        tbody.append('<tr><td colspan="5" class="text-center">Nenhum aluguel recente</td></tr>');
        return;
    }

    alugueis.forEach(function (aluguel) {
        let statusBadge = '';
        switch (aluguel.status) {
            case 1: 
                statusBadge = '<span class="badge bg-success">Ativo</span>';
                break;
            case 2:
                statusBadge = '<span class="badge bg-info">Devolvido</span>';
                break;
            case 3: 
                statusBadge = '<span class="badge bg-danger">Atrasado</span>';
                break;
            case 4:
                statusBadge = '<span class="badge bg-warning">Devolvido com atraso</span>';
                break;
        }

        const row = `
            <tr>
                <td>${aluguel.nomeUsuario}</td>
                <td>${aluguel.tituloLivro}</td>
                <td>${formatarData(aluguel.dataAluguel)}</td>
                <td>${formatarData(aluguel.dataPrevistaDevolucao)}</td>
                <td>${statusBadge}</td>
            </tr>
        `;
        tbody.append(row);
    });

    $('#tableAlugueisRecentes').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json'
        },
        pageLength: 5,
        order: [[2, 'desc']]
    });
}