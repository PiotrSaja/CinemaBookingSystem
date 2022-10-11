<template>
  <div class="container">
      <div class="row">
        <h5 class="text-white underline font-weight-bold">Recomendation configuration</h5>
      </div>
      <div class="row">
        <h6 class="text-white font-weight-bold ml-4">Please select favorite movies to future movies recommendation:</h6>
      </div>
      <div class="row mb-4">
        <div class="col-xl-3 col-lg-4 col-md-6 col-sm-6 col-6 padding"
        v-for="(movie, index) in movies.items"
        :key="index"
        :movie="movie"
        >
          <div class="movie-grid" @click="onMovieClicked(index, movie.id)">
            <div class="movie-image">
              <a class="image" :class="{checked: checked[index]}">
                <img class="pic-1" :src="movie.posterPath" />
              </a>
              <div class="heart" v-if="checked[index] == true">
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="row mt-4 mb-4 text-right underline" v-if="this.selectedList.length > 0">
         <b-button class="btn-success" v-on:click="getUserBaseRecomendation">Save favorite movies</b-button>
      </div>
    </div>
</template>

<script>
import MovieService from '@/api-services/movie-service'
export default {
  name: 'MoviesPreferencesList',
  data () {
    return {
      movies: {},
      checked: [],
      selectedList: []
    }
  },
  created () {
    this.fetchData()
  },
  methods: {
      fetchData () {
          MovieService.getForSelectingFavorite().then((response) => {
            this.movies = response.data

            for (var i = 0; i < this.movies.items.length; i++) {
              this.checked.push(false)
            }
        })
      },
      onMovieClicked (index, movieId) {
        if (this.checked[index] === false) {
          this.$set(this.checked, index, true)
          this.selectedList.push(movieId)
        } else if (this.checked[index] === true) {
          this.$set(this.checked, index, false)
          var indexInList = this.selectedList.indexOf(movieId)

          this.selectedList.splice(indexInList)
        }
      },
      getUserBaseRecomendation () {
          var response = {
            moviesIds: this.selectedList
          }
          if (this.selectedList.length > 0) {
                MovieService.pref(response).then((response) => {
                this.$router.go()
            })
          }
      }
  }
}
</script>

<style scoped lang="css">
.movie-grid{
  font-family: 'Montserrat', sans-serif;
  text-align: center;
  margin: 25px 8px 0;
  border-radius: 10px;
  box-shadow:  0 0 3px rgba(0,0,0,0.1);
  transition: all 0.5s;
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
  .padding {
    padding-right: 10px;
    padding-left: 10px;
  }
}
.movie-grid:hover{
  box-shadow:  0 0 30px rgba(0,0,0,0.4);
  transform: scale(1.05);
}
.movie-grid .movie-image{ position: relative; }
.movie-grid .movie-image a.image{
    border-radius: 10px 10px 10px 10px;
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
.movie-grid .movie-content{
    background-color: #fff;
    text-align: left;
    padding: 15px 10px;
    border-top: 1px solid transparent;
    border-radius: 0 0 10px 10px;
    transition: all 0.3s;
}
.movie-grid:hover .movie-content{ border-top-color: #dbdbdb; }
.movie-grid .title{
    text-align: center;
    font-size: 15px;
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
}
.page {
min-height: 100vh;
}
.checked {
 filter: grayscale(75%);
}
.heart{
  width: 75px;
  height: 75px;
  position: absolute;
  margin: auto;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url('https://cdn.pixabay.com/photo/2014/03/25/16/24/heart-296983_640.png') no-repeat center;
  background-size: 75px 75px;
}
</style>
