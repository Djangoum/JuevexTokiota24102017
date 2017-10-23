<template>
    <v-app id="app" class="container-fluid">
        <div class="row">
            <div class="col-sm-3" v-if="isLogged">
                <nav-menu params="route: route"></nav-menu>
            </div>
            <div class="col-sm-12">
                <router-view></router-view>
            </div>
        </div>

    </v-app>

</template>

<script>
import Vue from 'vue'
import CounterExample from './counter-example'
import FetchData from './fetch-data'
import HomePage from './home-page'
import NavMenu from './nav-menu'

Vue.component('counter-example', CounterExample);
Vue.component('fetch-data', FetchData);
Vue.component('home-page', HomePage);
Vue.component('nav-menu', NavMenu);

    export default {
        computed: {
            isLogged() {
                return this.$store.loggedIn;
            }
        },
        data() {
            return {
            }
    }
}
</script>

<style>
</style>
