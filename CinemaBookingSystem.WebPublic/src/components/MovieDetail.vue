<template>
    <div class="page">
        <b-alert v-model="showDismissibleAlert" variant="success" dismissible>
          Thanks you for voting!
        </b-alert>
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
                <span class="ml-3 text-white font-weight-regular">
                  <span v-for="(genre,index) in movie.genres" :key="genre.id">{{ genre.name }}<span v-if="index !== movie.genres.length-1">, </span>
                    </span>
                </span>
                </div>
                <div class="row" style="padding-top: 30px;">
                <div class="col-md-12 col-12">
                    <div v-if="ratingVisable">
                      <span class="ml-3 text-white font-weight-bold" v-if="ratingDisabled">Your rate</span>
                      <span class="ml-3 text-white font-weight-bold" v-else>Please rate the movie</span>
                      <star-rating class="ml-3"
                      :increment="1"
                      :star-size="25"
                      :show-rating="false"
                      :rating = voteRating
                      @rating-selected = "setRating"
                      :read-only="ratingDisabled"></star-rating><br>
                    </div>
                    <span class="ml-3 text-white font-weight-bold">Actors</span><br>
                    <p class="ml-3 text-white font-weight-regular">
                      <span v-for="(actor, index) in movie.actors" :key="actor.id">{{ actor.fullName }}<span v-if="index !== movie.actors.length-1">, </span>

                      </span>
                    </p><br><br>
                    <span class="ml-3 text-white font-weight-bold">Plot</span><br>
                    <p class="ml-3 text-white font-weight-regular">{{ movie.plot }}</p><br><br>
                </div>
                </div>
            </div>
            <div class="row">
              <subsequent-seances/>
            </div>
            <div class="row" v-if="movieRecomendationVisable">
              <horizontal-movie-recomandation-list :selectedMovieId="movie.id"/>
            </div>
        </div>
    </div>
</template>

<script>
import StarRating from 'vue-star-rating'
import MovieService from '@/api-services/movie-service'
import HorizontalMovieRecomandationList from '@/components/HorizontalMovieRecomandationList.vue'
import SubsequentSeances from '@/components/SubsequentSeances.vue'
import RecommendationService from '@/api-services/recommendation-service'
export default {
  components: { HorizontalMovieRecomandationList, SubsequentSeances, StarRating },
  name: 'MovieDetail',
  data () {
    return {
      movie: {
        actors: {
          fullName: '',
          id: 0
        },
        backgroundImagePath: '',
        country: '',
        director: {
          fullName: '',
          id: 0
        },
        duration: 0,
        genres: null,
        id: 0,
        imdbRating: '',
        language: '',
        plot: '',
        posterPath: '',
        released: '',
        title: ''
      },
      errorMessage: '',
      voteRating: 0,
      ratingDisabled: false,
      showDismissibleAlert: false,
      voteData: {},
      ratingVisable: true,
      movieRecomendationVisable: true
    }
  },
  created () {
    this.$auth.getProfile()
      .then(profile => {
        this.profile = profile
        if (this.profile === null) {
          this.ratingVisable = false
          this.movieRecomendationVisable = false
      } else {
        RecommendationService.getType().then((response) => {
          if (response.data === 2) {
              this.movieRecomendationVisable = false
          }
        }).catch((error) => {
          console.log(error.response.data)
        })

        MovieService.getUserMovieVote(this.$router.currentRoute.params.id).then((response) => {
          this.voteRating = response.data.rating
          this.ratingDisabled = true
        }).catch(error => {
          if (error.response.status === 404) {
          this.voteRating = 0
          }
        })
      }
      })
      .catch(error => {
        console.log(error)
      })

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
  },
  methods: {
    setRating (rating) {
      if (this.voteRating === 0) {
          this.voteRating = rating
          this.ratingDisabled = true
          this.showDismissibleAlert = true

          this.voteData.movieId = this.$router.currentRoute.params.id
          this.voteData.vote = rating

          MovieService.vote(this.voteData).then((response) => {
          }).catch(error => {
            console.log(error)
          })
      }
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
  max-width: 192px;
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
  -webkit-mask-image: -webkit-gradient(linear, left top, left bottom, color-stop(0, black), color-stop(0.35, black), color-stop(0.5, black), color-stop(0.65, black), color-stop(0.85, rgba(0, 0, 0, 0.6)), color-stop(1, transparent));
}
.page {
min-height: 100vh;
}
</style>
