$(document).ready(function () {
    carregarLivros();

    $('#btnBuscar').on('click', function () {
        const termo = $('#searchInput').val();
        carregarLivros(termo);
    });

    $('#searchInput').on('keypress', function (e) {
        if (e.which === 13) {
            const termo = $('#searchInput').val();
            carregarLivros(termo);
        }
    });
});

function carregarLivros(termo = '') {
    const container = $('#livrosContainer');
    container.html('<div class="col-12 text-center"><div class="spinner-border text-primary" role="status"></div></div>');

    $.ajax({
        url: '/Livro/GetLivros',
        type: 'GET',
        data: { termo: termo },
        success: function (response) {
            if (response.success) {
                renderizarLivros(response.data);
            } else {
                container.html('<div class="col-12"><div class="alert alert-warning">Erro ao carregar livros</div></div>');
            }
        },
        error: function () {
            container.html('<div class="col-12"><div class="alert alert-danger">Erro ao carregar livros</div></div>');
        }
    });
}

function renderizarLivros(livros) {
    const container = $('#livrosContainer');
    container.empty();

    if (livros.length === 0) {
        container.html('<div class="col-12"><div class="alert alert-info">Nenhum livro encontrado</div></div>');
        return;
    }

    livros.forEach(function (livro) {
        const disponibilidadeBadge = livro.quantidadeDisponivel > 0
            ? `<span class="badge bg-success">${livro.quantidadeDisponivel} disponível(is)</span>`
            : '<span class="badge bg-danger">Indisponível</span>';

        const imagemUrl = livro.imagemCapa || '/images/book-placeholder.jpg';

        const card = `
            <div class="col-md-3 mb-4">
                <div class="card h-100 shadow-sm">
                    <img src="${imagemUrl}" class="card-img-top" alt="${livro.titulo}" style="height: 300px; object-fit: cover;">
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title">${livro.titulo}</h5>
                        <p class="card-text text-muted">${livro.autor}</p>
                        <p class="card-text"><small>Categoria: ${livro.categoria || 'N/A'}</small></p>
                        <div class="mt-auto">
                            ${disponibilidadeBadge}
                            <a href="/Livro/Detalhes/${livro.idLivro}" class="btn btn-primary btn-sm mt-2 w-100">
                                <i class="fas fa-eye"></i> Ver Detalhes
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        `;

        container.append(card);
    });
}