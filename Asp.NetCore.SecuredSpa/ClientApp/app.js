import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import Vuetify from 'vuetify'
import VueNotify from 'vue-notification'

Vue.prototype.$http = axios;

Vue.use(Vuetify);

Vue.use(VueNotify);

sync(store, router)

const app = new Vue({
    store,
    router,
    ...App
})

export {
    app,
    router,
    store
}
