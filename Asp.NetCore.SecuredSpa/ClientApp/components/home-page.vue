<template>
    <v-container grid-list-xl text-xs-center>
        <v-layout row wrap>
            <v-flex xs12 sm5 offset-sm3>
                <v-card>
                    <v-card-media class="white--text"
                                  height="200px"
                                  src="/static/doc-images/cards/docks.jpg">
                        <v-container fill-height fluid>
                            <v-layout fill-height>
                                <v-flex xs12 align-end flexbox>
                                    <v-avatar :tile="true"
                                              :size="70"
                                              class="grey lighten-4">
                                        <img src="https://pbs.twimg.com/profile_images/520993615800061953/SW5bOw6a_400x400.jpeg" alt="avatar">
                                        </v-avatar>
                                        </v-flex>
                            </v-layout>
                        </v-container>
                    </v-card-media>
                    <v-card-title>
                        <div>
                            <span class="grey--text">Log In your user</span><br>
                        </div>
                    </v-card-title>
                    <v-card-actions>

                        <v-form v-model="valid">
                            <v-text-field label="Name"
                                          v-model="name"
                                          :rules="nameRules"
                                          :counter="10"
                                          required>
                            </v-text-field>
                            <v-text-field label="E-mail"
                                          v-model="email"
                                          :rules="emailRules"
                                          required>
                            </v-text-field>
                            <v-btn>submit</v-btn>
                            <v-btn>clear</v-btn>
                        </v-form>
                    </v-card-actions>
                </v-card>

            </v-flex>
        </v-layout>
</v-container>
</template>

<script>
    export default {
        data() {
            return {
                valid: false,
                name: '',
                nameRules: [
                    (v) => !!v || 'Name is required',
                    (v) => v.length <= 10 || 'Name must be less than 10 characters'
                ],
                email: '',
                emailRules: [
                    (v) => !!v || 'E-mail is required',
                    (v) => /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'E-mail must be valid'
                ]
            }
        }
    }
</script>

<style scoped>

    form{
        width:100%;
    }
</style>
