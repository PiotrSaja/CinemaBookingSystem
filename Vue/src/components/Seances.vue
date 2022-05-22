<template>
  <div class="page">
        <div class="container">
            <div class="row text-white mt-5">
                <div class="col-md-9 col-12">
                    <v-select
                    class="style-chooser"
                    placeholder="Choose a Cinema"
                    style="width:250px"
                    :options="cinemas.items"
                    v-model="selectedCinema"
                    label="name"
                    id="id"
                    :reduce="(cinema) => cinema.id"
                    @input="fetchMoviesWithShows()"
                    ></v-select>
                </div>
                <div class="col-md-1 col-4 text-center">
                    <a href="#">Price list</a>
                </div>
                <div class="col-md-1 col-4 text-center">
                    <a href="#">Map</a>
                </div>
                <div class="col-md-1 col-4 text-center">
                    <a href="#">Info</a>
                </div>
            </div>
            <div class="row">
            </div>
            <div class="container text-white mt-5">
                <div class="row">
                    <div class="col-md-3 col-3 d-none d-sm-block font-weight-bold">Now showing</div>
                    <div class="col-md-6 col-12 col-sm-12 text-center">
                      <b-button class="mr-2" @click="prevDay()" style="border-radius: 3px">&lt;</b-button>
                      {{currentDateString}}
                      <b-button class="ml-2" @click="nextDay()" style="border-radius: 3px">&gt;</b-button>
                      </div>
                    <div class="col-md-3 col-12 col-sm-12 text-right">
                        <b-button v-b-toggle.collapse-3>Filter</b-button>
                    </div>
                </div>
                <div class="row mt-3">
                  <div class="col-md-12 text-dark">
                    <b-collapse id="collapse-3">
                          <b-card>
                            <div>
                              <h5 class="text-center">Filtering</h5>
                              <v-select
                              class="style-chooser"
                              placeholder="Order by"
                              style="width:100%"
                              :options="sortOptions"
                              v-model="sortType"
                              label="name"
                              id="value"
                              @input="sortBy(sortType.value)"
                              ></v-select>
                            </div>
                          </b-card>
                    </b-collapse>
                  </div>
                </div>
            </div>
            <hr>
            <div v-for="movie in moviesWithSeances.items"
                :key="movie.id"
                :movie="movie">
                <div class="row text-white mt-4">
                <div class="col-lg-3 col-xl-2">
                    <span class="movie-label" v-if="movie.imdbRating >= 7.5">Mega hit!</span>
                    <img :src="movie.posterPath"  class="image-wrapper" style="cursor: pointer;" @click="onMovieClicked(movie.id)">
                </div>
                <div class="col-lg-4 col-xl-4">
                    <h4 @click="onMovieClicked(movie.id)" style="cursor: pointer;">{{ movie.title}}</h4>
                    <h6>IMDb: {{ movie.imdbRating }}/10</h6>
                    <h6>{{ movie.duration }} min.</h6><br>
                    <h6>{{ movie.plot }}</h6>
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
    </div>
</template>

<script>
import 'vue-select/dist/vue-select.css'
import CinemaService from '@/api-services/cinema-service'
import MovieService from '@/api-services/movie-service'
export default {
  name: 'Seances',
  data () {
      return {
          cinemas: {},
          moviesWithSeances: {},
          selectedCinema: 1,
          currentDate: 0,
          currentDateString: '',
          sortType: 'Sort By',
          sortOptions: [
          { name: 'Sort by: Title', value: 'title' },
          { name: 'Sort by: Genre', value: 'genre' },
          { name: 'Sort by: Imdb Rating', value: 'imdbRating' }
       ]
      }
  },
  created () {
    CinemaService.getAll().then((response) => {
      this.cinemas = response.data
    }).catch((error) => {
      console.log(error.response.data)
    })
    var mockTime = '2022-01-04T00:00'
    this.currentDate = new Date(mockTime)
    this.currentDateString = this.DateToString(this.currentDate)
    this.fetchMoviesWithShows()
  },
  methods: {
      fetchMoviesWithShows () {
          MovieService.getMoviesWithSeances(this.selectedCinema, (this.currentDateString + 'T00:00:00')).then((response) => {
            this.moviesWithSeances = response.data
        })
      },
      DateToString (current) {
      const date = `${current.getFullYear()}-${current.getMonth() + 1}-${current.getDate()}`
      return date
    },
    nextDay () {
      var mockTime = '2022-01-04T00:00'
      var date = new Date(mockTime).getDate()
      if ((date + 11) > this.currentDate.getDate()) {
        this.currentDate.setDate(this.currentDate.getDate() + 1)
        this.currentDateString = this.DateToString(this.currentDate)
        this.fetchMoviesWithShows()
      }
    },
    prevDay () {
      var mockTime = '2022-01-04T00:00'
      var date = new Date(mockTime).getDate()
      if (date < this.currentDate.getDate()) {
      this.currentDate.setDate(this.currentDate.getDate() - 1)
      this.currentDateString = this.DateToString(this.currentDate)
      this.fetchMoviesWithShows()
      }
    },
    showMovieDetail (movieId) {
      this.$router.push({name: 'MovieDetail', params: {id: movieId}})
      },
    onMovieClicked (movieId) {
      this.showMovieDetail(movieId)
      },
    showSeanceDetail (seanceId) {
      this.$router.push({name: 'SeanceDetail', params: {id: seanceId}})
    },
    onSeanceClicked (seanceId) {
      this.showSeanceDetail(seanceId)
    },
    sortBy (prop) {
      console.log(prop)
      this.moviesWithSeances.items.sort((a, b) => a[prop] < b[prop] ? -1 : 1)
    }
  },
  mounted () {
    if (localStorage.selectedCinema) {
      this.selectedCinema = parseInt(localStorage.selectedCinema)
      this.fetchMoviesWithShows()
    }
  },
  watch: {
    selectedCinema (newSelectedCinema) {
      localStorage.selectedCinema = newSelectedCinema
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
.style-chooser /deep/ .vs__search::placeholder,
.style-chooser /deep/ .vs__dropdown-toggle,
.style-chooser /deep/ .vs__dropdown-menu {
  background: white;
  border: none;
  color: #394066;
  text-transform: lowercase;
  font-variant: small-caps;
}

.style-chooser /deep/ .vs__clear,
.style-chooser /deep/ .vs__open-indicator {
  fill: #394066;
}
.show-time {
    background-color: #FF9100;
    border-radius: 10px;
    padding: 5px;
    cursor: pointer;
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
</style>
