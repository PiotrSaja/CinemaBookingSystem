<template>
<Card v-if="isUserLoggedIn"> 
    <template #title>
        Profile
    </template>
    <template #content>
        Email : {{profile.email}}
    </template>
</Card>
</template>

<script>
import Card from 'primevue/card'
export default {
  name: 'ProfileView',
  data () {
    return {
      isUserLoggedIn: false,
      profile: null
    }
  },
  components: {
    Card
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
  }
}
</script>

<style>
</style>
