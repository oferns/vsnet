
class BrowserLanguages extends HTMLSpanElement {



    constructor() {
        super();
    }

    connectedCallback(): void {
        this.innerHTML = navigator.languages.join(', ');
    }


    createdCallback(): void {

    };
}
window.customElements.define('browser-languages', BrowserLanguages, { extends: 'span' });
