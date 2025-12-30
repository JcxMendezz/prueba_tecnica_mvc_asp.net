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
