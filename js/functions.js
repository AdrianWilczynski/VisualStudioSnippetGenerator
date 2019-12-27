// @ts-check

/**
 * @param {string} id
 */
function copyToClipboard(id) {
    const input = document.getElementById(id);

    if (!(input instanceof HTMLTextAreaElement || input instanceof HTMLInputElement)) {
        throw new Error('Expected HTMLTextAreaElement or HTMLInputElement.');
    }

    input.select();
    document.execCommand('copy');
}

/**
 * @param {HTMLElement} element
 */
function focus(element) {
    element.focus();
}