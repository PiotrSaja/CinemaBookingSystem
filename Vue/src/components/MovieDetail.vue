<template>
    <div class="page">
        <div class="movie-background-image" :style="{backgroundImage:`url(${movie.backgroundImagePath})`}">
        </div>
        <div class="container" style="margin-top: -120px">
            <div class="row">
                <div class="col-xl-2 col-lg-3 col-md4 col-12">
                <img :src="movie.posterPath" class="image-wrapper">
                </div>
                <div class="col-xl-4 col-lg-4 col-md-4 col-6 movie-detail-text">
                <h2 class="ml ml-3 text-white font-weight-bold">{{ movie.title }}</h2>
                <h4 class="ml ml-3 text-white" v-if="movie.imdbRating !== 'N/A'"><img src="https://upload.wikimedia.org/wikipedia/commons/6/69/IMDB_Logo_2016.svg" alt="W3Schools.com" height="20px"> {{movie.imdbRating}}/10</h4>
                <span class="ml ml-3 text-white font-weight-bold">Director</span><br>
                <span class="ml ml-3 text-white font-weight-regular">{{movie.director.fullName}}</span>
                </div>
                <div class="col-xl-6 col-lg-5 col-md-4 col-6 movie-detail-text" style="text-align: right;">
                <span class="ml-3 text-white font-weight-bold">Released date</span><br>
                <span class="ml-3 text-white font-weight-regular">{{ movie.released | truncate(10) }}</span><br><br>
                <span class="ml-3 text-white font-weight-bold">Genre</span><br>
                <span class="ml-3 text-white font-weight-regular"><span v-for="genre in movie.genres" :key="genre.id">{{ genre.name }}, </span></span>
                </div>
                <div class="row" style="padding-top: 30px;">
                <div class="col-md-12 col-12">
                    <span class="ml-3 text-white font-weight-bold">Actors</span><br>
                    <p class="ml-3 text-white font-weight-regular"><span v-for="actor in movie.actors" :key="actor.id">{{ actor.fullName }}, </span>
                    </p><br><br>
                    <span class="ml-3 text-white font-weight-bold">Plot</span><br>
                    <p class="ml-3 text-white font-weight-regular">{{ movie.plot }}</p><br><br>
                </div>
                </div>
            </div>
            <div class="row">
              <subsequent-seances/>
            </div>
            <div class="row">
              <horizontal-movie-recomandation-list/>
            </div>
        </div>
    </div>
</template>

<script>
import MovieService from '@/api-services/movie-service'
import HorizontalMovieRecomandationList from '@/components/HorizontalMovieRecomandationList.vue'
import SubsequentSeances from '@/components/SubsequentSeances.vue'
export default {
  components: { HorizontalMovieRecomandationList, SubsequentSeances },
  name: 'MovieDetail',
  data () {
    return {
      movie: {},
      errorMessage: ''
    }
  },
  created () {
    MovieService.get(this.$router.currentRoute.params.id).then((response) => {
      this.movie = response.data
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
  background-position: top center;
  background-size: cover;
}
.page {
min-height: 100vh;
}
</style>
