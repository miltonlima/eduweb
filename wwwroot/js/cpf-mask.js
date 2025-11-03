// Máscara para CPF
function applyCpfMask(input) {
    input.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, ''); // Remove caracteres não numéricos
        
        // Aplica a máscara 000.000.000-00
        if (value.length <= 11) {
            value = value.replace(/(\d{3})(\d)/, '$1.$2');
            value = value.replace(/(\d{3})(\d)/, '$1.$2');
            value = value.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
        }
        
        e.target.value = value;
    });
}

// Validação de CPF em tempo real
function validateCpfRealTime(input) {
    input.addEventListener('blur', function (e) {
        const cpf = e.target.value.replace(/\D/g, ''); // Remove caracteres não numéricos
        const isValid = isValidCpf(cpf);
        
        // Remove classes anteriores
        e.target.classList.remove('is-valid', 'is-invalid');
        
        if (cpf && cpf.length === 11) {
            if (isValid) {
                e.target.classList.add('is-valid');
                hideCustomError(e.target);
            } else {
                e.target.classList.add('is-invalid');
                showCustomError(e.target, 'CPF inválido');
            }
        } else if (cpf && cpf.length > 0) {
            e.target.classList.add('is-invalid');
            showCustomError(e.target, 'CPF deve ter 11 dígitos');
        }
    });
}

// Função para validar CPF
function isValidCpf(cpf) {
    if (cpf.length !== 11) return false;
    
    // Verifica se todos os dígitos são iguais
    if (/^(\d)\1{10}$/.test(cpf)) return false;
    
    // Calcula o primeiro dígito verificador
    let sum = 0;
    for (let i = 0; i < 9; i++) {
        sum += parseInt(cpf.charAt(i)) * (10 - i);
    }
    let remainder = sum % 11;
    let firstDigit = remainder < 2 ? 0 : 11 - remainder;
    
    if (parseInt(cpf.charAt(9)) !== firstDigit) return false;
    
    // Calcula o segundo dígito verificador
    sum = 0;
    for (let i = 0; i < 10; i++) {
        sum += parseInt(cpf.charAt(i)) * (11 - i);
    }
    remainder = sum % 11;
    let secondDigit = remainder < 2 ? 0 : 11 - remainder;
    
    return parseInt(cpf.charAt(10)) === secondDigit;
}

// Função para mostrar erro customizado
function showCustomError(input, message) {
    let errorSpan = input.parentElement.querySelector('.cpf-error');
    if (!errorSpan) {
        errorSpan = document.createElement('span');
        errorSpan.className = 'text-danger cpf-error';
        errorSpan.style.fontSize = '0.875em';
        input.parentElement.appendChild(errorSpan);
    }
    errorSpan.textContent = message;
}

// Função para esconder erro customizado
function hideCustomError(input) {
    const errorSpan = input.parentElement.querySelector('.cpf-error');
    if (errorSpan) {
        errorSpan.remove();
    }
}

// Inicializar máscaras quando a página carregar
document.addEventListener('DOMContentLoaded', function() {
    // Aplica máscara em todos os campos de CPF
    const cpfInputs = document.querySelectorAll('input[data-mask="cpf"], input[name="Cpf"]');
    cpfInputs.forEach(function(input) {
        applyCpfMask(input);
        validateCpfRealTime(input);
    });
});