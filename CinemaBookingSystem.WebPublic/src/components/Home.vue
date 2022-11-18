<template>
  <div class="container page text-white">
    <movie-carousel/>
    <div class="container">
      <div class="row mt-3">
        <div class="col-md-12">
          <h5 class="text-white underline font-weight-bold">Premieres</h5>
          <vue-horizontal-list-autoscroll :items="movies" :options="options">
            <template v-slot:default="{ item }">
              <div class="movie-grid" @click="onMovieClicked(item.id)">
                <div class="image-container">
                  <div class="movie-image">
                    <a href="" class="image">
                      <img class="pic-1" :src="item.posterPath" />
                    </a>
                    <span class="movie-label" v-if="item.imdbRating >= 7.5">Mega hit!</span>
                  </div>
                  <div class="movie-content">
                    <h3 class="title"><a href="#">{{ item.title }}</a></h3>
                  </div>
                </div>
              </div>
            </template>
          </vue-horizontal-list-autoscroll>
        </div>
      </div>
      <div class="row mt-3 mb-3" v-if="soonMovies.length > 0">
        <div class="col-md-12">
          <h5 class="text-white underline font-weight-bold">Comming soon</h5>
          <vue-horizontal-list-autoscroll :items="soonMovies" :options="options">
            <template v-slot:default="{ item }">
              <div class="movie-grid" @click="onMovieClicked(item.id)">
                <div class="image-container">
                  <div class="movie-image">
                    <a href="" class="image">
                      <img class="pic-1" :src="item.posterPath" />
                    </a>
                    <span class="movie-label-soon" v-if="substractMovieDate(item.releasedDate) <= 14">Soon</span>
                  </div>
                  <div class="movie-content">
                    <h3 class="title"><a href="#">{{ item.title }}</a></h3>
                  </div>
                </div>
              </div>
            </template>
          </vue-horizontal-list-autoscroll>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import VueResizeText from 'vue-resize-text'
import moment from 'moment'
import MovieService from '@/api-services/movie-service'
import MovieCarousel from '@/components/MovieCarousel'
import VueHorizontalListAutoscroll from 'vue-horizontal-list-autoscroll'
export default {
  components: { MovieCarousel, VueHorizontalListAutoscroll, VueResizeText },
  name: 'Home',
  data () {
    return {
      movies: [],
      soonMovies: [],
      options: {
        autoscroll: {
          enabled: true,
          interval: 30000,
          repeat: true
        },
        slideshow: {
          enabled: true,
          interval: 5000,
          repeat: false
        },
        list: {
          // css class for the parent of item
          class: '',
          // maximum width of the list it can extend to before switching to windowed mode, basically think of the bootstrap container max-width
          // windowed is used to toggle between full-screen mode and container mode
          windowed: 100,
          // padding of the list, if container < windowed what is the left-right padding of the list
          // during full-screen mode the padding will added to left & right to centralise the item
          padding: 24
        },
        responsive: [
          {end: 576, size: 2},
          {start: 576, end: 768, size: 2},
          {start: 768, end: 992, size: 3},
          {start: 992, end: 1200, size: 4},
          {start: 1200, size: 5}
        ],
        navigation: {
          start: 992,
          color: '#FF9100'
        }
      }
    }
  },
  created () {
    MovieService.getAllSoon(1, 100, -1).then((response) => {
      this.movies = response.data.items
    }).catch((error) => {
      console.log(error.response.data)
    })
    MovieService.getAllSoon(1, 100, 1).then((response) => {
      this.soonMovies = response.data.items
    }).catch((error) => {
      console.log(error.response.data)
    })
  },
  methods: {
    showMovieDetail (movieId) {
        this.$router.push({name: 'MovieDetail', params: {id: movieId}})
      },
    onMovieClicked (movieId) {
      this.showMovieDetail(movieId)
    },
    substractMovieDate (date) {
      let movieDate = moment(date)
      let currentTime = moment()
      return movieDate.diff(currentTime, 'days')
    }
  }
}
</script>

<style scoped>
.movie-grid{
    font-family: 'Montserrat', sans-serif;
    text-align: center;
    margin: 20px 15px 0;
    border-radius: 10px;
    box-shadow:  0 0 3px rgba(0,0,0,0.1);
    transition: all 0.5s;
}
.movie-grid:hover{
  box-shadow:  0 0 30px rgba(0,0,0,0.4);
  transform: scale(1.05);
}
.movie-grid .movie-image img{
    width: 100%;
    height: 300px;
}
@media only screen and (min-width: 1px) and (max-width: 576px) {
  .movie-grid {
    font-family: 'Montserrat', sans-serif;
    text-align: center;
    margin: 15px 0px 0;
    border-radius: 10px;
    box-shadow:  0 0 3px rgba(0,0,0,0.1);
    transition: all 0.5s;
  }
  .movie-grid .movie-image img{
    width: 100%;
    height: 215px;
}
.container, .container-fluid, .container-lg, .container-md, .container-sm, .container-xl /deep/{
    padding-right: 0px;
    padding-left: 0px;
    margin-right: auto;
    margin-left: auto;
  }
}
.movie-grid .movie-image{ position: relative; }
.movie-grid .movie-image a.image{
    border-radius: 10px 10px 0 0;
    overflow: hidden;
    display: block;
}
.movie-grid .movie-label{
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
.movie-grid .movie-label-soon{
    color: #fff;
    background: #55c9ff;
    font-size: 13px;
    text-transform: uppercase;
    padding: 2px 6px;
    border-radius: 3px;
    position: absolute;
    left: 50%;
    transform: translateX(-50%);
    top: -3px;
}
.movie-grid .movie-content{
    background-color: #fff;
    text-align: left;
    padding: 5px 5px;
    border-top: 1px solid transparent;
    border-radius: 0 0 10px 10px;
    transition: all 0.3s;
    min-height: 67px;
    display: flex;
    justify-content: center; /* align horizontal */
    align-items: center; /* align vertical */
}
.movie-grid:hover .movie-content{ border-top-color: #dbdbdb; }
.movie-grid .title{
    text-align: center;
    font-size: 15.5px;
    font-weight: bold;
    text-transform: capitalize;
    margin: 0;
}
.movie-grid .title a{
    color: #323b45;
    transition: all 0.4s ease-out;
}
.movie-grid .title a:hover{ color: #FF9100; }
@media screen and (min-width: 577px) and (max-width:990px){
    .movie-grid{ margin: 5px 15px 30px; }
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
  padding-bottom:10px
}
.page {
min-height: 100vh;
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
