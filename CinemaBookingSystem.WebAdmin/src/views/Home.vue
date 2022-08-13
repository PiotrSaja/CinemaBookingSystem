<template>
  <h1>TEST</h1>
  <Button @click="onLogin()">Login</Button>
  <span v-if="isUserLoggedIn" >{{profile.email}}</span>
</template>

<script>
import Button from "primevue/button"
export default {
  name: 'HomeView',
  data () {
    return {
      isUserLoggedIn: false,
      profile: null
    }
  },
  components: {
    Button
  },
  created () {
    this.$auth.isUserLoggedIn()
      .then(isLoggedIn => {
        this.isUserLoggedIn = isLoggedIn
      })
      .catch(error => {
        console.log(error)
        this.isUserLoggedIn = false
      })
    this.$auth.getProfile()
      .then(profile => {
        this.profile = profile
      })
      .catch(error => {
        console.log(error)
        this.profile = {}
      })
  },
  methods: {
    onLogin () {
      this.$auth.login()
    },
  }
}
</script>

<style>
</style>
