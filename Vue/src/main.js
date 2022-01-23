// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import BootstrapVue from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import Axios from 'axios'
import auth from './auth'
import { initAxios } from './api'
import vSelect from 'vue-select'

Vue.config.productionTip = false

Axios.defaults.baseURL = process.env.API_ENDPOINT

Vue.use(BootstrapVue)

Vue.use(auth)

Vue.component('v-select', vSelect)

new Vue({
  router,
  created () {
    initAxios()
  },
  render: h => h(App)
}).$mount('#app')
