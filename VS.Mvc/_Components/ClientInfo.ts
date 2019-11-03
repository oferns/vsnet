import template from './ClientInfo.html';

class ClientInfo extends HTMLElement {
    private div: Element | null = null;
    constructor() {
        super();
        const shadow = this.attachShadow({ mode: 'open' });
        shadow.innerHTML = template;
        this.div = shadow.firstElementChild;

    }

    connectedCallback(): void {

        if (this.div) {
            let langsEls = this.div.getElementsByClassName('langs')
            Array.prototype.forEach.call(langsEls, function (e: Element) {
                e.textContent = navigator.languages.join(', ');
            });

            let langEls = this.div.getElementsByClassName('lang')
            Array.prototype.forEach.call(langEls, function (e: Element) {
                e.textContent = navigator.language;
            });

            let agentEls = this.div.getElementsByClassName('agent')
            Array.prototype.forEach.call(agentEls, function (e: Element) {
                e.textContent = navigator.userAgent;
            });

            
        }
    }
}

window.customElements.define('client-info', ClientInfo);