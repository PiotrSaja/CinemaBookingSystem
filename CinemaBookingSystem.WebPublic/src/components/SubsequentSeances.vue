<template>
  <div class="container text-white" v-if="todaySeances.length > 0 || tomorrowSeances.length > 0 || dayAfterTomorrowSeances.length > 0">
    <h5 class="underline font-weight-bold">Upcoming seances</h5>
    <h6 class="text-center">{{cinema.name}}, {{cinema.city}}</h6>
    <div class="row mt-4" v-if="todaySeances.length > 0">
      <h6>{{this.mockTime.slice(0, 10)}}</h6>
    </div>
    <div class="row mt-1" v-if="todaySeances.length > 0">
        <div class="mr-2 mt-3 show-time"
        v-for="seance in todaySeances"
        :key="seance.id"
        :seance="seance"
        @click="onSeanceClicked(seance.id)">{{ seance.date.split('T').pop().substring(0, 5) }}</div>
    </div>
    <div class="row mt-4" v-if="tomorrowSeances.length > 0">
      <h6>{{this.mockTime2.slice(0, 10)}}</h6>
    </div>
    <div class="row mt-1">
        <div class="mr-2 mt-3 show-time"
        v-for="seance in tomorrowSeances"
        :key="seance.id"
        :seance="seance"
        @click="onSeanceClicked(seance.id)">{{ seance.date.split('T').pop().substring(0, 5) }}</div>
    </div>
    <div class="row mt-4" v-if="dayAfterTomorrowSeances.length > 0">
      <h6>{{this.mockTime3.slice(0, 10)}}</h6>
    </div>
    <div class="row mt-1">
        <div class="mr-2 mt-3 show-time"
        v-for="seance in dayAfterTomorrowSeances"
        :key="seance.id"
        :seance="seance"
        @click="onSeanceClicked(seance.id)">{{ seance.date.split('T').pop().substring(0, 5) }}</div>
    </div>
  </div>
</template>

<script>
import SeanceService from '@/api-services/seance-service'
import CinemaService from '@/api-services/cinema-service'
export default {
  name: 'SubsequentSeances',
  data () {
    return {
      todaySeances: [],
      tomorrowSeances: [],
      dayAfterTomorrowSeances: [],
      selectedCinema: 1,
      cinema: {},
      mockTime: '2022-01-04T00:00',
      mockTime2: '2022-01-05T00:00',
      mockTime3: '2022-01-06T00:00'
    }
  },
  methods: {
    showSeanceDetail (seanceId) {
        this.$router.push({name: 'SeanceDetail', params: {id: seanceId}})
      },
    onSeanceClicked (seanceId) {
      this.showSeanceDetail(seanceId)
    }
  },
  mounted () {
    if (localStorage.selectedCinema) {
      this.selectedCinema = parseInt(localStorage.selectedCinema)
    }
    CinemaService.get(this.selectedCinema).then((response) => {
      this.cinema = response.data
    }).catch((error) => {
      console.log(error.response.data)
    })
    SeanceService.getSeanceOfCurrentMovieOnGivenCinemaAndDay(this.$router.currentRoute.params.id, this.selectedCinema, this.mockTime).then((response) => {
      this.todaySeances = response.data.items
    }).catch((error) => {
      console.log(error.response.data)
    })
    SeanceService.getSeanceOfCurrentMovieOnGivenCinemaAndDay(this.$router.currentRoute.params.id, this.selectedCinema, this.mockTime2).then((response) => {
      this.tomorrowSeances = response.data.items
    }).catch((error) => {
      console.log(error.response.data)
    })
    SeanceService.getSeanceOfCurrentMovieOnGivenCinemaAndDay(this.$router.currentRoute.params.id, this.selectedCinema, this.mockTime3).then((response) => {
      this.dayAfterTomorrowSeances = response.data.items
    }).catch((error) => {
      console.log(error.response.data)
    })
  }
}
</script>

<style scoped>
.show-time {
    background-color: #FF9100;
    border-radius: 10px;
    padding: 5px;
    cursor: pointer;
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
