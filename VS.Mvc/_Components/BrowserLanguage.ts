
class BrowserLanguage extends HTMLSpanElement {



    constructor() {
        super();
    }

    connectedCallback(): void {
        this.innerHTML = navigator.language;
    }


    createdCallback(): void {

    };
}
window.customElements.define('browser-language', BrowserLanguage, { extends: 'span' });
