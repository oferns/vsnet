import IEventInfo from "./IEventInfo";
import { EventType } from "./EventType";

export default class Analytics {

    private pingCnt: number = 0;
    private pingImg: HTMLImageElement;

    constructor(w: Window) {
        w.addEventListener('click', this.ClickHandle);
        w.addEventListener('keypress', this.KeyPressHandler)
        w.addEventListener('mousemove', this.MouseTrack);
        this.pingImg = new Image(1, 1);
        this.pingImg .onload = (ev: Event) => console.log("pong" + ev.timeStamp);
        this.pingImg.onerror = () => console.log("splat");
        // setInterval(async () => await this.Ping(++this.pingCnt), 5000)
    }

    private LastKnownX: number = 0;
    private LastKnownY: number = 0;

    private EventStack: IEventInfo[] = [];

    private ClickHandle = (ev: MouseEvent): void => {

        var eventDate = new Date();
        var obj: IEventInfo = {
            X: ev.screenX,
            Y: ev.screenY,
            VH: window.innerHeight,
            VW: window.innerWidth,
            SH: window.screen.height,
            SW: window.screen.width,
            EventType: EventType.Click,
            EventValue: null,
            FocusedControlTag: document.activeElement?.tagName,
            FocusedControlValue: document.activeElement?.textContent,
            TimeStamp: eventDate.valueOf(),
            OffSet: eventDate.getTimezoneOffset() / 60
        };

        console.log(obj);

        this.EventStack.push(obj);

    }

    private KeyPressHandler = (ev: KeyboardEvent): void => {
        var obj: IEventInfo = {
            X: this.LastKnownX,
            Y: this.LastKnownY,
            VH: window.innerHeight,
            VW: window.innerWidth,
            SH: window.screen.height,
            SW: window.screen.width,
            EventType: EventType.KeyPress,
            EventValue: ev.keyCode,
            FocusedControlTag: document.activeElement?.tagName,
            FocusedControlValue: document.activeElement?.textContent,
            TimeStamp: ev.timeStamp,
            OffSet: new Date(ev.timeStamp).getTimezoneOffset() / 60
        };

        console.log(obj);

        this.EventStack.push(obj);
    }
    
    private MouseTrack = (ev: MouseEvent): void => {
        this.LastKnownX = ev.screenX;
        this.LastKnownY = ev.screenY;
        setTimeout(() => {
            var obj: IEventInfo = {
                X: this.LastKnownX,
                Y: this.LastKnownY,
                VH: window.innerHeight,
                VW: window.innerWidth,
                SH: window.screen.height,
                SW: window.screen.width,
                EventType: EventType.Move,
                EventValue: null,
                FocusedControlTag: document.activeElement?.tagName,
                FocusedControlValue: document.activeElement?.textContent,
                TimeStamp: ev.timeStamp,
                OffSet: new Date(ev.timeStamp).getTimezoneOffset() / 60
            };

            console.log(obj);

            this.EventStack.push(obj);

        }, 250);

    }

    private Ping = (id: number) => {
        this.pingImg.src = "/ping?" + id;
    }
}