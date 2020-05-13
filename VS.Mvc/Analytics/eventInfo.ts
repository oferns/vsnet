import { EventType } from "./eventType";

export default interface EventInfo {
    ts: Number; // Event time (client)    
    mx: Number; // Mouse X Position 
    my: Number; // Mouse Y Position
    vh: Number;  // Current ViewHeight
    vw: Number; // Current ViewWidth
    sh: Number;  // Current ScreenHeight
    sw: Number; // Current ScreenWidth
    xp: string | null | undefined;
    et: EventType;
    ev: any;
    pg: number;
}
