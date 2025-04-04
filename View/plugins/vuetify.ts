import '@mdi/font/css/materialdesignicons.css';

import 'vuetify/styles';
import {createVuetify} from "vuetify/framework";

export default defineNuxtPlugin((app) => {
    const vuetify = createVuetify({
        theme: {
            defaultTheme: 'dark'
        }
    })
    app.vueApp.use(vuetify)
});