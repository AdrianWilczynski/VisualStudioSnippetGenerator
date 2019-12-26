// @ts-check

/**
 * @param {HTMLTextAreaElement | HTMLInputElement} textarea
 */
function copyToClipboard(textarea) {
    textarea.select();
    document.execCommand('copy');
}

/**
 * @param {HTMLElement} element
 */
function focus(element) {
    element.focus();
}