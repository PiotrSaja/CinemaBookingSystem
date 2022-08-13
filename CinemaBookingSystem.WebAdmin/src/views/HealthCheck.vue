<template>
  <Message severity="success" :closable="false"><h3>Health check</h3>Status: {{healthCheck}}</Message>
</template>

<script>
import HealthCheckService from '@/services/health-check-service';
import Message from 'primevue/message';
export default {
  name: 'HealthCheckView',
  data () {
    return {
      healthCheck: null,
    }
  },
  components: {
    Message
  },
  created () {
    HealthCheckService.get().then((response) => {
        this.healthCheck = response.data
        this.loading = false
        }).catch((error) => {
        console.log(error.response.data)
        })
  },
  methods: {
  }
}
</script>

<style>
</style>
