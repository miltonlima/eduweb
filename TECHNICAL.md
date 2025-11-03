# SAED - Technical Documentation 📋

*Documentação Técnica Completa do Sistema*

## Architecture Overview

### System Components

- **Framework**: ASP.NET Core 8.0 MVC
- **Authentication**: ASP.NET Core Identity 8.0
- **Database**: MySQL 8.0+ with UTF-8MB4 support
- **ORM**: Entity Framework Core 8.0 (Pomelo MySQL Provider)
- **Frontend**: Bootstrap 5.3, HTML5, CSS3, JavaScript ES6
- **Validation**: Data Annotations + Custom Attributes + Client-side JavaScript
- **Security**: HTTPS, CSRF Protection, XSS Prevention, SQL Injection Protection

### Complete Database Schema

#### Entity Relationship Diagram (ERD)

```
┌─────────────────────┐    ┌─────────────────────┐    ┌─────────────────────┐
│   ApplicationUser   │    │       Pessoa        │    │      Unidade        │
│    (Identity)       │    │     (Student)       │    │    (Facility)       │
├─────────────────────┤    ├─────────────────────┤    ├─────────────────────┤
│ Id (PK)            │    │ Id (PK)            │    │ Id (PK)            │
│ UserName           │    │ Nome               │    │ Nome               │
│ Email              │    │ Nascimento         │    │ Endereco           │
│ PasswordHash       │    │ Cpf                │    │ Capacidade         │
│ SecurityStamp      │    │ Email              │    └─────────────────────┘
│ Nome (Custom)      │    │ Matricula          │              │
│ DataCriacao (Custom)│   └─────────────────────┘              │
└─────────────────────┘              │                        │
                                     │                        │
┌─────────────────────┐              │           ┌─────────────────────┐
│    Modalidade       │              │           │       Turma         │
│   (Sports/Activity) │              │           │      (Class)        │
├─────────────────────┤              │           ├─────────────────────┤
│ Id (PK)            │              │           │ Id (PK)            │
│ Nome               │              │  ┌────────│ Nome               │
└─────────────────────┘              │  │        │ Descricao          │
         │                           │  │        │ DataInicio         │
         │                           │  │        │ DataFim            │
┌─────────────────────┐              │  │        │ Status             │
│  ModalidadeTurma    │              │  │        │ DataCriacao        │
│   (Many-to-Many)    │              │  │        │ UnidadeId (FK)     │◄──┘
├─────────────────────┤              │  │        └─────────────────────┘
│ ModalidadeId (PK/FK)│◄─────────────┘  │                  │
│ TurmaId (PK/FK)    │◄────────────────┘                  │
└─────────────────────┘                                   │
                                                          │
                                               ┌─────────────────────┐
                                               │   InscricaoTurma    │
                                               │   (Enrollment)      │
                                               ├─────────────────────┤
                                               │ Id (PK)            │
                                               │ PessoaId (FK)      │◄──┘
                                               │ TurmaId (FK)       │◄────┘
                                               │ DataInscricao      │
                                               └─────────────────────┘
```

### Database Tables and Constraints

#### Core Entity Tables

**Identity Tables (ASP.NET Core Identity)**
```sql
-- ApplicationUser (extended Identity user)
CREATE TABLE `AspNetUsers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `Nome` longtext,
  `DataCriacao` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
);

-- Additional Identity tables: AspNetRoles, AspNetUserRoles, etc.
```

**Business Entity Tables**
```sql
-- Pessoa (Student)
CREATE TABLE `Pessoa` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Nascimento` datetime(6) NOT NULL,
  `Cpf` varchar(14) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Matricula` varchar(15) NOT NULL,
  PRIMARY KEY (`Id`)
);

-- Unidade (Facility/Unit)
CREATE TABLE `Unidade` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Endereco` longtext,
  `Capacidade` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
);

-- Turma (Class)
CREATE TABLE `Turma` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Descricao` longtext,
  `DataInicio` datetime(6) NOT NULL,
  `DataFim` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  `DataCriacao` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `UnidadeId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Turma_UnidadeId` (`UnidadeId`),
  CONSTRAINT `FK_Turma_Unidade_UnidadeId` FOREIGN KEY (`UnidadeId`) REFERENCES `Unidade` (`Id`) ON DELETE CASCADE
);

-- Modalidade (Sports/Activity Modality)
CREATE TABLE `Modalidade` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
);
```

#### Relationship Tables

```sql
-- ModalidadeTurma (Many-to-Many: Modality-Class)
CREATE TABLE `ModalidadeTurma` (
  `ModalidadeId` int NOT NULL,
  `TurmaId` int NOT NULL,
  PRIMARY KEY (`ModalidadeId`,`TurmaId`),
  KEY `IX_ModalidadeTurma_TurmaId` (`TurmaId`),
  CONSTRAINT `FK_ModalidadeTurma_Modalidade_ModalidadeId` FOREIGN KEY (`ModalidadeId`) REFERENCES `Modalidade` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ModalidadeTurma_Turma_TurmaId` FOREIGN KEY (`TurmaId`) REFERENCES `Turma` (`Id`) ON DELETE CASCADE
);

-- InscricaoTurma (Student-Class Enrollment)
CREATE TABLE `InscricaoTurma` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PessoaId` int NOT NULL,
  `TurmaId` int NOT NULL,
  `DataInscricao` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_InscricaoTurma_PessoaId_TurmaId` (`PessoaId`,`TurmaId`),
  KEY `IX_InscricaoTurma_TurmaId` (`TurmaId`),
  CONSTRAINT `FK_InscricaoTurma_Pessoa_PessoaId` FOREIGN KEY (`PessoaId`) REFERENCES `Pessoa` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_InscricaoTurma_Turma_TurmaId` FOREIGN KEY (`TurmaId`) REFERENCES `Turma` (`Id`) ON DELETE CASCADE
);
```

#### Database Constraints Summary

**Primary Keys**
- `AspNetUsers.Id` - String GUID (Identity)
- `Pessoa.Id` - Auto-increment integer
- `Unidade.Id` - Auto-increment integer
- `Turma.Id` - Auto-increment integer
- `Modalidade.Id` - Auto-increment integer
- `InscricaoTurma.Id` - Auto-increment integer
- `ModalidadeTurma.(ModalidadeId, TurmaId)` - Composite primary key

**Foreign Key Constraints**
- `Turma.UnidadeId` → `Unidade.Id` (CASCADE DELETE)
- `InscricaoTurma.PessoaId` → `Pessoa.Id` (CASCADE DELETE)
- `InscricaoTurma.TurmaId` → `Turma.Id` (CASCADE DELETE)
- `ModalidadeTurma.ModalidadeId` → `Modalidade.Id` (CASCADE DELETE)
- `ModalidadeTurma.TurmaId` → `Turma.Id` (CASCADE DELETE)

**Unique Constraints**
- `InscricaoTurma.(PessoaId, TurmaId)` - Prevents duplicate enrollments

**Performance Indexes**
- `IX_Turma_UnidadeId` - Optimizes facility-class queries
- `IX_InscricaoTurma_TurmaId` - Optimizes enrollment queries
- `IX_ModalidadeTurma_TurmaId` - Optimizes modality-class queries

## Entity Framework Configuration

### Complete DbContext Configuration

```csharp
public class MvcSaedContext : IdentityDbContext<ApplicationUser>
{
    public MvcSaedContext(DbContextOptions<MvcSaedContext> options) : base(options) { }

    // Business entities
    public DbSet<Pessoa> Pessoa { get; set; }
    public DbSet<Turma> Turma { get; set; }
    public DbSet<Unidade> Unidade { get; set; }
    public DbSet<Modalidade> Modalidade { get; set; }
    public DbSet<InscricaoTurma> InscricaoTurma { get; set; }
    public DbSet<ModalidadeTurma> ModalidadeTurma { get; set; }
    
    // Legacy entities
    public DbSet<Movie> Movie { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Essential for Identity

        // Configure Turma-Unidade relationship
        modelBuilder.Entity<Turma>()
            .HasOne(t => t.Unidade)
            .WithMany(u => u.Turmas)
            .HasForeignKey(t => t.UnidadeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure ModalidadeTurma many-to-many relationship
        modelBuilder.Entity<ModalidadeTurma>()
            .HasKey(mt => new { mt.ModalidadeId, mt.TurmaId });

        modelBuilder.Entity<ModalidadeTurma>()
            .HasOne(mt => mt.Modalidade)
            .WithMany(m => m.ModalidadeTurmas)
            .HasForeignKey(mt => mt.ModalidadeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ModalidadeTurma>()
            .HasOne(mt => mt.Turma)
            .WithMany(t => t.ModalidadeTurmas)
            .HasForeignKey(mt => mt.TurmaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure InscricaoTurma relationships
        modelBuilder.Entity<InscricaoTurma>()
            .HasOne(i => i.Pessoa)
            .WithMany()
            .HasForeignKey(i => i.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InscricaoTurma>()
            .HasOne(i => i.Turma)
            .WithMany()
            .HasForeignKey(i => i.TurmaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure unique constraint for enrollments (prevent duplicates)
        modelBuilder.Entity<InscricaoTurma>()
            .HasIndex(i => new { i.PessoaId, i.TurmaId })
            .IsUnique();

        // Configure default values
        modelBuilder.Entity<Turma>()
            .Property(t => t.DataCriacao)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.DataCriacao)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Configure string lengths for better performance
        modelBuilder.Entity<Pessoa>()
            .Property(p => p.Nome)
            .HasMaxLength(100);

        modelBuilder.Entity<Pessoa>()
            .Property(p => p.Cpf)
            .HasMaxLength(14);

        modelBuilder.Entity<Pessoa>()
            .Property(p => p.Email)
            .HasMaxLength(100);

        modelBuilder.Entity<Pessoa>()
            .Property(p => p.Matricula)
            .HasMaxLength(15);

        modelBuilder.Entity<Turma>()
            .Property(t => t.Nome)
            .HasMaxLength(100);

        modelBuilder.Entity<Unidade>()
            .Property(u => u.Nome)
            .HasMaxLength(100);

        modelBuilder.Entity<Modalidade>()
            .Property(m => m.Nome)
            .HasMaxLength(100);
    }
}
```

### Program.cs Configuration

```csharp
var builder = WebApplication.CreateBuilder(args);

// Database configuration
builder.Services.AddDbContext<MvcSaedContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MvcSaedContext");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
    {
        mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
        mySqlOptions.CharSet(CharSet.Utf8Mb4);
    });
});

// Identity configuration
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    // Sign in settings
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<MvcSaedContext>();

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Required for Identity UI

app.Run();
```

### Migration History

The system has evolved through multiple migrations to support the complete feature set:

#### Migration Timeline
1. **InitialCreate_Consolidated** - Basic entities (Pessoa, Turma, Modalidade, Movie)
2. **SeedInitialData** - Initial sample data (6 students, 4 classes, 6 modalities)
3. **CreateUnidade** - Added Unidade entity for facility management
4. **AddTurmaIdToUnidade** - Initial relationship attempt (later corrected)
5. **AddIdentity** - Integrated ASP.NET Core Identity system
6. **TurmaUnidadeRelationship** - Proper Turma-Unidade relationship
7. **AddMoreModalidadesETurmas** - Massive data expansion (700+ records)

#### Current Data Volume
- **42 Students** with realistic Brazilian data
- **34 Classes** distributed across facilities
- **26 Sports Modalities** covering diverse activities
- **10 Facilities** in different locations
- **Complete Identity System** with user management

### Migration Commands Reference

#### Creating Migrations
```bash
# Create new migration
dotnet ef migrations add MigrationName

# Create migration with specific context
dotnet ef migrations add MigrationName --context MvcSaedContext

# Add migration to specific output directory
dotnet ef migrations add MigrationName --output-dir Migrations
```

#### Applying Migrations
```bash
# Update to latest migration
dotnet ef database update

# Update to specific migration
dotnet ef database update MigrationName

# Update with verbose output
dotnet ef database update --verbose

# Update with specific connection string
dotnet ef database update --connection "Server=localhost;Database=saed_db;Uid=root;Pwd=password;Port=3306;CharSet=utf8mb4;"
```

#### Managing Migrations
```bash
# List all migrations
dotnet ef migrations list

# Remove last migration (if not applied)
dotnet ef migrations remove

# Generate SQL script from migrations
dotnet ef migrations script

# Generate SQL for specific migration range
dotnet ef migrations script InitialCreate AddMoreModalidadesETurmas

# Generate script for production deployment
dotnet ef migrations script --idempotent --output migration-script.sql
```

#### Database Operations
```bash
# Drop database (development only)
dotnet ef database drop

# Drop with force (no confirmation)
dotnet ef database drop --force

# Get database information
dotnet ef dbcontext info

# List available DbContexts
dotnet ef dbcontext list

# Scaffold DbContext from existing database (reverse engineering)
dotnet ef dbcontext scaffold "Server=localhost;Database=saed_db;..." Pomelo.EntityFrameworkCore.MySql
```

## Complete Entity Models

### ApplicationUser (Identity Extension)

```csharp
public class ApplicationUser : IdentityUser
{
    [StringLength(100)]
    public string? Nome { get; set; }
    
    public DateTime DataCriacao { get; set; } = DateTime.Now;
}
```

### Core Business Entities

#### Pessoa (Student) Model

```csharp
public class Pessoa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Nascimento")]
    public DateTime Nascimento { get; set; }

    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(14, ErrorMessage = "CPF deve ter formato xxx.xxx.xxx-xx")]
    [CpfValidation(ErrorMessage = "CPF inválido")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Matrícula é obrigatória")]
    [StringLength(15, ErrorMessage = "Matrícula deve ter no máximo 15 caracteres")]
    [Display(Name = "Matrícula")]
    public string Matricula { get; set; }
}
```

#### Unidade (Facility) Model

```csharp
public class Unidade
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome da unidade é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
    [Display(Name = "Endereço")]
    public string? Endereco { get; set; }

    [Range(1, 1000, ErrorMessage = "Capacidade deve estar entre 1 e 1000")]
    public int? Capacidade { get; set; }

    // Navigation property
    public virtual ICollection<Turma> Turmas { get; set; } = new List<Turma>();
}
```

#### Turma (Class) Model

```csharp
public class Turma
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome da turma é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    [Display(Name = "Descrição")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "Data de início é obrigatória")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Início")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "Data de fim é obrigatória")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Fim")]
    [DateGreaterThan("DataInicio", ErrorMessage = "Data de fim deve ser posterior à data de início")]
    public DateTime DataFim { get; set; }

    [Display(Name = "Status")]
    public StatusTurma Status { get; set; } = StatusTurma.Ativa;

    [Display(Name = "Data de Criação")]
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Unidade é obrigatória")]
    [Display(Name = "Unidade")]
    public int UnidadeId { get; set; }

    // Navigation properties
    public virtual Unidade? Unidade { get; set; }
    public virtual ICollection<ModalidadeTurma> ModalidadeTurmas { get; set; } = new List<ModalidadeTurma>();
}

public enum StatusTurma
{
    Ativa = 1,
    Inativa = 0
}
```

#### Modalidade (Sports/Activity) Model

```csharp
public class Modalidade
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome da modalidade é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    // Navigation property
    public virtual ICollection<ModalidadeTurma> ModalidadeTurmas { get; set; } = new List<ModalidadeTurma>();
}
```

#### Relationship Models

```csharp
// Many-to-Many: Modalidade-Turma
public class ModalidadeTurma
{
    public int ModalidadeId { get; set; }
    public virtual Modalidade? Modalidade { get; set; }

    public int TurmaId { get; set; }
    public virtual Turma? Turma { get; set; }
}

// Student-Class Enrollment
public class InscricaoTurma
{
    public int Id { get; set; }

    [Required]
    public int PessoaId { get; set; }
    public virtual Pessoa? Pessoa { get; set; }

    [Required]
    public int TurmaId { get; set; }
    public virtual Turma? Turma { get; set; }

    [Required]
    [Display(Name = "Data de Inscrição")]
    public DateTime DataInscricao { get; set; } = DateTime.Now;
}
```

### View Models for Identity

```csharp
// Profile management
public class ProfileViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }

    [Display(Name = "Data de Criação")]
    public DateTime DataCriacao { get; set; }
}

// Password change
public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Senha atual é obrigatória")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha Atual")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "Nova senha é obrigatória")]
    [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Nova Senha")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Nova Senha")]
    [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação não coincidem.")]
    public string ConfirmPassword { get; set; }
}
```

### Custom Validation Attributes

#### CPF Validation

```csharp
public class CpfValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return false;

        string cpf = value.ToString().Replace(".", "").Replace("-", "");
        
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        // Check for invalid sequences (all same digits)
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Validate first check digit
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        
        int remainder = sum % 11;
        int firstDigit = remainder < 2 ? 0 : 11 - remainder;
        
        if (int.Parse(cpf[9].ToString()) != firstDigit)
            return false;

        // Validate second check digit
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        
        remainder = sum % 11;
        int secondDigit = remainder < 2 ? 0 : 11 - remainder;
        
        return int.Parse(cpf[10].ToString()) == secondDigit;
    }
}
```

#### Date Comparison Validation

```csharp
public class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var currentValue = (DateTime?)value;
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            throw new ArgumentException("Property with this name not found");

        var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

        if (currentValue.HasValue && comparisonValue.HasValue)
        {
            if (currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be greater than {_comparisonProperty}");
        }

        return ValidationResult.Success;
    }
}
```

## Frontend Technologies

### JavaScript Validation and Masking

#### CPF Masking Implementation (cpf-mask.js)

```javascript
// CPF input masking with real-time formatting
function applyCpfMask(element) {
    element.addEventListener('input', function(e) {
        let value = e.target.value.replace(/\D/g, '');
        
        if (value.length <= 11) {
            value = value.replace(/(\d{3})(\d)/, '$1.$2');
            value = value.replace(/(\d{3})(\d)/, '$1.$2');
            value = value.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
        }
        
        e.target.value = value;
    });

    // CPF validation on blur
    element.addEventListener('blur', function(e) {
        if (e.target.value) {
            validateCpf(e.target);
        }
    });
}

function validateCpf(input) {
    const cpf = input.value.replace(/\D/g, '');
    
    if (cpf.length !== 11 || /^(\d)\1{10}$/.test(cpf)) {
        showValidationError(input, 'CPF inválido');
        return false;
    }
    
    let sum = 0;
    for (let i = 0; i < 9; i++) {
        sum += parseInt(cpf.charAt(i)) * (10 - i);
    }
    let remainder = (sum * 10) % 11;
    if (remainder === 10 || remainder === 11) remainder = 0;
    if (remainder !== parseInt(cpf.charAt(9))) {
        showValidationError(input, 'CPF inválido');
        return false;
    }
    
    sum = 0;
    for (let i = 0; i < 10; i++) {
        sum += parseInt(cpf.charAt(i)) * (11 - i);
    }
    remainder = (sum * 10) % 11;
    if (remainder === 10 || remainder === 11) remainder = 0;
    if (remainder !== parseInt(cpf.charAt(10))) {
        showValidationError(input, 'CPF inválido');
        return false;
    }
    
    hideValidationError(input);
    return true;
}

// Initialize CPF masks on page load
document.addEventListener('DOMContentLoaded', function() {
    const cpfInputs = document.querySelectorAll('input[data-mask="cpf"]');
    cpfInputs.forEach(applyCpfMask);
});
```

#### Date Masking Implementation (site.js)

```javascript
// Date masking for dd/mm/yyyy format with validation
function applyDateMask(element) {
    element.addEventListener('input', function(e) {
        let value = e.target.value.replace(/\D/g, '');
        
        if (value.length <= 8) {
            value = value.replace(/(\d{2})(\d)/, '$1/$2');
            value = value.replace(/(\d{2})(\d)/, '$1/$2');
        }
        
        e.target.value = value;
    });
    
    element.addEventListener('blur', function(e) {
        if (e.target.value) {
            validateDate(e.target);
        }
    });
}

function validateDate(input) {
    const value = input.value;
    const dateRegex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
    const match = value.match(dateRegex);
    
    if (!match) {
        showValidationError(input, 'Formato de data inválido (dd/mm/aaaa)');
        return false;
    }
    
    const day = parseInt(match[1], 10);
    const month = parseInt(match[2], 10);
    const year = parseInt(match[3], 10);
    
    // Create date and check if it's valid
    const date = new Date(year, month - 1, day);
    
    if (date.getFullYear() !== year || 
        date.getMonth() !== month - 1 || 
        date.getDate() !== day ||
        month < 1 || month > 12 ||
        day < 1 || day > 31) {
        showValidationError(input, 'Data inválida');
        return false;
    }
    
    hideValidationError(input);
    return true;
}

// Validation helper functions
function showValidationError(input, message) {
    input.classList.add('is-invalid');
    let feedback = input.parentNode.querySelector('.invalid-feedback');
    if (!feedback) {
        feedback = document.createElement('div');
        feedback.className = 'invalid-feedback';
        input.parentNode.appendChild(feedback);
    }
    feedback.textContent = message;
}

function hideValidationError(input) {
    input.classList.remove('is-invalid');
    const feedback = input.parentNode.querySelector('.invalid-feedback');
    if (feedback) {
        feedback.remove();
    }
}

// Initialize date masks on page load
document.addEventListener('DOMContentLoaded', function() {
    const dateInputs = document.querySelectorAll('input[data-mask="date"]');
    dateInputs.forEach(applyDateMask);
});
```

#### Layout Enhancements (layout.js)

```javascript
// Email truncation functionality
document.addEventListener('DOMContentLoaded', function() {
    const emailElements = document.querySelectorAll('.email-truncate');
    
    emailElements.forEach(function(element) {
        const fullEmail = element.textContent;
        if (fullEmail.length > 20) {
            const truncated = fullEmail.substring(0, 17) + '...';
            element.textContent = truncated;
            element.title = fullEmail; // Show full email on hover
            
            // Toggle functionality
            element.style.cursor = 'pointer';
            element.addEventListener('click', function() {
                if (element.textContent === truncated) {
                    element.textContent = fullEmail;
                } else {
                    element.textContent = truncated;
                }
            });
        }
    });
});

// Password strength indicator
function checkPasswordStrength(password) {
    let strength = 0;
    const indicators = {
        length: password.length >= 6,
        lowercase: /[a-z]/.test(password),
        uppercase: /[A-Z]/.test(password),
        digit: /\d/.test(password),
        special: /[!@#$%^&*(),.?":{}|<>]/.test(password)
    };
    
    strength = Object.values(indicators).filter(Boolean).length;
    
    return {
        score: strength,
        indicators: indicators,
        level: strength < 2 ? 'weak' : strength < 4 ? 'medium' : 'strong'
    };
}

// Apply password strength checking
document.addEventListener('DOMContentLoaded', function() {
    const passwordInputs = document.querySelectorAll('input[type="password"][data-strength]');
    
    passwordInputs.forEach(function(input) {
        const strengthMeter = document.createElement('div');
        strengthMeter.className = 'password-strength-meter mt-2';
        input.parentNode.appendChild(strengthMeter);
        
        input.addEventListener('input', function() {
            const strength = checkPasswordStrength(input.value);
            updateStrengthMeter(strengthMeter, strength);
        });
    });
});

function updateStrengthMeter(meter, strength) {
    const colors = { weak: '#dc3545', medium: '#ffc107', strong: '#28a745' };
    const labels = { weak: 'Fraca', medium: 'Média', strong: 'Forte' };
    
    meter.innerHTML = `
        <div class="progress" style="height: 5px;">
            <div class="progress-bar" role="progressbar" 
                 style="width: ${(strength.score / 5) * 100}%; background-color: ${colors[strength.level]}">
            </div>
        </div>
        <small class="text-muted">Força da senha: <span style="color: ${colors[strength.level]}">${labels[strength.level]}</span></small>
    `;
}
```

### CSS Styling (layout.css)

```css
/* Compact navigation styling */
.navbar-nav .nav-item {
    margin-right: 10px;
}

.navbar-nav .nav-link {
    padding: 0.25rem 0.5rem;
    font-size: 0.95rem;
    transition: all 0.2s ease-in-out;
}

.navbar-nav .nav-link:hover {
    background-color: rgba(255, 255, 255, 0.1);
    border-radius: 4px;
}

/* Email truncation styles */
.email-truncate {
    cursor: pointer;
    transition: color 0.2s ease;
}

.email-truncate:hover {
    color: #0d6efd;
    text-decoration: underline;
}

/* Password strength meter */
.password-strength-meter .progress {
    height: 5px;
    background-color: #e9ecef;
    border-radius: 3px;
    overflow: hidden;
}

.password-strength-meter .progress-bar {
    transition: width 0.3s ease, background-color 0.3s ease;
}

/* Form validation enhancements */
.is-invalid {
    border-color: #dc3545;
    box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
}

.invalid-feedback {
    display: block;
    width: 100%;
    margin-top: 0.25rem;
    font-size: 0.875em;
    color: #dc3545;
}

/* Responsive design improvements */
@@media (max-width: 768px) {
    .navbar-nav .nav-item {
        margin-right: 5px;
    }
    
    .navbar-nav .nav-link {
        padding: 0.5rem;
        font-size: 0.9rem;
    }
    
    .email-truncate {
        font-size: 0.85rem;
    }
}

/* Authentication form styling */
.auth-form {
    max-width: 400px;
    margin: 2rem auto;
    padding: 2rem;
    border: 1px solid #dee2e6;
    border-radius: 8px;
    background-color: #ffffff;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.auth-form .form-control {
    border-radius: 6px;
    border: 1px solid #ced4da;
    padding: 0.75rem 1rem;
}

.auth-form .btn {
    border-radius: 6px;
    padding: 0.75rem 1.5rem;
    font-weight: 500;
}
```

## Controller Implementation Patterns

### Authentication Controller (AccountController)

```csharp
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser 
            { 
                UserName = model.Email, 
                Email = model.Email,
                Nome = model.Nome,
                DataCriacao = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToLocal(returnUrl);
            }
            
            ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var model = new ProfileViewModel
        {
            Nome = user.Nome ?? "",
            Email = user.Email ?? "",
            DataCriacao = user.DataCriacao
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.Nome = model.Nome;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            
            if (result.Succeeded)
            {
                TempData["Success"] = "Perfil atualizado com sucesso!";
                return RedirectToAction(nameof(Profile));
            }
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
```

### Authorized CRUD Controller (PessoasController)

```csharp
[Authorize] // Require authentication for all actions
[Route("[controller]")]
public class PessoasController : Controller
{
    private readonly MvcSaedContext _context;
    private readonly ILogger<PessoasController> _logger;

    public PessoasController(MvcSaedContext context, ILogger<PessoasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Pessoas
    public async Task<IActionResult> Index()
    {
        try
        {
            var pessoas = await _context.Pessoa
                .OrderBy(p => p.Nome)
                .ToListAsync();
            return View(pessoas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading pessoas list");
            TempData["Error"] = "Erro ao carregar lista de pessoas.";
            return View(new List<Pessoa>());
        }
    }

    // GET: Pessoas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        try
        {
            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (pessoa == null) return NotFound();

            return View(pessoa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading pessoa details for ID: {Id}", id);
            TempData["Error"] = "Erro ao carregar detalhes da pessoa.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Pessoas/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Pessoas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Nascimento,Cpf,Email,Matricula")] Pessoa pessoa)
    {
        try
        {
            // Remove navigation property validation
            ModelState.Remove("InscricaoTurmas");

            if (ModelState.IsValid)
            {
                // Check for duplicate CPF
                if (await _context.Pessoa.AnyAsync(p => p.Cpf == pessoa.Cpf))
                {
                    ModelState.AddModelError("Cpf", "CPF já cadastrado no sistema.");
                    return View(pessoa);
                }

                _context.Add(pessoa);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("New pessoa created: {Nome} (ID: {Id})", pessoa.Nome, pessoa.Id);
                TempData["Success"] = "Pessoa criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Duplicate entry") == true)
        {
            ModelState.AddModelError("Cpf", "CPF já cadastrado no sistema.");
            _logger.LogWarning("Duplicate CPF attempt: {Cpf}", pessoa.Cpf);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating pessoa: {Nome}", pessoa.Nome);
            ModelState.AddModelError("", "Erro interno do servidor. Tente novamente.");
        }

        return View(pessoa);
    }

    // Additional CRUD methods with similar error handling...

    private bool PessoaExists(int id)
    {
        return _context.Pessoa.Any(e => e.Id == id);
    }
}
```

### Advanced Controller with Business Logic (TurmasController)

```csharp
[Authorize]
[Route("[controller]")]
public class TurmasController : Controller
{
    private readonly MvcSaedContext _context;
    private readonly ILogger<TurmasController> _logger;

    public TurmasController(MvcSaedContext context, ILogger<TurmasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Turmas
    public async Task<IActionResult> Index()
    {
        try
        {
            var turmas = await _context.Turma
                .Include(t => t.Unidade)
                .OrderBy(t => t.Nome)
                .Select(t => new TurmaIndexViewModel
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    DataInicio = t.DataInicio,
                    DataFim = t.DataFim,
                    Status = t.Status,
                    UnidadeNome = t.Unidade!.Nome,
                    TotalInscritos = _context.InscricaoTurma.Count(i => i.TurmaId == t.Id)
                })
                .ToListAsync();

            return View(turmas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading turmas list");
            TempData["Error"] = "Erro ao carregar lista de turmas.";
            return View(new List<TurmaIndexViewModel>());
        }
    }

    // GET: Turmas/Create
    public async Task<IActionResult> Create()
    {
        try
        {
            await PopulateUnidadesDropDown();
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create turma form");
            TempData["Error"] = "Erro ao carregar formulário.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Turmas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Descricao,DataInicio,DataFim,Status,UnidadeId")] Turma turma)
    {
        try
        {
            // Remove navigation property validation
            ModelState.Remove("Unidade");
            ModelState.Remove("ModalidadeTurmas");

            // Business rule validation
            if (turma.DataFim <= turma.DataInicio)
            {
                ModelState.AddModelError("DataFim", "Data de fim deve ser posterior à data de início.");
            }

            // Check if Unidade exists
            if (!await _context.Unidade.AnyAsync(u => u.Id == turma.UnidadeId))
            {
                ModelState.AddModelError("UnidadeId", "Unidade selecionada não existe.");
            }

            if (ModelState.IsValid)
            {
                turma.DataCriacao = DateTime.Now;
                _context.Add(turma);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("New turma created: {Nome} (ID: {Id})", turma.Nome, turma.Id);
                TempData["Success"] = "Turma criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating turma: {Nome}", turma.Nome);
            ModelState.AddModelError("", "Erro interno do servidor. Tente novamente.");
        }

        await PopulateUnidadesDropDown(turma.UnidadeId);
        return View(turma);
    }

    private async Task PopulateUnidadesDropDown(object selectedUnidade = null)
    {
        var unidades = await _context.Unidade
            .OrderBy(u => u.Nome)
            .Select(u => new { u.Id, u.Nome })
            .ToListAsync();

        ViewBag.UnidadeId = new SelectList(unidades, "Id", "Nome", selectedUnidade);
    }
}

// ViewModel for optimized listing
public class TurmaIndexViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public StatusTurma Status { get; set; }
    public string UnidadeNome { get; set; } = "";
    public int TotalInscritos { get; set; }
}
```



## Performance Optimization

### Efficient Entity Framework Queries

#### Optimized List Queries
```csharp
// Optimized Index with projection and minimal data transfer
public async Task<IActionResult> Index()
{
    var pessoas = await _context.Pessoa
        .AsNoTracking() // Read-only, improves performance
        .OrderBy(p => p.Nome)
        .Select(p => new PessoaListViewModel
        {
            Id = p.Id,
            Nome = p.Nome,
            Email = p.Email,
            Cpf = p.Cpf,
            Matricula = p.Matricula
        })
        .ToListAsync();
    
    return View(pessoas);
}

// Optimized Turmas with related data
public async Task<IActionResult> TurmasIndex()
{
    var turmas = await _context.Turma
        .AsNoTracking()
        .Include(t => t.Unidade)
        .Select(t => new TurmaIndexViewModel
        {
            Id = t.Id,
            Nome = t.Nome,
            DataInicio = t.DataInicio,
            DataFim = t.DataFim,
            Status = t.Status,
            UnidadeNome = t.Unidade!.Nome,
            TotalInscritos = _context.InscricaoTurma.Count(i => i.TurmaId == t.Id)
        })
        .OrderBy(t => t.Nome)
        .ToListAsync();
    
    return View(turmas);
}
```

#### Advanced Query Optimization
```csharp
// Efficient search with pagination
public async Task<IActionResult> Search(string searchTerm, int page = 1, int pageSize = 10)
{
    var query = _context.Pessoa.AsNoTracking();
    
    if (!string.IsNullOrEmpty(searchTerm))
    {
        query = query.Where(p => 
            p.Nome.Contains(searchTerm) || 
            p.Email.Contains(searchTerm) || 
            p.Cpf.Contains(searchTerm));
    }
    
    var totalCount = await query.CountAsync();
    var pessoas = await query
        .OrderBy(p => p.Nome)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new PessoaListViewModel
        {
            Id = p.Id,
            Nome = p.Nome,
            Email = p.Email,
            Cpf = p.Cpf
        })
        .ToListAsync();
    
    var viewModel = new PaginatedViewModel<PessoaListViewModel>
    {
        Items = pessoas,
        CurrentPage = page,
        PageSize = pageSize,
        TotalCount = totalCount,
        SearchTerm = searchTerm
    };
    
    return View(viewModel);
}

// Complex reporting query with proper grouping
public async Task<IActionResult> Statistics()
{
    var modalityStats = await _context.ModalidadeTurma
        .AsNoTracking()
        .GroupBy(mt => mt.Modalidade!.Nome)
        .Select(g => new ModalityStatistic
        {
            ModalityName = g.Key,
            ClassCount = g.Count(),
            TotalEnrollments = g.Sum(mt => 
                _context.InscricaoTurma.Count(i => i.TurmaId == mt.TurmaId))
        })
        .OrderByDescending(s => s.ClassCount)
        .ToListAsync();
    
    return View(modalityStats);
}
```

### Database Connection Configuration

```csharp
// Program.cs - Optimized connection configuration
builder.Services.AddDbContext<MvcSaedContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MvcSaedContext");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
    {
        // Connection resilience
        mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
        
        // Performance optimizations
        mySqlOptions.CharSet(CharSet.Utf8Mb4);
        mySqlOptions.CommandTimeout(30);
    });
    
    // Performance settings
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
    
    // Query optimization
    options.ConfigureWarnings(warnings =>
        warnings.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
});

// Connection pooling configuration
builder.Services.AddDbContextPool<MvcSaedContext>(options =>
{
    // Configure as above
}, poolSize: 128); // Adjust based on expected load
```

### Caching Strategies

```csharp
// In-memory caching for relatively static data
public class CachedModalidadeService
{
    private readonly MvcSaedContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedModalidadeService> _logger;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

    public CachedModalidadeService(
        MvcSaedContext context, 
        IMemoryCache cache, 
        ILogger<CachedModalidadeService> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<Modalidade>> GetAllModalidadesAsync()
    {
        const string cacheKey = "all_modalidades";
        
        if (!_cache.TryGetValue(cacheKey, out List<Modalidade> modalidades))
        {
            modalidades = await _context.Modalidade
                .AsNoTracking()
                .OrderBy(m => m.Nome)
                .ToListAsync();
            
            _cache.Set(cacheKey, modalidades, _cacheExpiration);
            _logger.LogInformation("Modalidades cached with {Count} items", modalidades.Count);
        }
        
        return modalidades;
    }

    public void InvalidateCache()
    {
        _cache.Remove("all_modalidades");
        _logger.LogInformation("Modalidades cache invalidated");
    }
}

// Register in Program.cs
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CachedModalidadeService>();
```

## Error Handling and Logging

### Global Exception Handling

```csharp
// Program.cs
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Custom error handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Log error
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Unhandled exception occurred");
        
        // Redirect to error page
        context.Response.Redirect("/Home/Error");
    }
});
```

### Model Validation Error Handling

```csharp
[HttpPost]
public async Task<IActionResult> Create(Pessoa pessoa)
{
    try
    {
        if (ModelState.IsValid)
        {
            _context.Add(pessoa);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Pessoa criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
    catch (DbUpdateException ex)
    {
        if (ex.InnerException?.Message.Contains("Duplicate entry") == true)
        {
            ModelState.AddModelError("Cpf", "CPF já cadastrado no sistema.");
        }
        else
        {
            ModelState.AddModelError("", "Erro interno do servidor. Tente novamente.");
        }
    }
    
    return View(pessoa);
}
```

## Security Considerations

### CSRF Protection

```html
<!-- All forms should include anti-forgery tokens -->
<form asp-action="Create" method="post">
    @Html.AntiForgeryToken()
    <!-- form fields -->
</form>
```

### Input Sanitization

```csharp
[HttpPost]
public async Task<IActionResult> Create([Bind("Nome,Email,Cpf")] Pessoa pessoa)
{
    // Sanitize inputs
    pessoa.Nome = pessoa.Nome?.Trim();
    pessoa.Email = pessoa.Email?.Trim().ToLowerInvariant();
    pessoa.Cpf = pessoa.Cpf?.Replace(".", "").Replace("-", "");
    
    if (ModelState.IsValid)
    {
        _context.Add(pessoa);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    return View(pessoa);
}
```

### SQL Injection Prevention

```csharp
// Entity Framework automatically parameterizes queries
// This is safe from SQL injection
var pessoas = await _context.Pessoa
    .Where(p => p.Nome.Contains(searchTerm))
    .ToListAsync();

// If using raw SQL (avoid when possible), use parameters
var pessoas = await _context.Pessoa
    .FromSqlRaw("SELECT * FROM Pessoa WHERE Nome LIKE {0}", $"%{searchTerm}%")
    .ToListAsync();
```

## Security Implementation

### Authentication & Authorization

#### Password Policies
```csharp
// Program.cs - Identity configuration
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<MvcSaedContext>();
```

#### Authorization Attributes
```csharp
// Global authorization requirement
[Authorize] // Applied to all controllers requiring authentication
public class PessoasController : Controller { }

// Public endpoints (no authentication required)
[AllowAnonymous]
public class HomeController : Controller
{
    public IActionResult Index() => View();
    
    [AllowAnonymous]
    public IActionResult Privacy() => View();
}
```

### Data Protection

#### Input Validation & Sanitization
```csharp
public class SecurityHelper
{
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        
        // Remove potentially dangerous characters
        input = input.Trim();
        input = System.Web.HttpUtility.HtmlEncode(input);
        
        // Additional sanitization for specific cases
        input = input.Replace("<script", "&lt;script", StringComparison.OrdinalIgnoreCase);
        input = input.Replace("javascript:", "", StringComparison.OrdinalIgnoreCase);
        
        return input;
    }
    
    public static bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf)) return false;
        
        cpf = cpf.Replace(".", "").Replace("-", "");
        
        // Additional security: Check against common invalid patterns
        string[] invalidCpfs = { "00000000000", "11111111111", "22222222222" /* ... */ };
        if (invalidCpfs.Contains(cpf)) return false;
        
        // Standard CPF validation algorithm
        return ValidateCpfAlgorithm(cpf);
    }
}
```

## Deployment & DevOps

### Production Configuration

#### Environment-specific Settings
```json
// appsettings.Production.json
{
  "ConnectionStrings": {
    "MvcSaedContext": "Server=prod-mysql.server.com;Database=saed_production;Uid=saed_app_user;Pwd={SECURE_PASSWORD};Port=3306;CharSet=utf8mb4;SSL Mode=Required;CertificateFile=/certs/client-cert.pem;CertificateKey=/certs/client-key.pem;CertificatePassword={CERT_PASSWORD};"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  },
  "AllowedHosts": "saed.yourdomain.com",
  "Identity": {
    "RequireHttps": true,
    "CookieSecure": "Always",
    "SessionTimeout": 30
  }
}
```

#### Docker Configuration
```dockerfile
# Multi-stage Dockerfile for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install system dependencies
RUN apt-get update && apt-get install -y \
    curl \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MvcSaed.csproj", "."]
RUN dotnet restore "./MvcSaed.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MvcSaed.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MvcSaed.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create non-root user for security
RUN groupadd -r saedapp && useradd -r -g saedapp saedapp
RUN chown -R saedapp:saedapp /app
USER saedapp

ENTRYPOINT ["dotnet", "MvcSaed.dll"]
```

#### Docker Compose for Development
```yaml
# docker-compose.yml
version: '3.8'

services:
  webapp:
    build: .
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=https://+:443;http://+:80
      - ASPNETCORE_Kestrel__Certificates__Default__Password=YourPassword
      - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
    volumes:
      - ~/.aspnet/https:/https:ro
    depends_on:
      - mysql
    networks:
      - saed-network

  mysql:
    image: mysql:8.0
    ports:
      - "3306:3306"
    environment:
      MYSQL_ROOT_PASSWORD: rootpassword
      MYSQL_DATABASE: saed_dev
      MYSQL_USER: saed_user
      MYSQL_PASSWORD: saed_password
    volumes:
      - mysql_data:/var/lib/mysql
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql
    networks:
      - saed-network

volumes:
  mysql_data:

networks:
  saed-network:
    driver: bridge
```

### CI/CD Pipeline

#### GitHub Actions Workflow
```yaml
# .github/workflows/deploy.yml
name: Build and Deploy SAED

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --no-restore
      
    - name: Test
      run: dotnet test --no-build --verbosity normal

  deploy:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
        
    - name: Publish
      run: dotnet publish -c Release -o ./publish
      
    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'saed-webapp'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: './publish'
```

### Monitoring & Logging

#### Application Insights Integration
```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();

// Custom telemetry
public class TelemetryService
{
    private readonly TelemetryClient _telemetryClient;

    public TelemetryService(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    public void TrackUserActivity(string userId, string activity)
    {
        _telemetryClient.TrackEvent("UserActivity", new Dictionary<string, string>
        {
            ["UserId"] = userId,
            ["Activity"] = activity,
            ["Timestamp"] = DateTime.UtcNow.ToString()
        });
    }
}
```

#### Health Checks
```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MvcSaedContext>()
    .AddCheck("External API", () => 
    {
        // Custom health check logic
        return HealthCheckResult.Healthy("API is responsive");
    });

app.MapHealthChecks("/health");
```

---

## System Architecture Summary

This comprehensive technical documentation covers the complete SAED (Sistema de Atividades Educacionais) implementation, featuring:

### Core Technologies
- **ASP.NET Core 8.0 MVC** with modern C# patterns
- **ASP.NET Core Identity 8.0** for authentication & authorization
- **Entity Framework Core 8.0** with MySQL provider
- **Bootstrap 5.3** responsive UI framework
- **MySQL 8.0** database with UTF-8MB4 support

### Key Features Implemented
- **Complete Authentication System** with user management
- **Authorization Protection** across all business controllers
- **Comprehensive Data Model** with 700+ sample records
- **Advanced Validation** including Brazilian CPF validation
- **Responsive Modern UI** with compact navigation and email truncation
- **Performance Optimization** with query optimization and caching
- **Security Best Practices** with input sanitization and CSRF protection
- **Production-Ready Deployment** configurations and Docker support

### Development Best Practices
- **Separation of Concerns** with proper MVC architecture
- **Dependency Injection** throughout the application
- **Error Handling** with comprehensive logging
- **Performance Monitoring** with Application Insights integration
- **Database Migrations** for version control and deployment
- **Responsive Design** with mobile-first approach

This system serves as a complete reference implementation for educational management systems with modern web development practices, security considerations, and production deployment strategies.