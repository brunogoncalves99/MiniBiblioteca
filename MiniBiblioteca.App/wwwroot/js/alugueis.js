let tableAtivos;
let tableHistorico;

$(document).ready(function () {
    carregarAlugueisAtivos();
    carregarHistorico();

    $('#historico-tab').on('shown.bs.tab', function () {
        if (tableHistorico) {
            tableHistorico.columns.adjust();
        }
    });
});

function carregarAlugueisAtivos() {
    $.ajax({
        url: '/Aluguel/GetAlugueisAtivos',
        type: 'GET',
        success: function (response) {
            if (response.success) {
                renderizarTabelaAtivos(response.data);
            }
        },
        error: function () {
            exibirErro('Erro ao carregar aluguéis ativos');
        }
    });
}

function carregarHistorico() {
    $.ajax({
        url: '/Aluguel/GetMeusAlugueis',
        type: 'GET',
        success: function (response) {
            if (response.success) {
                renderizarTabelaHistorico(response.data);
            }
        },
        error: function () {
            exibirErro('Erro ao carregar histórico');
        }
    });
}

function renderizarTabelaAtivos(alugueis) {
    if (tableAtivos) {
        tableAtivos.destroy();
    }

    const tbody = $('#tableAtivos tbody');
    tbody.empty();

    if (alugueis.length === 0) {
        tbody.append('<tr><td colspan="7" class="text-center">Nenhum aluguel ativo</td></tr>');
        return;
    }

    alugueis.forEach(function (aluguel) {
        const hoje = new Date();
        const dataPrevista = new Date(aluguel.dataPrevistaDevolucao);
        const diffTime = dataPrevista - hoje;
        const diasRestantes = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

        let statusBadge = '';
        if (aluguel.status === 1) {
            statusBadge = '<span class="badge bg-success">Ativo</span>';
        } else if (aluguel.status === 3) {
            statusBadge = '<span class="badge bg-danger">Atrasado</span>';
        }
        debugger;
        let diasRestantesTexto = diasRestantes >= 0
            ? `${diasRestantes} dias`
            : `<span class="text-danger">${Math.abs(diasRestantes)} dias atrasado</span>`;

        const row = `
            <tr>
                <td>${aluguel.tituloLivro}</td>
                <td>${aluguel.nomeAutor}</td>
                <td>${formatarData(aluguel.dataAluguel)}</td>
                <td>${formatarData(aluguel.dataPrevistaDevolucao)}</td>
                <td>${statusBadge}</td>
                <td>${diasRestantesTexto}</td>
                <td>
                    <button class="btn btn-sm btn-success" onclick="devolverLivro(${aluguel.idAluguel})">
                        <i class="fas fa-check"></i> Devolver
                    </button>
                </td>
            </tr>
        `;
        tbody.append(row);
    });

    tableAtivos = $('#tableAtivos').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json'
        }
    });
}

function renderizarTabelaHistorico(alugueis) {
    if (tableHistorico) {
        tableHistorico.destroy();
    }

    const tbody = $('#tableHistorico tbody');
    tbody.empty();

    if (alugueis.length === 0) {
        tbody.append('<tr><td colspan="6" class="text-center">Nenhum aluguel no histórico</td></tr>');
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
                statusBadge = '<span class="badge bg-warning">Devolvido com Atraso</span>';
                break;
        }

        const dataDevolucao = aluguel.dataDevolucao
            ? formatarData(aluguel.dataDevolucao)
            : '-';

        const multa = aluguel.valorMulta
            ? formatarMoeda(aluguel.valorMulta)
            : '-';

        const row = `
            <tr>
                <td>${aluguel.tituloLivro}</td>
                <td>${aluguel.nomeAutor}</td>
                <td>${formatarData(aluguel.dataAluguel)}</td>
                <td>${dataDevolucao}</td>
                <td>${statusBadge}</td>
                <td>${multa}</td>
            </tr>
        `;
        tbody.append(row);
    });

    tableHistorico = $('#tableHistorico').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json'
        },
        order: [[2, 'desc']]
    });
}

function devolverLivro(aluguelId) {
    Swal.fire({
        title: 'Confirmar Devolução',
        text: 'Deseja devolver este livro?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sim, devolver',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Aluguel/DevolverLivro/' + aluguelId,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Sucesso!',
                            text: response.message,
                            html: response.multa > 0 ? `${response.message}<br><strong>Multa: ${formatarMoeda(response.multa)}</strong>` : response.message
                        }).then(() => {
                            carregarAlugueisAtivos();
                            carregarHistorico();
                        });
                    } else {
                        exibirErro(response.message);
                    }
                },
                error: function () {
                    exibirErro('Erro ao devolver livro');
                }
            });
        }
    });
}