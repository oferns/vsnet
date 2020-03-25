
export default class SearchBox extends HTMLElement {

    constructor() {
        super()
    }

    public connectedCallback(): void {
        if (this.isConnected) {
            this.addEventListener('keyup', this.keyUp);
            return;
        }
    }

    public disconnectedCallback(): void {
        if (!this.isConnected) {
            this.removeEventListener('keyup', this.keyUp);
            return;
        }
    }


    public adoptedCallback(): void {

    }


    private keyUp(ev: KeyboardEvent): void {
        const t = ev.keyCode;
    }
}