<template>
    <v-container class="mt-5" grid-list-xl text-xs-center>
        <v-layout row wrap>
            <v-flex xs12 sm5 offset-sm3>
                <v-card>
                    <v-card-media class="white--text"
                                  height="200px"
                                  src="http://images4.fanpop.com/image/photos/16100000/Cute-Kitten-kittens-16123796-500-313.jpg">
 
                    </v-card-media>
                    <v-card-title>
                        <div>
                            <span class="grey--text">Log In your user</span><br>
                        </div>
                    </v-card-title>
                    <v-card-actions>
                        <v-form ref="loginForm" v-model="valid">
                            <v-text-field label="Name"
                                          v-model="name"
                                          :rules="nameRules"
                                          :counter="10"
                                          required>
                            </v-text-field>
                            <v-text-field label="Password"
                                          v-model="password"
                                          :rules="passwordRules"
                                          :append-icon="passwordVisibility ? 'visibility' : 'visibility_off'"
                                          :append-icon-cb="() => (passwordVisibility = !passwordVisibility)"
                                          :type="passwordVisibility ? 'password' : 'text'"
                                          counter
                                          required>
                            </v-text-field>
                            <v-btn v-on:click="login">submit</v-btn>
                            <v-btn>clear</v-btn>
                        </v-form>
                    </v-card-actions>
                </v-card>

            </v-flex>
        </v-layout>
</v-container>
</template>

<script>
    import axios from 'axios'

    export default {
        data() {
            return {
                valid: false,
                name: '',
                nameRules: [
                    (v) => !!v || 'Name is required',
                    (v) => v.length <= 10 || 'Name must be less than 10 characters'
                ],
                password: '',
                passwordRules: [
                    (v) => !!v || 'E-mail is required',
                    (v) => v.length <= 10 || 'Name must be less than 10 characters'
                ],
                passwordVisibility: true
            }
        },
        methods: {
            login: function () {
                if (this.$refs.loginForm.validate()) {
                    axios.post('/Account/Authorize',
                        {
                            Username: this.name,
                            Password: this.password
                        })
                        .then(result => {
                            this.$store.state.authenticationToken = result.data.token;
                            if (result !== null && result !== undefined && result.data.token !== '' && result.data.token !== null && result.data.token !== undefined)
                                this.$router.push('/counter');

                            axios.defaults.headers.common['Authorization'] = 'Bearer ' + result.data.token;
                        })
                        .catch(error => {
                            this.$notify({
                                group:"generalNotifications", type: "error", text: "Invalid Login Attempt" });
                        });
                }
            }
        }
    }
</script>

<style scoped>

    form{
        width:100%;
    }
</style>
