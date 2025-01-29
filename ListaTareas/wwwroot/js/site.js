// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('.done-checkbox').on('click', function (e) {
        markDoneComplete(e.target);
    });

    $('.detail-data').on('click', function (e) {
        var row = e.target;

        if (row && row.dataset.id) {
            const todoId = row.dataset.id;
            window.location.href = `/Details/${todoId}`;
        }
    });

    fixDropdownPosition()

    window.addEventListener('resize', () => {
        fixDropdownPosition()
    });
});

// Functions site.
function markDoneComplete(checkbox) {
    checkbox.disabled = true;

    var row = checkbox.closest('tr');
    $(row).addClass('done');

    var form = checkbox.closest('form');
    form.submit();
}

function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;
    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

function fixDropdownPosition() {
    var dropdown = $('.fix-dropdown');

    if (window.innerWidth <= 575) {
        dropdown.removeClass('dropdown-menu-end');
    }
    else {
        dropdown.addClass('dropdown-menu-end');
    }
}

// Color theme logic.
(() => {
    'use strict'

    const getStoredTheme = () => localStorage.getItem('theme')
    const setStoredTheme = theme => localStorage.setItem('theme', theme)

    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme()
        if (storedTheme) {
            return storedTheme
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
    }

    const setTheme = theme => {
        if (theme === 'auto') {
            document.documentElement.setAttribute('data-bs-theme', (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'))
        } else {
            document.documentElement.setAttribute('data-bs-theme', theme)
        }
    }

    setTheme(getPreferredTheme())

    const showActiveTheme = (theme) => {
        const activeIcons = document.querySelectorAll('.theme-icon-active use');
        const activeButton = document.querySelectorAll(`[data-bs-theme-value="${theme}"]`);

        if (activeIcons.length > 0) {
            const activeSvg = activeButton[0].querySelector('svg use').getAttribute('href');
            activeIcons.forEach((icon) => icon.setAttribute('href', activeSvg));
        }

        document.querySelectorAll('[data-bs-theme-value]').forEach((button) => {
            button.classList.remove('active');
            button.setAttribute('aria-pressed', 'false');
        });

        activeButton.forEach((button) => {
            button.classList.add('active');
            button.setAttribute('aria-pressed', 'true');
        });
    }

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme()
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme())
        }
    })

    window.addEventListener('DOMContentLoaded', () => {
        showActiveTheme(getPreferredTheme())

        document.querySelectorAll('[data-bs-theme-value]')
            .forEach(toggle => {
                toggle.addEventListener('click', () => {
                    const theme = toggle.getAttribute('data-bs-theme-value')
                    setStoredTheme(theme)
                    setTheme(theme)
                    showActiveTheme(theme)
                })
            })
    });
})();
