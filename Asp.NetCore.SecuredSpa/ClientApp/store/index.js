import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'
const SET_COUNTER = 'setCounter'

// STATE
const state = {
    authenticationToken: '',
    counter: 0
}

// MUTATIONS
const mutations = {
    [MAIN_SET_COUNTER](state, obj) {
        state.counter = obj.counter
    }
}

// ACTIONS
const actions = ({
    [SET_COUNTER]({ commit }, obj) {
        commit(MAIN_SET_COUNTER, obj)
    }
})

export default new Vuex.Store({
    state,
    mutations,
    actions
});
