import { EventType } from "./EventType";


export default interface IEventInfo {

    TimeStamp: Number; // Event time (client)
    OffSet: Number; // Client timezone offset
    X: Number; // Mouse X Position 
    Y: Number; // Mouse Y Position
    VH: Number;  // Current ViewHeight
    VW: Number; // Current ViewWidth
    SH: Number;  // Current ScreenHeight
    SW: Number; // Current ScreenWidth
    FocusedControlValue: string | number | null | undefined;    
    FocusedControlTag: string | null | undefined;
    EventType: EventType;
    EventValue: any;
}
