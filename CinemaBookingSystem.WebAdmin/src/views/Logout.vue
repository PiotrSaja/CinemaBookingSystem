<template>

<Message severity="success" :closable="false"><h3>You have been logout.</h3></Message>
</template>

<script>
import Message from 'primevue/message';
export default {
  name: 'LogoutView',
  data () {
    return {
    }
  },
  components: {
    Message
  },
  created () {
    this.$auth.logout()
  }
}
</script>

<style>
</style>
