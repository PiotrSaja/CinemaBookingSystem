<template>
  <div class="page">
    <div class="row custom-margin-top">
        <div class="col-md-12">
          <b-alert v-if="!this.validate_errors.length && !this.booking_error_status" variant="success" show class="text-center">
            Seats was successful locked for 10 minutes.
          </b-alert>
          <b-alert v-if="this.validate_errors.length" variant="danger" show class="text-center">
            <b>Please correct the following error(s):</b>
            <ul>
              <li v-for="error in this.validate_errors" :key="error">{{ error }}</li>
            </ul>
          </b-alert>
          <b-alert v-if="this.booking_error_status" variant="danger" show class="text-center">
            Seats not locked in service. Please select new seats for reservation.
          </b-alert>
        </div>
      </div>
    <div class="row text-white">
      <div class="col-md-2">
          <span class="movie-label" v-if="seance.movie.imdbRating >= 7.5">Mega hit!</span>
          <img :src="seance.movie.posterPath" class="image-wrapper">
      </div>
      <div class="col-md-4">
          <h4>{{ seance.movie.title}}</h4>
          <h6>{{ seance.movie.imdbRating }}/10</h6>
          <h6><span v-for="genre in seance.movie.genres" :key="genre.id">{{ genre.name }}, </span></h6><br>
          <h6>{{ seance.movie.plot }}</h6>
        </div>
        <div class="col-md-2"></div>
          <div class="col-md-4">
          <h6>Hall: {{ seance.cinemaHall.name }}</h6><br>
          <h6>Start time: {{ seance.date.replace('T', ' : ').slice(0, -3) }}</h6><br>
          </div>
      <hr>
      </div>
      <div class="row text-white mt-5">
        <div class="col-md-6">
          <h4 class="text-center">User booking information</h4>
        </div>
        <div class="col-md-2">
        </div>
        <div class="col-md-4">
          <h4 class="text-center">Summary</h4>
        </div>
      </div>
      <form
      v-on:submit.prevent="validate"
      >
      <div class="row text-white mt-3">
        <div class="col-md-6">
          <div class="row mt-2">
            <div class="col-md-6">
            <label for="first_name">First name</label>
            </div>
            <div class="col-md-6">
            <b-form-input v-model="first_name" type="text" id="first_name" placeholder="Enter your first name" :disabled="input_disabled"></b-form-input>
            </div>
          </div>
          <div class="row mt-2">
            <div class="col-md-6">
              <label for="last_name">Last name</label>
            </div>
            <div class="col-md-6">
              <b-form-input v-model="last_name" type="text" id="last_name" placeholder="Enter your last name" :disabled="input_disabled"></b-form-input>
            </div>
          </div>
          <div class="row mt-2">
            <div class="col-md-6">
              <label for="phone_number">Phone number</label>
            </div>
            <div class="col-md-6">
               <b-form-input v-model="phone_number" type="tel" id="phone_number" placeholder="Enter your phone number" :disabled="input_disabled"></b-form-input>
            </div>
          </div>
          <div class="row mt-2">
            <div class="col-md-6">
              <label for="payment_method">Payment method:</label>
            </div>
            <div class="col-md-6">
               <b-form-select v-model="payment_method" id="payment_method" name="payment_method" :options="payment_options" :disabled="input_disabled">
               </b-form-select>
            </div>
          </div>
        </div>
        <div class="col-md-2"></div>
        <div class="col-md-4 summary-box">
          <ul class="pt-3 pl-5">
            <li v-for="seat in seance_seats" :key="seat.id">Row: {{ seat.cinemaSeat.seatRow }} Number: {{ seat.cinemaSeat.seatNumber }} - {{ seat.price }}$</li>
          </ul>
          <hr>
          <h5 class="text-center pb-3">Total price: {{ total_tickets_price }}$</h5>
        </div>
      </div>
      <div class="row">
        <div class="col-md-12 text-right">
          <b-button v-if="!confirm" class="mt-5 right" style="background-color: #FF9100" squared type="submit">Confirm inserted data and go to booking confirmation</b-button>
        </div>
      </div>
      </form>
        <div class="text-right">
          <b-button v-if="confirm" class="mt-5 right" style="background-color: #FF9100" squared @click="editFields">Edit input fields</b-button>
          <b-button v-if="confirm" class="mt-5 right" style="background-color: #FF9100" squared @click="createBooking">Pay via {{ payment_method }}</b-button>
        </div>
  </div>
</template>

<script>
import SeanceService from '@/api-services/seance-service'
import SeanceSeatsService from '@/api-services/seance-seats-service'
import BookingService from '@/api-services/booking-service'
export default {
  name: 'BookingUserInformation',
  props: {
    seanceId: {
      type: Number
  },
  seatsIds: {
      type: Array
  }
  },
  data () {
      return {
          seance: {},
          profile: null,
          first_name: null,
          last_name: null,
          phone_number: null,
          seance_seats: [],
          total_tickets_price: 0,
          validate_errors: [],
          payment_method: null,
          payment_options: [
          { value: null, text: 'Please select an payment method' },
          { value: 'paypal', text: 'PayPal', disabled: true },
          { value: 'payu', text: 'PayU', disabled: true },
          { value: 'cash', text: 'Cash in cinema', disabled: false }
          ],
          confirm: false,
          input_disabled: false,
          booking: {
            firstName: '',
            lastName: '',
            phoneNumber: '',
            showId: 0,
            showSeatIds: []
          },
          booking_error_status: false
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
      SeanceService.get(this.seanceId).then((response) => {
      this.seance = response.data
    }).catch(error => {
      console.log(error)
    })
    for (var id in this.seatsIds) {
      SeanceSeatsService.getDetail(this.seatsIds[id]).then((response) => {
        this.seance_seats.push(response.data)
        this.total_tickets_price += response.data.price
      }).catch(error => {
        console.log(error)
      })
    }
  },
  methods: {
    validate () {
      if (this.first_name && this.last_name && this.phone_number && this.payment_method) {
        this.confirm = true
        this.input_disabled = true
        return true
      }

      this.validate_errors = []

      if (!this.first_name) {
        this.validate_errors.push('First name required.')
      }
      if (!this.last_name) {
        this.validate_errors.push('Last name required.')
      }
      if (!this.phone_number) {
        this.validate_errors.push('Phone number required.')
      }
      if (!this.payment_method) {
        this.validate_errors.push('Payment method required.')
      }
    },
    editFields () {
      this.input_disabled = !this.input_disabled
      this.confirm = !this.confirm
    },
    async createBooking () {
      if (!this.confirm) {
        return false
      } else {
        this.booking.firstName = this.first_name
        this.booking.lastName = this.last_name
        this.booking.phoneNumber = this.phone_number
        this.booking.seanceId = this.seance.id
        this.booking.seanceSeatIds = this.seatsIds
        await BookingService.create(this.booking).then((response) => {
            console.log(response.data)
            this.$router.replace({name: 'BookingConfirmation', params: {id: response.data.bookingId}})
          }).catch(error => {
            this.booking_error_status = true
            console.log(error)
          })
      }
    }
}
}
</script>

<style scoped lang="css">
.page {
min-height: 100vh;
}
.image-wrapper {
  height:240px;
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
 .movie-label{
    color: #fff;
    background: #FF5555;
    font-size: 13px;
    text-transform: uppercase;
    padding: 2px 6px;
    border-radius: 3px;
    position: absolute;
    left: 50%;
    transform: translateX(-50%);
    top: -3px;
}
.summary-box{
  box-shadow: rgba(149, 157, 165, 0.2) 0px 8px 24px;
  background-color: gray;
  border-radius: 25px;
}

.custom-margin-top {
  margin-top: 50px
}

@media only screen and (min-width: 1px) and (max-width: 576px) {
.movie-label{
  color: #fff;
  background: #FF5555;
  font-size: 13px;
  text-transform: uppercase;
  padding: 2px 6px;
  border-radius: 3px;
  position: absolute;
  left: 5%;
  transform: translateX(-5%);
  top: -3px;
}
.custom-margin-top {
  margin-top: 10px
}
}
</style>
