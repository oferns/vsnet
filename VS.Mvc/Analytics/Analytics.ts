import EventInfo from "./eventInfo";
import { EventType } from "./eventType";

export default class Analytics {

    private pingCnt: number = 0; // Counts the pings
    private lastPingMs: number = 0; // Last ping time in ms
    private LastKnownX: number = 0; // Last logged mouse x
    private LastKnownY: number = 0; // Last logged mouse y
    private scheduled: boolean = false;

    private pingTimeoutHandle: NodeJS.Timeout | undefined;
    private w: Window;

    constructor(w: Window) {
        this.w = w;
        this.w.document.addEventListener('click', this.ClickHandle);
        this.w.document.addEventListener('keypress', this.KeyPressHandler)
        this.w.document.addEventListener('mousemove', this.MouseTrack);
        this.w.document.addEventListener('visibilitychange', this.ToggleActive);
        this.w.document.addEventListener('focus', this.Focus);
        this.Ping();
        this.ResumePing();
    }

    private EventStack: EventInfo[] = [];

    private SuspendPing = (): void => {
        if (this.pingTimeoutHandle !== undefined) {
            clearTimeout(this.pingTimeoutHandle);
        }
    }

    private ResumePing = (): void => {
        this.pingTimeoutHandle = setInterval(() => this.Ping(), 5000);
    }

    private ToggleActive = (ev: Event): void => {
        if (this.w.document.hidden) {
            console.log('Suspending at ' + new Date(ev.timeStamp).toISOString());
            this.SuspendPing();
        } else {
            console.log('Resuming at ' + new Date(ev.timeStamp).toISOString());
            this.ResumePing();
        }
    }

    private Focus = (ev: FocusEvent): void => {
        this.SuspendPing();
        var obj: EventInfo = {
            mx: this.LastKnownX,
            my: this.LastKnownY,
            vh: this.w.innerHeight,
            vw: this.w.innerWidth,
            sh: this.w.screen.height,
            sw: this.w.screen.width,
            et: EventType.Click,
            ev: null,
            xp: this.getXPathForElement(this.w.document.activeElement),
            ts: Math.round(ev.timeStamp),
            pg: this.lastPingMs
        };

        console.log(JSON.stringify(obj).length);
        this.EventStack.push(obj);
        this.ResumePing();
    };

    private ClickHandle = (ev: MouseEvent): void => {
        this.SuspendPing();
        this.LastKnownX = ev.pageX;
        this.LastKnownY = ev.pageY;
        var obj: EventInfo = {
            mx: ev.pageX,
            my: ev.pageY,
            vh: this.w.innerHeight,
            vw: this.w.innerWidth,
            sh: this.w.screen.height,
            sw: this.w.screen.width,
            et: EventType.Click,
            ev: null,
            xp: this.getXPathForElement(this.w.document.activeElement),
            ts: Math.round(ev.timeStamp),         
            pg: this.lastPingMs
        };

        console.log(JSON.stringify(obj).length);
        this.EventStack.push(obj);
        this.Flush();
        this.ResumePing();
    }

    private KeyPressHandler = (ev: KeyboardEvent): void => {        
        var obj: EventInfo = {
            mx: this.LastKnownX,
            my: this.LastKnownY,
            vh: this.w.innerHeight,
            vw: this.w.innerWidth,
            sh: this.w.screen.height,
            sw: this.w.screen.width,
            et: EventType.KeyPress,
            ev: ev.keyCode,
            xp: this.getXPathForElement(this.w.document.activeElement),
            ts: Math.round(ev.timeStamp),
            pg: this.lastPingMs
        };

        console.log(JSON.stringify(obj).length);
        this.EventStack.push(obj);
    }

    private MouseTrack = (ev: MouseEvent): void => {
        this.LastKnownX = ev.screenX;
        this.LastKnownY = ev.screenY;

        if (!this.scheduled) {
            this.scheduled = true;
            setTimeout(() => {
                this.scheduled = false;
                var obj: EventInfo = {
                    mx: this.LastKnownX,
                    my: this.LastKnownY,
                    vh: this.w.innerHeight,
                    vw: this.w.innerWidth,
                    sh: this.w.screen.height,
                    sw: this.w.screen.width,
                    et: EventType.Move,
                    ev: null,
                    xp: this.getXPathForElement(this.w.document.activeElement),
                    ts: Math.round(ev.timeStamp),
                    pg: this.lastPingMs
                };

                this.EventStack.push(obj);
                console.log(JSON.stringify(obj).length);

            }, 250);
        }

    }

    private Ping = () => {
        const pingImg = new Image(1, 1);
        const start = new Date().getTime();
        pingImg.onload = () => this.lastPingMs = (new Date().getTime() - start);
        pingImg.src = "/ping?" + this.pingCnt++;
    }


    private Flush = () => {
        let cookieval: string = ''; 
        let cookies: string[] = [];
        let numberOfEvents = this.EventStack.length;

        for (let x = 0; x < numberOfEvents; x++) {
            let event = this.EventStack.shift();
            let eventString = encodeURIComponent(JSON.stringify(event));
            let newLength = cookieval.length + eventString.length;
            if (newLength < 4096) {
                cookieval = cookieval + eventString;
            } else {
                cookies.push(`vsa${cookies.length}=${cookieval}; path=/an`);
                cookieval = eventString;
            }            
        }
        if (cookieval.length > 0) {
            cookies.push(`vsa${cookies.length}=${cookieval}; path=/an`);
        }

        for (let x = 0; x < cookies.length; x++) {
            document.cookie = cookies[x];
        }

        var img = new Image(1, 1);
        img.onload = () => {
            //for (let x = 0; x < cookies.length; x++) {
            //    document.cookie = `vsa${x}=; Max-Age=-99999999;`;
            //}
            console.log('analytics sent');
        };
        img.src = "/an";
    }

    private getXPathForElement = (element: Element | null): string  => {

        const idx = (sib: Element | null, name: string): number => sib
            ? idx(sib?.previousElementSibling, name || sib?.localName) + (sib?.localName == name ? 1 : 0)
            : 1;
        const segs = (elm: Element | null): string[] => !elm || elm.nodeType !== 1
            ? ['']
            : elm.id && this.w.document.getElementById(elm.id) === elm
                ? [`id("${elm.id}")`]
                : [...segs(elm.parentElement as Element), `${elm.localName.toLowerCase()}[${idx(elm, elm.localName.toLowerCase())}]`];
        return segs(element).join('/');
    }
}