// @ts-check

/**
 * @param {HTMLTextAreaElement | HTMLInputElement} element
 */
function copyToClipboard(element) {
    element.select();
    document.execCommand('copy');
}

/**
 * @param {HTMLElement} element
 */
function focus(element) {
    element.focus();
}

/**
 * @param {string} fileName
 * @param {string} contents
 */
function saveFile(fileName, contents) {
    const link = document.createElement('a');
    link.download = fileName;
    link.href = "data:application/octet-stream;charset=utf-8," + encodeURIComponent(contents)
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}