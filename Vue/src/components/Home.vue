<template>
  <div class="container text-white page">
    <h1>Welcome!</h1>
  </div>
</template>

<script>
import BookingService from '@/api-services/booking-service'
export default {
  name: 'Home',
  created () {
    BookingService.getAll(1).then((response) => {
      console.log(response.data)
    }).catch((error) => {
      console.log(error.response.data)
    })
  }
}
</script>

<style scoped>
.page {
min-height: 100vh;
}
</style>
