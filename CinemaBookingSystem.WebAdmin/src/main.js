import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config'
import router from './router'
import auth from './auth'
import { initAxios } from './api'

import 'primevue/resources/themes/saga-blue/theme.css'       //theme
import 'primevue/resources/primevue.min.css'                 //core css
import 'primeicons/primeicons.css'                          //icons

const app = createApp(App);

    app
    .use(PrimeVue)
    .use(router)
    .use(auth.install(app))
    .use(initAxios)
    .mount('#app')
