import ReactDOM from 'react-dom/client';
import App from './App.jsx';
import "./index.css";
import { Provider } from "react-redux";
import store from "./Store/store.js";

ReactDOM.createRoot(document.getElementById("root")).render(
    <Provider store={store}>
        <div className="bg-base-300">
            <App />
        </div>
    </Provider>
);
