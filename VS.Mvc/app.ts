import ClientInfo from "./_WebComponents/ClientInfo";

// We know this exists. The polyfills provided by webcomponentsjs in the _Layout file
declare const WebComponents: any;


WebComponents.waitFor(() => {

    window.customElements.define("client-info", ClientInfo);
});
