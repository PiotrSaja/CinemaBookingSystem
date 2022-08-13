<template>
  <div class="container text-white" v-if="todaySeances.items.length > 0">
    <h5 class="underline font-weight-bold">Today in cinema</h5>
    <div class="row mt-4">
    </div>
    <div v-for="movie in todaySeances.items"
                :key="movie.id"
                :movie="movie">
                <div class="row text-white mt-4">
                <div class="col-lg-3 col-xl-2">
                    <span class="movie-label" v-if="movie.imdbRating >= 7.5">Mega hit!</span>
                    <img :src="movie.posterPath"  class="image-wrapper" style="cursor: pointer;" @click="onMovieClicked(movie.id)">
                </div>
                <div class="col-lg-4 col-xl-4">
                    <h4 @click="onMovieClicked(movie.id)" style="cursor: pointer;">{{ movie.title}}</h4>
                    <h6>{{ movie.genre }}</h6><br>
                    <h6>IMDb: {{ movie.imdbRating }}/10</h6>
                    <h6>Duration: {{ movie.duration }} min.</h6>
                </div>
                    <div class="col-lg-1 col-xl-2"></div>
                    <div class="col-lg-4 col-xl-4">
                        <div class="container">
                            <div class="row mt-4">
                                <div class="ml-2 mt-3 show-time"
                                v-for="seance in movie.seances"
                                :key="seance.id"
                                :seance="seance"
                                @click="onSeanceClicked(seance.id)">{{ seance.date.split('T').pop().substring(0, 5) }}</div>
                            </div>
                        </div>
                    </div>
                </div>
                <hr>
            </div>
  </div>
</template>

<script>
import MovieService from '@/api-services/movie-service'
export default {
  name: 'SubsequentSeancesInCinema',
  data () {
    return {
      todaySeances: {},
      mockTime: '2022-01-04T00:00'
    }
  },
  created () {
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
    MovieService.getMoviesWithSeances(this.$router.currentRoute.params.id, this.mockTime).then((response) => {
      this.todaySeances = response.data
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
@media (min-width:0px) and (max-width:767px){
    .movie-label{
      color: #fff;
      background: #FF5555;
      font-size: 13px;
      text-transform: uppercase;
      padding: 2px 6px;
      border-radius: 3px;
      position: absolute;
      left: 19%;
      transform: translateX(-50%);
      top: -3px;
}
}
@media (min-width:768px) and (max-width:991px){
    .movie-label{
      color: #fff;
      background: #FF5555;
      font-size: 13px;
      text-transform: uppercase;
      padding: 2px 6px;
      border-radius: 3px;
      position: absolute;
      left: 14%;
      transform: translateX(-50%);
      top: -3px;
}
}
@media (min-width:992px) and (max-width:1199px){
    .movie-label{
      color: #fff;
      background: #FF5555;
      font-size: 13px;
      text-transform: uppercase;
      padding: 2px 6px;
      border-radius: 3px;
      position: absolute;
      left: 41%;
      transform: translateX(-50%);
      top: -3px;
}
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
</style>
