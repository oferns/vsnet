!function(e){var t={};function n(o){if(t[o])return t[o].exports;var r=t[o]={i:o,l:!1,exports:{}};return e[o].call(r.exports,r,r.exports,n),r.l=!0,r.exports}n.m=e,n.c=t,n.d=function(e,t,o){n.o(e,t)||Object.defineProperty(e,t,{enumerable:!0,get:o})},n.r=function(e){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},n.t=function(e,t){if(1&t&&(e=n(e)),8&t)return e;if(4&t&&"object"==typeof e&&e&&e.__esModule)return e;var o=Object.create(null);if(n.r(o),Object.defineProperty(o,"default",{enumerable:!0,value:e}),2&t&&"string"!=typeof e)for(var r in e)n.d(o,r,function(t){return e[t]}.bind(null,r));return o},n.n=function(e){var t=e&&e.__esModule?function(){return e.default}:function(){return e};return n.d(t,"a",t),t},n.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},n.p="",n(n.s=0)}([function(e,t,n){n(2),e.exports=n(1)},function(e,t,n){},function(e,t,n){"use strict";function o(e){return(o="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(e){return typeof e}:function(e){return e&&"function"==typeof Symbol&&e.constructor===Symbol&&e!==Symbol.prototype?"symbol":typeof e})(e)}function r(e,t){for(var n=0;n<t.length;n++){var o=t[n];o.enumerable=o.enumerable||!1,o.configurable=!0,"value"in o&&(o.writable=!0),Object.defineProperty(e,o.key,o)}}function a(e){if(void 0===e)throw new ReferenceError("this hasn't been initialised - super() hasn't been called");return e}function u(e){var t="function"==typeof Map?new Map:void 0;return(u=function(e){if(null===e||(n=e,-1===Function.toString.call(n).indexOf("[native code]")))return e;var n;if("function"!=typeof e)throw new TypeError("Super expression must either be null or a function");if(void 0!==t){if(t.has(e))return t.get(e);t.set(e,o)}function o(){return l(e,arguments,c(this).constructor)}return o.prototype=Object.create(e.prototype,{constructor:{value:o,enumerable:!1,writable:!0,configurable:!0}}),i(o,e)})(e)}function l(e,t,n){return(l=function(){if("undefined"==typeof Reflect||!Reflect.construct)return!1;if(Reflect.construct.sham)return!1;if("function"==typeof Proxy)return!0;try{return Date.prototype.toString.call(Reflect.construct(Date,[],(function(){}))),!0}catch(e){return!1}}()?Reflect.construct:function(e,t,n){var o=[null];o.push.apply(o,t);var r=new(Function.bind.apply(e,o));return n&&i(r,n.prototype),r}).apply(null,arguments)}function i(e,t){return(i=Object.setPrototypeOf||function(e,t){return e.__proto__=t,e})(e,t)}function c(e){return(c=Object.setPrototypeOf?Object.getPrototypeOf:function(e){return e.__proto__||Object.getPrototypeOf(e)})(e)}n.r(t);var f=document.createElement("template");f.innerHTML='\n<p id="client_languages">\n    <slot name="languageslabel"><label>Client Languages</label></slot>\n    <span id="languages"></span>\n    <br />\n    <slot name="languagelabel"><label>Client Language</label></slot>\n    <span id="language"></span>\n    <br />\n    <slot name="agentlabel"><label>Agent</label></slot>\n    <span id="agent"></span>\n</div>\n<style>\n    p#client_languages > span {\n        color:blue;\n    }\n</style>\n';var p=function(e){function t(){var e,n,r,u,l,i;return function(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")}(this,t),n=this,e=!(r=c(t).call(this))||"object"!==o(r)&&"function"!=typeof r?a(n):r,u=a(e),i=void 0,(l="shadowRoot")in u?Object.defineProperty(u,l,{value:i,enumerable:!0,configurable:!0,writable:!0}):u[l]=i,e.shadowRoot=e.attachShadow({mode:"open"}),e.shadowRoot.appendChild(f.content.cloneNode(!0)),e}var n,u,l;return function(e,t){if("function"!=typeof t&&null!==t)throw new TypeError("Super expression must either be null or a function");e.prototype=Object.create(t&&t.prototype,{constructor:{value:e,writable:!0,configurable:!0}}),t&&i(e,t)}(t,e),n=t,(u=[{key:"connectedCallback",value:function(){var e=this.shadowRoot.getElementById("languages")||new HTMLSpanElement,t=this.shadowRoot.getElementById("language")||new HTMLSpanElement,n=this.shadowRoot.getElementById("agent")||new HTMLSpanElement;e.textContent=navigator.languages.join(", "),t.textContent=navigator.language,n.textContent=navigator.userAgent}}])&&r(n.prototype,u),l&&r(n,l),t}(u(HTMLElement));WebComponents.waitFor((function(){window.customElements.define("client-info",p)}))}]);
//# sourceMappingURL=app.js.map