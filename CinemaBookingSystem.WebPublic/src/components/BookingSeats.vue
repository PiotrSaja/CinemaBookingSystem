<template>
  <div class="page">
    <div class="container text-white" style="margin-top: 100px">
      <div class="row">
        <div class="col-md-12">
          <b-alert v-model="showAlert" variant="danger" dismissible>
            {{ alertMessage }}
          </b-alert>
        </div>
      </div>
      <div class="row">
        <div class="col-md-6">
          <h2>{{ seance.movie.title}}</h2>
          Please select seats
        </div>
        <div class="col-md-6 text-right">
          <h5>{{ seance.date.replace('T', ' ')}}</h5>
          <h5>{{ seance.cinemaHall.name}}</h5>
        </div>
      </div>
      <div class="row mt-5">
        <div style="width: 30%"></div>
        <div class="screen" :style="{backgroundImage:`url(${seance.movie.backgroundImagePath})`}"></div>
        <div style="width: 30%"></div>
        <table class="mx-auto">
            <!--<tr>
              <th></th>
              <th class="pl-3 font-weight-normal">1</th>
              <th class="pl-3 font-weight-normal">2</th>
              <th class="pl-3 font-weight-normal">3</th>
              <th class="pl-3 font-weight-normal">4</th>
              <th class="pl-3 font-weight-normal">5</th>
            </tr>-->
            <tr v-for='index in 3' :key='index'>
              <!--<th class="pt-3 pr-5 font-weight-normal">Row: {{index}}</th>-->
                <td v-for="seat in seats.items"
                :key="seat.id" v-if='seat.cinemaSeat.row === index' class="pt-2" style="width: 30px">
                  <svg style="width:24px;height:24px; cursor: pointer" viewBox="0 0 24 24" v-if='seat.cinemaSeat.seatType === 1 && seat.seatStatus === false'>
                  <path :style="selectedIds.indexOf(seat.id) !== -1 ? { 'fill': '#FF9100' } : null" @click="onSeatSelected(seat.id)" class="can-select" fill="currentColor" d="M5 9.15V7C5 5.9 5.9 5 7 5H17C18.1 5 19 5.9 19 7V9.16C17.84 9.57 17 10.67 17 11.97V14H7V11.96C7 10.67 6.16 9.56 5 9.15M20 10C18.9 10 18 10.9 18 12V15H6V12C6 10.9 5.11 10 4 10S2 10.9 2 12V17C2 18.1 2.9 19 4 19V21H6V19H18V21H20V19C21.1 19 22 18.1 22 17V12C22 10.9 21.1 10 20 10Z" />
                  </svg>
                  <svg style="width:24px;height:24px" viewBox="0 0 24 24" v-else-if='seat.cinemaSeat.seatType === 1 && seat.seatStatus === true'>
                  <path :style="selectedIds.indexOf(seat.id) !== -1 ? { 'fill': '#FF9100' } : null" fill="gray" d="M5 9.15V7C5 5.9 5.9 5 7 5H17C18.1 5 19 5.9 19 7V9.16C17.84 9.57 17 10.67 17 11.97V14H7V11.96C7 10.67 6.16 9.56 5 9.15M20 10C18.9 10 18 10.9 18 12V15H6V12C6 10.9 5.11 10 4 10S2 10.9 2 12V17C2 18.1 2.9 19 4 19V21H6V19H18V21H20V19C21.1 19 22 18.1 22 17V12C22 10.9 21.1 10 20 10Z" />
                  </svg>
                  <svg style="width:24px;height:24px" viewBox="0 0 24 24" v-else-if='seat.seatStatus === true'>
                  <path :style="selectedIds.indexOf(seat.id) !== -1 ? { 'fill': '#FF9100' } : null" fill="gray" d="M4,18V21H7V18H17V21H20V15H4V18M19,10H22V13H19V10M2,10H5V13H2V10M17,13H7V5A2,2 0 0,1 9,3H15A2,2 0 0,1 17,5V13Z" />
                  </svg>
                  <svg style="width:24px;height:24px;cursor: pointer" viewBox="0 0 24 24" v-else>
                  <path :style="selectedIds.indexOf(seat.id) !== -1 ? { 'fill': '#FF9100' } : null" @click="onSeatSelected(seat.id)" class="can-select" fill="currentColor" d="M4,18V21H7V18H17V21H20V15H4V18M19,10H22V13H19V10M2,10H5V13H2V10M17,13H7V5A2,2 0 0,1 9,3H15A2,2 0 0,1 17,5V13Z" />
                  </svg>
                </td>
            </tr>
        </table>
      </div>
      <div class="row mt-5">
        <div class="col-md-6 col-6">
          <p>Choosen seats:{{selectedIds}}</p>
        </div>
        <div class="col-md-6 col-6 text-right">
          <b-button v-on:click="goToBookingUserInformation">Next step</b-button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import SeanceService from '@/api-services/seance-service'
import SeanceSeats from '@/api-services/seance-seats-service'
export default {
  name: 'BookingSeats',
  data () {
      return {
          seance: {},
          seats: {},
          errorMessage: '',
          isClicked: false,
          selectedIds: [],
          showAlert: false,
          alertMessage: 'Please select any seat!',
          lockData: {
            ShowSeatId: 0
          },
          ok: false,
          lockedIds: [],
          noLockedIds: []
      }
  },
  created () {
    SeanceService.get(this.$router.currentRoute.params.id).then((response) => {
      this.seance = response.data
    }).catch(error => {
      if (error.response.status === 404) {
      this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
      }
    })
    SeanceSeats.getBySeanceId(this.$router.currentRoute.params.id).then((response) => {
      this.seats = response.data
    }).catch(error => {
      if (error.response.status === 404) {
      this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
      }
    })
  },
  methods: {
    onSeatSelected (seatId) {
      if (this.selectedIds.indexOf(seatId) !== -1) {
        this.selectedIds.splice(this.selectedIds.indexOf(seatId), 1)
        } else if (this.selectedIds.length === 5) {
          this.alertMessage = 'You can select 5 seats on one reservation'
          this.showAlert = true
        } else {
        this.selectedIds.push(seatId)
      }
    },
    async goToBookingUserInformation () {
      this.lockedIds = []
      if (await this.checkSelectedSeats() === true) {
        this.$router.replace({name: 'BookingUserInformation', params: {seanceId: this.seance.id, seatsIds: this.selectedIds}})
      }
    },
    async checkSelectedSeats () {
        this.noLockedIds = []
        if (this.selectedIds.length === 0) {
          this.showAlert = true
          return false
        } else {
          return this.lockSeatsCommand()
        }
    },
    async lockSeatsCommand () {
      for (var i in this.selectedIds) {
        this.lockData.SeanceSeatId = this.selectedIds[i]
          await SeanceSeats.lock(this.lockData).then((response) => {
            this.ok = response.data
            if (this.ok) {
              this.lockedIds.push(this.lockData.ShowSeatId)
            } else {
              this.noLockedIds.push(this.lockData.ShowSeatId)
            }
          }).catch(error => {
            console.log(error)
          })
      }
      if (this.noLockedIds.length === 0) {
              return true
            } else {
              this.reloadSeatsForShow()
              this.selectedIds = this.lockedIds
              this.alertMessage = 'Please select other seats, probably someone choose them faster than you. Seat ids: ' + this.noLockedIds
              this.showAlert = true
              this.noLockedIds = []
            }
    },
    reloadSeatsForShow () {
      SeanceSeats.getByShowId(this.$router.currentRoute.params.id).then((response) => {
      this.seats = response.data
    }).catch(error => {
      if (error.response.status === 404) {
      this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
      }
    })
    }
  }
}
</script>

<style scoped lang="css">
.page {
min-height: 100vh;
}
.can-select:hover {
  fill: #4b4a49;
}
.screen {
  background-color: #fff;
  height: 235px;
  width: 40%;
  margin: 15px 0;
  transform: rotateX(-45deg);
  box-shadow: 0 3px 10px rgba(255, 255, 255, 0.75);
  margin: 0 auto;
  background-repeat: no-repeat;
  background-position: top center;
  background-size: cover;
}

</style>
