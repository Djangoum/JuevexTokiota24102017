<template>
    <v-app id="app" v-bind:class="{ 'backgroundImage' : !authenticationToken }" class="container-fluid">
        <notifications group="generalNotifications" />
        <div class="col-sm-3" v-if="authenticationToken">
            <nav-menu params="route: route"></nav-menu>
        </div>
        <div v-bind:class="{ 'col-sm-offset-3 col-sm-9': (authenticationToken) }">
            <router-view></router-view>
        </div>
    </v-app>
</template>

<script>
import Vue from 'vue'
import CounterExample from './counter-example'
import FetchData from './fetch-data'
import HomePage from './home-page'
import NavMenu from './nav-menu'
import axios from 'axios'

Vue.component('counter-example', CounterExample);
Vue.component('fetch-data', FetchData);
Vue.component('home-page', HomePage);
Vue.component('nav-menu', NavMenu);

    export default {
        computed: {
            authenticationToken() {
                return this.$store.state.loggedIn;
            }
        },
        mounted: function () {
            axios.post('/Account/CheckLogin',
                {
                    Username: this.name,
                    Password: this.password
                })
                .then(result => {
                    this.$store.state.loggedIn = result.success;
                    if (result.data.success)
                        this.$router.push('/counter');
                })
                .catch(error => {

                });
        },
        data() {
            return {
            }
    }
}
</script>

<style scoped>

    .backgroundImage {
        background-image: url("http://cdn.pcwallart.com/images/new-york-city-at-night-black-and-white-wallpaper-1.jpg");
        background-size: cover;
    }

</style>
