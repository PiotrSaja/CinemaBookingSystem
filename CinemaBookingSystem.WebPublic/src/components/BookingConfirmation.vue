<template>
<div class="container page">
    <div class="row">
        <div class="xs-12 md-6 mx-auto">
            <div id="text-container">
                <div class="number">Booking Confirmation</div>
                <div class="text">Thank you for your reservation</div>
                <div class="text">Payment status: {{ booking.bookingStatus}}</div>
                <div class="text" v-if="this.id != null">Booking number: {{this.id}}</div>
            </div>
        </div>
    </div>
</div>
</template>

<script>
import BookingService from '@/api-services/booking-service'
export default {
  name: 'BookingConfirmation',
  props: {
    id: {
      type: Number
  }
  },
  data () {
      return {
        booking: {}
      }
  },
  created () {
      BookingService.getUserBooking(this.id).then((response) => {
      this.booking = response.data
    }).catch(error => {
      console.log(error)
    })
  }
}
</script>

<style scoped>
p {
    font-weight: bold;
    font-size: 50px;
    text-align: center;
    color: #ffffff;
}
.page {
min-height: 100vh;
min-height: 100vh;
color: rgba(255,255,255,.87);
}

.mx-auto {
    margin-left: auto;
    margin-right: auto;
}
#text-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
}
#text-container {
  margin-top: 120px;
}
#text-container .number {
  font-size: 3rem;
  font-weight: 500;
}
#text-container .text {
      margin: 0 0 1rem;
}
#text-container .text {
  font-weight: 300;
  text-align: center;
}
</style>
