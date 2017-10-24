import Vue from 'vue'
import VueRouter from 'vue-router'
import store from './store/index'

import { routes } from './routes'

Vue.use(VueRouter);

let router = new VueRouter({
    mode: 'history',
    routes
})

router.beforeEach((to, from, next) => {
    if (to.path !== '/' && store.state.authenticationToken === '') {
        next("/");
    } else {
        next();
    }
});

export default router
