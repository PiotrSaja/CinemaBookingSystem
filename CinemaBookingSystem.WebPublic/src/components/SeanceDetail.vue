<template>
  <div class="page">
      <div class="row text-white custom-margin-top">
      <div class="col-xl-2 col-lg-3">
          <span class="movie-label" v-if="seance.movie.imdbRating >= 7.5">Mega hit!</span>
          <img :src="seance.movie.posterPath" class="image-wrapper">
      </div>
      <div class="col-xl-4 col-lg-4">
          <h4>{{ seance.movie.title}}</h4>
          <h6>{{ seance.movie.imdbRating }}/10</h6>
          <h6><span v-for="(genre,index) in seance.movie.genres" :key="genre.id">{{ genre.name }}<span v-if="index !== seance.movie.genres.length-1">, </span>
                    </span></h6><br>
          <h6>{{ seance.movie.plot }}</h6>
          </div>
          <div class="col-xl-2 col-lg-1"></div>
          <div class="col-xl-4 col-lg-4">
          <h6>{{ seance.cinemaHall.name }}</h6><br><br>
          <h6>{{ seance.date.replace('T', ' : ').slice(0, -3) }}</h6><br>
          <b-button class="mt-3" @click="onGoToBookingClicked(seance.id)">Go to booking</b-button>
          </div>
      </div>
      <hr>
    </div>
</template>

<script>
import SeanceService from '@/api-services/seance-service'
export default {
  name: 'SeanceDetail',
  data () {
      return {
          seance: {},
          errorMessage: ''
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
  },
  methods: {
    showBookingSeats (showId) {
      this.$router.push({name: 'BookingSeats', params: {id: showId}})
      },
    onGoToBookingClicked (showId) {
      this.showBookingSeats(showId)
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

.custom-margin-top {
  margin-top: 100px
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
