/**
 * Task Form Module
 * Validaciones y funcionalidades para formularios de tareas
 */

var TaskForm = (function () {
    'use strict';

    // Configuración
    var config = {
        titleMaxLength: 200,
        descriptionMaxLength: 1000,
        titleMinLength: 3
    };

    /**
     * Inicializa el módulo
     */
    function init() {
        setupCharacterCounters();
        setupDateValidation();
        setupFormValidation();
        setupAutoSave();
    }

    /**
     * Configura contadores de caracteres
     */
    function setupCharacterCounters() {
        var titleInput = document.getElementById('Title');
        var titleCounter = document.getElementById('titleCounter');

        if (titleInput && titleCounter) {
            // Actualizar contador inicial
            titleCounter.textContent = titleInput.value.length;

            titleInput.addEventListener('input', function () {
                var length = this.value.length;
                titleCounter.textContent = length;

                // Cambiar color si se acerca al límite
                if (length >= config.titleMaxLength * 0.9) {
                    titleCounter.classList.add('text-danger');
                } else if (length >= config.titleMaxLength * 0.75) {
                    titleCounter.classList.add('text-warning');
                    titleCounter.classList.remove('text-danger');
                } else {
                    titleCounter.classList.remove('text-warning', 'text-danger');
                }
            });
        }

        var descInput = document.getElementById('Description');
        var descCounter = document.getElementById('descriptionCounter');

        if (descInput && descCounter) {
            // Actualizar contador inicial
            descCounter.textContent = descInput.value.length;

            descInput.addEventListener('input', function () {
                var length = this.value.length;
                descCounter.textContent = length;

                if (length >= config.descriptionMaxLength * 0.9) {
                    descCounter.classList.add('text-danger');
                } else if (length >= config.descriptionMaxLength * 0.75) {
                    descCounter.classList.add('text-warning');
                    descCounter.classList.remove('text-danger');
                } else {
                    descCounter.classList.remove('text-warning', 'text-danger');
                }
            });
        }
    }

    /**
     * Configura validación de fecha
     */
    function setupDateValidation() {
        var dueDateInput = document.getElementById('DueDate');
        if (!dueDateInput) return;

        // Establecer fecha mínima como hoy (solo para crear)
        var isCreateForm = window.location.pathname.toLowerCase().includes('/create');
        if (isCreateForm) {
            var today = new Date().toISOString().split('T')[0];
            dueDateInput.setAttribute('min', today);
        }

        dueDateInput.addEventListener('change', function () {
            validateDueDate(this, isCreateForm);
        });

        // Validación al blur
        dueDateInput.addEventListener('blur', function () {
            validateDueDate(this, isCreateForm);
        });
    }

    /**
     * Valida la fecha de vencimiento
     * @param {HTMLInputElement} input - El input de fecha
     * @param {boolean} checkFuture - Si debe validar que sea fecha futura
     */
    function validateDueDate(input, checkFuture) {
        var value = input.value;
        var errorSpan = document.querySelector('[data-valmsg-for="DueDate"]') ||
                        input.nextElementSibling;

        // Limpiar error previo
        if (errorSpan && errorSpan.classList.contains('form-error')) {
            errorSpan.textContent = '';
        }
        input.classList.remove('is-invalid');

        if (!value) return true; // Fecha es opcional

        var selectedDate = new Date(value);
        var today = new Date();
        today.setHours(0, 0, 0, 0);

        if (checkFuture && selectedDate < today) {
            input.classList.add('is-invalid');
            if (errorSpan) {
                errorSpan.textContent = 'La fecha de vencimiento no puede ser anterior a hoy';
            }
            return false;
        }

        return true;
    }

    /**
     * Configura validación del formulario
     */
    function setupFormValidation() {
        var form = document.getElementById('taskForm');
        if (!form) return;

        form.addEventListener('submit', function (e) {
            var isValid = true;

            // Validar título
            var titleInput = document.getElementById('Title');
            if (titleInput) {
                var title = titleInput.value.trim();
                var titleError = document.querySelector('[data-valmsg-for="Title"]');

                titleInput.classList.remove('is-invalid');
                if (titleError) titleError.textContent = '';

                if (!title) {
                    titleInput.classList.add('is-invalid');
                    if (titleError) titleError.textContent = 'El título es obligatorio';
                    isValid = false;
                } else if (title.length < config.titleMinLength) {
                    titleInput.classList.add('is-invalid');
                    if (titleError) titleError.textContent = 'El título debe tener al menos ' + config.titleMinLength + ' caracteres';
                    isValid = false;
                } else if (title.length > config.titleMaxLength) {
                    titleInput.classList.add('is-invalid');
                    if (titleError) titleError.textContent = 'El título no puede exceder ' + config.titleMaxLength + ' caracteres';
                    isValid = false;
                }
            }

            // Validar descripción (longitud máxima)
            var descInput = document.getElementById('Description');
            if (descInput && descInput.value.length > config.descriptionMaxLength) {
                descInput.classList.add('is-invalid');
                var descError = document.querySelector('[data-valmsg-for="Description"]');
                if (descError) descError.textContent = 'La descripción no puede exceder ' + config.descriptionMaxLength + ' caracteres';
                isValid = false;
            }

            // Validar fecha (solo para crear)
            var dueDateInput = document.getElementById('DueDate');
            var isCreateForm = window.location.pathname.toLowerCase().includes('/create');
            if (dueDateInput && isCreateForm) {
                if (!validateDueDate(dueDateInput, true)) {
                    isValid = false;
                }
            }

            if (!isValid) {
                e.preventDefault();
                e.stopPropagation();

                // Scroll al primer error
                var firstError = form.querySelector('.is-invalid');
                if (firstError) {
                    firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
                    firstError.focus();
                }

                // Restaurar botón de submit
                if (typeof App !== 'undefined' && App.restoreSubmitButton) {
                    App.restoreSubmitButton(form);
                }
            }
        });

        // Limpiar errores al escribir
        var inputs = form.querySelectorAll('input, textarea, select');
        inputs.forEach(function (input) {
            input.addEventListener('input', function () {
                this.classList.remove('is-invalid');
                var errorSpan = document.querySelector('[data-valmsg-for="' + this.name + '"]');
                if (errorSpan) {
                    errorSpan.textContent = '';
                }
            });
        });
    }

    /**
     * Configura auto-guardado en localStorage (opcional)
     */
    function setupAutoSave() {
        var form = document.getElementById('taskForm');
        if (!form) return;

        var isCreateForm = window.location.pathname.toLowerCase().includes('/create');
        if (!isCreateForm) return; // Solo para crear

        var storageKey = 'taskDraft';

        // Cargar borrador si existe
        try {
            var draft = localStorage.getItem(storageKey);
            if (draft) {
                var data = JSON.parse(draft);
                var shouldRestore = confirm('Se encontró un borrador guardado. ¿Desea restaurarlo?');

                if (shouldRestore) {
                    if (data.title) document.getElementById('Title').value = data.title;
                    if (data.description) document.getElementById('Description').value = data.description;
                    if (data.dueDate) document.getElementById('DueDate').value = data.dueDate;

                    // Actualizar contadores
                    var titleCounter = document.getElementById('titleCounter');
                    var descCounter = document.getElementById('descriptionCounter');
                    if (titleCounter) titleCounter.textContent = (data.title || '').length;
                    if (descCounter) descCounter.textContent = (data.description || '').length;
                }

                localStorage.removeItem(storageKey);
            }
        } catch (e) {
            console.warn('Error loading draft:', e);
        }

        // Guardar borrador cada 30 segundos
        setInterval(function () {
            try {
                var titleInput = document.getElementById('Title');
                var descInput = document.getElementById('Description');
                var dateInput = document.getElementById('DueDate');

                if (titleInput && titleInput.value.trim()) {
                    var draft = {
                        title: titleInput.value,
                        description: descInput ? descInput.value : '',
                        dueDate: dateInput ? dateInput.value : '',
                        savedAt: new Date().toISOString()
                    };
                    localStorage.setItem(storageKey, JSON.stringify(draft));
                }
            } catch (e) {
                console.warn('Error saving draft:', e);
            }
        }, 30000);

        // Limpiar borrador al enviar exitosamente
        form.addEventListener('submit', function () {
            localStorage.removeItem(storageKey);
        });
    }

    // Inicializar cuando el DOM esté listo
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // API pública
    return {
        init: init,
        validateDueDate: validateDueDate
    };
})();
