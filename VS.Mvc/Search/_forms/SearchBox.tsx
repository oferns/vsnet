
export default class SearchBox extends HTMLElement {
    
    private searchBox: HTMLInputElement; 

    lastSearchTerm: string = '';
    cache = {};
    constructor() {
        super();
        this.searchBox = this.firstElementChild as HTMLInputElement;
    }

    public connectedCallback(): void {
        if (this.isConnected) {
            this.addEventListener('keyup', this.keyUp);            
        }
    }

    public disconnectedCallback(): void {
        if (!this.isConnected) {
            this.removeEventListener('keyup', this.keyUp);            
        }
    }


    public adoptedCallback(): void {
        this.connectedCallback(); 
    }


    private keyUp(ev: KeyboardEvent): void {
        const t = ev.keyCode;  
        const r = this.searchBox.value;
    }
}