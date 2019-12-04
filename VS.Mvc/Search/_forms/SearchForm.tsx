
export default class ClientInfo extends HTMLFormElement {
    public shadowRoot: ShadowRoot

    constructor() {
        super()
        this.shadowRoot = this.attachShadow({ mode: "open" })
    }


    public connectedCallback(): void {
        const languagesEle = this.shadowRoot.getElementById("languages");
        if (languagesEle) {

        }
    }
}