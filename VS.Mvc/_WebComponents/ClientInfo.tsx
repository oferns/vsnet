
const template = document.createElement('template');


template.innerHTML = `
<p id="client_languages">
    <slot name="languageslabel"><label>Client Languages</label></slot>
    <span id="languages"></span>
    <br />
    <slot name="languagelabel"><label>Client Language</label></slot>
    <span id="language"></span>
    <br />
    <slot name="agentlabel"><label>Agent</label></slot>
    <span id="agent"></span>
</div>
<style>
    p#client_languages > span {
        color:blue;
    }
</style>
`;

export default class ClientInfo extends HTMLElement {
    public shadowRoot: ShadowRoot;


    constructor() {
        super();
        this.shadowRoot = this.attachShadow({ mode: "open" });
        this.shadowRoot.appendChild(template.content.cloneNode(true));

    }

    public connectedCallback(): void {
        const languagesEle = this.shadowRoot.getElementById("languages") || new HTMLSpanElement();
        const languageEle = this.shadowRoot.getElementById("language") || new HTMLSpanElement();
        const agentEle = this.shadowRoot.getElementById("agent") || new HTMLSpanElement();

        languagesEle.textContent = navigator.languages.join(", ");
        languageEle.textContent = navigator.language;
        agentEle.textContent = navigator.userAgent;
    }
}