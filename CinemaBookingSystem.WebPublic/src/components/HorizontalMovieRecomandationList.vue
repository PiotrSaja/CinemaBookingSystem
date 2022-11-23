<template>
  <div class="container text-white">
    <h5 class="text-white underline font-weight-bold">Recomendation</h5>
      <vue-horizontal-list-autoscroll :items="movies" :options="options">
        <template v-slot:default="{ item }">
          <movie-item :movieItem="item"></movie-item>
        </template>
      </vue-horizontal-list-autoscroll>
  </div>
</template>

<script>
import MovieService from '@/api-services/movie-service'
import VueHorizontalListAutoscroll from 'vue-horizontal-list-autoscroll'
import MovieItem from '@/components/MovieItem'
export default {
  props: ['selectedMovieId'],
  components: { VueHorizontalListAutoscroll, MovieItem },
  name: 'HorizontalMovieRecomendationList',
  data () {
    return {
      movies: [],
      movieId: -1,
      type: null,
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
    MovieService.getMoviesDetailPrediction(1, 12, this.selectedMovieId).then((response) => {
            this.movies = response.data.items
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
    }
  }
}
</script>

<style scoped>
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
