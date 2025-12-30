/**
 * Task List Module
 * Funcionalidades para la lista de tareas
 */

var TaskList = (function () {
    'use strict';

    /**
     * Inicializa el módulo
     */
    function init() {
        setupSearch();
        setupKeyboardShortcuts();
        highlightOverdue();
    }

    /**
     * Configura búsqueda con debounce
     */
    function setupSearch() {
        var searchInput = document.getElementById('SearchTerm');
        if (!searchInput) return;

        var debounceTimer;
        searchInput.addEventListener('input', function () {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(function () {
                // Auto-submit después de 500ms de inactividad
                // Solo si hay más de 2 caracteres o está vacío
                if (searchInput.value.length >= 3 || searchInput.value.length === 0) {
                    document.getElementById('filterForm').submit();
                }
            }, 500);
        });

        // Submit on Enter
        searchInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                document.getElementById('filterForm').submit();
            }
        });
    }

    /**
     * Configura atajos de teclado
     */
    function setupKeyboardShortcuts() {
        document.addEventListener('keydown', function (e) {
            // Ctrl+N o Cmd+N = Nueva tarea
            if ((e.ctrlKey || e.metaKey) && e.key === 'n') {
                e.preventDefault();
                window.location.href = '/Tasks/Create';
            }

            // Ctrl+F o Cmd+F = Enfocar búsqueda
            if ((e.ctrlKey || e.metaKey) && e.key === 'f') {
                var searchInput = document.getElementById('SearchTerm');
                if (searchInput) {
                    e.preventDefault();
                    searchInput.focus();
                    searchInput.select();
                }
            }

            // Escape = Limpiar búsqueda
            if (e.key === 'Escape') {
                var searchInput = document.getElementById('SearchTerm');
                if (searchInput && document.activeElement === searchInput) {
                    searchInput.value = '';
                    searchInput.blur();
                }
            }
        });
    }

    /**
     * Resalta tareas vencidas con animación
     */
    function highlightOverdue() {
        var overdueRows = document.querySelectorAll('tr.is-overdue');
        overdueRows.forEach(function (row) {
            row.classList.add('fade-in');
        });
    }

    /**
     * Elimina una tarea por AJAX y actualiza la UI
     * @param {number} taskId
     * @param {HTMLElement} row
     */
    function deleteTaskAjax(taskId, row) {
        if (!confirm('¿Seguro que deseas eliminar esta tarea?')) return;

        fetch(`/api/tasksapi/${taskId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() || ''
            }
        })
        .then(res => res.ok ? res.json() : Promise.reject(res))
        .then(data => {
            Alerts.success(data.message || 'Tarea eliminada exitosamente');
            if (row) row.remove();
        })
        .catch(() => Alerts.error('Error al eliminar la tarea'));
    }

    // Delegar evento en la tabla
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.task-actions .btn-delete-ajax').forEach(btn => {
            btn.addEventListener('click', function (e) {
                e.preventDefault();
                const row = btn.closest('tr');
                const taskId = btn.dataset.taskId;
                deleteTaskAjax(taskId, row);
            });
        });
    });

    // Inicializar cuando el DOM esté listo
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // API pública
    return {
        init: init
    };
})();
