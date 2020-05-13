import ClientInfo from "./_WebComponents/clientInfo";
import SearchBox from "./Search/SearchBox";
import Analytics from "./Analytics/Analytics";

// We know this exists. The polyfills provided by webcomponentsjs in the _Layout file
// but we dont want the JS files to be webapck-ed so we don't import
declare const WebComponents: any;


new Analytics(window); // Start tracking

WebComponents.waitFor(() => {

    window.customElements.define("client-info", ClientInfo);
    window.customElements.define("search-box", SearchBox);

});
