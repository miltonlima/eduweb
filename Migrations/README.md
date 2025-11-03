# Migrations - MvcSaed

## Estrutura Otimizada das Migrations

Este projeto utiliza Entity Framework Core com MySQL e foi organizado para ter migrations otimizadas e bem estruturadas.

### Organização Atual

#### 📁 **InitialCreate_Consolidated** (20251030000232)
**Descrição**: Migration inicial consolidada que cria toda a estrutura base do banco de dados.

**Tabelas criadas:**
- **Modalidade**: Tabela para armazenar modalidades (apenas com Id e Nome)
- **Movie**: Tabela para filmes (exemplo - pode ser removida se não for necessária)
- **Pessoa**: Tabela para pessoas/alunos
- **Turma**: Tabela para turmas
- **InscricaoTurma**: Tabela de relacionamento entre Pessoa e Turma
- **modalidade_turma**: Tabela de relacionamento many-to-many entre Modalidade e Turma

**Relacionamentos:**
- Pessoa ↔ Turma (many-to-many através de InscricaoTurma)
- Modalidade ↔ Turma (many-to-many através de modalidade_turma)

**Índices criados:**
- `IX_InscricaoTurma_PessoaId_TurmaId` (único)
- `IX_InscricaoTurma_TurmaId`
- `IX_modalidade_turma_TurmaId`

### Benefícios da Organização Atual

1. **Migration Única**: Uma única migration inicial evita problemas de dependências
2. **Estrutura Limpa**: Sem colunas desnecessárias (ex: removida coluna Descricao da Modalidade)
3. **Relacionamentos Bem Definidos**: Many-to-many relationships corretamente implementados
4. **Namespaces Consistentes**: Uso correto do namespace MvcSaed
5. **Arquivo Snapshot Atualizado**: MvcSaedContextModelSnapshot.cs corretamente nomeado

### Próximas Migrations

Para futuras alterações no banco, criar migrations específicas com nomes descritivos:

```bash
# Exemplos de futuras migrations
dotnet ef migrations add AddNewFieldToTurma
dotnet ef migrations add CreateRelatorioTable
dotnet ef migrations add UpdatePessoaValidation
```

### Comandos Úteis

```bash
# Listar migrations
dotnet ef migrations list

# Aplicar migrations
dotnet ef database update

# Remover última migration (se ainda não aplicada)
dotnet ef migrations remove

# Ver SQL da migration
dotnet ef migrations script
```

### Estrutura do Banco

```sql
tst (Database)
├── Modalidade (Id, Nome)
├── Movie (Id, Title, ReleaseDate, Genre, Price)
├── Pessoa (Id, Nome, Nascimento, Cpf, Email, Matricula)
├── Turma (Id, Nome, Descricao, DataInicio, DataFim, Status, DataCriacao)
├── InscricaoTurma (Id, PessoaId, TurmaId, DataInscricao)
└── modalidade_turma (ModalidadeId, TurmaId)
```