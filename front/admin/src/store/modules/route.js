const route = {
  state: {
    currentRouteName: '',
  },
  mutations: {
    SET_CURRENT_ROUTE_NAME: (state, name) => {
      state.currentRouteName = name
    }
  },
  actions: {
    SetCurrentRouteName({ commit }, routeName) {
      commit('SET_CURRENT_ROUTE_NAME', routeName)
    },
  }
}

export default route
