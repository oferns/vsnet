import template from './ClientInfo.html';

class ClientInfo extends HTMLElement {

    private _shadowRoot: ShadowRoot;
    constructor() {
        super();
        this._shadowRoot = this.attachShadow({ mode: 'open' });
        this._shadowRoot.innerHTML = template;

    }

    connectedCallback(): void {

        if (this._shadowRoot.firstElementChild) {
            let langsEls = this._shadowRoot.firstElementChild.getElementsByClassName('langs');
            Array.prototype.forEach.call(langsEls, function (e: Element) {
                e.textContent = navigator.languages.join(', ');
            });

            let langEls = this._shadowRoot.firstElementChild.getElementsByClassName('lang');
            Array.prototype.forEach.call(langEls, function (e: Element) {
                e.textContent = navigator.language;
            });

            let agentEls = this._shadowRoot.firstElementChild.getElementsByClassName('agent');
            Array.prototype.forEach.call(agentEls, function (e: Element) {
                e.textContent = navigator.userAgent;
            });
        }
    }
}

window.customElements.define('client-info', ClientInfo);