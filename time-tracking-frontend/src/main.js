import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import PrimeVue from "primevue/config";


import "./style.css"; // Optional styling

const app = createApp(App);

app.use(router);
app.use(PrimeVue, {
    ripple: true,
    inputVariant: "filled"
});

app.mount("#app");
