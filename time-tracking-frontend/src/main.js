import { createApp } from 'vue';
import App from './App.vue';
import router from './router';

import { definePreset } from '@primevue/themes';
import Material from '@primevue/themes/material';
import PrimeVue from 'primevue/config';
import ToastService from 'primevue/toastservice'; // Import ToastService
import Toast from 'primevue/toast'; // Import Toast component

import "/src/assets/styles.scss";
import "/src/assets/tailwind.css";

const app = createApp(App);

app.use(router);

const MyPreset = definePreset(Material, {
    semantic: {
        primary: {
            50: '{indigo.50}',
            100: '{indigo.100}',
            200: '{indigo.200}',
            300: '{indigo.300}',
            400: '{indigo.400}',
            500: '{indigo.500}',
            600: '{indigo.600}',
            700: '{indigo.700}',
            800: '{indigo.800}',
            900: '{indigo.900}',
            950: '{indigo.950}'
        }
    }
});

app.use(PrimeVue, {
    ripple: true,
    inputVariant: 'filled',
    theme: {
        preset: MyPreset,
        options: {
            darkModeSelector: '.app-dark'
        }
    }
});

// Register ToastService and Toast component globally
app.use(ToastService);
app.component('Toast', Toast);

app.mount('#app');
