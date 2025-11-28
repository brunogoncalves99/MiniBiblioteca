# 📚 MiniBiblioteca - Sistema de Gerenciamento de Biblioteca

Sistema web completo para gerenciamento de biblioteca com controle de aluguéis de livros, desenvolvido com .NET Core 8 e arquitetura em camadas.

![.NET Core](https://img.shields.io/badge/.NET-8.0-blue)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-red)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 🚀 Funcionalidades

### 👤 Para Usuários
- ✅ Cadastro e login de usuários
- ✅ Busca de livros por título, autor, categoria ou ISBN
- ✅ Visualização de livros disponíveis
- ✅ Aluguel de livros com período customizado (até 30 dias)
- ✅ Visualização de aluguéis ativos e histórico
- ✅ Devolução antecipada de livros
- ✅ Cálculo automático de multas por atraso

### 👨‍💼 Para Administradores
- ✅ Dashboard com estatísticas em tempo real
- ✅ Gerenciamento completo de livros (adicionar, editar, remover)
- ✅ Controle de quantidade e disponibilidade
- ✅ Visualização de todos os aluguéis ativos
- ✅ Relatórios de aluguéis atrasados
- ✅ Gestão de multas acumuladas

---

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET Core 8** - Framework principal
- **ASP.NET Core MVC** - Padrão MVC
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados relacional
- **C#** - Linguagem de programação

### Frontend
- **HTML5** e **CSS3** - Estrutura e estilização
- **Bootstrap 5** - Framework CSS responsivo
- **jQuery** - Biblioteca JavaScript
- **AJAX** - Requisições assíncronas
- **Font Awesome** - Ícones
- **SweetAlert2** - Alertas e modais elegantes
- **DataTables** - Tabelas interativas

### Arquitetura
- **Clean Architecture** - Separação em camadas
- **Repository Pattern** - Abstração de acesso a dados
- **Dependency Injection** - Injeção de dependências
- **DTOs** - Data Transfer Objects

---

## 📁 Estrutura do Projeto
```
MiniBiblioteca/
│
├── MiniBiblioteca.Domain/          # Camada de Domínio
│   ├── Entities/                   # Entidades do sistema
│   ├── Enums/                      # Enumeradores
│   └── Interfaces/                 # Interfaces dos repositórios e services
│
├── MiniBiblioteca.Infrastructure/  # Camada de Infraestrutura
│   ├── Data/                       # Contexto do banco de dados
│   ├── Repositories/               # Implementação dos repositórios
│   └── Migrations/                 # Migrações do EF Core
│
├── MiniBiblioteca.Application/     # Camada de Aplicação
│   ├── Services/                   # Serviços de negócio
│   ├── DTOs/                       # Data Transfer Objects
│   └── Validators/                 # Validadores
│
└── MiniBiblioteca.Web/             # Camada de Apresentação
    ├── Controllers/                # Controllers MVC
    ├── Views/                      # Views Razor
    └── wwwroot/                    # Arquivos estáticos (CSS, JS, imagens)
```

---

## ⚙️ Pré-requisitos

Antes de começar, você precisa ter instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/sql-server/sql-server-downloads) ou [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-editions-express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

---

## 🔧 Instalação e Configuração

### 1️⃣ Clonar o Repositório
```bash
git clone https://github.com/seu-usuario/minibiblioteca.git
cd minibiblioteca
```

### 2️⃣ Configurar a Connection String

Edite o arquivo `MiniBiblioteca.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MiniBiblioteca;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Para SQL Express:**
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=MiniBiblioteca;Trusted_Connection=True;TrustServerCertificate=True;"
```

**Para autenticação SQL Server:**
```json
"DefaultConnection": "Server=localhost;Database=MiniBiblioteca;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
```

### 3️⃣ Restaurar Pacotes
```bash
dotnet restore
```

### 4️⃣ Criar o Banco de Dados

#### Via Package Manager Console (Visual Studio):
```powershell
# No Package Manager Console
# Default Project: MiniBiblioteca.Infrastructure

Add-Migration InitialCreate
Update-Database
```

#### Via .NET CLI:
```bash
cd MiniBiblioteca.Web
dotnet ef migrations add InitialCreate --project ../MiniBiblioteca.Infrastructure
dotnet ef database update --project ../MiniBiblioteca.Infrastructure
```

### 5️⃣ Inserir Usuário Administrador


- Clique em **"Registrar"** na tela de login
- Preencha os dados
- Marque o checkbox **"Cadastrar como Administrador"** se desejar criar um admin

### 6️⃣ Executar o Projeto

#### Visual Studio:
- Pressione **F5** ou clique no botão **Play**

#### .NET CLI:
```bash
cd MiniBiblioteca.Web
dotnet run
```

### 7️⃣ Acessar o Sistema

Abra o navegador e acesse: `https://localhost:7000` (a porta pode variar)


---

## 📖 Como Usar

### Para Usuários

1. **Fazer Login**
   - Acesse `/Auth/Login`
   - Insira email e senha

2. **Buscar Livros**
   - Vá em "Livros" no menu
   - Use a barra de busca para filtrar

3. **Alugar um Livro**
   - Clique em "Ver Detalhes"
   - Clique em "Alugar Este Livro"
   - Escolha quantos dias deseja ficar com o livro
   - Confirme o aluguel

4. **Visualizar Meus Aluguéis**
   - Vá em "Meus Aluguéis" no menu
   - Veja livros ativos e histórico

5. **Devolver um Livro**
   - Em "Meus Aluguéis" → Aba "Ativos"
   - Clique em "Devolver"

### Para Administradores

1. **Acessar Dashboard**
   - Menu "Administração" → "Dashboard"
   - Visualize estatísticas gerais

2. **Gerenciar Livros**
   - Menu "Administração" → "Gerenciar Livros"
   - Adicionar: Clique em "Adicionar Novo Livro"
   - Editar: Clique no botão de edição
   - Remover: Clique no botão de exclusão

3. **Visualizar Aluguéis**
   - Menu "Administração" → "Gerenciar Aluguéis"
   - Veja todos os aluguéis ativos do sistema

---

## 📊 Regras de Negócio

- ✅ Usuário pode alugar até **3 livros simultaneamente**
- ✅ Período de aluguel: **1 a 30 dias**
- ✅ Multa por atraso: **R$ 2,50 por dia**
- ✅ Livro só pode ser alugado se houver exemplares disponíveis
- ✅ Sistema de reservas para livros indisponíveis
- ✅ Validação de CPF e email únicos

---

## 🗂️ Banco de Dados

### Entidades Principais

| Tabela | Descrição |
|--------|-----------|
| **Usuarios** | Dados dos usuários e administradores |
| **Livros** | Catálogo de livros da biblioteca |
| **Alugueis** | Registro de todos os aluguéis |
| **Reservas** | Sistema de reservas de livros |

### Diagrama de Relacionamento
```
Usuarios (1) ─────── (N) Alugueis
   │
   └─────── (N) Reservas
   
Livros (1) ─────── (N) Alugueis
   │
   └─────── (N) Reservas
```

---

## 🎨 Screenshots

### Tela de Login # Imagens ainda vão ser geradas
![Login](docs/screenshots/login.png)

### Dashboard Admin
![Dashboard](docs/screenshots/dashboard.png)

### Catálogo de Livros
![Livros](docs/screenshots/livros.png)

### Gerenciar Livros
![Admin](docs/screenshots/gerenciar-livros.png)

---

## 🧪 Testes
```bash
# Executar testes (quando implementado)
dotnet test
```

---

## 🐛 Problemas Conhecidos

- Nenhum problema crítico identificado no momento

---

## 🚀 Melhorias Futuras

- [ ] Sistema de notificações por email
- [ ] Relatórios em PDF
- [ ] Sistema de avaliações de livros
- [ ] API REST completa
- [ ] Aplicativo mobile
- [ ] Integração com APIs de livros (Google Books)
- [ ] Sistema de recomendações
- [ ] Modo escuro

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um Fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/NovaFeature`)
5. Abra um Pull Request

---

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👨‍💻 Autor

**Seu Nome**
- GitHub: [brunogoncalves99](https://github.com/brunogoncalves99)
- LinkedIn: [brunogoncalveslemos](https://linkedin.com/in/brunogoncalveslemos)
- Email: bruno.goncalves1999@hotmail.com

---

## 📞 Suporte

Se você tiver alguma dúvida ou problema, abra uma [issue](https://github.com/seu-usuario/minibiblioteca/issues).

---

## ⭐ Mostre seu Apoio

Se este projeto te ajudou, dê uma ⭐ no repositório!

---

**Desenvolvido com ❤️ e ☕**
