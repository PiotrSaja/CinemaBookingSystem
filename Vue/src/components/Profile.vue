<template>
  <div class="text-white page container">
    <div class="row mt-5">
      <h5 class="text-white underline font-weight-bold">Profile information</h5>
      <div class="col-md-12">
        <template v-if="profile !== null">
          <p>User name: {{ profile.email }}</p>
          Roles:
          <p>{{ profile.role }}</p>
        </template>
      </div>
    </div>
    <div class="row mt-4">
      <div class="col-md-12">
        <h5 class="text-white underline font-weight-bold">Last confirmed bookings</h5>
        <div class="row mt-4 booking-item"
        v-for="(booking, index) in userBookings.items"
        :key="booking.id"
        :booking="booking"
        @click="showBookingDetail(booking.id)"
        :style="{backgroundImage:`url(${booking.seance.movie.backgroundImagePath})`}"
        v-if='index < bookingsToShow'
        >
            <div class="col-md-5 col-12 auto">
              <div class="p-3 glass-effect">
                <h3>{{booking.seance.movie.title}}</h3>
                <span>{{booking.seance.cinemaHall.cinema.name}}</span><br>
                <span>{{booking.createdDate.substring(0,10)}}</span>
              </div>
            </div>
        </div>
        <hr>
        <div class="text-center mb-5" v-if="bookingsToShow < userBookings.items.length || userBookings.items.length > bookingsToShow">
          <b-button @click="bookingsToShow += 4" >Show more</b-button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BookingService from '@/api-services/booking-service'
export default {
  name: 'Profile',
  data () {
    return {
      profile: null,
      userBookings: {},
      bookingsToShow: 3,
      totalBookings: 0
    }
  },
  mounted () {
    this.$auth.getProfile()
      .then(profile => {
        this.profile = profile
        if (this.profile === null) {
          this.$router.replace({name: 'NotAuth', params: {err: 'User must be sign-in'}})
        }
      })
      .catch(error => {
        console.log(error)
        this.profile = {}
      })
  },
  created () {
    BookingService.getUserBookings().then((response) => {
    this.userBookings = response.data
  }).catch(error => {
    console.log(error)
  })
},
  methods: {
    showBookingDetail (bookingId) {
      this.$router.push({name: 'BookingDetail', params: {id: bookingId}})
      }
  }
}
</script>

<style scoped>
.page {
min-height: 100vh;
}
.booking-item {
  cursor: pointer;
  height: 200px;
  min-height: 200px;
  width: 100%;
  background-repeat: no-repeat;
  background-position: top center;
  background-size: cover;
}
.auto {
  margin-top: auto;
  margin-bottom: auto;
  color: white;
  text-shadow: 0 0 2px black;
}
.glass-effect{
  background-color: #ffffff10;
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border-radius: 10px;
}
.underline {
  margin-top: 15px;
  margin-bottom: 30px;
  text-align: center;
  width:150px;
  border-bottom: 2px solid #FF9100;
  position: relative;
  left: 50%;
  transform: translateX(-50%);
}
</style>
