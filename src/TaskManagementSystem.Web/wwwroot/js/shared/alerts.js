/**
 * Site.js - JavaScript Global
 * Funcionalidades compartidas en toda la aplicación
 */

var App = (function () {
    'use strict';

    /**
     * Inicializa la aplicación
     */
    function init() {
        setupTooltips();
        setupConfirmDialogs();
        setupLoadingState();
        setupFormValidation();
    }

    /**
     * Inicializa tooltips de Bootstrap
     */
    function setupTooltips() {
        var tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
        tooltipTriggerList.forEach(function (tooltipTriggerEl) {
            new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    /**
     * Configura diálogos de confirmación
     */
    function setupConfirmDialogs() {
        document.addEventListener('click', function (e) {
            var confirmBtn = e.target.closest('[data-confirm]');
            if (confirmBtn) {
                var message = confirmBtn.dataset.confirm || '¿Está seguro de realizar esta acción?';
                if (!confirm(message)) {
                    e.preventDefault();
                    e.stopPropagation();
                }
            }
        });
    }

    /**
     * Configura estado de carga para formularios
     */
    function setupLoadingState() {
        document.addEventListener('submit', function (e) {
            var form = e.target;
            if (form.tagName === 'FORM' && !form.dataset.noLoading) {
                var submitBtn = form.querySelector('[type="submit"]');
                if (submitBtn && !submitBtn.disabled) {
                    submitBtn.disabled = true;
                    var originalText = submitBtn.innerHTML;
                    submitBtn.dataset.originalText = originalText;
                    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-1" role="status"></span> Procesando...';
                }
            }
        });
    }

    /**
     * Restaura el botón de envío
     * @param {HTMLFormElement} form
     */
    function restoreSubmitButton(form) {
        var submitBtn = form.querySelector('[type="submit"]');
        if (submitBtn && submitBtn.dataset.originalText) {
            submitBtn.disabled = false;
            submitBtn.innerHTML = submitBtn.dataset.originalText;
        }
    }

    /**
     * Configura validación de formularios
     */
    function setupFormValidation() {
        var forms = document.querySelectorAll('.needs-validation');
        forms.forEach(function (form) {
            form.addEventListener('submit', function (e) {
                if (!form.checkValidity()) {
                    e.preventDefault();
                    e.stopPropagation();
                    restoreSubmitButton(form);
                }
                form.classList.add('was-validated');
            });
        });
    }

    /**
     * Muestra overlay de carga
     */
    function showLoading() {
        var overlay = document.querySelector('.loading-overlay');
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.className = 'loading-overlay';
            overlay.innerHTML =
                '<div class="text-center">' +
                '<div class="spinner-border text-primary" role="status">' +
                '<span class="visually-hidden">Cargando...</span>' +
                '</div>' +
                '<p class="mt-2 text-muted">Cargando...</p>' +
                '</div>';
            document.body.appendChild(overlay);
        }
        setTimeout(function () {
            overlay.classList.add('show');
        }, 10);
    }

    /**
     * Oculta overlay de carga
     */
    function hideLoading() {
        var overlay = document.querySelector('.loading-overlay');
        if (overlay) {
            overlay.classList.remove('show');
        }
    }

    /**
     * Formatea una fecha
     * @param {string|Date} date
     * @returns {string}
     */
    function formatDate(date) {
        if (!date) return 'Sin fecha';
        var d = new Date(date);
        return d.toLocaleDateString('es-ES', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    }

    /**
     * Formatea fecha y hora
     * @param {string|Date} date
     * @returns {string}
     */
    function formatDateTime(date) {
        if (!date) return 'Sin fecha';
        var d = new Date(date);
        return d.toLocaleDateString('es-ES', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
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
        showLoading: showLoading,
        hideLoading: hideLoading,
        formatDate: formatDate,
        formatDateTime: formatDateTime,
        restoreSubmitButton: restoreSubmitButton
    };
})();
