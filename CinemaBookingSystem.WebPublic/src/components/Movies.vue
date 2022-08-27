<template>
  <div class="container page">
    <movie-carousel/>
    <div class="row mt-4">
        <div class="col-12 col-md-2">
            <b-form-input v-model="searchString" @keydown.native="fetchData()" placeholder="Search movie" class="mt-2 mr-5"></b-form-input>
        </div>
        <div class="col-12 col-md-8">
        <h5 class="text-white underline font-weight-bold">All movies</h5>
        </div>
        <div class="col-12 col-md-2">
            <b-dropdown text="Records number" class="mt-2 mr-5">
            <b-dropdown-item @click="updateLimitOnPage(8)">8</b-dropdown-item>
            <b-dropdown-item @click="updateLimitOnPage(16)">16</b-dropdown-item>
            <b-dropdown-item @click="updateLimitOnPage(32)">32</b-dropdown-item>
            </b-dropdown>
        </div>
    </div>
    <div class="container">
      <div class="row">
        <div class="col-xl-3 col-lg-4 col-md-6 col-sm-6 col-6 padding"
        v-for="movie in movies.items"
        :key="movie.id"
        :movie="movie"
        >
          <div class="movie-grid" @click="onMovieClicked(movie.id)">
            <div class="movie-image">
              <a href="" class="image">
                <img class="pic-1" :src="movie.posterPath" />
              </a>
              <span class="movie-label" v-if="movie.imdbRating >= 7.5">Mega hit!</span>
            </div>
            <div class="movie-content">
              <h3 class="title"><a href="#">{{ movie.title }}</a></h3>
            </div>
          </div>
        </div>
      </div>
    </div>
    <b-pagination
      @input="fetchData()"
      v-model="currentPage"
      :total-rows="movies.totalItems"
      :per-page="limitOnPage"
      size="sm"
      align="center"
      hide-goto-end-buttons
      pills
      class="mt-5 custom-pagination"
    ></b-pagination>
  </div>
</template>

<script>
import MovieService from '@/api-services/movie-service'
import MovieCarousel from '@/components/MovieCarousel'
export default {
  components: { MovieCarousel },
  name: 'Movies',
  data () {
    return {
      movies: {},
      limitOnPage: 16,
      currentPage: 1,
      searchString: ''
    }
  },
  created () {
   this.fetchData()
  },
  methods: {
      fetchData () {
          MovieService.getAll(this.currentPage, this.limitOnPage, this.searchString).then((response) => {
            this.movies = response.data
        })
      },
      updateLimitOnPage (number) {
          this.limitOnPage = number
          this.fetchData()
      },
      showMovieDetail (movieId) {
        this.$router.push({name: 'MovieDetail', params: {id: movieId}})
      },
      onMovieClicked (movieId) {
        this.showMovieDetail(movieId)
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
  .container, .container-fluid, .container-lg, .container-md, .container-sm, .container-xl /deep/{
    padding-right: 0px;
    padding-left: 0px;
    margin-right: auto;
    margin-left: auto;
  }
}
.movie-grid:hover{
  box-shadow:  0 0 30px rgba(0,0,0,0.4);
  transform: scale(1.05);
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
.custom-pagination /deep/ .page-link {
  color: gray;
}
.custom-pagination /deep/ .page-item {
  color: white;
}
.custom-pagination /deep/ .page-item.active .page-link {
  background-color: #FF9100;
  border-color: #FF9100;
  color: white;
}
</style>
