<template>
    <div class="page">
        <div class="movie-background-image" :style="{backgroundImage:`url(${booking.seance.movie.backgroundImagePath})`}">
        </div>
        <div class="container" style="margin-top: -120px">
            <div class="row">
                <div class="col-xl-2 col-lg-3 col-md4 col-12">
                <img :src="booking.seance.movie.posterPath" class="image-wrapper">
                </div>
                <div class="col-xl-4 col-lg-4 col-md-4 col-6 movie-detail-text">
                <h2 class="ml ml-3 text-white font-weight-bold">{{ booking.seance.movie.title }}</h2>
                <span class="ml ml-3 text-white font-weight-bold">Booking number</span><br>
                <span class="ml ml-3 text-white font-weight-regular">{{booking.bookingId}}</span><br>
                <span class="ml ml-3 text-white font-weight-bold">Number of seats</span><br>
                <span class="ml ml-3 text-white font-weight-regular">{{booking.numberOfSeats}}</span><br>
                </div>
                <div class="col-xl-6 col-lg-5 col-md-4 col-6 movie-detail-text" style="text-align: right;">
                <span class="ml-3 text-white font-weight-bold">Seance time</span><br>
                <span class="ml-3 text-white font-weight-regular">{{ booking.seance.date | truncate(10) }}</span><br><br>
                <span class="ml-3 text-white font-weight-bold">Cinema</span><br>
                <span class="ml-3 text-white font-weight-regular">{{ booking.seance.cinemaHall.cinema.name }}, {{ booking.seance.cinemaHall.cinema.city}}</span><br>
                <span class="ml-3 text-white font-weight-regular">{{ booking.seance.cinemaHall.name }}</span>
                </div>
              </div>
              <div class="row" style="padding-top: 30px;">
                <div class="col-md-12 col-12">
                  <span class="text-white font-weight-bold">Seats</span><br>
                </div>
              </div>
              <div v-for="seat in booking.seanceSeats"
                  :key="seat.id" class="row text-white pb-2">
                  <div class="col-12 seat-item">
                    <div class="row">
                      <div class="col-8">
                      Type: <span v-if="seat.cinemaSeat.seatType === 0">Normal</span>
                                    <span v-else>VIP</span><br>
                      </div>
                      <div class="col-4">
                      Row: {{seat.cinemaSeat.row}}<br>
                      Seat Number: {{seat.cinemaSeat.seatNumber}}<br>
                      </div>
                    </div>
                  </div>
              </div>
        </div>
    </div>
</template>

<script>
import BookingService from '@/api-services/booking-service'
export default {
  name: 'BookingDetail',
  data () {
    return {
      booking: {},
      errorMessage: ''
    }
  },
  created () {
    BookingService.getUserBooking(this.$router.currentRoute.params.id).then((response) => {
      this.booking = response.data
    }).catch(error => {
      if (error.response.status === 404) {
      this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
      }
    })
  },
  filters: {
        truncate: function (data, num) {
            const reqdString = data.toString().split('').slice(0, num).join('')
            return reqdString
        }
    }
}
</script>

<style scoped>
.image-wrapper {
  height:280px;
  display:flex;
  flex-direction:column;
  justify-content:center;
  text-align:center;
  box-shadow:  0 0 3px rgba(0,0,0,0.3);
 }
 .image-wrapper img {
  max-height:100px;
  max-width:100%;
 }
 .movie-detail-text{
  margin-top: 140px;
 }
@media only screen and (min-width: 1px) and (max-width: 990px) {
  .movie-detail-text {
    margin-top: 30px;
  }
}
@media only screen and (min-width: 1px) and (max-width: 576px) {
  .ml {
    margin-left: 0 !important;
  }
}
.movie-background-image{
  height: 350px;
  min-height: 350px;
  width: 100%;
  background-repeat: no-repeat;
  background-position: center center;
  background-size: cover;
}
.page {
min-height: 100vh;
}
.seat-item {
  background-color: gray;
  border-radius: 3px;
  padding-top: 10px;
  padding-bottom: 10px
}
</style>
