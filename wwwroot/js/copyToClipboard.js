// @ts-check

/**
 * @param {HTMLTextAreaElement} textarea
 */
function copyToClipboard(textarea) {
    textarea.select();
    document.execCommand('copy');
}