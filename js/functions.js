// @ts-check

/**
 * @param {HTMLTextAreaElement | HTMLInputElement} input
 */
function copyToClipboard(input) {
    input.select();
    document.execCommand('copy');
}

/**
 * @param {HTMLElement} element
 */
function focus(element) {
    element.focus();
}