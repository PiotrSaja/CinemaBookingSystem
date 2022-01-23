<template>
  <div class="text-white page">
    <h2>Profile</h2>
    <template v-if="profile !== null">
      <p>User: {{ profile.email }}</p>
      Roles:
      <p>User: {{ profile.role }}</p>
    </template>
  </div>
</template>

<script>
export default {
  name: 'Profile',
  data () {
    return {
      profile: null
    }
  },
  mounted () {
    this.$auth.getProfile()
      .then(profile => {
        this.profile = profile
      })
      .catch(error => {
        console.log(error)
        this.profile = {}
      })
  }
}
</script>

<style scoped>
.page {
min-height: 100vh;
}
</style>
