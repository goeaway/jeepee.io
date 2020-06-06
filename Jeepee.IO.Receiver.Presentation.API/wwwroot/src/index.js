import React from "react";
import { render } from "react-dom";
import App from "./components/app";
var root = document.createElement("div");
root.id = "monitor-root";
document.body.appendChild(root);
render(React.createElement(App, null), root);
//# sourceMappingURL=index.js.map